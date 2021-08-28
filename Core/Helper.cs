using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public static class Helper
    {
        public static Model.ICallback callback = null;
        public const string ИсправлениеДаты = " от ";
        public const byte DEGREE = 2;

        private static readonly Regex regexGTD = new Regex("^((\\;)?([0-9]{8}\\/[0-9]{6}\\/[0-9]{7}(\\/[0-9]{1,2})?))*$");
        public const int LogLines = 10000;

        /// <summary>
        /// Конкантенация строк
        /// </summary>
        public static void Add(this StringBuilder sb, string value)
        {
            sb.Append(value ?? "");
        }
        /// <summary>
        /// Преобразует строку в Атрибут XML вида: name="value"
        /// </summary>
        /// <param name="name">Имя атрибута</param>
        /// <param name="value">Значение атрибута</param>
        /// <param name="feature">Признак обязательности</param>
        /// <returns></returns>
        public static string AsAttr(this string name, object value, Feature feature = Feature.Обязательно)
        {
            return feature is Feature.Обязательно
                ? $" {name}=\"{(value ?? "").ToString().Trim()}\""
                : string.IsNullOrEmpty(value.ToString().Trim())
                    ? null
                    : $" {name}=\"{(value ?? "").ToString().Trim()}\"";
        }
        /// <summary>
        /// Преобразует строку в Тэг XML вида: <name>value</name>
        /// </summary>
        /// <param name="name">Имя Тэга</param>
        /// <param name="value">Значение</param>
        /// <param name="feature">Признак обязательности</param>
        /// <returns></returns>
        public static string AsSingleTag(this string name, object value, Feature feature = Feature.Обязательно)
        {
            return feature is Feature.Обязательно
                ? $"<{name}>{(value ?? "").ToString().Trim()}</{name}>"
                : string.IsNullOrEmpty(value.ToString().Trim())
                    ? null
                    : $"<{name}>{(value ?? "").ToString().Trim()}</{name}>";
        }
        /// <summary>
        /// Вывод времени в формате: чч:мм:сс
        /// </summary>
        public static string TimeFormat(this TimeSpan ts)
        {
            return $"{ts.Hours.ToString().PadLeft(2, '0')}:{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.Seconds.ToString().PadLeft(2, '0')}";
        }
        /// <summary>
        /// Получить массив результатов из поля набора данных
        /// </summary>
        /// <param name="data">Набор данных</param>
        /// <param name="column">Поле</param>
        /// <param name="splitter">Разделитель</param>
        /// <returns></returns>
        public static string[] ValArray(this object[] data, byte column, char splitter)
        {
            string[] arr = data[column]?.ToString().Split(splitter);
            return (arr?.Length > 0 && !string.IsNullOrEmpty(arr?[0])) ? arr : null;
        }
        /// <summary>
        /// Получить 2-е значение из результатов разделения строки
        /// </summary>
        /// <param name="data">Рабор данных</param>
        /// <param name="column">Поле</param>
        /// <param name="splitter">Разделитель</param>
        /// <returns></returns>
        public static string ValSecond(this object[] data, byte column, char splitter)
        {
            string[] arr = data[column]?.ToString().Split(splitter);
            return (arr?.Length > 1) ? arr[1] : null;
        }
        /// <summary>
        /// Представить вещественное число как строку
        /// </summary>
        /// <param name="data">Набор данных</param>
        /// <param name="column">Поле</param>
        /// <param name="degree">Степень округления</param>
        /// <param name="roundType">Тип округления</param>
        /// <returns></returns>
        public static string AsDec(this object[] data, byte column, byte degree = DEGREE)
        {
            object x = data[column];
            if (x == null)
                return "0";
            if (x is double @double)
                return @double.AsString(degree);

            string s = data[column]?.ToString();
            if (string.IsNullOrEmpty(s))
                return "0";

            return s.AsDec(degree);
        }
        /// <summary>
        /// Предствить вещественное число как строку
        /// </summary>
        /// <param name="s">Строка вида вещестенного числа</param>
        /// <param name="degree">Степень округления</param>
        /// <param name="roundType">Тип округления</param>
        /// <returns></returns>
        public static string AsDec(this string s, byte degree = DEGREE)
        {
            if (string.IsNullOrEmpty(s))
                return "0";

            string res;
            try
            {
                s = s.Replace('.', ',').Replace(' ', '\0');
                decimal dec = 0;
                dec = Math.Round(decimal.Parse(s), degree);

                res = dec.ToString($"N{degree}");
                res = res.Replace(',', '.').Replace(" ", "");
            }
            catch
            {
                res = "0";
            }
            return res;
        }
        /// <summary>
        /// Предствить вещественное число как строку
        /// </summary>
        /// <param name="x">Значение</param>
        /// <param name="degree">Степень округления</param>
        /// <param name="roundType">Тип округления</param>
        /// <returns></returns>
        public static string AsString(this double x, byte degree = DEGREE)
        {
            x = Math.Round(x, degree);
            return (x.ToString($"N{degree}")).Replace(',', '.').Replace(" ", "");
        }
        /// <summary>
        /// Сложение двух вещественных чисел в строковом типе
        /// </summary>
        public static double AsDouble(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            s = s.Replace(".", ",").Replace(" ", "");
            if (!char.IsNumber(s[0]) && ((s[0].Equals('-')) && (s.Length.Equals(1))))
                return 0;

            try
            {
                return double.Parse(s);
            }
            catch { return 0; }
        }
        /// <summary>
        /// Очистить строку от "мусора"
        /// </summary>
        public static string ClearTrash(this string s)
        {
            return s.Replace("\n", "").Replace("  ", " ");
        }
        /// <summary>
        /// Представить строку в виде двух атрибутов связки НомерДокумента - ДатаДокумента
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="docNumTagName">Имя Тэга номера документа</param>
        /// <param name="docDateTagName">Имя Тэга даты документа</param>
        /// <param name="delimiter">Разделитель</param>
        /// <returns></returns>
        public static string AsAttrDocNumDate(this string value, string docNumTagName, string docDateTagName, char delimiter = ';')
        {
            if (string.IsNullOrEmpty(value))
                return null;

            value = value.ClearTrash().Replace(" ", "");
            if (value.Contains(ИсправлениеДаты))
                value = value.Replace(ИсправлениеДаты, delimiter.ToString());

            string[] arr = value.Split(delimiter);
            if (arr.Length < 1)
                return null;
            
            return docNumTagName.AsAttr(arr[0]) +
                ((arr.Length > 1) ? docDateTagName.AsAttr(arr[1]) : "");
        }
        /// <summary>
        /// Проверка GTD на соответствие формату
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckGTD(this string value)
        {
            return regexGTD.IsMatch(value);
        }        
        /// <summary>
        /// Запись в ЛОГ
        /// </summary>
        /// <param name="value">Записываемые данные</param>
        /// <param name="logMode">Режим записи в лог</param>
        /// <param name="isCallback">Нужно ли сообщать слушателю</param>
        public static void Log(string value, LogMode logMode = LogMode.Сообщение, bool isCallback = true)
        {
            Console.WriteLine(value);
            if (isCallback)
            {
                switch (logMode)
                {
                    case LogMode.Сообщение:
                        callback?.OnMessage(value);
                        break;

                    case LogMode.Успех:
                        callback?.OnSuccess(value);
                        break;

                    case LogMode.Ошибка:
                        callback?.OnFailed(value);
                        break;
                }
            }
        }
        /// <summary>
        /// Русское представление перечислений
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string EnumToString<T>(T val)
        {
            Type t = typeof(T);
            if (t == typeof(ModeType))
            {
                switch ((ModeType)Enum.Parse(typeof(ModeType), val.ToString()))
                {
                    case ModeType.Summary:
                        return "Подсчет сумм XML файла";

                    case ModeType.ExcelToXml:
                        return "Создать XML из Excel файла";

                    case ModeType.Validate:
                        return "Проверить корректность XML файла";

                    default:
                        return val.ToString();
                }
            }
            if (t == typeof(RoundType))
            {
                switch ((RoundType)Enum.Parse(typeof(RoundType), val.ToString()))
                {
                    case RoundType.Clipping:
                        return "Отсечение";

                    case RoundType.Matematic:
                        return "Математическое";

                    default:
                        return val.ToString();
                }
            }
            if (t == typeof(BookType))
            {
                switch ((BookType)Enum.Parse(typeof(BookType), val.ToString()))
                {
                    case BookType.Book08:
                        return "Книга Покупок (08)";

                    case BookType.Book09:
                        return "Книга Продаж (09)";

                    case BookType.Book10:
                        return "Журнал Выставленных СФ (10)";

                    case BookType.Book11:
                        return "Журнал Полученных СФ (11)";

                    default:
                        return val.ToString();
                }
            }
            if (t == typeof(VersionSbis))
            {
                switch ((VersionSbis)Enum.Parse(typeof(VersionSbis), val.ToString()))
                {
                    case VersionSbis.v5_08:
                        return "5.08 (действующий с 01.07.21)";

                    case VersionSbis.v5_07:
                        return "5.07 (устарел 01.10.20 - 30.06.21)";

                    case VersionSbis.v5_06:
                        return "5.06 (устарел 01.01.19 - 30.09.21)";

                    default:
                        return val.ToString();
                }
            }

            return val.ToString();
        }
    }
}
