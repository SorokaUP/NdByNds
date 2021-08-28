using System;
using System.Diagnostics;

namespace BookLoadConsole
{
    class Program
    {
        /*
           8  - Книга покупок"
           9  - Книга продаж"
           10 - Журнал выставленных СФ"
           11 - Журнал полученных СФ"
        */

        static readonly string Book_PathDefault = @"C:\Files\NDS\09-1.xlsx";
        static readonly int Book_BookTypeDefault = 9;
        static readonly string Book_BookFormatDefault = "5.08";
        static readonly int Book_NumberKorr = 0;
        static readonly string XmlValidation_PathXmlDefault = @"C:\Files\NO_NDS.9_7802_7802_7719022542771501001_20210811_133633447.xml";
        static readonly string XmlValidation_PathXsdDefault = @"G:\Work\TaxDeclaration\xsd\9_5.07_Сбис.xsd";
        static readonly string Book_ExportPathXml = @"C:\Files\";
        static readonly string Book_ExportPathExcel = @"C:\Files\";

        //======================================================================================================
        //======================================================================================================
        //======================================================================================================

        [STAThread]
        public static void Main(string[] args)
        {
            string cmd = "";
            do
            {
                Book book = new Book();
                string mode = "";
                bool isFromForm = false;

                // Запускаем интерфейс
                frmMain frm = new frmMain();
                /*if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mode = frm.returnMode;
                    book = frm.returnBook;
                    isFromForm = true;
                }
                else*/
                {
                    Console.WriteLine("Введите одну из команд:"
                                + "\n   exchk    - Проверка файла Excel"
                                + "\n   ex2xml   - Экспорт данных Excel файла в Xml"
                                //+ "\n   dbfb2xml - Сформировать XML из базы данных"
                                //+ "\n   dbfb2ex  - Сформировать Excel из базы данных"
                                + "\n   vl       - Проверка Xml файла по Xsd схеме"
                                + "\n   diff     - Проверка Xml файла по Excel файлу исходника"
                                + "\n   xmlchk   - Проверка Xml файла на суммы"
                                + "\n   exit     - Выход из программы"
                                );
                    Console.WriteLine("==================================================================");
                    mode = cmdReader();
                }

                DateTime startJob = DateTime.Now;
                switch (mode)
                {
                    case "exchk": // CheckData
                        if (!isFromForm)
                        {
                            cmdBook_FilePathExcel(book);
                            cmdBook_BookType(book);
                            cmdBook_BookFormat(book);
                        }
                        
                        startJob = DateTime.Now;
                        book.ChecExcelkData();
                        break;

                    case "ex2xml": // ExportToXml
                        if (!isFromForm)
                        {
                            cmdBook_FilePathExcel(book);
                            cmdBook_BookType(book);
                            cmdBook_BookFormat(book);
                            cmdBook_NumberKorr(book);
                            cmdBook_ExportPathXml(book);
                        }
                            
                        startJob = DateTime.Now;
                        book.ExcelToXml();
                        break;

                    case "xmlchk": // XmlCheck
                        if (!isFromForm)
                        {
                            cmdBook_FilePathXml(book);
                            cmdBook_BookType(book);
                        }

                        startJob = DateTime.Now;
                        book.XmlCheckSum();
                        break;

                    case "vl": // XmlValidate
                        if (!isFromForm)
                        {
                            cmdBook_FilePathXml(book);
                            cmdBook_FilePathXsd(book);
                        }

                        startJob = DateTime.Now;
                        book.ValidateXmlForXsd();
                        break;

                    case "diff": // DifferenceXmlAndExcel
                        if (!isFromForm)
                        {
                            cmdBook_BookType(book);
                            cmdBook_FilePathExcel(book);
                            cmdBook_FilePathXml(book);
                        }

                        startJob = DateTime.Now;
                        book.DifferenceXmlAndExcel();
                        break;

                    case "dbfb2xml": // DataBaseToXml
                        if (!isFromForm)
                        {
                            cmdBook_BookType(book);
                            cmdBook_BookFormat(book);
                            cmdBook_NumberKorr(book);
                            cmdBook_FbExDates(book);
                            cmdBook_ExportPathXml(book);
                        }
                            
                        startJob = DateTime.Now;
                        book.DataBaseToXml();
                        break;

                    case "dbfb2ex": // DatabaseToExcel
                        if (!isFromForm)
                        {
                            cmdBook_BookType(book);
                            cmdBook_BookFormat(book);
                            cmdBook_FbExDates(book);
                            cmdBook_ExportPathExcel(book);
                        }
                            
                        startJob = DateTime.Now;
                        book.DataBaseToExcel();
                        break;

                    case "dbfb2dbss":
                        break;

                    case "ex2dbss":
                        break;

                    //------------------------------------------------------------------------------

                    default:
                        Console.WriteLine("Комманда введена не верно");
                        break;
                }

                TimeSpan TotalTime = DateTime.Now.Subtract(startJob);
                Console.WriteLine("\n\r    Итоговое время: {0} \n\r", TimeFormat(TotalTime.Hours, TotalTime.Minutes, TotalTime.Seconds));

                Console.WriteLine("==================================================================");
                Console.WriteLine("Для выхода из программы введите \"exit\" или нажмите Enter для продолжения...");

                // Выход из приложения внутри перехватчика
                cmd = cmdReader();
                Console.Clear();
            } while (true);
        }

