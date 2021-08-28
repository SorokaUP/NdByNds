using System;
using System.IO;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using ExcelDataReader;
using System.Data;
using System.Collections.Generic;
using System.Xml;

namespace BookLoadConsole
{
    public class Book
    {
        #region Свойства
        /// <summary>
        /// Полный путь к файлу Excel
        /// </summary>
        public string FilePathExcel { get; set; }
        /// <summary>
        /// Директория формирования выходного файла XML
        /// </summary>
        public string ExportPathXml { get; set; }
        /// <summary>
        /// Директория формирования выходного файла Excel
        /// </summary>
        public string ExportPathExcel { get; set; }
        /// <summary>
        /// Тип обрабатываемой книги: 8 - Книга покупок; 9 - Книга продаж; 10 - Журнал выставленных СФ; 11 - Журнал полученных СФ
        /// </summary>
        public int BookType { get; set; }
        /// <summary>
        /// Номер корректировки
        /// </summary>
        public int NumberKorr { get; set; }
        /// <summary>
        /// Настройки
        /// </summary>
        public Settings St { get; set; }
        /// <summary>
        /// Период выгрузки из базы ОТ
        /// </summary>
        public DateTime FbExDateFrom { get; set; }
        /// <summary>
        /// Период выгрузки из базы ДО
        /// </summary>
        public DateTime FbExDateTo { get; set; }
        /// <summary>
        /// База данных
        /// </summary>
        public Firebird Fb { get; set; }
        /// <summary>
        /// Путь к XML файлу
        /// </summary>
        public string FilePathXml { get; set; }
        /// <summary>
        /// Путь к XSD схеме
        /// </summary>
        public string FilePathXsd { get; set; }
        /// <summary>
        /// База данных SQLServer (tax)
        /// </summary>
        public SQLServer Ss { get; set; }

        public Book()
        {
            St = new Settings();
            Fb = new Firebird(FbExDateFrom, FbExDateTo);
            Ss = new SQLServer();
        }
        #endregion

        private string GenerateFileName;

        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ПРОВЕРКА EXCEL ФАЙЛА (exchk)



        #region CheckExcelData
        /// <summary>
        /// Запустить обработку Excel-файла
        /// </summary>
        public void ChecExcelkData()
        {
            Console.WriteLine("Запуск процесса обработки Excel-файла");
            if (!File.Exists(FilePathExcel))
            {
                Console.WriteLine("Ошибка. Файл не существует или к нему нет доступа.");
                return;
            }
            if (BookType != 8 && BookType != 9)
            {
                Console.WriteLine("Ошибка. Тип книги не определен или введен не корректно. Требуется 8 или 9.");
                return;
            }
            Console.WriteLine("   Настройка \"Перенумеровать\" установлена в \"{0}\"", St.IsRenumber);
            Console.WriteLine("   Настройка \"Перезапись ИНН и КПП на исправленные\" установлена в \"{0}\"", St.IsRewriteInnKpp);
            Console.WriteLine("   Настройка \"Запись ошибок в файл\" установлена в \"{0}\"", St.IsWriteMsg);

            Console.WriteLine("");
            Console.WriteLine(">>> Начало проверки файла Excel");
            CheckReadExcel();
            Console.WriteLine(">>> Конец проверки файла Excel");
        }

        /// <summary>
        /// Чтение и проверка Excel файла
        /// </summary>
        public void CheckReadExcel()
        {
            // EPPlus. Актуально только для *.xlsx (используя Interop можно пересохранить *.xls в *.xlsx (Excel 12) формат)

            Console.WriteLine("EPPlus. Открытие файла");
            var package = new ExcelPackage(new FileInfo(EPPlus_ResaveToExcel12()));
            Console.WriteLine("Файл открыт, получение первого листа");
            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            Console.WriteLine("Первый лист получен, чтение данных");
            int rowCount = workSheet.Dimension.End.Row;
            int colCount = workSheet.Dimension.End.Column;
            ExcelRange Cells = workSheet.Cells;

            switch (BookType)
            {
                case 8:
                    CheckReadExcel_Body8(Cells, rowCount, colCount);
                    break;

                case 9:
                    CheckReadExcel_Body9(Cells, rowCount, colCount);
                    break;
            }

            Console.WriteLine("EPPlus. Сохранение файла");
            package.Save();
            Console.WriteLine("EPPlus. Завершение работы");
            workSheet.Dispose();
            package.Dispose();
        }

        /// <summary>
        /// Проверки книги (9) Продаж
        /// </summary>
        /// <param name="Cells"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public void CheckReadExcel_Body9(ExcelRange Cells, int rowCount, int colCount)
        {
            Console.WriteLine("Запуск процедуры проверки книги (9) Продаж");

            int renumValue = 0;
            string msg = "";
            string tmsg = "";
            string KontrInnKpp = "";
            string newKontrInnKpp = "";
            string AgentInnKpp = "";
            string newAgentInnKpp = "";

            // Производим чтение
            for (int i = St.RowStart9; i <= rowCount; i++) //по всем строкам
            {
                Console.Write("\rСтрока {0} из {1} ({2}%)", i, rowCount, (i * 100 / rowCount));
                msg = "";

                //-------------------------------------------------------------------------------------
                // Проверка на наличие непечатных символов (возврат каретки, табуляция) - все ячейки
                CheckNonPrintableCharacter(Cells, i, colCount);

                // ИНН/КПП Контрагента
                KontrInnKpp = Cells[i, St.Cols9.KontrInnKpp].Text;
                CheckDataInnKpp(KontrInnKpp, out tmsg, out newKontrInnKpp);
                msg += (msg != "") ? "; " + tmsg : tmsg;

                // ИНН/КПП Посредника (при наличии)
                AgentInnKpp = Cells[i, St.Cols9.AgentInnKpp].Text;
                newAgentInnKpp = AgentInnKpp;
                if (AgentInnKpp != "")
                {
                    CheckDataInnKpp(AgentInnKpp, out tmsg, out newAgentInnKpp);
                    msg += (msg != "") ? "; " + tmsg : tmsg;
                }
                //-------------------------------------------------------------------------------------

                #region Окончание проверки
                // Пишем ошибку в ячейку
                EndOfCheckingEvent_WriteMsg(Cells, i, St.Cols9.WarningMessage, msg);

                // Перезаписываем ячейку с "ИНН/КПП" и фиксируем сообщение об ошибках
                EndOfCheckingEvent_RewriteInnKpp(Cells, i, St.Cols9.KontrInnKpp, St.Cols9.AgentInnKpp, KontrInnKpp, newKontrInnKpp, AgentInnKpp, newAgentInnKpp);

                // Перенумируем, если настройка стоит
                EndOfCheckingEvent_Renumber(Cells, i, St.Cols9.ListNum, renumValue, out renumValue);
                #endregion
            }

            Console.WriteLine("\r\nЗавершение процедуры проверки книги (9) Продаж");
        }

        /// <summary>
        /// Проверка книги (8) Покупок
        /// </summary>
        /// <param name="Cells"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public void CheckReadExcel_Body8(ExcelRange Cells, int rowCount, int colCount)
        {
            Console.WriteLine("Запуск процедуры проверки книги (8) Покупок");

            int renumValue = 0;
            string msg = "";
            string tmsg = "";
            string SellerInnKpp = "";
            string newSellerInnKpp = "";
            string AgentInnKpp = "";
            string newAgentInnKpp = "";
            string GTD = "";
            string newGTD = "";

            // Производим чтение
            for (int i = St.RowStart8; i <= rowCount; i++) //по всем строкам
            {
                Console.Write("\rСтрока {0} из {1} ({2}%)", i, rowCount, (i * 100 / rowCount));
                msg = "";

                //-------------------------------------------------------------------------------------
                // Проверка на наличие непечатных символов (возврат каретки, табуляция) - все ячейки
                CheckNonPrintableCharacter(Cells, i, colCount);

                // ИНН/КПП Продавца
                SellerInnKpp = Cells[i, St.Cols8.SellerInnKpp].Text;
                CheckDataInnKpp(SellerInnKpp, out tmsg, out newSellerInnKpp);
                msg += (msg != "") ? "; " + tmsg : tmsg;

                // ИНН/КПП Посредника (при наличии)
                AgentInnKpp = Cells[i, St.Cols8.AgentInnKpp].Text;
                newAgentInnKpp = AgentInnKpp;
                if (AgentInnKpp != "")
                {
                    CheckDataInnKpp(AgentInnKpp, out tmsg, out newAgentInnKpp);
                    msg += (msg != "") ? "; " + tmsg : tmsg;
                }

                // Номер ТД
                GTD = Cells[i, St.Cols8.NumberTD].Text;
                CheckDataGTD(GTD, out tmsg, out newGTD);
                msg += (msg != "") ? "; " + tmsg : tmsg;
                //-------------------------------------------------------------------------------------

                #region Окончание проверки
                // Пишем ошибку в ячейку
                EndOfCheckingEvent_WriteMsg(Cells, i, St.Cols8.WarningMessage, msg);

                // Перезаписываем ячейку с "ИНН/КПП" и фиксируем сообщение об ошибках
                EndOfCheckingEvent_RewriteInnKpp(Cells, i, St.Cols8.SellerInnKpp, St.Cols8.AgentInnKpp, SellerInnKpp, newSellerInnKpp, AgentInnKpp, newAgentInnKpp);

                // Перенумируем, если настройка стоит
                EndOfCheckingEvent_Renumber(Cells, i, St.Cols8.ListNum, renumValue, out renumValue);
                #endregion
            }

            Console.WriteLine("\r\nЗавершение процедуры проверки книги (8) Покупок");
        }

        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ЭКСПОРТ ДАННЫХ EXCEL В XML (ex2xml)



