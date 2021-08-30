using ExcelDataReader;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Core.Model
{
    public abstract class ModelMaster
    {
        public BookType bookType;
        public string fileName;
        public byte correctNum;
        public string versionName;
        protected long numberLine;
        protected delegate string DGetBodyBook(object[] data);
        public int numberLineStartReadExcel;
        protected StringBuilder res;

        
        // Заполняется в конструкторе классов наследников
        protected string currentModelName;
        protected IMap currentMap;
        protected DGetBodyBook currentBodyBook;
        protected string currentXsdFileFromResources;

        /// <summary>
        /// Формат СБИС
        /// </summary>
        /// <param name="bookType">Тип документа</param>
        /// <param name="correctNum">Номер корректировки</param>
        public ModelMaster(BookType bookType, byte correctNum, string versionName, IMap map08, IMap map09, IMap map10, IMap map11)
        {           
            this.correctNum = correctNum;
            this.bookType = bookType;
            this.fileName = GenFileName();
            this.numberLine = 1;

            this.versionName = versionName;
            this.res = new StringBuilder();

            switch (this.bookType)
            {
                case BookType.Book08:
                    currentMap = map08;
                    currentBodyBook = GetBodyBook08;
                    currentXsdFileFromResources = map08.PathToFileXSD;
                    numberLineStartReadExcel = map08.NumberLineStartReadExcel;
                    break;

                case BookType.Book09:
                    currentMap = map09;
                    currentBodyBook = GetBodyBook09;
                    currentXsdFileFromResources = map09.PathToFileXSD;
                    numberLineStartReadExcel = map09.NumberLineStartReadExcel;
                    break;

                case BookType.Book10:
                    currentMap = map10;
                    currentBodyBook = GetBodyBook10;
                    currentXsdFileFromResources = map10.PathToFileXSD;
                    numberLineStartReadExcel = map10.NumberLineStartReadExcel;
                    break;

                case BookType.Book11:
                    currentMap = map11;
                    currentBodyBook = GetBodyBook11;
                    currentXsdFileFromResources = map11.PathToFileXSD;
                    numberLineStartReadExcel = map11.NumberLineStartReadExcel;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #region Помощники в работе с номерами строк
        /// <summary>
        /// Генератор нумерации строк для итогового XML файла
        /// </summary>
        protected long GetNumberLine()
        {
            return numberLine++;
        }
        #endregion
        #region Формирование шапки и подвала XML файла
        /// <summary>
        /// Формирование шапки (начальной части XML файла)
        /// </summary>
        /// <returns></returns>
        public virtual string GetHeader()
        {
            return (
                $"<?xml version=\"1.0\" encoding=\"windows-1251\" ?>" + Environment.NewLine +
                $"<Файл {"ИдФайл".AsAttr(fileName)} {"ВерсПрог".AsAttr(currentModelName)} {"ВерсФорм".AsAttr(versionName)}>" +
                $"<Документ {"Индекс".AsAttr(GenBookIndex())} {"НомКорр".AsAttr(correctNum)}>" +
                ((bookType is BookType.Book08) ? $"<{currentMap.Tag} {"СумНДСВсКПк".AsAttr("0")}>" : $"<{currentMap.Tag}>")
            ).ClearTrash();
        }
        /// <summary>
        /// Формирование подвала (завершающей части XML файла)
        /// </summary>
        public virtual string GetFooter()
        {
            return $"</{currentMap.Tag}></Документ></Файл>".ClearTrash();
        }
        /// <summary>
        /// Генератор имени файла
        /// </summary>
        public virtual string GenFileName()
        {
            #region  Приказ ФНС России от 29.10.2014 N ММВ-7-3/558@ (ред. от 20.12.2016) 
            /*
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
            #endregion

            string R_T_ = $"NO_NDS.{(int)bookType}_";
            string A_K_ = "7802_7802_";
            string O_ = "7719022542771501001_";

            DateTime dt = DateTime.Now;

            string MM = dt.Month.ToString();
            if (MM.Length == 1) MM = "0" + MM;
            string DD = dt.Day.ToString();
            if (DD.Length == 1) DD = "0" + DD;
            string GGGGMMDD_ = dt.Year.ToString() + MM + DD + "_";
            string N = dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();

            return R_T_ + A_K_ + O_ + GGGGMMDD_ + N;
        }
        /// <summary>
        /// Генератор индекса книги/журнала
        /// </summary>
        protected virtual string GenBookIndex()
        {
            return $"0000{((int)bookType).ToString().PadLeft(2, '0')}0";
        }
        #endregion
        #region Обработчики формирования тела книги/журнала
        /// <summary>
        /// Обработчик формирования XML тела Книги покупок
        /// </summary>
        /// <param name="data">Набор данных строки Excel файла</param>
        public abstract string GetBodyBook08(object[] data);
        /// <summary>
        /// Обработчик формирования XML тела Книги продаж
        /// </summary>
        /// <param name="data">Набор данных строки Excel файла</param>
        public abstract string GetBodyBook09(object[] data);
        /// <summary>
        /// Обработчик формирования XML тела Журнала выставленных счетов-фактур
        /// </summary>
        /// <param name="data">Набор данных строки Excel файла</param>
        public abstract string GetBodyBook10(object[] data);
        /// <summary>
        /// Обработчик формирования XML тела Журнала полученных счетов-фактур
        /// </summary>
        /// <param name="data">Набор данных строки Excel файла</param>
        public abstract string GetBodyBook11(object[] data);
        #endregion

        #region Валидация
        /// <summary>
        /// Проверка XML файла по XSD схеме по версии СБИС
        /// </summary>
        /// <param name="pathXml">Путь к XML файлу</param>
        public bool Validate(string pathXml)
        {
            XmlDocument xsdDoc = new XmlDocument();
            XmlSchemaSet xsdSchema = new XmlSchemaSet();
            xsdErrorQnt = 0;

            try
            {
                Helper.Log("Загрузка XSD файла");
                xsdDoc.LoadXml(currentXsdFileFromResources);
                //Формирование XSD DOM
                xsdSchema.Add(null, new XmlNodeReader(xsdDoc));
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, LogMode.Ошибка);
                return false;
            }

            XmlDocument xml = new XmlDocument();

            try
            {
                Helper.Log("Загрузка XML файла");
                xml.Load(pathXml);

                Helper.Log(">>> Начат процесс валидации...");
                xml.Schemas.Add(xsdSchema);
                xml.Validate(new ValidationEventHandler(ValidationCallBack));

                Helper.Log(">>> Валидация завершена");
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, LogMode.Ошибка);
                return false;
            }

            return xsdErrorQnt == 0;
        }
        /// <summary>
        /// Обработчик событий валидации XSD
        /// </summary>
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Console.ForegroundColor = (e.Severity.ToString().Equals("Error")) ? ConsoleColor.Red : ConsoleColor.DarkYellow;
            Helper.Log($"  {e.Severity}: {e.Message}", LogMode.Ошибка);
            Console.ResetColor();
            xsdErrorQnt++;
        }

        int xsdErrorQnt = 0;
        #endregion
        #region Подсчет сумм по XML файлу
        /// <summary>
        /// Подсчет сумм по XML файлу
        /// </summary>
        /// <param name="pathXml">Путь к XML файлу</param>
        public virtual void Summary(string pathXml)
        {
            string s;
            try
            {
                s = $"Начало рассчета данных...";
                Console.WriteLine(s);
                Helper.callback?.OnMessage(s);

                DateTime startJob = DateTime.Now;
                SummaryProcess(pathXml, currentMap.SumTag, currentMap.SumFields);
                TimeSpan TotalTime = DateTime.Now.Subtract(startJob);

                s = $"Итоговое время: {TotalTime.TimeFormat()}";
                Console.WriteLine(s);
                Helper.callback?.OnMessage(s);                
            }
            catch (Exception e)
            {
                s = $"Ошибка на уровне обработки: {e.Message}";
                Console.WriteLine(s);
                Helper.callback?.OnMessage(s);
            }
            finally
            {
                s = "Выполнено";
                Console.WriteLine(s);
                Helper.callback?.OnMessage(s);

                GC.Collect();
                Console.WriteLine($"Сборка мусора окончена");
            }
        }
        /// <summary>
        /// Обработчик подсчета сумм по XML файлу
        /// </summary>
        /// <param name="pathXml">Путь к XML файлу</param>
        /// <param name="mainField">Тэг в котором находятся атрибуты с суммами</param>
        /// <param name="attributes">Атрибуты с суммами</param>
        private static void SummaryProcess(string pathXml, string mainField, string[] attributes)
        {
            StringBuilder res = new StringBuilder();
            double[] values = new double[attributes.Length];

            res.Append("Открытие файла Xml");
            Console.WriteLine(res.ToString());
            Helper.callback?.OnMessage(res.ToString());
            res.Clear();

            XmlTextReader reader = new XmlTextReader(pathXml);
            res.Append(">> Расчет...");
            Console.WriteLine(res.ToString());
            Helper.callback?.OnMessage(res.ToString());
            res.Clear();

            int count = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                {
                    if (!mainField.Equals(reader.Name))
                        continue;

                    for (int i = 0; i < attributes.Length; i++)
                    {
                        values[i] += reader.GetAttribute(attributes[i])?.AsDouble() ?? 0;
                    }

                    count++;
                    Console.Write($"\rСчитано данных: {count}");
                }
            }

            // Выводим результат            
            res.Append(">> Расчет окончен");
            res.AppendLine();
            res.Append("Общие суммы по файлу:");
            res.AppendLine();

            for (int i = 0; i < attributes.Length; i++)
            {
                res.Append($"{attributes[i]}: {values[i].ToString("C")}");
                res.AppendLine();
            }

            Console.WriteLine(Environment.NewLine + res.ToString());
            Helper.callback?.OnMessage(res.ToString());
        }
        #endregion
        #region Конвертация файлов Excel в XML
        /// <summary>
        /// Конвертор фалов Excel в XML по версии структуры СБИС
        /// </summary>
        /// <param name="modeType">Режим работы</param>
        /// <param name="model">Модель данных</param>
        /// <param name="importFilePaths">Пути к файлам Excel</param>
        /// <param name="pathSaveFile">Путь для сохранения результатов</param>
        public static void ExcelToXml(ModelMaster model, string[] filePaths, string pathSaveFile)
        {
            if (model.numberLineStartReadExcel < 0)
            {
                Helper.Log($"Версия модели {model.versionName} содержит не верные номера начала считывания строк.");
                return;
            }

            string filePath = $@"{pathSaveFile}\{model.fileName}.xml";
            StreamWriter xml = new StreamWriter(filePath, false, Encoding.GetEncoding("Windows-1251"));

            Helper.Log($"Создан файл: {filePath}");
            try
            {
                Helper.Log($"Запись шапки");
                xml.WriteLine(model.GetHeader());
                Helper.Log($"Начало считывания строк данных...");

                DateTime startJob = DateTime.Now;
                model.ExcelToXmlProcess(filePaths, model.numberLineStartReadExcel, xml);
                TimeSpan TotalTime = DateTime.Now.Subtract(startJob);
                Helper.Log($"Итоговое время: {TotalTime.TimeFormat()}", LogMode.Успех);

                xml.WriteLine(model.GetFooter());
            }
            catch (Exception e)
            {
                Helper.Log($"Ошибка на уровне обработки: {e.Message}", LogMode.Ошибка);
            }
            finally
            {
                Helper.Log($"Сохранение файла...");
                xml.Close();
                Helper.Log($"Выполнено");

                xml.Dispose();
                GC.Collect();
                Console.WriteLine($"Сборка мусора окончена");
            }
        }
        /// <summary>
        /// Обработчик файлов Excel в один XML
        /// </summary>
        /// <param name="filePaths">Пути к файлам Excel</param>
        /// <param name="getBodyBook">Ссылка на Метод обработки книги/журнала</param>
        /// <param name="iLineBegin">Строка начала считывания данных из Excel файла (одинаково для всех выбранных Excel файлов в рамках выбранной книги/журнала)</param>
        /// <param name="xml">Поток записи в файл XML</param>
        private void ExcelToXmlProcess(string[] filePaths, int iLineBegin, StreamWriter xml)
        {
            //Открываем по очереди каждый выбранный файл Excel
            foreach (string filePath in filePaths)
            {
                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        Helper.Log($"Открываем Excel файл {filePath}");

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            Helper.Log($"Начало считывания. Обнаружено {reader.RowCount} строк данных");

                            //Пропускаем лишние строки, в соответствии с настройками версии и книги
                            int iLine = 0;
                            while (reader.Read())
                            {
                                iLine++;
                                if (iLine < iLineBegin)
                                {
                                    continue;
                                }

                                try
                                {
                                    //Преобразуем строку данных Excel в массив
                                    object[] data = new object[reader.FieldCount + 1];
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        data[i + 1] = reader.GetValue(i);
                                    }

                                    //Выполняем обработку строки
                                    xml.WriteLine(currentBodyBook(data));

                                    //Логирование
                                    if (iLine % Helper.LogLines == 0)
                                    {
                                        Helper.Log($"   Считано: {iLine}", LogMode.Сообщение, false);
                                        Helper.callback?.OnProgress(iLine, reader.RowCount);
                                    }
                                }
                                catch (Exception exReader)
                                {
                                    Helper.Log($"Ошибка на уровне чтения строки: {exReader.Message}", LogMode.Ошибка);
                                }
                            }

                            //Сообщаем в UI о продвижении
                            Helper.callback?.OnProgress(reader.RowCount, reader.RowCount);
                        }
                    }
                }
                catch (Exception exFile)
                {
                    Helper.Log($"Ошибка на уровне чтения файла: {exFile.Message}", LogMode.Ошибка);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Ключевые поля для обработки книги/журнала
    /// </summary>
    public abstract class IMap
    {
        /// <summary>
        /// XML тэг книги/журнала
        /// </summary>
        public abstract string Tag { get; }
        /// <summary>
        /// XML тэг строки книги/журнала
        /// </summary>
        public abstract string TagLine { get; }
        /// <summary>
        /// Путь к файлу XSD схемы для валидации
        /// </summary>
        public abstract string PathToFileXSD { get; }
        /// <summary>
        /// Строка начала считывания Excel файла (начиная с 1)
        /// </summary>
        public abstract int NumberLineStartReadExcel { get; }
        /// <summary>
        /// XML тэг в котором хранятся атрибуты для подсчета сумм
        /// </summary>
        public abstract string SumTag { get; }
        /// <summary>
        /// Список атрибутов тэга для подсчета сумм
        /// </summary>
        public abstract string[] SumFields { get; }
    }
}