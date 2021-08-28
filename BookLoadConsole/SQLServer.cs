using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace BookLoadConsole
{
    public class DatesFromTo
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class SQLServer
    {
        const string conStr = "Data Source=pmserv; Initial Catalog=tax; User Id=sa; Password=studio;";

        /// <summary>
        /// Id типа книги по таблице dbo.BookTypes
        /// </summary>
        public int BookTypeId { get; set; }
        /// <summary>
        /// Id формата книги по таблице dbo.FileFormats
        /// </summary>
        public int FileFormatId { get; set; }
        /// <summary>
        /// Id периода по таблице dbo.Periods
        /// </summary>
        public int PeriodId { get; set; }



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------



        /// <summary>
        /// Универсальная процедура загрузки данных в таблицы
        /// </summary>
        /// <param name="bookType">Тип книги 8-11</param>
        /// <param name="data">Данные (включая служебное [0] поле)</param>
        /// <param name="rowStart">Строка начала считывания</param>
        public void LoadDataToTables(int bookType, List<string[]> data, int rowStart = 0)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tran;
            int execRows = 0;

            string tempTableName = "";
            string storedProcedure = "";

            string[] dsBookTypes = GetTableNamesFromFileFormatsByNum(bookType);
            tempTableName = dsBookTypes[0];
            storedProcedure = dsBookTypes[1];

            // Формируем временную табличку (Возможно потребуется построчная вставка)
            try
            {
                rowStart = (rowStart == 0) ? rowStart : rowStart - 1;
                CreateTempTable(cmd, tempTableName, data, rowStart);
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL-скрипта создания {0}: {1}", tempTableName, ex.Message);
                conn.Close();
                return;
            }

            // Выполняем процедуру, которая будет ее дрегать
            cmd.CommandText = storedProcedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter { ParameterName = "@periodId", Value = PeriodId });
            cmd.Parameters.Add(new SqlParameter { ParameterName = "@fileFormatId", Value = FileFormatId });
            try
            {
                execRows = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL процедуры {0}: {1}", storedProcedure, ex.Message);
            }

            conn.Close();
        }

        /// <summary>
        /// Формирование скрипта создания временной таблицы
        /// </summary>
        /// <param name="TempTableName">Наименование таблицы</param>
        /// <param name="data">Данные</param>
        /// <returns>SQL-скрипт в виде строки</returns>
        private void CreateTempTable(SqlCommand cmd, string TempTableName, List<string[]> data, int rowStart = 0)
        {
            // ВНИМАНИЕ!!! data имеет служебное пустое поле [0]
            string res = "";
            string insertFields = "(";
            int rowCount = data[0].Count();
            int execRows = 0;
            cmd.CommandType = CommandType.Text;
            string fieldName = "f";

            // В случае, если таблица существует - удаляем
            res += "if object_id('tempdb..#" + TempTableName + "') is not null drop table #" + TempTableName + "\n";
            // Создаем таблицу средствами автоматического создания select
            res += "select * into #" + TempTableName + " from (\nselect ";
            // Формируем структуру
            for (int i = 0; i < rowCount; i++)
            {
                res += "cast('' as varchar(max)) as " + fieldName + i.ToString() + ((i + 1 != rowCount) ? ", " : "");
                insertFields += fieldName + i.ToString() + ((i + 1 != rowCount) ? ", " : "");
            }
            res += "\n) t";
            insertFields += ")";
            // Как только сформировалась таблица, очищаем, оставляя только структуру
            res += "\ndelete from #" + TempTableName;

            // Создаем таблицу
            Console.WriteLine(res);
            cmd.CommandText = res;
            execRows = cmd.ExecuteNonQuery();

            Settings St = new Settings();

            int rowsPack = 0; // Записей в пакете
            int countPack = 0; // Кол-во пакетов
            int limitPack = St.LimitRowsPack; // Лимит записей на пакет
            Console.WriteLine("Лимит записей на пакет: {0}", limitPack);

            // Пакетная выгрузка
            for (int i = rowStart; i < data.Count(); i++)
            {
                // Для каждого нового пакета
                if (rowsPack == 0)
                    res = "insert into #" + TempTableName + insertFields + "\n";

                string[] row = data[i];
                rowsPack++;

                // Формируем заполнение
                res += "select ";
                for (int c = 0; c < rowCount; c++)
                    res += "'" + ((String.IsNullOrEmpty(row[c])) ? "" : row[c].Replace("'", "''")) + "'" + ((c + 1 != rowCount) ? ", " : "");
                res += ((rowsPack != limitPack) && (i + 1 != data.Count())) ? "\nUNION ALL\n" : "\n";

                // Если пакет заполнен - выгружаем
                if (rowsPack == limitPack)
                    RunPack(cmd, res, countPack, rowsPack, out countPack, out rowsPack);
            }

            // Если остались записи в пакете (пакет не полный)
            if (rowsPack != 0)
                RunPack(cmd, res, countPack, rowsPack, out countPack, out rowsPack);
        }

        /// <summary>
        /// Запуск обработки пакета
        /// </summary>
        /// <param name="cmd">Объект SqlCommand</param>
        /// <param name="sqlQuery">Запрос</param>
        /// <param name="countPack">Номер пакета</param>
        /// <param name="rowsPack">Кол-во записей в пакете</param>
        /// <param name="newCountPack">Приращение пакета</param>
        /// <param name="newRowsPack">Обнуление записей пакета</param>
        private void RunPack(SqlCommand cmd, string sqlQuery, int countPack, int rowsPack, out int newCountPack, out int newRowsPack)
        {
            int execRows = 0;

            Console.WriteLine(sqlQuery);
            cmd.CommandText = sqlQuery;
            execRows = cmd.ExecuteNonQuery();

            newCountPack = countPack + 1;
            Console.WriteLine("Пакет {0}: записей в пакете {1} : обработано {2} записей", countPack, rowsPack, execRows);
            newRowsPack = rowsPack = 0;
        }



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // Вспомогательные функции и процедуры



        /// <summary>
        /// Номер типа книги по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetNumFromBookTypeById(int id)
        {
            int res = 0;

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand();

            // Сочетать Command с Connection.
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "select top 1 num from dbo.BookTypesView where id = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                    res = Convert.ToInt32(dr.GetValue(0));
            }

            conn.Close();

            return res;
        }

        /// <summary>
        /// Наименование таблицы по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTableNameFromBookTypeById(int id)
        {
            string res = "";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand();

            // Сочетать Command с Connection.
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "select top 1 tableName from dbo.BookTypesView where id = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                    res = Convert.ToString(dr.GetValue(0));
            }

            conn.Close();

            return res;
        }

        /// <summary>
        /// Даты по периоду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DatesFromTo GetDatesFromPeriodsById(int id)
        {
            DatesFromTo res = new DatesFromTo();

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand();

            // Сочетать Command с Connection.
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "select top 1 fromDate, toDate from dbo.PeriodsView where id = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    res.From = Convert.ToDateTime(dr.GetValue(0));
                    res.To = Convert.ToDateTime(dr.GetValue(1));
                }
            }

            conn.Close();

            return res;
        }

        /// <summary>
        /// Получение имени временной таблицы и процедуры загрузки
        /// </summary>
        /// <param name="num">Номер раздела книг</param>
        /// <returns>[0] tempTableName, [1] loadStoredProcedure</returns>
        public string[] GetTableNamesFromFileFormatsByNum(int num)
        {
            string[] res = new string[2];

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand();

            // Сочетать Command с Connection.
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "select top 1 tempTableName, loadStoredProcedure from dbo.FileFormatsView where num = @num";
            cmd.Parameters.Add(new SqlParameter("@num", num));

            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    res[0] = Convert.ToString(dr.GetValue(0)); // tempTableName
                    res[1] = Convert.ToString(dr.GetValue(1)); // loadStoredProcedure
                }
            }

            conn.Close();

            return res;
        }

        /// <summary>
        /// Получаем последний (самый свежий) действующий формат файлов по системе СБИС
        /// </summary>
        /// <returns></returns>
        public int GetCurrentFileFormatId()
        {
            int res = 0;

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand();

            // Сочетать Command с Connection.
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "select top 1 max(id) from dbo.FileFormatsView where isActive = 1";

            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                    res = Convert.ToInt32(dr.GetValue(0));
            }

            conn.Close();

            return res;
        }



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // Работа со справочниками


        /// <summary>
        /// Получение данных таблицы Периоды
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataFromView(string TableName, int id = 0, string sqlOrderByFields = "", string sqlWhereQuery = "")
        {
            // Создать объект Command.
            SqlCommand cmd = new SqlCommand();

            // Сочетать Command с Connection.
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            cmd.Connection = conn;
            sqlWhereQuery = (String.IsNullOrEmpty(sqlWhereQuery)) ? "" : " and " + sqlWhereQuery;
            sqlOrderByFields = (String.IsNullOrEmpty(sqlOrderByFields)) ? "" : " order by " + sqlOrderByFields;
            TableName = (TableName.IndexOf(".") < 0) ? "dbo." + TableName : TableName;

            cmd.CommandText = "select * from " + TableName + "View where ((@id = id) or (@id = 0))" + sqlWhereQuery + sqlOrderByFields;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = id;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet res = new DataSet();
            da.Fill(res);

            conn.Close();
            return res;
        }

        /// <summary>
        /// Удалить запись из таблицы
        /// </summary>
        /// <param name="TableName">Наименование таблицы</param>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>Кол-во затронутых строк</returns>
        public int DeleteFromTable(string TableName, int id = 0)
        {
            if (id == 0)
            {
                Console.WriteLine("Ошибка удаления записи из таблицы dbo." + TableName + ". Идентификатор записи не передан.");
                return 0;
            }
            int res = 0;

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tran;
            cmd.CommandText = "delete from dbo." + TableName + " where id = @id";
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = id;

            try
            {
                Console.WriteLine(cmd.CommandText);
                res = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL-скрипта: {0}", ex.Message);
                conn.Close();
                return 0;
            }

            conn.Close();
            return res;
        }

        /// <summary>
        /// Вставка в таблицу Периоды
        /// </summary>
        /// <param name="valNum">Номер периода</param>
        /// <param name="valYear">Год</param>
        /// <param name="valFromDate">Начало периода</param>
        /// <param name="valToDate">Окончание периода</param>
        /// <returns>Кол-во затронутых строк</returns>
        public int AddToPeriods(int valNumQuarter, int valYear)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tran;
            cmd.CommandText = "insert into dbo.Periods (numQuarter, year) values (@numQuarter, @year)";

            cmd.Parameters.Add(new SqlParameter("@numQuarter", SqlDbType.Int));
            cmd.Parameters["@numQuarter"].Value = valNumQuarter;
            cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.Int));
            cmd.Parameters["@year"].Value = valYear;

            try
            {
                Console.WriteLine(cmd.CommandText);
                res = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL-скрипта: {0}", ex.Message);
                conn.Close();
                return 0;
            }

            conn.Close();
            return res;
        }

        /// <summary>
        /// Измененеие записи таблицы Периоды
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="valNum">Номер периода</param>
        /// <param name="valYear">Год</param>
        /// <param name="valFromDate">Начало периода</param>
        /// <param name="valToDate">Окончание периода</param>
        /// <returns>Кол-во затронутых строк</returns>
        public int UpdatePeriods(int id, int valNumQuarter, int valYear)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tran;
            cmd.CommandText = "update dbo.Periods set numQuarter = @numQuarter, year = @year where id = @id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters.Add(new SqlParameter("@numQuarter", SqlDbType.Int));
            cmd.Parameters["@numQuarter"].Value = valNumQuarter;
            cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.Int));
            cmd.Parameters["@year"].Value = valYear;

            try
            {
                Console.WriteLine(cmd.CommandText);
                res = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL-скрипта: {0}", ex.Message);
                conn.Close();
                return 0;
            }

            conn.Close();
            return res;
        }

        //---------------------------------------------------------------------------------------------------

        /// <summary>
        /// Вставка в таблицу Форматы файлов
        /// </summary>
        /// <returns>Кол-во затронутых строк</returns>
        public int AddToFileFormats(string valName, DateTime valFromDate, DateTime valToDate)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tran;
            cmd.CommandText = "insert into dbo.FileFormats (name, fromDate, toDate) values (@name, @fromDate, @toDate)";

            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 255));
            cmd.Parameters["@name"].Value = valName;
            cmd.Parameters.Add(new SqlParameter("@fromDate", SqlDbType.DateTime));
            cmd.Parameters["@fromDate"].Value = valFromDate;
            cmd.Parameters.Add(new SqlParameter("@toDate", SqlDbType.DateTime));
            cmd.Parameters["@toDate"].Value = valToDate;

            try
            {
                Console.WriteLine(cmd.CommandText);
                res = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL-скрипта: {0}", ex.Message);
                conn.Close();
                return 0;
            }

            conn.Close();
            return res;
        }

        /// <summary>
        /// Измененеие записи таблицы Форматы файлов
        /// </summary>
        /// <returns>Кол-во затронутых строк</returns>
        public int UpdateFileFormats(int id, string valName, DateTime valFromDate, DateTime valToDate)
        {
            int res = 0;

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tran;
            cmd.CommandText = "update dbo.FileFormats set name = @name, fromDate = @fromDate, toDate = @toDate where id = @id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 255));
            cmd.Parameters["@name"].Value = valName;
            cmd.Parameters.Add(new SqlParameter("@fromDate", SqlDbType.DateTime));
            cmd.Parameters["@fromDate"].Value = valFromDate;
            cmd.Parameters.Add(new SqlParameter("@toDate", SqlDbType.DateTime));
            cmd.Parameters["@toDate"].Value = valToDate;

            try
            {
                Console.WriteLine(cmd.CommandText);
                res = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (conn.State != (ConnectionState.Broken & ConnectionState.Closed))
                    tran.Rollback();
                Console.WriteLine("ОШИБКА выполнения SQL-скрипта: {0}", ex.Message);
                conn.Close();
                return 0;
            }

            conn.Close();
            return res;
        }
    }
}
