using System;
using System.Text;
using System.Xml;

namespace Core.Model
{
    public abstract class ModelMaster
    {
        public BookType bookType;
        public string fileName;
        public byte correctNum;
        public string versionName;
        protected long numberLine;
        public delegate string DGetBodyBook(object[] data);
        protected IMap map08;
        protected IMap map09;
        protected IMap map10;
        protected IMap map11;
        public int LineStartReadExcel;

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
            this.map08 = map08;
            this.map09 = map09;
            this.map10 = map10;
            this.map11 = map11;

            this.LineStartReadExcel = GetLineStartReadExcel();            
        }

        private int GetLineStartReadExcel()
        {
            switch (bookType)
            {
                case BookType.Book08:
                    return map08.LineStartReadExcel;

                case BookType.Book09:
                    return map09.LineStartReadExcel;

                case BookType.Book10:
                    return map10.LineStartReadExcel;

                case BookType.Book11:
                    return map11.LineStartReadExcel;

                default:
                    return 0;
            }
        }

        public DGetBodyBook GetBodyBook()
        {
            switch (bookType)
            {
                case BookType.Book08:
                    return GetBodyBook08;

                case BookType.Book09:
                    return GetBodyBook09;

                case BookType.Book10:
                    return GetBodyBook10;

                case BookType.Book11:
                    return GetBodyBook11;

                default:
                    return null;
            }
        }

        public bool CheckNumberLineValues()
        {
            return !(map08.LineStartReadExcel < 0 ||
                map09.LineStartReadExcel < 0 ||
                map10.LineStartReadExcel < 0 ||
                map11.LineStartReadExcel < 0);
        }
        protected long GetNumberLine()
        {
            return numberLine++;
        }

        public abstract string GetHeader();
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
        protected virtual string GenBookIndex()
        {
            return $"0000{((int)bookType).ToString().PadLeft(2, '0')}0";
        }

        public abstract string GetBodyBook08(object[] data);
        public abstract string GetBodyBook09(object[] data);
        public abstract string GetBodyBook10(object[] data);
        public abstract string GetBodyBook11(object[] data);
        public abstract string GetFooter();
        public virtual bool Validate(string pathXml)
        {
            string pathXsd = "";
            switch (bookType)
            {
                case BookType.Book08:
                    pathXsd = map08.PathToFileXSD;
                    break;

                case BookType.Book09:
                    pathXsd = map09.PathToFileXSD;
                    break;

                case BookType.Book10:
                    pathXsd = map10.PathToFileXSD;
                    break;

                case BookType.Book11:
                    pathXsd = map11.PathToFileXSD;
                    break;
            }

            return new XmlValidate
            {
                PathXml = pathXml,
                PathXsd = pathXsd
            }.Validate();
        }
               

        public virtual void Summary(string pathXml)
        {
            string s = "";
            try
            {
                s = $"Начало рассчета данных...";
                Console.WriteLine(s);
                Helper.callback?.OnMessage(s);

                DateTime startJob = DateTime.Now;
                string SumTag = "";
                string[] SumFields = null;
                switch (bookType)
                {
                    case BookType.Book08:
                        SumTag = map08.SumTag;
                        SumFields = map08.SumFields;
                        break;

                    case BookType.Book09:
                        SumTag = map09.SumTag;
                        SumFields = map09.SumFields;
                        break;

                    case BookType.Book10:
                        SumTag = map10.SumTag;
                        SumFields = map10.SumFields;
                        break;

                    case BookType.Book11:
                        SumTag = map11.SumTag;
                        SumFields = map11.SumFields;
                        break;
                }
                SummaryProcess(pathXml, SumTag, SumFields);
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
        protected static void SummaryProcess(string pathXml, string mainField, string[] attributes)
        {
            StringBuilder res = new StringBuilder();
            try
            {
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
            }
            catch (Exception ex)
            {
                res.Clear();
                res.Append("ОШИБКА: " + ex.Message);
            }

            Console.WriteLine(Environment.NewLine + res.ToString());
            Helper.callback?.OnMessage(res.ToString());
        }        
    }

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
        public abstract int LineStartReadExcel { get; }
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