        //======================================================================================================
        //======================================================================================================
        //======================================================================================================

        #region Вспомогательные процедуры
        /// <summary>
        /// Вывод времени в формате
        /// </summary>
        /// <param name="hh">Часы</param>
        /// <param name="mm">Минуты</param>
        /// <param name="ss">Секунды</param>
        private static string TimeFormat(int hh, int mm, int ss)
        {
            string res = "";

            res = "";
            if (hh > 0)
                res += ((hh < 10) ? "0" + hh.ToString() : hh.ToString()) + " ч. ";
            if (mm > 0)
                res += ((mm < 10) ? "0" + mm.ToString() : mm.ToString()) + " мин. ";
            res += ((ss < 10) ? "0" + ss.ToString() : ss.ToString()) + " сек.";

            return res;
        }

        /// <summary>
        /// Директория файла Excel
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_FilePathExcel(Book book)
        {
            Console.WriteLine("Укажите директорию файла Excel"
                            + "\n   (по умолчанию " + Book_PathDefault + "): ");
            book.FilePathExcel = cmdReader();
            if (book.FilePathExcel == "")
                book.FilePathExcel = Book_PathDefault;
            Console.WriteLine("Директория файла Excel: " + book.FilePathExcel);
        }

        /// <summary>
        /// Тип файла
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_BookType(Book book)
        {
            Console.WriteLine("Укажите один из типов файла:"
                            + "\n   8  - Книга покупок"
                            + "\n   9  - Книга продаж"
                            + "\n   10 - Журнал выставленных СФ"
                            + "\n   11 - Журнал полученных СФ"
                            + "\n   (по умолчанию " + Book_BookTypeDefault + ")");
            try { book.BookType = int.Parse(cmdReader()); }
            catch { book.BookType = Book_BookTypeDefault; }
            Console.WriteLine("Тип файла: " + book.BookType);
        }

        /// <summary>
        /// Формат книги
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_BookFormat(Book book)
        {
            Console.WriteLine("Укажите один из форматов типа книги/журнала:"
                + "\n   5.08 - с 01.07.2021"
                + "\n   5.07 - с 01.10.2020"
                + "\n   5.06 - с 01.01.2019"
                + "\n   5.05 - c 01.01.2017"
                + "\n   (по умолчанию " + Book_BookFormatDefault + ")");
            string BookFormat = "";
            try
            {
                string readed = cmdReader();
                switch (readed)
                {
                    case "5.05":
                        BookFormat = readed;
                        break;

                    case "5.06":
                        BookFormat = readed;
                        break;

                    case "5.07":
                        BookFormat = readed;
                        break;

                    case "5.08":
                        BookFormat = readed;
                        break;

                    default:
                        BookFormat = Book_BookFormatDefault;
                        break;
                };
            }
            catch
            {
                BookFormat = Book_BookFormatDefault;
            }
            // Обновляем настройки (необходимо для того, чтобы применился новый набор колонок (St.Cols9 и St.Cols8), 
            // а так же строки начала чтения (St.RowStart9 и St.RowStart8)
            Settings St = new Settings(BookFormat);
            book.St = St;
            Console.WriteLine("Формат типа книги/журнала: " + BookFormat);
        }

        /// <summary>
        /// Номер корректировки
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_NumberKorr(Book book)
        {
            Console.WriteLine("Укажите номер корректировки (по умолчанию {0}):", Book_NumberKorr);
            try { book.NumberKorr = int.Parse(cmdReader()); }
            catch { book.NumberKorr = Book_NumberKorr; }
            Console.WriteLine("Номер корректировки: " + book.NumberKorr);
        }

