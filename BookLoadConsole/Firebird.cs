using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace BookLoadConsole
{
    public class Firebird
    {
        // https://www.firebirdsql.org/en/net-examples-of-use/

        #region Свойства
        /// <summary>
        /// Дата начала периода
        /// </summary>
        public DateTime FbExDateFrom { get; set; }
        /// <summary>
        /// Дата окончания периода
        /// </summary>
        public DateTime FbExDateTo { get; set; }
        #endregion

        public Firebird (DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom == null || dateTo == null)
                return;
            FbExDateFrom = dateFrom;
            FbExDateTo = dateTo;
        }


        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // СТРОКА ПОДКЛЮЧАНИЯ И ПРИМЕРЫ



        /// <summary>
        /// Строка соединения с базой данных
        /// </summary>
        public const string connectionString =
            "User=twuser;" +
            "Password=54321;" +
            "Database=profitmed_week;" +
            "DataSource=192.168.127.111;" +
            "Port=;" +
            "Dialect=1;" +
            "Charset=NONE;" +
            "Role=;" +
            "Connection lifetime=15;" +
            "Pooling=true;" +
            "MinPoolSize=0;" +
            "MaxPoolSize=50;" +
            "Packet Size=8192;" +
            "ServerType=0";

        /// <summary>
        /// Соединение с базой данных
        /// </summary>
        FbConnection con = new FbConnection(connectionString);

        /// <summary>
        /// Открытие соединения с базой данных
        /// </summary>
        private void FbConnect()
        {
            try
            {
                Console.WriteLine("Открытие соединения с базой данных Firebird");
                con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка соединения: {0}", ex);
            }
        }

        /// <summary>
        /// Закрытие соединения с базой данных
        /// </summary>
        private void FbDisconnect()
        {
            try
            {
                Console.WriteLine("Закрытие соединения с базой данных Firebird");
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка закрытия соединения: {0}", ex);
            }
        }

        /// <summary>
        /// Выполнение запроса не возвращающего результаты
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteNonQuery(string query)
        {
            FbConnect();

            FbTransaction tran = con.BeginTransaction();
            FbCommand sql = new FbCommand
            {
                CommandText = query,
                Connection = con,
                Transaction = tran
            };

            try
            {
                sql.ExecuteNonQuery();
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
            }

            sql.Dispose();
            FbDisconnect();
        }

        /// <summary>
        /// Выполнение Select-запроса к базе данных
        /// </summary>
        /// <param name="query">Select запрос</param>
        /// <returns>Список массивов (фактически двумерный массив)</returns>
        public List<string[]> SelectQuery(string query)
        {
            FbConnect();

            FbCommand sql = new FbCommand
            {
                CommandText = query,
                Connection = con
            };

            FbDataReader dr = sql.ExecuteReader();
            List<string[]> res = new List<string[]>();

            // Получаем каждую запись (Чтение идет строго один раз)
            while (dr.Read())
            {
                string[] resLine = new string[dr.FieldCount];
                // Для каждой записи, выводим каждое значение поля
                for (int i = 0; i < dr.FieldCount; i++)
                    resLine[i] = dr.GetString(i);

                res.Add(resLine);
            }

            sql.Dispose();
            FbDisconnect();

            return res;
        }



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ОСНОВНЫЕ МЕТОДЫ



        /*  Получение SQL запроса к процедуре
         *  !!! Выводится перечень колонок, отсортированный согласно порядку по процедуре
         
            select first 1 'select ' ||
                ( -- Получаем первый параметр для строки select
                 select first 1 pp.RDB$PARAMETER_NAME || coalesce(' as ' || pp.RDB$DESCRIPTION, '') sqlLine
                 from RDB$PROCEDURE_PARAMETERS pp
                 where pp.RDB$PROCEDURE_NAME = p.RDB$PROCEDURE_NAME
                   and pp.RDB$PARAMETER_TYPE = 1
                 order by pp.RDB$PARAMETER_NUMBER
                )
                || ASCII_CHAR(13) || ASCII_CHAR(10) ||
                ( -- Получаем список остальных параметров с разделителем с новой строки
                 select list ('     , ' || t.sqlLine, ASCII_CHAR(13) || ASCII_CHAR(10))
                 from
                 (select skip 1 pp.RDB$PARAMETER_NAME || coalesce(' as ' || pp.RDB$DESCRIPTION, '') sqlLine
                  from RDB$PROCEDURE_PARAMETERS pp
                  where pp.RDB$PROCEDURE_NAME = p.RDB$PROCEDURE_NAME
                    and pp.RDB$PARAMETER_TYPE = 1
                    and pp.RDB$DESCRIPTION not like '%NULL -- NULL%'
                  order by pp.RDB$PARAMETER_NUMBER) t
                )
                || ASCII_CHAR(13) || ASCII_CHAR(10) || 'from ' || trim(p.RDB$PROCEDURE_NAME) || '(@DateFrom, @DateTo)'
            from RDB$PROCEDURES p
            where p.RDB$PROCEDURE_NAME = :ProcName
        */

        // ВНИМАНИЕ! Порядок колонок должен строго соответствовать Excel файлу из примеров СБИС https://sbis.ru/formats/docFormatCard/115867/format
        // Так как это так же завязано и на настройках (Settings.cs). В настройках прописаны все идентификаторы столбцов для Excel файла (Cols8,9,10,11)
        // что в свою очередь освобождает от раздумий, какая колонка результирующего запроса к чему привязана. Нумирация колонок начинается с единицы, 
        // по этому требуется наличие служебного поля (первая "0" колонка). Другими словами на выход идет массив с шириной равной количеству колонок + 1.



        /// <summary>
        /// Вызов процедуры SP$TAX_INCOMING_INVOICES (11 - Журнал полученных СФ)
        /// </summary>
        /// <returns>Список массивов (фактически двумерный массив)</returns>
        public List<string[]> ExecIncomingInvoices()
        {
            FbConnect();
            FbTransaction tran = con.BeginTransaction();

            // Формируем запрос (команду)
            FbCommand sql = new FbCommand
            {
                CommandText = @"select O$INC                      as LISTNUM                         -- Порядковый номер
                                     , O$INVOICE_DATE             as DATERECEIVING                   -- Дата получения
                                     , O$OPER_CODE                as OPERTYPECODE                    -- Код вида операции
                                     , O$INVOICE_INFO             as NUMANDDATESF                    -- Номер и дата счета-фактуры
                                     , O$RECALC_INVOICE_INFO      as NUMANDDATESFRED                 -- Номер и дата исправления счета-фактуры
                                     , O$CORR_INVOICE_INFO        as NUMANDDATESFKOR                 -- Номер и дата корректировочного счета-фактуры
                                     , O$RECALC_CORR_INVOICE_INFO as NUMANDDATESFKORRED              -- Номер и дата исправления корректировочного счета-фактуры
                                     , O$CLIENT_NAME              as SELLERNAME                      -- Наименование продавца
                                     , O$CLIENT_INFO              as SELLERINNKPP                    -- ИНН/КПП продавца
                                     , O$INTERMEDIARY_NAME        as AGENTNAME                       -- Сведения о посреднической дейстельности, указываемые коммисионером (агентом). Наименование субкомиссионера (субагента)
                                     , O$INTERMEDIARY_INFO        as AGENTINNKPP                     -- Сведения о посреднической дейстельности, указываемые коммисионером (агентом). ИНН/КПП субкомиссионера (субагента)
                                     , O$DEAL_CODE                as DEALCODE                        -- Сведения о посреднической дейстельности, указываемые коммисионером (агентом). Код вида сделки
                                     , O$CURRENCY_INFO            as NAMEANDCODECURRENCY             -- Наименование и код валюты
                                     , O$INVOICE_SUM              as COSTSUMGOODSANDSERVICES         -- Стоимость товаров (работ,услуг), имущественных прав по счету-фактуре всего
                                     , O$INVOICE_NDS_SUM          as COSTSUMNDS                      -- В том числе сумма НДС по счету-фактуре
                                     , O$INVOICE_SUM_DIFF_INC     as DIFFCOSTWITHNDSOFSFKORPLUS      -- Разница стоимости с учетом НДС по корректировочному счету-фактуре. Уменьшение
                                     , O$INVOICE_SUM_DIFF_RED     as DIFFCOSTWITHNDSOFSFKORMINUS     -- Разница стоимости с учетом НДС по корректировочному счету-фактуре. Увеличение
                                     , O$INVOICE_NDS_SUM_DIFF_INC as DIFFCOSTWITHOUTNDSOFSFKORPLUS   -- Разница НДС по корректировочному счету-фактуре. Уменьшение
                                     , O$INVOICE_NDS_SUM_DIFF_RED as DIFFCOSTWITHOUTNDSOFSFKORMINUS  -- Разница НДС по корректировочному счету-фактуре. Увеличение
                                from SP$TAX_INCOMING_INVOICES(@DateFrom, @DateTo)",
                Connection = con,
                Transaction = tran
            };

            // Передаем параметры
            sql.Parameters.Add("@DateFrom", FbDbType.Date);
            sql.Parameters[0].Value = FbExDateFrom;
            sql.Parameters.Add("@DateTo", FbDbType.Date);
            sql.Parameters[1].Value = FbExDateTo;

            // Вызываем запрос
            List<string[]> res = BodyExecute(sql, "SP$TAX_INCOMING_INVOICES");

            // Подкатываем транзакцию и закрываем соединение
            tran.Commit();
            sql.Dispose();
            FbDisconnect();

            return res;
        }

        /// <summary>
        /// Вызов процедуры SP$TAX_OUTGOING_INVOICES (10 - Журнал выставленных СФ)
        /// </summary>
        /// <returns>Список массивов (фактически двумерный массив)</returns>
        public List<string[]> ExecOutgoingInvoices()
        {
            FbConnect();
            FbTransaction tran = con.BeginTransaction();

            // Формируем запрос (команду)
            FbCommand sql = new FbCommand
            {
                CommandText = @"select O$INC                           as LISTNUM -- Порядковый номер
                                     , O$INVOICE_DATE                  as DATERECEIVING -- Дата получения
                                     , O$OPER_CODE                     as OPERTYPECODE -- Код вида операции
                                     , O$INVOICE_INFO                  as NUMANDDATESF -- Номер и дата счета-фактуры
                                     , O$RECALC_INVOICE_INFO           as NUMANDDATESFRED -- Номер и дата исправления счета-фактуры
                                     , O$CORR_INVOICE_INFO             as NUMANDDATESFKOR -- Номер и дата корректировочного счета-фактуры
                                     , O$RECALC_CORR_INVOICE_INFO      as NUMANDDATESFKORRED -- Номер и дата исправления корректировочного счета-фактуры
                                     , O$CLIENT_NAME                   as SELLERNAME -- Наименование продавца
                                     , O$CLIENT_INFO                   as SELLERINNKPP -- ИНН/КПП продавца
                                     , O$INTERMEDIARY_NAME             as AGENTNAME -- Сведения о посреднической дейстельности, указываемые коммисионером (агентом). Наименование субкомиссионера (субагента)
                                     , O$INTERMEDIARY_INFO             as AGENTINNKPP -- Сведения о посреднической дейстельности, указываемые коммисионером (агентом). ИНН/КПП субкомиссионера (субагента)
                                     , O$DEAL_CODE                     as DEALCODE -- Сведения о посреднической дейстельности, указываемые коммисионером (агентом). Код вида сделки
                                     , O$CURRENCY_INFO                 as NAMEANDCODECURRENCY -- Наименование и код валюты
                                     , O$INVOICE_SUM                   as COSTSUMGOODSANDSERVICES -- Стоимость товаров (работ,услуг), имущественных прав по счету-фактуре всего
                                     , O$INVOICE_NDS_SUM               as COSTSUMNDS -- В том числе сумма НДС по счету-фактуре
                                     , O$INVOICE_SUM_DIFF_INC          as DIFFCOSTWITHNDSOFSFKORPLUS -- Разница стоимости с учетом НДС по корректировочному счету-фактуре. Уменьшение
                                     , O$INVOICE_SUM_DIFF_RED          as DIFFCOSTWITHNDSOFSFKORMINUS -- Разница стоимости с учетом НДС по корректировочному счету-фактуре. Увеличение
                                     , O$INVOICE_NDS_SUM_DIFF_INC      as DIFFCOSTWITHOUTNDSOFSFKORPLUS -- Разница НДС по корректировочному счету-фактуре. Уменьшение
                                     , O$INVOICE_NDS_SUM_DIFF_RED      as DIFFCOSTWITHOUTNDSOFSFKORMINUS -- Разница НДС по корректировочному счету-фактуре. Увеличение
                                from SP$TAX_OUTGOING_INVOICES(@DateFrom, @DateTo)",
                Connection = con,
                Transaction = tran
            };

            // Передаем параметры
            sql.Parameters.Add("@DateFrom", FbDbType.Date);
            sql.Parameters[0].Value = FbExDateFrom;
            sql.Parameters.Add("@DateTo", FbDbType.Date);
            sql.Parameters[1].Value = FbExDateTo;

            // Вызываем запрос
            List<string[]> res = BodyExecute(sql, "SP$TAX_OUTGOING_INVOICES");

            // Подкатываем транзакцию и закрываем соединение
            tran.Commit();
            sql.Dispose();
            FbDisconnect();

            return res;
        }

        /// <summary>
        /// Вызов процедуры SP$TAX_SALES_BOOK (9 - Книга продаж)
        /// </summary>
        /// <returns>Список массивов (фактически двумерный массив)</returns>
        public List<string[]> ExecSalesBook()
        {
            FbConnect();
            FbTransaction tran = con.BeginTransaction();

            // Формируем запрос (команду)
            FbCommand sql = new FbCommand
            {
                CommandText = @"select O$INC                           as LISTNUM -- Номер по порядку
                                     , O$OPER_CODE                     as OPERTYPECODE -- Код вида опреации
                                     , O$INVOICE_INFO                  as SELLERNUMANDDATESF -- Номер и дата счета-фактуры продавца
                                     , O$GTD                           as NUMBERTD -- Регистрационный номер таможенной декларации
                                     , O$GOOD_TYPE_CODE                as GOODTYPECODE -- Код вида товара
                                     , O$RECALC_INVOICE_INFO           as SELLERNUMANDDATESFRED -- Номер и дата исправления счета-фактуры продавца
                                     , O$CORR_INVOICE_INFO             as SELLERNUMANDDATESFKOR -- Номер и дата корректировочного счета-фактуры продавца
                                     , O$RECALC_CORR_INVOICE_INFO      as SELLERNUMANDDATESFKORRED -- Номер и дата исправления корректировочного счета-фактуры продавца
                                     , O$CLIENT_NAME                   as KONTRNAME -- Наименование покупателя
                                     , O$CLIENT_INFO                   as KONTRINNKPP -- ИНН и КПП покупателя
                                     , O$INTERMEDIARY_NAME             as AGENTNAME -- Наименование посредника
                                     , O$INTERMEDIARY_INFO             as AGENTINNKPP -- ИНН и КПП посредника
                                     , O$PAYMENT_DOC_INFO              as NUMANDDATEDOCCONFIRMPAY -- Номер и дата документа подтверждающего оплату
                                     , O$CURRENCY_INFO                 as NAMEANDCODECURRENCY -- Наименование и код валюты
                                     , OINVOICE_CURRENCY_SUM           as COSTCURRENCYSF -- Сумма продажи в валюте с/ф
                                     , O$INVOICE_SUM                   as COSTRUBKOP -- Сумма продажи в рублях и копейках
                                     , O$INVOICE_SUM_20                as COSTRUBKOPWITHOUTNDS20 -- Сумма продаж с налогом 20
                                     , O$INVOICE_SUM_18                as COSTRUBKOPWITHOUTNDS18 -- Сумма продаж с налогом 18
                                     , O$INVOICE_SUM_10                as COSTRUBKOPWITHOUTNDS10 -- Сумма продаж с налогом 10
                                     , O$INVOICE_SUM_0                 as COSTRUBKOPWITHOUTNDS0 -- Сумма продаж с налогом 0
                                     , O$INVOICE_NDS_SUM_20            as SUMNDS20 -- Сумма НДС с налогом 20
                                     , O$INVOICE_NDS_SUM_18            as SUMNDS18 -- Сумма НДС с налогом 18
                                     , O$INVOICE_NDS_SUM_10            as SUMNDS10 -- Сумма НДС с налогом 10
                                     , O$INVOICE_SUM_WONDS             as COSTSALESWITHOUTNDS -- Сумма продаж необлагаемых налогом
                                from SP$TAX_SALES_BOOK(@DateFrom, @DateTo)",
                Connection = con,
                Transaction = tran
            };

            // Передаем параметры
            sql.Parameters.Add("@DateFrom", FbDbType.Date);
            sql.Parameters[0].Value = FbExDateFrom;
            sql.Parameters.Add("@DateTo", FbDbType.Date);
            sql.Parameters[1].Value = FbExDateTo;

            // Вызываем запрос
            List<string[]> res = BodyExecute(sql, "SP$TAX_SALES_BOOK");

            // Подкатываем транзакцию и закрываем соединение
            tran.Commit();
            sql.Dispose();
            FbDisconnect();

            return res;
        }

        /// <summary>
        /// Вызов процедуры SP$TAX_PURCHASES_BOOK (8 - Книга покупок)
        /// </summary>
        /// <returns>Список массивов (фактически двумерный массив)</returns>
        public List<string[]> ExecPurchasesBook()
        {
            FbConnect();
            FbTransaction tran = con.BeginTransaction();

            // Формируем запрос (команду)
            FbCommand sql = new FbCommand
            {
                CommandText = @"select O$INC                           as LISTNUM -- Порядковый номер
                                     , O$OPER_CODE                     as OPERTYPECODE -- Код вида операции
                                     , O$INVOICE_INFO                  as SELLERNUMANDDATESF -- Номер и дата счета-фактуры продавца
                                     , O$RECALC_INVOICE_INFO           as SELLERNUMANDDATESFRED -- Номер и дата исправления счета-фактуры продавца
                                     , O$CORR_INVOICE_INFO             as SELLERNUMANDDATESFKOR -- Номер и дата корректировочного счета-фактуры продавца
                                     , O$RECALC_CORR_INVOICE_INFO      as SELLERNUMANDDATESFKORRED -- Номер и дата исправления корректировочного счета-фактуры продавца
                                     , O$PAYMENT_DOC_INFO              as NUMANDDATEDOCCONFIRMPAY -- Номер и дата документа, подтверждающего уплату налога
                                     , O$REGISTR_DOC_INFO              as DATEACCEPTGOODORSERVICES -- Дата принятия на учет товаров (работ, услуг), имущественных прав
                                     , O$CLIENT_NAME                   as SELLERNAME -- Наименование продавца
                                     , O$CLIENT_INFO                   as SELLERINNKPP -- ИНН/КПП продавца
                                     , O$INTERMEDIARY_NAME             as AGENTNAME -- Сведения о посреднике (комиссионере, агенте). Наименование посредника
                                     , O$INTERMEDIARY_INFO             as AGENTINNKPP -- Сведения о посреднике (комиссионере, агенте). ИНН/КПП посредника
                                     , O$GTD                           as NUMBERTD -- Регистрационный номер таможенной декларации
                                     , O$CURRENCY_INFO                 as NAMEANDCODECURRENCY -- Наименование и код валюты
                                     , O$INVOICE_SUM                   as COSTPAYMENTOFSF -- Стоимость покупок по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая НДС) в валюте счета-фактуры
                                     , O$INVOICE_TAX_SUM               as SUMNDSOFSF -- Сумма НДС по счету-фактуре, разница суммы НДС по корректировочному счету-фактуре, принимаемая к вычету в рублях и копейках
                                from SP$TAX_PURCHASES_BOOK(@DateFrom, @DateTo)",
                Connection = con,
                Transaction = tran
            };

            // Передаем параметры
            sql.Parameters.Add("@DateFrom", FbDbType.Date);
            sql.Parameters[0].Value = FbExDateFrom;
            sql.Parameters.Add("@DateTo", FbDbType.Date);
            sql.Parameters[1].Value = FbExDateTo;

            // Вызываем запрос
            List<string[]> res = BodyExecute(sql, "SP$TAX_PURCHASES_BOOK");
            
            // Подкатываем транзакцию и закрываем соединение
            tran.Commit();
            sql.Dispose();
            FbDisconnect();

            return res;
        }

        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ВСПОМОНАТЕЛЬНЫЕ МЕТОДЫ

        /// <summary>
        /// Разбор результатов
        /// </summary>
        /// <param name="sql">Комманда SQL</param>
        /// <param name="ProcedureName">Наименование вызываемой процедуры</param>
        /// <returns></returns>
        private List<string[]> BodyExecute(FbCommand sql, string ProcedureName)
        {
            Console.WriteLine("SQL: Выполнение запроса к {0}", ProcedureName);
            FbDataReader dr = sql.ExecuteReader();
            List<string[]> res = new List<string[]>();

            int ii = 0;
            // Получаем результаты (Чтение идет строго один раз)
            while (dr.Read())
            {
                Console.Write("\rSQL: Извлечение данных {0}", ii++);
                string[] resLine = new string[dr.FieldCount + 1]; // +1 служебное
                // Для каждой записи, выводим каждое значение поля
                for (int i = 0; i < dr.FieldCount; i++)
                    resLine[i + 1] = dr.GetString(i);
                res.Add(resLine);
            }
            Console.WriteLine("");

            return res;
        }
    }
}