        #region ExcelToXml
        /// <summary>
        /// Экспортирование Excel в XML
        /// </summary>
        public void ExcelToXml()
        {
            StreamWriter w = SWCreate();
            try
            {
                //----------------------------------------------------------------------------------------------------------------------------

                Console.WriteLine("Открытие файла");
                FileStream stream = new FileStream(FilePathExcel, FileMode.Open);
                IExcelDataReader edr = (FilePathExcel.IndexOf(".xlsx") > 0)
                    ? ExcelReaderFactory.CreateOpenXmlReader(stream) // OpenXml Excel file (2007 format; *.xlsx)
                    : ExcelReaderFactory.CreateBinaryReader(stream); // Binary Excel file ('97-2003 format; *.xls)
                Console.WriteLine("Извлечение данных в память");
                DataSet result = edr.AsDataSet(); // Метод находится в NuGet: ExcelDataReader.DataSet
                Console.WriteLine("Определяем таблицу");
                DataTable edrTable = result.Tables[0];
                Console.WriteLine("Таблица определена, начало считывания...");
                Console.WriteLine("Обнаружено {0} строк и {1} столбцов данных", edrTable.Rows.Count, edrTable.Columns.Count);

                SWWriteHeaderOrFooter(w, true);
                switch (BookType)
                {
                    case 8:
                        XmlWriter_Body8(w, St.ConvertToListStr(edrTable), St.RowStart8);
                        break;

                    case 9:
                        XmlWriter_Body9(w, St.ConvertToListStr(edrTable), St.RowStart9);
                        break;

                    case 10:
                        XmlWriter_Body10(w, St.ConvertToListStr(edrTable), St.RowStart10);
                        break;

                    case 11:
                        XmlWriter_Body11(w, St.ConvertToListStr(edrTable), St.RowStart11);
                        break;

                    default:
                        break;
                }
                SWWriteHeaderOrFooter(w, false);

                edr.Close();

                //----------------------------------------------------------------------------------------------------------------------------
            }
            finally
            {
                w.Close();
            }

            Console.WriteLine("Окончание экспорта в XML");
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ЭКСПОРТ ДАННЫХ БАЗЫ ДАННЫХ В XML (dbfb2xml)



        #region DataBaseToXml
        /// <summary>
        /// Выгрузка DataBase в XML
        /// </summary>
        public void DataBaseToXml()
        {
            StreamWriter w = SWCreate();
            try
            {
                //----------------------------------------------------------------------------------------------------------------------------

                SWWriteHeaderOrFooter(w, true);
                switch (BookType)
                {
                    case 8:
                        XmlWriter_Body8(w, Fb.ExecPurchasesBook());
                        break;

                    case 9:
                        XmlWriter_Body9(w, Fb.ExecSalesBook());
                        break;

                    case 10:
                        XmlWriter_Body10(w, Fb.ExecOutgoingInvoices());
                        break;

                    case 11:
                        XmlWriter_Body11(w, Fb.ExecIncomingInvoices());
                        break;

                    default:
                        break;
                }
                SWWriteHeaderOrFooter(w, false);

                //----------------------------------------------------------------------------------------------------------------------------
            }
            finally
            {
                w.Close();
            }

            Console.WriteLine("Окончание выгрузки в XML");
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ЭКСПОРТ ДАННЫХ БАЗЫ ДАННЫХ В EXCEL (dbfb2ex)



        #region DataBaseToExcel
        public void DataBaseToExcel()
        {
            string templatesPath = St.TemplatesPath + St.BookFormat + "\\";
            string templateFileName = "";
            string fileFormat = ".xlsx";
            string newFilePath = "";

            try
            {
                templateFileName = templatesPath + "0000" + BookType.ToString().PadLeft(2, '0') + "0" + ".xltx";
                newFilePath = ExportPathExcel + GenerateExportFileName() + fileFormat;

                //----------------------------------------------------------------------------------------------
                // 1. Открываем шаблон

                Console.WriteLine("Открытие файла шаблона");
                var package = new ExcelPackage(new FileInfo(templateFileName));
                Console.WriteLine("Файл открыт, получение первого листа");
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Console.WriteLine("Первый лист получен, чтение диапозона данных");

                //----------------------------------------------------------------------------------------------
                // 2. Заполняем файл

                // ВНИМАНИЕ! Порядок колонок должен строго соответствовать Excel файлу из примеров СБИС
                List<string[]> FbData = null;
                int row = 0; // Управляющая переменная записи
                switch (BookType)
                {
                    case 8:
                        Console.WriteLine(">>> Запуск процедуры выгрузки в Excel тела книги (8) Покупок");
                        FbData = Fb.ExecPurchasesBook();
                        row = St.RowStart8;
                        break;

                    case 9:
                        Console.WriteLine(">>> Запуск процедуры выгрузки в Excel тела книги (9) Продаж");
                        FbData = Fb.ExecSalesBook();
                        row = St.RowStart9;
                        break;

                    case 10:
                        Console.WriteLine(">>> Запуск процедуры выгрузки в Excel тела журнала (10) Выставленных СФ");
                        FbData = Fb.ExecOutgoingInvoices();
                        row = St.RowStart10;
                        break;

                    case 11:
                        Console.WriteLine(">>> Запуск процедуры выгрузки в Excel тела журнала (11) Полученных СФ");
                        FbData = Fb.ExecIncomingInvoices();
                        row = St.RowStart11;
                        break;

                    default:
                        throw new Exception();
                }
                int rowCount = FbData.Count;
                // Дополняем строками начиная с row, дабы не нарушить шаблон (сохраняя данные подвала)
                workSheet.InsertRow(row, rowCount);

                // Производим запись в Excel по полученным данным
                for (int i = 0; i <= rowCount - 1; i++)
                {
                    Console.Write("\rСтрока {0} из {1} ({2}%)", (i + 1), rowCount, ((i + 1) * 100 / rowCount));
                    string[] strCells = FbData[i];
                    // Цикл по колонкам результата
                    for (int col = 1; col < strCells.Length; col++)
                        workSheet.Cells[row, col].Value = strCells[col];
                    row++;
                }

                Console.WriteLine("\r\n>>> Завершение процедуры выгрузки в XML");

                //----------------------------------------------------------------------------------------------
                // 3. Завершаем работу

                Console.WriteLine(">>> Файл Excel сформирован {0}", newFilePath);
                package.SaveAs(new FileInfo(newFilePath));
                workSheet.Dispose();
                package.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> Критическая ошибка. Excel не сформирован. " + ex.Message);
            }
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ВАЛИДАЦИЯ XML ПО XSD (vl)



        #region ValidateXmlForXsd
        public void ValidateXmlForXsd()
        {
            XmlValidate xml = new XmlValidate();
            xml.PathXml = FilePathXml;
            xml.PathXsd = FilePathXsd;
            xml.Validate();
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // СРАВНЕНИЕ XML ПО EXCEL (diff)


        #region DifferenceXmlAndExcel
        public void DifferenceXmlAndExcel()
        {
            List<AttributeClasses.атрибутыКнПокСтр> attr8Xml = new List<AttributeClasses.атрибутыКнПокСтр>();
            AttributeClasses.атрибутыКнПокСтр attr8XmlTotal = new AttributeClasses.атрибутыКнПокСтр();
            List<AttributeClasses.атрибутыКнПокСтр> attr8Excel = new List<AttributeClasses.атрибутыКнПокСтр>();
            AttributeClasses.атрибутыКнПокСтр attr8ExcelTotal = new AttributeClasses.атрибутыКнПокСтр();
            List<AttributeClasses.атрибутыКнПродСтр> attr9Xml = new List<AttributeClasses.атрибутыКнПродСтр>();
            AttributeClasses.атрибутыКнПродСтр attr9XmlTotal = new AttributeClasses.атрибутыКнПродСтр();
            List<AttributeClasses.атрибутыКнПродСтр> attr9Excel = new List<AttributeClasses.атрибутыКнПродСтр>();
            AttributeClasses.атрибутыКнПродСтр attr9ExcelTotal = new AttributeClasses.атрибутыКнПродСтр();

            #region Чтение >> Xml
            //FilePathXml = "C:\\Files\\NO_NDS.9_7802_7802_7719022542771501001_20190614_115530295.xml";
            Console.WriteLine("Открытие файла Xml");
            XmlTextReader reader = new XmlTextReader(FilePathXml);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // Узел является элементом.
                        if (reader.HasAttributes)
                        {
                            switch (BookType)
                            {
                                case 8:
                                    if (String.IsNullOrEmpty(reader.GetAttribute("НомСчФПрод")))
                                        break;
                                    attr8Xml.Add(new AttributeClasses.атрибутыКнПокСтр
                                    {
                                        НомСчФПрод = reader.GetAttribute("НомСчФПрод"),
                                        СтоимПокупВ = reader.GetAttribute("СтоимПокупВ"),
                                        СумНДСВыч = reader.GetAttribute("СумНДСВыч")
                                    });
                                    break;

                                case 9:
                                    if (String.IsNullOrEmpty(reader.GetAttribute("НомСчФПрод")))
                                        break;
                                    attr9Xml.Add(new AttributeClasses.атрибутыКнПродСтр
                                    {
                                        НомСчФПрод = reader.GetAttribute("НомСчФПрод"),
                                        СтоимПродСФВ = reader.GetAttribute("СтоимПродСФВ"),
                                        СтоимПродСФ = reader.GetAttribute("СтоимПродСФ"),
                                        СтоимПродСФ20 = reader.GetAttribute("СтоимПродСФ20"),
                                        СтоимПродСФ18 = reader.GetAttribute("СтоимПродСФ18"),
                                        СтоимПродСФ10 = reader.GetAttribute("СтоимПродСФ10"),
                                        СтоимПродСФ0 = reader.GetAttribute("СтоимПродСФ0"),
                                        СумНДССФ20 = reader.GetAttribute("СумНДССФ20"),
                                        СумНДССФ18 = reader.GetAttribute("СумНДССФ18"),
                                        СумНДССФ10 = reader.GetAttribute("СумНДССФ10"),
                                        СтоимПродОсв = reader.GetAttribute("СтоимПродОсв")
                                    });
                                    break;
                            }
                        }
                        break;
                }
            }
            #endregion

            //-----------------------------------------------------------------------------------------------------------

            #region Чтение >> Excel
            //FilePathExcel = "C:\\Files\\1.2019-1\\0000090.xlsx";
            // Читаем данные из файла
            Console.WriteLine("Открытие файла Excel");
            FileStream stream = new FileStream(FilePathExcel, FileMode.Open);
            IExcelDataReader edr = (FilePathExcel.IndexOf(".xlsx") > 0)
                ? ExcelReaderFactory.CreateOpenXmlReader(stream) // OpenXml Excel file (2007 format; *.xlsx)
                : ExcelReaderFactory.CreateBinaryReader(stream); // Binary Excel file ('97-2003 format; *.xls)
            DataSet result = edr.AsDataSet(); // Метод находится в NuGet: ExcelDataReader.DataSet
            DataTable edrTable = result.Tables[0];
            List<string[]> data = St.ConvertToListStr(edrTable);

            int iStart = 0;
            switch (BookType)
            {
                case 8:
                    iStart = St.RowStart8;
                    break;

                case 9:
                    iStart = St.RowStart9;
                    break;
            }

            iStart = (iStart > 0) ? iStart - 1 : iStart;
            int j = 0;
            for (int i = iStart; i < data.Count; i++)
            {
                string[] Cells = data[i];
                string[] temp = new string[2];
                switch (BookType)
                {
                    case 8:
                        attr8Excel.Add(new AttributeClasses.атрибутыКнПокСтр
                        {
                            НомСчФПрод = St.SFormat(St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols8.SellerNumAndDateSf]), ";")[0]),
                            СтоимПокупВ = Cells[St.Cols8.CostPaymentOfSf],
                            СумНДСВыч = Cells[St.Cols8.SumNdsOfSf]
                        });

                        break;

                    case 9:
                        //attr9Excel.Add(new AttributeClasses.атрибутыКнПродСтр
                        //{
                        //    НомСчФПрод = St.SFormat(St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols9.SellerNumAndDateSf]), ";")[0]),
                        //    СтоимПродСФ20 = Cells[St.Cols9.CostRubKopWithoutNDS20],
                        //    СтоимПродСФ18 = Cells[St.Cols9.CostRubKopWithoutNDS18],
                        //    СтоимПродСФ10 = Cells[St.Cols9.CostRubKopWithoutNDS10],
                        //    СтоимПродСФ0 = Cells[St.Cols9.CostRubKopWithoutNDS0],
                        //    СумНДССФ20 = Cells[St.Cols9.SumNDS20],
                        //    СумНДССФ18 = Cells[St.Cols9.SumNDS18],
                        //    СумНДССФ10 = Cells[St.Cols9.SumNDS10],
                        //    СтоимПродОсв = Cells[St.Cols9.CostSalesWithoutNDS]
                        //});

                        AttributeClasses.атрибутыКнПродСтр attr9 = new AttributeClasses.атрибутыКнПродСтр
                        {
                            НомСчФПрод = St.SFormat(St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols9.SellerNumAndDateSf]), ";")[0]),
                            СтоимПродСФ20 = Cells[St.Cols9.CostRubKopWithoutNDS20],
                            СтоимПродСФ18 = Cells[St.Cols9.CostRubKopWithoutNDS18],
                            СтоимПродСФ10 = Cells[St.Cols9.CostRubKopWithoutNDS10],
                            СтоимПродСФ0 = Cells[St.Cols9.CostRubKopWithoutNDS0],
                            СумНДССФ20 = Cells[St.Cols9.SumNDS20],
                            СумНДССФ18 = Cells[St.Cols9.SumNDS18],
                            СумНДССФ10 = Cells[St.Cols9.SumNDS10],
                            СтоимПродОсв = Cells[St.Cols9.CostSalesWithoutNDS]
                        };
                        if (attr9Xml[j].НомСчФПрод == attr9.НомСчФПрод)
                        {
                            ConsoleWriteDiffResult("СтоимПродСФ20 - " + attr9.НомСчФПрод, attr9Xml[j].СтоимПродСФ20, attr9.СтоимПродСФ20, false);
                            j++;
                            break;
                        }
                        break;
                }
            }
            #endregion

            //-----------------------------------------------------------------------------------------------------------

            #region
            //switch (BookType)
            //{
            //    case 8:
            //        if (attr8Xml.Count != attr8Excel.Count)
            //        {
            //            Console.WriteLine("Не совпадает количество документов!");
            //            return;
            //        }
            //        for (int i = 0; i < attr8Xml.Count; i++)
            //        {
            //            attr8XmlTotal.СтоимПокупВ = (St.GetDecFromString(attr8XmlTotal.СтоимПокупВ) + St.GetDecFromString(attr8Xml[i].СтоимПокупВ)).ToString();
            //            attr8XmlTotal.СумНДСВыч = (St.GetDecFromString(attr8XmlTotal.СумНДСВыч) + St.GetDecFromString(attr8Xml[i].СумНДСВыч)).ToString();

            //            attr8ExcelTotal.СтоимПокупВ = (St.GetDecFromString(attr8ExcelTotal.СтоимПокупВ) + St.GetDecFromString(attr8Excel[i].СтоимПокупВ)).ToString();
            //            attr8ExcelTotal.СумНДСВыч = (St.GetDecFromString(attr8ExcelTotal.СумНДСВыч) + St.GetDecFromString(attr8Excel[i].СумНДСВыч)).ToString();
            //        }
            //        ConsoleWriteDiffResult("покупкам", attr8XmlTotal.СтоимПокупВ, attr8ExcelTotal.СтоимПокупВ);
            //        ConsoleWriteDiffResult("НДС", attr8XmlTotal.СумНДСВыч, attr8ExcelTotal.СумНДСВыч);
            //        break;

            //    case 9:
            //        if (attr9Xml.Count != attr9Excel.Count)
            //        {
            //            Console.WriteLine("Не совпадает количество документов!");
            //            return;
            //        }
            //        /*for (int i = 0; i < attr9Xml.Count; i++)
            //        {
            //            attr9XmlTotal.СтоимПродСФ20 = (St.GetDecFromString(attr9XmlTotal.СтоимПродСФ20) + St.GetDecFromString(attr9Xml[i].СтоимПродСФ20)).ToString();
            //            attr9XmlTotal.СтоимПродСФ18 = (St.GetDecFromString(attr9XmlTotal.СтоимПродСФ18) + St.GetDecFromString(attr9Xml[i].СтоимПродСФ18)).ToString();
            //            attr9XmlTotal.СтоимПродСФ10 = (St.GetDecFromString(attr9XmlTotal.СтоимПродСФ10) + St.GetDecFromString(attr9Xml[i].СтоимПродСФ10)).ToString();
            //            attr9XmlTotal.СтоимПродСФ0 = (St.GetDecFromString(attr9XmlTotal.СтоимПродСФ0) + St.GetDecFromString(attr9Xml[i].СтоимПродСФ0)).ToString();
            //            attr9XmlTotal.СумНДССФ20 = (St.GetDecFromString(attr9XmlTotal.СумНДССФ20) + St.GetDecFromString(attr9Xml[i].СумНДССФ20)).ToString();
            //            attr9XmlTotal.СумНДССФ18 = (St.GetDecFromString(attr9XmlTotal.СумНДССФ18) + St.GetDecFromString(attr9Xml[i].СумНДССФ18)).ToString();
            //            attr9XmlTotal.СумНДССФ10 = (St.GetDecFromString(attr9XmlTotal.СумНДССФ10) + St.GetDecFromString(attr9Xml[i].СумНДССФ10)).ToString();
            //            attr9XmlTotal.СтоимПродОсв = (St.GetDecFromString(attr9XmlTotal.СтоимПродОсв) + St.GetDecFromString(attr9Xml[i].СтоимПродОсв)).ToString();

            //            attr9ExcelTotal.СтоимПродСФ20 = (St.GetDecFromString(attr9ExcelTotal.СтоимПродСФ20) + St.GetDecFromString(attr9Excel[i].СтоимПродСФ20)).ToString();
            //            attr9ExcelTotal.СтоимПродСФ18 = (St.GetDecFromString(attr9ExcelTotal.СтоимПродСФ18) + St.GetDecFromString(attr9Excel[i].СтоимПродСФ18)).ToString();
            //            attr9ExcelTotal.СтоимПродСФ10 = (St.GetDecFromString(attr9ExcelTotal.СтоимПродСФ10) + St.GetDecFromString(attr9Excel[i].СтоимПродСФ10)).ToString();
            //            attr9ExcelTotal.СтоимПродСФ0 = (St.GetDecFromString(attr9ExcelTotal.СтоимПродСФ0) + St.GetDecFromString(attr9Excel[i].СтоимПродСФ0)).ToString();
            //            attr9ExcelTotal.СумНДССФ20 = (St.GetDecFromString(attr9ExcelTotal.СумНДССФ20) + St.GetDecFromString(attr9Excel[i].СумНДССФ20)).ToString();
            //            attr9ExcelTotal.СумНДССФ18 = (St.GetDecFromString(attr9ExcelTotal.СумНДССФ18) + St.GetDecFromString(attr9Excel[i].СумНДССФ18)).ToString();
            //            attr9ExcelTotal.СумНДССФ10 = (St.GetDecFromString(attr9ExcelTotal.СумНДССФ10) + St.GetDecFromString(attr9Excel[i].СумНДССФ10)).ToString();
            //            attr9ExcelTotal.СтоимПродОсв = (St.GetDecFromString(attr9ExcelTotal.СтоимПродОсв) + St.GetDecFromString(attr9Excel[i].СтоимПродОсв)).ToString();
            //        }
            //        ConsoleWriteDiffResult("покупкам 20%", attr9XmlTotal.СтоимПродСФ20, attr9ExcelTotal.СтоимПродСФ20);
            //        ConsoleWriteDiffResult("покупкам 18%", attr9XmlTotal.СтоимПродСФ18, attr9ExcelTotal.СтоимПродСФ18);
            //        ConsoleWriteDiffResult("покупкам 10%", attr9XmlTotal.СтоимПродСФ10, attr9ExcelTotal.СтоимПродСФ10);
            //        ConsoleWriteDiffResult("покупкам 0%", attr9XmlTotal.СтоимПродСФ0, attr9ExcelTotal.СтоимПродСФ0);
            //        ConsoleWriteDiffResult("НДС 20%", attr9XmlTotal.СумНДССФ20, attr9ExcelTotal.СумНДССФ20);
            //        ConsoleWriteDiffResult("НДС 18%", attr9XmlTotal.СумНДССФ18, attr9ExcelTotal.СумНДССФ18);
            //        ConsoleWriteDiffResult("НДС 10%", attr9XmlTotal.СумНДССФ10, attr9ExcelTotal.СумНДССФ10);
            //        ConsoleWriteDiffResult("БЕЗ НДС%", attr9XmlTotal.СтоимПродОсв, attr9ExcelTotal.СтоимПродОсв);*/
            //        for (int i = 0; i < attr9Xml.Count; i++)
            //        {
            //            ConsoleWriteDiffResult(("СтоимПродСФ20 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СтоимПродСФ20, attr9ExcelTotal.СтоимПродСФ20, false);
            //            ConsoleWriteDiffResult(("СтоимПродСФ18 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СтоимПродСФ18, attr9ExcelTotal.СтоимПродСФ18, false);
            //            ConsoleWriteDiffResult(("СтоимПродСФ10 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СтоимПродСФ10, attr9ExcelTotal.СтоимПродСФ10, false);
            //            ConsoleWriteDiffResult(("СтоимПродСФ0 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СтоимПродСФ0, attr9ExcelTotal.СтоимПродСФ0, false);
            //            ConsoleWriteDiffResult(("СумНДССФ20 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СумНДССФ20, attr9ExcelTotal.СумНДССФ20, false);
            //            ConsoleWriteDiffResult(("СумНДССФ18 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СумНДССФ18, attr9ExcelTotal.СумНДССФ18, false);
            //            ConsoleWriteDiffResult(("СумНДССФ10 - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СумНДССФ10, attr9ExcelTotal.СумНДССФ10, false);
            //            ConsoleWriteDiffResult(("СтоимПродОсв - " + attr9XmlTotal.НомСчФПрод), attr9XmlTotal.СтоимПродОсв, attr9ExcelTotal.СтоимПродОсв, false);
            //        }
            //        break;
            //}
            #endregion
        }

        private void ConsoleWriteDiffResult(string h, string xmlSumm, string excelSumm, bool isNeedSuccess = true)
        {
            xmlSumm = String.IsNullOrEmpty(xmlSumm) ? "0" : xmlSumm;
            xmlSumm = xmlSumm.Replace(",", ".").Replace(" ", "");
            excelSumm = String.IsNullOrEmpty(excelSumm) ? "0" : excelSumm;
            excelSumm = excelSumm.Replace(",", ".").Replace(" ", "");
            if (xmlSumm != excelSumm)
                Console.WriteLine("Есть расхождения по {0}. Excel: {1} руб., Xml: {2} руб.", h, excelSumm, xmlSumm);
            else
                if (isNeedSuccess)
                Console.WriteLine("По {0} расхождений нет.", h);
        }
        #endregion

        #region XmlCheck
        public void XmlCheckSum()
        {
            double b8_СтоимПокупВ = 0;
            double b8_СумНДСВыч = 0;

            double b9_СтоимПродСФВ = 0;
            double b9_СтоимПродСФ = 0;
            double b9_СтоимПродСФ20 = 0;
            double b9_СтоимПродСФ18 = 0;
            double b9_СтоимПродСФ10 = 0;
            double b9_СтоимПродСФ0 = 0;
            double b9_СумНДССФ20 = 0;
            double b9_СумНДССФ18 = 0;
            double b9_СумНДССФ10 = 0;
            double b9_СтоимПродОсв = 0;

            Console.WriteLine("Открытие файла Xml");
            XmlTextReader reader = new XmlTextReader(FilePathXml);
            Console.WriteLine(">> Расчет...");

            int count = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                {
                    switch (BookType)
                    {
                        case 8:
                            if (String.IsNullOrEmpty(reader.GetAttribute("НомСчФПрод")))
                                continue;

                            b8_СтоимПокупВ += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПокупВ"));
                            b8_СумНДСВыч   += XmlCheckSum_DecSum(reader.GetAttribute("СумНДСВыч"));
                            break;

                        case 9:
                            if (String.IsNullOrEmpty(reader.GetAttribute("НомСчФПрод")))
                                continue;

                            b9_СтоимПродСФВ  += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродСФВ"));
                            b9_СтоимПродСФ   += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродСФ"));
                            b9_СтоимПродСФ20 += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродСФ20"));
                            b9_СтоимПродСФ18 += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродСФ18"));
                            b9_СтоимПродСФ10 += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродСФ10"));
                            b9_СтоимПродСФ0  += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродСФ0"));
                            b9_СумНДССФ20    += XmlCheckSum_DecSum(reader.GetAttribute("СумНДССФ20"));
                            b9_СумНДССФ18    += XmlCheckSum_DecSum(reader.GetAttribute("СумНДССФ18"));
                            b9_СумНДССФ10    += XmlCheckSum_DecSum(reader.GetAttribute("СумНДССФ10"));
                            b9_СтоимПродОсв  += XmlCheckSum_DecSum(reader.GetAttribute("СтоимПродОсв"));
                            break;
                    }

                    count++;
                    Console.Write("\rСчитано данных: {0}", count);
                }
            }

            // Выводим результат
            Console.WriteLine(">> Расчет окончен");
            Console.WriteLine("Общие суммы по файлу: \n\r");
            switch (BookType)
            {
                case 8:
                    Console.WriteLine("\tСтоимПокупВ: {0}", b8_СтоимПокупВ.ToString("C"));
                    Console.WriteLine("\tСумНДСВыч:   {0}", b8_СумНДСВыч.ToString("C"));
                    break;

                case 9:
                    Console.WriteLine("\tСтоимПродСФВ:  {0}", XmlCheckSum_PrintResult(b9_СтоимПродСФВ));
                    Console.WriteLine("\tСтоимПродСФ:   {0}", XmlCheckSum_PrintResult(b9_СтоимПродСФ));
                    Console.WriteLine("\tСтоимПродСФ20: {0}", XmlCheckSum_PrintResult(b9_СтоимПродСФ20));
                    Console.WriteLine("\tСтоимПродСФ18: {0}", XmlCheckSum_PrintResult(b9_СтоимПродСФ18));
                    Console.WriteLine("\tСтоимПродСФ10: {0}", XmlCheckSum_PrintResult(b9_СтоимПродСФ10));
                    Console.WriteLine("\tСтоимПродСФ0:  {0}", XmlCheckSum_PrintResult(b9_СтоимПродСФ0));
                    Console.WriteLine("\tСумНДССФ20:    {0}", XmlCheckSum_PrintResult(b9_СумНДССФ20));
                    Console.WriteLine("\tСумНДССФ18:    {0}", XmlCheckSum_PrintResult(b9_СумНДССФ18));
                    Console.WriteLine("\tСумНДССФ10:    {0}", XmlCheckSum_PrintResult(b9_СумНДССФ10));
                    Console.WriteLine("\tСтоимПродОсв:  {0}", XmlCheckSum_PrintResult(b9_СтоимПродОсв));
                    break;
            }
        }

        /// <summary>
        /// Сложение двух вещественных чисел в строковом типе
        /// </summary>
        /// <param name="s">Число строкой</param>
        /// <returns>Результат</returns>
        private double XmlCheckSum_DecSum(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;
            else
                s = s.Replace(".", ",").Replace(" ", "");
            if (!Char.IsNumber(s[0]) && ((s[0] == '-') && (s.Length == 1)))
                return 0;

            try
            {
                return Double.Parse(s);
            }
            catch { return 0; }

            //double dec = 0;
            //try { dec = Double.Parse(s.Replace(".", ",").Replace(" ", "")); } catch { dec = 0; }
            //return dec;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private string XmlCheckSum_PrintResult(double d)
        {
            string s = d.ToString("C");
            s = s.Substring(0, s.Length - 2);
            s = s.Replace(" ", ".");
            return s;
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ИМПОРТ ДАННЫХ В БАЗУ ДАННЫХ SQLSERVER ИЗ EXCEL (ex2dbss)



        #region ImportDataFromExcelToDataBase
        public void ImportDataFromExcelToDataBase()
        {
            // Читаем данные из файла
            Console.WriteLine("Открытие файла");
            FileStream stream = new FileStream(FilePathExcel, FileMode.Open);
            IExcelDataReader edr = (FilePathExcel.IndexOf(".xlsx") > 0)
                ? ExcelReaderFactory.CreateOpenXmlReader(stream) // OpenXml Excel file (2007 format; *.xlsx)
                : ExcelReaderFactory.CreateBinaryReader(stream); // Binary Excel file ('97-2003 format; *.xls)
            Console.WriteLine("Извлечение данных в память");
            DataSet result = edr.AsDataSet(); // Метод находится в NuGet: ExcelDataReader.DataSet
            Console.WriteLine("Определяем таблицу");
            DataTable edrTable = result.Tables[0];
            Console.WriteLine("Таблица определена, начало считывания...");
            Console.WriteLine("Обнаружено {0} строк и {1} столбцов данных", edrTable.Rows.Count, edrTable.Columns.Count);
            List<string[]> data = St.ConvertToListStr(edrTable);

            int rowStart = 0;
            switch (BookType)
            {
                case 8:
                    rowStart = St.RowStart8;
                    break;

                case 9:
                    rowStart = St.RowStart9;
                    break;

                case 10:
                    rowStart = St.RowStart10;
                    break;

                case 11:
                    rowStart = St.RowStart11;
                    break;
            }

            // Загружаем данные
            Ss.LoadDataToTables(BookType, data, rowStart);
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ИМПОРТ ДАННЫХ В БАЗУ ДАННЫХ SQLSERVER ИЗ БАЗЫ ДАННЫХ FIREBIRD (dbfb2dbss)



        #region ImportDataFromFirebirdToSQLServer
        public void ImportDataFromFirebirdToSQLServer()
        {
            // Получаем данные из Firebird
            List<string[]> data = new List<string[]>();
            Ss.FileFormatId = Ss.GetCurrentFileFormatId();

            // Пишем в базу SQLServer
            switch (BookType)
            {
                case 8:
                    data = Fb.ExecPurchasesBook();
                    break;

                case 9:
                    data = Fb.ExecSalesBook();
                    break;

                case 10:
                    data = Fb.ExecOutgoingInvoices();
                    break;

                case 11:
                    data = Fb.ExecIncomingInvoices();
                    break;
            }

            // Загружаем данные
            Ss.LoadDataToTables(BookType, data, 0);
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ И ПРОЦЕДУРЫ



        #region ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ И ПРОЦЕДУРЫ
        /// <summary>
        /// Генерирование имени файла для обмена
        /// </summary>
        /// <returns></returns>
        private string GenerateExportFileName()
        {
            /*
                Приказ ФНС России от 29.10.2014 N ММВ-7-3/558@ (ред. от 20.12.2016)     
                
                Имя файла обмена должно иметь следующий вид:
                R_T_A_K_O_GGGGMMDD_N, где:
                [Книга Покупок]
                R_T - префикс, принимающий значение NO_NDS.8;
                [Книга Продаж]
                R_T - префикс, принимающий значение NO_NDS.9;
                A_K - идентификатор получателя информации, где: A - идентификатор получателя, которому направляется файл обмена, K - идентификатор конечного получателя, для которого предназначена информация из данного файла обмена <1>. Каждый из идентификаторов (A и K) имеет вид для налоговых органов - четырехразрядный код (код налогового органа в соответствии с классификатором "Система обозначения налоговых органов" (СОНО);
                --------------------------------
                <1> Передача файла от отправителя к конечному получателю (K) может осуществляться в несколько этапов через другие налоговые органы, осуществляющие передачу файла на промежуточных этапах, которые обозначаются идентификатором A. В случае передачи файла от отправителя к конечному получателю при отсутствии налоговых органов, осуществляющих передачу на промежуточных этапах, значения идентификаторов A и K совпадают. 
                O - идентификатор отправителя информации, имеет вид:
                для организаций - девятнадцатиразрядный код (идентификационный номер налогоплательщика (далее - ИНН) и код причины постановки на учет (далее - КПП) организации (обособленного подразделения);
                для физических лиц - двенадцатиразрядный код (ИНН физического лица при наличии. При отсутствии ИНН - последовательность из двенадцати нулей);
                GGGG - год формирования передаваемого файла, MM - месяц, DD - день;
                N - идентификационный номер файла. (Длина - от 1 до 36 знаков. Идентификационный номер файла должен обеспечивать уникальность файла).
                Расширение имени файла - xml. Расширение имени файла может указываться как строчными, так и прописными буквами.
            */

            string R_T_ = "NO_NDS." + BookType.ToString() + "_";
            string A_K_ = "7802_7802_";
            string O_ = "7719022542771501001_";

            string MM = DateTime.Now.Month.ToString();
            if (MM.Length == 1) MM = "0" + MM;
            string DD = DateTime.Now.Day.ToString();
            if (DD.Length == 1) DD = "0" + DD;
            string GGGGMMDD_ = DateTime.Now.Year.ToString() + MM + DD + "_";
            string N = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

#if DEBUG
            //N += "_test";
#endif

            return R_T_ + A_K_ + O_ + GGGGMMDD_ + N;
        }

        /// <summary>
        /// Создание пишушего потока в файл Xml
        /// </summary>
        /// <returns></returns>
        private StreamWriter SWCreate()
        {
            Console.WriteLine("Начало выгрузки в XML");
            // Создаем текстовый файл и потоком пишем в него Xml разметку без сериализации (так быстрее)
            GenerateFileName = GenerateExportFileName();
            string PathWithName = ExportPathXml + GenerateFileName + ".xml";
            Console.WriteLine("Директория выходного файла: {0}", PathWithName);
            StreamWriter w = new StreamWriter(PathWithName, false, Encoding.GetEncoding("Windows-1251"));
            w.WriteLine("<?xml version=\"1.0\" encoding=\"windows-1251\" ?>");
            return w;
        }

        /// <summary>
        /// Запись СвПос/СвПрод/СвПокуп по шаблону в файл xml
        /// </summary>
        /// <param name="w">Поток записи</param>
        /// <param name="tag">СвПос/СвПрод/СвПокуп или др.</param>
        /// <param name="s">ИНН/КПП - будет разложено автоматически</param>
        private void SWWriteInformation(StreamWriter w, string tag, string s)
        {
            // Подготовка и проверка данных
            string msg = "";
            CheckDataInnKpp(s, out msg, out s);
            //!!! По хорошему вывалить Exception
            //if (msg != "") return;

            // Запись данных
            string[] temp = St.SeparationTwo(s);
            w.WriteLine("               <" + tag + ">");
            //if (temp[1] != "") отключено
            if (temp[0].Length == 10)
                w.WriteLine("                   <СведЮЛ ИННЮЛ=\"" + temp[0] + "\" КПП=\"" + temp[1] + "\"></СведЮЛ>");
            else
                w.WriteLine("                   <СведИП ИННФЛ=\"" + temp[0] + "\"></СведИП>");
            w.WriteLine("               </" + tag + ">");
        }

        /// <summary>
        /// Запись Упл/Опл
        /// </summary>
        /// <param name="w"></param>
        /// <param name="tag"></param>
        /// <param name="s"></param>
        private void SWWriteDocSuccess(StreamWriter w, string tag, string s)
        {
            // !!! Внимание: передается перечисление, знак разделителя между "номер;дата" является ","
            // Запись данных
            string[] sepr = St.SeparationMulty(s, ",");
            int cnt = sepr.Length;
            for (int i = 0; i < cnt; i++)
            {
                string[] temp = St.SeparationTwo(sepr[i], ";");
                if (String.IsNullOrEmpty(temp[1]))
                    temp = St.SeparationTwo(sepr[i], " от ");
                w.Write("               <ДокПдтв" + tag + " ");
                // Перечисляем аргументы
                w.Write("НомДокПдтв" + tag + "=\"" + temp[0] + "\" ");
                w.Write("ДатаДокПдтв" + tag + "=\"" + temp[1] + "\"");
                w.WriteLine("></ДокПдтв" + tag + ">");
            }
        }

        /// <summary>
        /// РегНомТД
        /// </summary>
        /// <param name="w"></param>
        /// <param name="tag"></param>
        /// <param name="s"></param>
        private void SWWriteNumberTD(StreamWriter w, string s)
        {
            // !!! Внимание: передается перечисление, знак разделителя между "номер;дата" является ","
            // Запись данных
            string[] sepr = St.SeparationMulty(s, ";");
            int cnt = sepr.Length;
            for (int i = 0; i < cnt; i++)
            {
                if (St.BookFormat == "5.08")
                {
                    w.WriteLine("               <СвРегНом РегНомПросл=\"" + sepr[i] + "\"></СвРегНом>");
                }
                else
                {
                    w.WriteLine("               <РегНомТД>" + sepr[i] + "</РегНомТД>");
                }
            }
        }

        /// <summary>
        /// Вывод шапки и подвала документа XML
        /// </summary>
        /// <param name="w"></param>
        /// <param name="onlyHeader">Выводить только шапку</param>
        private void SWWriteHeaderOrFooter(StreamWriter w, bool onlyHeader)
        {
            if (onlyHeader)
            {
                w.WriteLine("<Файл ВерсПрог=\"1\" ИдФайл=\"" + GenerateFileName + "\" ВерсФорм=\"" + St.BookFormat + "\" >");
                w.WriteLine("   <Документ НомКорр=\"" + NumberKorr.ToString() + "\" Индекс=\"0000" + BookType.ToString().PadLeft(2, '0') + "0\" >");
            }
            else
            {
                w.WriteLine("   </Документ>");
                w.WriteLine("</Файл>");
            }
        }

        /// <summary>
        /// Проверка данных ИНН и КПП
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="msg"></param>
        /// <param name="newValue"></param>
        private void CheckDataInnKpp(string oldValue, out string msg, out string newValue)
        {
            msg = "";
            oldValue = oldValue.Replace(" ", "").Replace("\t", "");
            newValue = oldValue;

            if (oldValue == "")
            {
                msg = "Не указаны ИНН и КПП";
            }
            else
            {
                string[] arr = St.SeparationTwo(oldValue); // ИНН и КПП
                switch (arr[0].Length)
                {
                    //Если 10 – это юр. лицо, проверяем длину КПП(=9)
                    case 10:
                        if (arr[1].Length != 9) msg = "Значение КПП введено не верно";
                        break;

                    //Если 12 – это ИП, КПП не проверяем(отбрасываем из отчета)
                    case 12:
                        arr[1] = "";
                        newValue = arr[0];
                        break;

                    default:
                        msg = "Значение ИНН введено не верно";
                        break;
                }
            }
        }

        /// <summary>
        /// Проверка данных ГТД
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="msg"></param>
        /// <param name="newValue"></param>
        private void CheckDataGTD(string oldValue, out string msg, out string newValue)
        {
            msg = "";
            newValue = oldValue;

            if (oldValue == "")
            {
                msg = "Не указан номер таможенной декларации";
            }
            else
            {
                // Маска ГТД: "XXXXXXXX/XXXXXX/XXXXXXX/XX" - "/XX" не является обязательным
                // Пример: "10714040/140917/0090376/12"
                // Regex: ^((\;)?([0-9]{8}\/[0-9]{6}\/[0-9]{7}(\/[0-9]{1,2})?))*$
                // !!! Присутствуют символы экранирования C# 
                // !!! Номер ГТД может быть перечислен ";"
                Regex x = new Regex("^((\\;)?([0-9]{8}\\/[0-9]{6}\\/[0-9]{7}(\\/[0-9]{1,2})?))*$");
                if (!x.IsMatch(oldValue))
                    msg = "Не верно заполнен номер ТД";
            }
        }

        /// <summary>
        /// Проверка на непечатаемые символы
        /// </summary>
        /// <param name="s">Исходная строка</param>
        /// <param name="res">Результат</param>
        /// <param name="equal">Сравнение результата с исходной строкой</param>
        private void CheckCellNonPrintCharacter(string s, out string res, out bool equal)
        {
            res = Regex.Replace(s, @"\p{C}+", string.Empty);
            equal = s.Equals(res);
        }

        /// <summary>
        /// Пишем ошибку в ячейку
        /// </summary>
        /// <param name="Cells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="msg"></param>
        private void EndOfCheckingEvent_WriteMsg(ExcelRange Cells, int row, int col, string msg)
        {
            if (msg != "")
            {
                Console.WriteLine("   {0}", msg);
                if (St.IsWriteMsg)
                    Cells[row, col].Value = msg;
            }
        }

        /// <summary>
        /// Перезаписываем ячейку с "ИНН/КПП" и фиксируем сообщение об ошибках
        /// </summary>
        /// <param name="Cells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="SellerInnKpp"></param>
        /// <param name="newSellerInnKpp"></param>
        /// <param name="AgentInnKpp"></param>
        /// <param name="newAgentInnKpp"></param>
        private void EndOfCheckingEvent_RewriteInnKpp(ExcelRange Cells, int row, int colGeneral, int colAgent, string GeneralInnKpp, string newGeneralInnKpp, string AgentInnKpp, string newAgentInnKpp)
        {
            if (St.IsRewriteInnKpp)
            {
                // Продавца / Контрагента
                if (newGeneralInnKpp != GeneralInnKpp)
                    Cells[row, colGeneral].Value = newGeneralInnKpp;

                // Агента (посредника)
                if (newAgentInnKpp != AgentInnKpp)
                    Cells[row, colAgent].Value = newAgentInnKpp;
            }
        }

        /// <summary>
        /// Перенумируем, если стоит настройка
        /// </summary>
        /// <param name="Cells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="renumValue"></param>
        /// <param name="newRenumValue"></param>
        private void EndOfCheckingEvent_Renumber(ExcelRange Cells, int row, int col, int renumValue, out int newRenumValue)
        {
            if (St.IsRenumber)
            {
                renumValue++;
                Cells[row, col].Value = renumValue;
            }
            newRenumValue = renumValue;
        }

        /// <summary>
        /// Проверка на наличие непечатных символов (возврат каретки, табуляция) - все ячейки
        /// </summary>
        /// <param name="Cells"></param>
        /// <param name="row"></param>
        /// <param name="colCount"></param>
        private void CheckNonPrintableCharacter(ExcelRange Cells, int row, int colCount)
        {
            string defString = "";
            string beautyString = "";
            bool isEqual = true;

            for (int j = 1; j <= colCount; j++)
            {
                defString = Cells[row, j].Text;
                if (String.IsNullOrEmpty(defString)) continue;
                CheckCellNonPrintCharacter(defString, out beautyString, out isEqual);
                if (!isEqual)
                    Cells[row, St.Cols9.WarningMessage].Value = beautyString;
            }
        }

        /// <summary>
        /// Пересохраняет FilePath в формат Excel 12 (*.xlsx), если это требуется, иначе возвращает FilePath
        /// </summary>
        /// <returns>FilePath для EPPlus</returns>
        private string EPPlus_ResaveToExcel12()
        {
            string newFilePath = FilePathExcel;
            try
            {
                if (Path.GetExtension(FilePathExcel) == ".xlsx")
                {
                    // Файл не нуждается в пересохранении в формат Excel 12
                    return FilePathExcel;
                }

                Excel.Application ObjWorkExcel = new Excel.Application();
                Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(newFilePath, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
                Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист

                newFilePath = FilePathExcel.Substring(0, FilePathExcel.IndexOf(".xls"));// + "_epplus.xlsx";
                ObjWorkBook.SaveAs(newFilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, Type.Missing,
                                    Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                                    Excel.XlSaveConflictResolution.xlLocalSessionChanges, true,
                                    Type.Missing, Type.Missing, Type.Missing);

                ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть сохраняя
                ObjWorkExcel.Quit(); // выйти из экселя
                GC.Collect(); // убрать за собой

                Console.WriteLine(">>> Файл Excel пересохранен для проверки в формат Excel 12 (*.xlsx)");
            }
            catch
            {
                newFilePath = FilePathExcel;
            }

            return newFilePath;
        }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // УНИВЕРСАЛЬНЫЕ ФУНКЦИИ И ПРОЦЕДУРЫ


        #region УНИВЕРСАЛЬНЫЕ ФУНКЦИИ И ПРОЦЕДУРЫ
        /// <summary>
        /// Запись в XML тела книги (8) Покупок
        /// </summary>
        /// <param name="w"></param>
        /// <param name="Data"></param>
        /// <param name="iStart"></param>
        private void XmlWriter_Body8(StreamWriter w, List<string[]> Data, int iStart = 0)
        {
            string field = "";

            Console.WriteLine(">>> Запуск процедуры выгрузки в XML тела книги (8) Покупок");
            w.WriteLine("       <КнигаПокуп СумНДСВсКПк=\"0\" >"); 

            Console.WriteLine("Чтение данных начато с {0} строки", iStart);
            int renum = 0;
            int rowCount = Data.Count;
            iStart = (iStart > 0) ? iStart-1 : iStart;
            for (int i = iStart; i < rowCount; i++)
            {
                Console.Write("\rСтрока {0} из {1} ({2}%)", (i + 1), rowCount, ((i + 1) * 100 / rowCount));
                string[] Cells = Data[i];
                string[] temp = new string[2];

                // При КОД ОПЕРАЦИИ 18 - должны быть заполнены графы 3 и 5
                if (Cells[St.Cols8.OperTypeCode].Trim() == "18")
                {
                    string DataCol3 = Cells[St.Cols8.SellerNumAndDateSf];
                    string DataCol5 = Cells[St.Cols8.SellerNumAndDateSfKor];

                    if (DataCol3 == "" && DataCol5 != "")
                        Cells[St.Cols8.SellerNumAndDateSf] = DataCol5;
                    if (DataCol3 != "" && DataCol5 == "")
                        Cells[St.Cols8.SellerNumAndDateSfKor] = DataCol3;
                }
                renum++;

                //------------------------------------------------------------
                #region Заполняем список атрибутов
                AttributeClasses.атрибутыКнПокСтр attr = new AttributeClasses.атрибутыКнПокСтр();
                attr.НомерПор = renum.ToString();

                // Разделитель ";". Пример: 47815/178-18;26.10.2018
                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols8.SellerNumAndDateSf]), ";");
                attr.НомСчФПрод = St.SFormat(temp[0]);
                attr.ДатаСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols8.SellerNumAndDateSfRed]), ";");
                attr.НомИспрСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрСчФ = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols8.SellerNumAndDateSfKor]), ";");
                attr.НомКСчФПрод = St.SFormat(temp[0]);
                attr.ДатаКСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols8.SellerNumAndDateSfKorRed]), ";");
                attr.НомИспрКСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрКСчФ = temp[1];

                attr.ОКВ = St.SeparationTwo(Cells[St.Cols8.NameAndCodeCurrency], ";")[1].Trim(' ').Replace(",", ""); // Российский рубль - RUB - 643 - Россия

                // Передаем данные как есть, без замены "," на ".", обработка идет внутри класса
                attr.СтоимПокупВ = Cells[St.Cols8.CostPaymentOfSf];
                attr.СумНДСВыч = Cells[St.Cols8.SumNdsOfSf];
                #endregion
                //------------------------------------------------------------

                w.WriteLine("           <КнПокСтр " + attr.ПолучитьАтрибуты() + ">");
                w.WriteLine("               <КодВидОпер>" + Cells[St.Cols8.OperTypeCode].PadLeft(2, '0') + "</КодВидОпер>");

                field = Cells[St.Cols8.NumAndDateDocConfirmPay];
                if (field != "")
                    SWWriteDocSuccess(w, "Упл", field);

                field = Cells[St.Cols8.DateAcceptGoodOrServices];
                if (field == "")
                    field = attr.ДатаСчФПрод;
                if (field != "")
                    w.WriteLine("               <ДатаУчТов>" + St.GetDateFormat(field) + "</ДатаУчТов>");

                field = Cells[St.Cols8.SellerInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПрод", St.SFormat(field));

                field = Cells[St.Cols8.AgentInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПос", St.SFormat(field));

                field = Cells[St.Cols8.NumberTD];
                if (field != "")
                    SWWriteNumberTD(w, field);

                w.WriteLine("           </КнПокСтр>");
            }

            w.WriteLine("       </КнигаПокуп>");
            Console.WriteLine("\r\n>>> Завершение процедуры выгрузки в XML тела книги (8) Покупок");
        }

        /// <summary>
        /// Запись в XML тела книги (9) Продаж
        /// </summary>
        /// <param name="w"></param>
        /// <param name="Data"></param>
        /// <param name="iStart"></param>
        private void XmlWriter_Body9(StreamWriter w, List<string[]> Data, int iStart = 0)
        {
            string field = "";

            Console.WriteLine(">>> Запуск процедуры выгрузки в XML тела книги (9) Продаж");
            w.WriteLine("       <КнигаПрод>");

            Console.WriteLine("Чтение данных начато с {0} строки", iStart);
            int renum = 0;
            int rowCount = Data.Count;
            iStart = (iStart > 0) ? iStart - 1 : iStart;
            for (int i = iStart; i < rowCount; i++)
            {
                Console.Write("\rСтрока {0} из {1} ({2}%)", (i + 1), rowCount, ((i + 1) * 100 / rowCount));
                string[] Cells = Data[i];
                string[] temp = new string[2];
                // При КОД ОПЕРАЦИИ 18 - должны быть заполнены графы 3 и 7
                if (Cells[St.Cols9.SellerNumAndDateSf][0] == '-')
                {
                    Cells[St.Cols9.SellerNumAndDateSf] = "0000" + Cells[St.Cols9.SellerNumAndDateSf];
                }
                if (Cells[St.Cols9.OperTypeCode].Trim() == "18")
                {
                    string DataCol3 = Cells[St.Cols9.SellerNumAndDateSf];
                    string DataCol7 = Cells[St.Cols9.SellerNumAndDateSfKor];

                    if (DataCol3 == "" && DataCol7 != "")
                        Cells[St.Cols9.SellerNumAndDateSf] = DataCol7;
                    if (DataCol3 != "" && DataCol7 == "")
                        Cells[St.Cols9.SellerNumAndDateSfKor] = DataCol3;
                }                
                renum++;

                //------------------------------------------------------------
                #region Заполняем список атрибутов
                AttributeClasses.атрибутыКнПродСтр attr = new AttributeClasses.атрибутыКнПродСтр();
                attr.НомерПор = renum.ToString();

                // Разделитель ";". Пример: 47815/178-18;26.10.2018
                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols9.SellerNumAndDateSf]), ";");
                attr.НомСчФПрод = St.SFormat(temp[0]);
                attr.ДатаСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols9.SellerNumAndDateSfRed]), ";");
                attr.НомИспрСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрСчФ = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols9.SellerNumAndDateSfKor]), ";");
                attr.НомКСчФПрод = St.SFormat(temp[0]);
                attr.ДатаКСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols9.SellerNumAndDateSfKorRed]), ";");
                attr.НомИспрКСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрКСчФ = temp[1];

                attr.ОКВ = St.SeparationTwo(Cells[St.Cols9.NameAndCodeCurrency], ";")[1].Trim(' ').Replace(",", ""); // Российский рубль - RUB - 643 - Россия

                // Передаем данные как есть, без замены "," на ".", обработка идет внутри класса
                attr.СтоимПродСФВ = Cells[St.Cols9.CostCurrencySf];
                attr.СтоимПродСФ = Cells[St.Cols9.CostRubKop];
                attr.СтоимПродСФ20 = Cells[St.Cols9.CostRubKopWithoutNDS20];
                attr.СтоимПродСФ18 = Cells[St.Cols9.CostRubKopWithoutNDS18];
                attr.СтоимПродСФ10 = Cells[St.Cols9.CostRubKopWithoutNDS10];
                attr.СтоимПродСФ0 = Cells[St.Cols9.CostRubKopWithoutNDS0];
                attr.СумНДССФ20 = Cells[St.Cols9.SumNDS20];
                attr.СумНДССФ18 = Cells[St.Cols9.SumNDS18];
                attr.СумНДССФ10 = Cells[St.Cols9.SumNDS10];
                attr.СтоимПродОсв = Cells[St.Cols9.CostSalesWithoutNDS];
                #endregion
                //------------------------------------------------------------

                w.WriteLine("           <КнПродСтр " + attr.ПолучитьАтрибуты() + ">");

                field = Cells[St.Cols9.OperTypeCode];
                if (field != "")
                    w.WriteLine("               <КодВидОпер>" + field.PadLeft(2, '0') + "</КодВидОпер>");

                field = Cells[St.Cols9.NumberTD];
                if (field != "")
                    SWWriteNumberTD(w, field);

                field = Cells[St.Cols9.GoodTypeCode];
                if (field != "")
                    w.WriteLine("               <КодВидТовар>" + field + "</КодВидТовар>");

                field = Cells[St.Cols9.NumAndDateDocConfirmPay];
                if (field != "")
                    SWWriteDocSuccess(w, "Опл", field);

                field = Cells[St.Cols9.KontrInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПокуп", St.SFormat(field));

                field = Cells[St.Cols9.AgentInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПос", St.SFormat(field));

                w.WriteLine("           </КнПродСтр>");
            }

            w.WriteLine("       </КнигаПрод>");
            Console.WriteLine("\r\n>>> Завершение процедуры выгрузки в XML тела книги (9) Продаж");
        }

        /// <summary>
        /// Запись в XML тела журнала (10) Выставленных СФ
        /// </summary>
        /// <param name="w"></param>
        /// <param name="Data"></param>
        /// <param name="iStart"></param>
        private void XmlWriter_Body10(StreamWriter w, List<string[]> Data, int iStart = 0)
        {
            string field = "";

            Console.WriteLine(">>> Запуск процедуры выгрузки в XML тела журнала (10) Выставленных СФ");
            w.WriteLine("       <ЖУчВыстСчФ>");

            Console.WriteLine("Чтение данных начато с {0} строки", iStart);
            int renum = 0;
            int rowCount = Data.Count;
            iStart = (iStart > 0) ? iStart - 1 : iStart;
            for (int i = iStart; i < rowCount; i++)
            {
                Console.Write("\rСтрока {0} из {1} ({2}%)", (i + 1), rowCount, ((i + 1) * 100 / rowCount));
                string[] Cells = Data[i];
                string[] temp = new string[2];
                renum++;

                //------------------------------------------------------------
                #region Заполняем список атрибутов
                AttributeClasses.атрибутыЖУчВыстСчФСтр attr = new AttributeClasses.атрибутыЖУчВыстСчФСтр();
                attr.НомерПор = renum.ToString();

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols10.NumAndDateSf]), ";");
                attr.НомСчФПрод = St.SFormat(temp[0]);
                attr.ДатаСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols10.NumAndDateSfRed]), ";");
                attr.НомИспрСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрСчФ = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols10.NumAndDateSfKor]), ";");
                attr.НомКСчФПрод = St.SFormat(temp[0]);
                attr.ДатаКСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols10.NumAndDateSfKorRed]), ";");
                attr.НомИспрКСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрКСчФ = temp[1];
                #endregion
                //------------------------------------------------------------

                w.WriteLine("           <ЖУчВыстСчФСтр " + attr.ПолучитьАтрибуты() + ">");

                field = Cells[St.Cols10.OperTypeCode];
                if (field != "")
                    w.WriteLine("               <КодВидОпер>" + field.PadLeft(2, '0') + "</КодВидОпер>");

                field = Cells[St.Cols10.KontrInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПокуп", St.SFormat(field));

                //------------------------------------------------------------
                #region Заполняем список атрибутов
                AttributeClasses.атрибутыСвСчФОтПрод addit = new AttributeClasses.атрибутыСвСчФОтПрод();
                // Костыль. Берем данные СФ, если не указано у продажной
                field = Cells[St.Cols10.SellerNumAndDateSfKor];
                if (field == "" && Cells[St.Cols10.SellerInnKpp] != "")
                    field = Cells[St.Cols10.NumAndDateSf]; 
                temp = St.SeparationTwo(field, ";");
                addit.НомСчФОтПрод = St.SFormat(temp[0]);
                addit.ДатаСчФОтПрод = temp[1];

                //addit.ОКВ = Cells[St.Cols10.NameAndCodeCurrency].Trim(' ').Replace(",", "");
                addit.ОКВ = St.SeparationTwo(Cells[St.Cols10.NameAndCodeCurrency], ";")[1].Trim(' ').Replace(",", ""); // Российский рубль - RUB - 643 - Россия;
                addit.СтоимТовСчФВс = Cells[St.Cols10.CostSumGoodsAndServices];
                addit.СумНДССчФ = (String.IsNullOrEmpty(attr.НомКСчФПрод)) ? Cells[St.Cols10.CostSumNds] : ""; // Костыль.
                addit.РазСтКСчФУм = Cells[St.Cols10.DiffCostWithNdsOfSfKorMinus];
                addit.РазСтКСчФУв = Cells[St.Cols10.DiffCostWithNdsOfSfKorPlus];
                addit.РазНДСКСчФУм = Cells[St.Cols10.DiffCostNdsOfSfKorMinus];
                addit.РазНДСКСчФУв = Cells[St.Cols10.DiffCostNdsOfSfKorPlus];
                #endregion
                //------------------------------------------------------------

                w.WriteLine("               <СвСчФОтПрод " + addit.ПолучитьАтрибуты() + ">");

                field = Cells[St.Cols10.SellerInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПрод", St.SFormat(field));

                w.WriteLine("               </СвСчФОтПрод>");
                w.WriteLine("           </ЖУчВыстСчФСтр>");
            }

            w.WriteLine("       </ЖУчВыстСчФ>");
            Console.WriteLine("\r\n>>> Завершение процедуры выгрузки в XML тела журнала (10) Выставленных СФ");
        }
        
        /// <summary>
        /// Запись в XML тела журнала (11) Полученных СФ
        /// </summary>
        /// <param name="w"></param>
        /// <param name="Data"></param>
        /// <param name="iStart"></param>
        private void XmlWriter_Body11(StreamWriter w, List<string[]> Data, int iStart = 0)
        {
            string field = "";

            Console.WriteLine(">>> Запуск процедуры выгрузки тела в XML журнала (11) Полученных СФ");
            w.WriteLine("       <ЖУчПолучСчФ>");

            Console.WriteLine("Чтение данных начато с {0} строки", iStart);
            int renum = 0;
            int rowCount = Data.Count;
            iStart = (iStart > 0) ? iStart - 1 : iStart;
            for (int i = iStart; i < rowCount; i++)
            {
                Console.Write("\rСтрока {0} из {1} ({2}%)", (i + 1), rowCount, ((i + 1) * 100 / rowCount));
                string[] Cells = Data[i];
                string[] temp = new string[2];
                renum++;

                //------------------------------------------------------------
                #region Заполняем список атрибутов
                AttributeClasses.атрибутыЖУчПолучСчФСтр attr = new AttributeClasses.атрибутыЖУчПолучСчФСтр();
                attr.НомерПор = renum.ToString();

                // Разделитель ";". Пример: 47815/178-18;26.10.2018
                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols11.NumAndDateSf]), ";");
                attr.НомСчФПрод = St.SFormat(temp[0]);
                attr.ДатаСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols11.NumAndDateSfRed]), ";");
                attr.НомИспрСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрСчФ = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols11.NumAndDateSfKor]), ";");
                attr.НомКСчФПрод = St.SFormat(temp[0]);
                attr.ДатаКСчФПрод = temp[1];

                temp = St.SeparationTwo(St.GetCorrectNumAndDate(Cells[St.Cols11.NumAndDateSfKorRed]), ";");
                attr.НомИспрКСчФ = St.SFormat(temp[0]);
                attr.ДатаИспрКСчФ = temp[1];

                attr.КодВидСд = Cells[St.Cols11.DealCode];
                attr.ОКВ = St.SeparationTwo(Cells[St.Cols11.NameAndCodeCurrency], ";")[1].Trim(' ').Replace(",", ""); // Российский рубль - RUB - 643 - Россия

                attr.СтоимТовСчФВс = Cells[St.Cols11.CostSumGoodsAndServices];
                attr.СумНДССчФ = Cells[St.Cols11.CostSumNds];
                attr.РазСтКСчФУм = Cells[St.Cols11.DiffCostWithNdsOfSfKorMinus];
                attr.РазСтКСчФУв = Cells[St.Cols11.DiffCostWithNdsOfSfKorPlus];
                attr.РазНДСКСчФУм = Cells[St.Cols11.DiffCostNdsOfSfKorMinus];
                attr.РазНДСКСчФУв = Cells[St.Cols11.DiffCostNdsOfSfKorPlus];
                #endregion
                //------------------------------------------------------------

                w.WriteLine("           <ЖУчПолучСчФСтр " + attr.ПолучитьАтрибуты() + ">");

                field = Cells[St.Cols11.OperTypeCode];
                if (field != "")
                    w.WriteLine("               <КодВидОпер>" + field.PadLeft(2, '0') + "</КодВидОпер>");

                field = Cells[St.Cols11.SellerInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвПрод", St.SFormat(field));

                field = Cells[St.Cols11.AgentInnKpp];
                if (field != "")
                    SWWriteInformation(w, "СвКомис", St.SFormat(field));

                w.WriteLine("           </ЖУчПолучСчФСтр>");
            }

            w.WriteLine("       </ЖУчПолучСчФ>");
            Console.WriteLine("\r\n>>> Завершение процедуры выгрузки в XML тела журнала (11) Полученных СФ");
        }
        #endregion
    }
}