        /// <summary>
        /// Ссылка на Xml файл
        /// </summary>
        /// <param name="xml"></param>
        private static void cmdBook_FilePathXml(Book book)
        {
            Console.WriteLine("Укажите директорию файла XML (по умолчанию " + XmlValidation_PathXmlDefault + "): ");
            book.FilePathXml = cmdReader();
            if (book.FilePathXml == "")
                book.FilePathXml = XmlValidation_PathXmlDefault;
            Console.WriteLine("Директория файла XML: " + book.FilePathXml);
        }

        /// <summary>
        /// Ссылка на Xsd схему
        /// </summary>
        /// <param name="xml"></param>
        private static void cmdBook_FilePathXsd(Book book)
        {
            Console.WriteLine("Укажите директорию файла XSD (по умолчанию " + XmlValidation_PathXsdDefault + "): ");
            book.FilePathXsd = cmdReader();
            if (book.FilePathXsd == "")
                book.FilePathXsd = XmlValidation_PathXsdDefault;
            Console.WriteLine("Директория файла XSD: " + book.FilePathXsd);
        }

        /// <summary>
        /// Перехватчик комманд. Перенаправляет программу в случае обнаружения команды.
        /// </summary>
        /// <returns></returns>
        private static string cmdReader()
        {
            string cmd = Console.ReadLine();
            if (cmd == "exit") cmdExit();
            return cmd;
        }

        /// <summary>
        /// Команда выхода из программы на любом шаге
        /// </summary>
        private static void cmdExit()
        {
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// Дата начала периода
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_FbExDateFrom(Book book)
        {
            Console.WriteLine("Укажите дату начала периода (дд.мм.гггг):"
                            + "\n   (по умолчанию " + DateTime.Now.ToString("dd.MM.yyyy") + ")");
            DateTime FbExDateFrom = DateTime.Now;
            try
            {
                string readed = cmdReader();
                FbExDateFrom = DateTime.Parse(readed);
                if (FbExDateFrom > DateTime.Now)
                {
                    FbExDateFrom = DateTime.Now;
                    Console.WriteLine("Введенная дата является датой из будущего. Будет установлена текущая дата.");
                }
            }
            catch
            {
                FbExDateFrom = DateTime.Now;
            }
            book.FbExDateFrom = FbExDateFrom;
            Console.WriteLine("Дата начала периода: " + FbExDateFrom.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Дата окончания периода
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_FbExDateTo(Book book)
        {
            Console.WriteLine("Укажите дату окончания периода (дд.мм.гггг):"
                            + "\n   (по умолчанию " + DateTime.Now.ToString("dd.MM.yyyy") + ")");
            DateTime FbExDateTo = DateTime.Now;
            try
            {
                string readed = cmdReader();
                FbExDateTo = DateTime.Parse(readed);
                if (FbExDateTo > DateTime.Now)
                {
                    FbExDateTo = DateTime.Now;
                    Console.WriteLine("Введенная дата является датой из будущего. Будет установлена текущая дата.");
                }
            }
            catch
            {
                FbExDateTo = DateTime.Now;
            }
            book.FbExDateTo = FbExDateTo;
            Console.WriteLine("Дата окончания периода: " + FbExDateTo.ToString("dd.MM.yyyy"));
        }

        /// <summary>
        /// Установка периода
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_FbExDates(Book book)
        {
            cmdBook_FbExDateFrom(book);
            cmdBook_FbExDateTo(book);

            Firebird Fb = new Firebird(book.FbExDateFrom, book.FbExDateTo);
            book.Fb = Fb;

            if (book.FbExDateFrom > book.FbExDateTo)
            {
                book.FbExDateFrom = book.FbExDateTo;
                Console.WriteLine("Дата начала периода превышает дату окончания. Дата начала периода установлена равной дате окончания периода.");
            }
        }

        /// <summary>
        /// Директория формирования файла Xml
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_ExportPathXml(Book book)
        {
            Console.WriteLine("Укажите директорию формирования файла Xml"
                            + "\n   (по умолчанию " + Book_ExportPathXml + "): ");
            book.ExportPathXml = cmdReader();
            if (book.ExportPathXml == "")
                book.ExportPathXml = Book_ExportPathXml;
            Console.WriteLine("Директория файла Excel: " + book.ExportPathXml);
        }

        /// <summary>
        /// Директория формирования файла Excel
        /// </summary>
        /// <param name="book"></param>
        private static void cmdBook_ExportPathExcel(Book book)
        {
            Console.WriteLine("Укажите директорию формирования файла Excel"
                            + "\n   (по умолчанию " + Book_ExportPathExcel + "): ");
            book.ExportPathExcel = cmdReader();
            if (book.ExportPathExcel == "")
                book.ExportPathExcel = Book_ExportPathExcel;
            Console.WriteLine("Директория файла Excel: " + book.ExportPathExcel);
        }
        #endregion
    }
}
