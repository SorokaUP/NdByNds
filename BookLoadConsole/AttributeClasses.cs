namespace BookLoadConsole
{
    class AttributeClasses
    {
        /// <summary>
        /// Класс подготовки атрибутов узла <КнПокСтр>
        /// </summary>
        public class атрибутыКнПокСтр : Book
        {
            #region Атрибуты
            /// <summary>
            /// Обязательный-N(12)Целое
            /// </summary>
            public string НомерПор { get; set; }
            /// <summary>
            /// Обязательный-Т(1000)Строка
            /// </summary>
            public string НомСчФПрод { get; set; }
            /// <summary>
            /// НеОбязательный-D(10)Дата
            /// </summary>
            public string ДатаСчФПрод { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(3)Целое
            /// </summary>
            public string НомИспрСчФ { get; set; }
            /// <summary>
            /// УсловноОбязательный-D(10)Дата
            /// </summary>
            public string ДатаИспрСчФ { get; set; }
            /// <summary>
            /// УсловноОбязательный-Т(256)Строка
            /// </summary>
            public string НомКСчФПрод { get; set; }
            /// <summary>
            /// УсловноОбязательный-D(10)Дата
            /// </summary>
            public string ДатаКСчФПрод { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(3)Целое
            /// </summary>
            public string НомИспрКСчФ { get; set; }
            /// <summary>
            /// УсловноОбязательный-D(10)Дата
            /// </summary>
            public string ДатаИспрКСчФ { get; set; }
            /// <summary>
            /// НеОбязательный-Т(=3)Строка
            /// </summary>
            public string ОКВ { get; set; }
            /// <summary>
            /// Обязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПокупВ { get; set; }
            /// <summary>
            /// Обязательный-N(19.2)Вещественное
            /// </summary>
            public string СумНДСВыч { get; set; }
            #endregion

            /// <summary>
            /// Получить строку атрибутов в формате XML
            /// </summary>
            /// <returns>Строка атрибутов вида: атрибут="значение"</returns>
            public string ПолучитьАтрибуты()
            {
                string res = "";

                // * - Обязательный
                // ! - Условно-обязательный

                res += (St.ValAttr(НомерПор))     ? "НомерПор=\"" + НомерПор + "\" "                           : "НомерПор=\"0\" "; // *
                res += (St.ValAttr(НомСчФПрод))   ? "НомСчФПрод=\"" + НомСчФПрод + "\" "                       : "НомСчФПрод=\"0\" "; // *
                res += (St.ValAttr(ДатаСчФПрод))  ? "ДатаСчФПрод=\"" + St.GetDateFormat(ДатаСчФПрод) + "\" "   : "";
                res += (St.ValAttr(НомИспрСчФ))   ? "НомИспрСчФ=\"" + НомИспрСчФ + "\" "                       : ""; // !
                res += (St.ValAttr(ДатаИспрСчФ))  ? "ДатаИспрСчФ=\"" + St.GetDateFormat(ДатаИспрСчФ) + "\" "   : ""; // !
                res += (St.ValAttr(НомКСчФПрод))  ? "НомКСчФПрод=\"" + НомКСчФПрод + "\" "                     : ""; // !
                res += (St.ValAttr(ДатаКСчФПрод)) ? "ДатаКСчФПрод=\"" + St.GetDateFormat(ДатаКСчФПрод) + "\" " : ""; // !
                res += (St.ValAttr(НомИспрКСчФ))  ? "НомИспрКСчФ=\"" + НомИспрКСчФ + "\" "                     : ""; // !
                res += (St.ValAttr(ДатаИспрКСчФ)) ? "ДатаИспрКСчФ=\"" + St.GetDateFormat(ДатаИспрКСчФ) + "\" " : ""; // !
                res += (St.ValAttr(ОКВ))          ? "ОКВ=\"" + ОКВ + "\" "                                     : "";
                res += (St.ValAttr(СтоимПокупВ))  ? "СтоимПокупВ=\"" + St.GetDecFormat(СтоимПокупВ) + "\" "    : "СтоимПокупВ=\"0\" "; // *
                res += (St.ValAttr(СумНДСВыч))    ? "СумНДСВыч=\"" + St.GetDecFormat(СумНДСВыч) + "\" "        : "СумНДСВыч=\"0\" "; // *

                return res;
            }
        }

        /// <summary>
        /// Класс подготовки атрибутов узла <КнПродСтр>
        /// </summary>
        public class атрибутыКнПродСтр : Book
        {
            #region Атрибуты
            /// <summary>
            /// Номер по порядку. Обязательный-N(12)Целое
            /// </summary>
            public string НомерПор { get; set; }
            /// <summary>
            /// Номер счета-фактуры продавца. Обязательный-Т(1000)Строка
            /// </summary>
            public string НомСчФПрод { get; set; }
            /// <summary>
            /// Дата счета-фактуры продавца. НеОбязательный-D(10)Дата
            /// </summary>
            public string ДатаСчФПрод { get; set; }
            /// <summary>
            /// Номер исправления счета-фактуры. УсловноОбязательный-N(3)Целое
            /// </summary>
            public string НомИспрСчФ { get; set; }
            /// <summary>
            /// Дата исправления счета-фактуры. УсловноОбязательный-D(10)Дата
            /// </summary>
            public string ДатаИспрСчФ { get; set; }
            /// <summary>
            /// Номер корректировочного счета-фактуры продавца. УсловноОбязательный-Т(256)Строка
            /// </summary>
            public string НомКСчФПрод { get; set; }
            /// <summary>
            /// Дата корректировочного счета-фактуры. УсловноОбязательный-D(10)Дата
            /// </summary>
            public string ДатаКСчФПрод { get; set; }
            /// <summary>
            /// Номер исправления корректировочного счета-фактуры. УсловноОбязательный-N(3)Целое
            /// </summary>
            public string НомИспрКСчФ { get; set; }
            /// <summary>
            /// Дата исправления корректировочного счета-фактуры. УсловноОбязательный-D(10)Дата
            /// </summary>
            public string ДатаИспрКСчФ { get; set; }
            /// <summary>
            /// Код валюты. Обязательный-Т(=3)Строка
            /// </summary>
            public string ОКВ { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродСФВ { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродСФ { get; set; }
            /// <summary>
            /// НеОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродСФ20 { get; set; }
            /// <summary>
            /// НеОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродСФ18 { get; set; }
            /// <summary>
            /// НеОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродСФ10 { get; set; }
            /// <summary>
            /// НеОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродСФ0 { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СумНДССФ20 { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СумНДССФ18 { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СумНДССФ10 { get; set; }
            /// <summary>
            /// УсловноОбязательный-N(19.2)Вещественное
            /// </summary>
            public string СтоимПродОсв { get; set; }
            #endregion

            /// <summary>
            /// Получить строку атрибутов в формате XML
            /// </summary>
            /// <returns>Строка атрибутов вида: атрибут="значение"</returns>
            public string ПолучитьАтрибуты()
            {
                string res = "";

                // * - Обязательный
                // ! - Условно-обязательный

                res += (St.ValAttr(НомерПор))      ? "НомерПор=\"" + НомерПор + "\" "                            : "НомерПор=\"0\" "; // *
                res += (St.ValAttr(НомСчФПрод))    ? "НомСчФПрод=\"" + НомСчФПрод + "\" "                        : "НомСчФПрод=\"0\" "; // *
                res += (St.ValAttr(ДатаСчФПрод))   ? "ДатаСчФПрод=\"" + St.GetDateFormat(ДатаСчФПрод) + "\" "    : "";
                res += (St.ValAttr(НомИспрСчФ))    ? "НомИспрСчФ=\"" + НомИспрСчФ + "\" "                        : ""; // !
                res += (St.ValAttr(ДатаИспрСчФ))   ? "ДатаИспрСчФ=\"" + St.GetDateFormat(ДатаИспрСчФ) + "\" "    : ""; // !
                res += (St.ValAttr(НомКСчФПрод))   ? "НомКСчФПрод=\"" + НомКСчФПрод + "\" "                      : ""; // !
                res += (St.ValAttr(ДатаКСчФПрод))  ? "ДатаКСчФПрод=\"" + St.GetDateFormat(ДатаКСчФПрод) + "\" "  : ""; // !
                res += (St.ValAttr(НомИспрКСчФ))   ? "НомИспрКСчФ=\"" + НомИспрКСчФ + "\" "                      : ""; // !
                res += (St.ValAttr(ДатаИспрКСчФ))  ? "ДатаИспрКСчФ=\"" + St.GetDateFormat(ДатаИспрКСчФ) + "\" "  : ""; // !
                res += (St.ValAttr(ОКВ))           ? "ОКВ=\"" + ОКВ + "\" "                                      : "";
                res += (St.ValAttr(СтоимПродСФВ))  ? "СтоимПродСФВ=\"" + St.GetDecFormat(СтоимПродСФВ) + "\" "   : ""; // !
                res += (St.ValAttr(СтоимПродСФ))   ? "СтоимПродСФ=\"" + St.GetDecFormat(СтоимПродСФ) + "\" "     : "СтоимПродСФ=\"0\" "; // !
                res += (St.ValAttr(СтоимПродСФ20)) ? "СтоимПродСФ20=\"" + St.GetDecFormat(СтоимПродСФ20) + "\" " : "";
                res += (St.ValAttr(СтоимПродСФ18)) ? "СтоимПродСФ18=\"" + St.GetDecFormat(СтоимПродСФ18) + "\" " : "";
                res += (St.ValAttr(СтоимПродСФ10)) ? "СтоимПродСФ10=\"" + St.GetDecFormat(СтоимПродСФ10) + "\" " : "";
                res += (St.ValAttr(СтоимПродСФ0))  ? "СтоимПродСФ0=\"" + St.GetDecFormat(СтоимПродСФ0) + "\" "   : "";
                res += (St.ValAttr(СумНДССФ20))    ? "СумНДССФ20=\"" + St.GetDecFormat(СумНДССФ20) + "\" "       : ""; // !
                res += (St.ValAttr(СумНДССФ18))    ? "СумНДССФ18=\"" + St.GetDecFormat(СумНДССФ18) + "\" "       : ""; // !
                res += (St.ValAttr(СумНДССФ10))    ? "СумНДССФ10=\"" + St.GetDecFormat(СумНДССФ10) + "\" "       : ""; // !
                res += (St.ValAttr(СтоимПродОсв))  ? "СтоимПродОсв=\"" + St.GetDecFormat(СтоимПродОсв) + "\" "   : ""; // !

                return res;
            }
        }

        /// <summary>
        /// Класс подготовки атрибутов узла <ЖУчПолучСчФСтр>
        /// </summary>
        public class атрибутыЖУчПолучСчФСтр : Book
        {
            #region Атрибуты
            /// <summary>
            /// Обязательный N(12) Целое
            /// </summary>
            public string НомерПор { get; set; }
            /// <summary>
            /// Обязательный T(1000) Строка
            /// </summary>
            public string НомСчФПрод { get; set; }
            /// <summary>
            /// Обязательный D(10) Дата
            /// </summary>
            public string ДатаСчФПрод { get; set; }
            /// <summary>
            /// Условно-Обязательный N(3) Целое
            /// </summary>
            public string НомИспрСчФ { get; set; }
            /// <summary>
            /// Условно-Обязательный D(10) Дата
            /// </summary>
            public string ДатаИспрСчФ { get; set; }
            /// <summary>
            /// Условно-Обязательный T(256) Строка
            /// </summary>
            public string НомКСчФПрод { get; set; }
            /// <summary>
            /// Условно-Обязательный D(10) Дата
            /// </summary>
            public string ДатаКСчФПрод { get; set; }
            /// <summary>
            /// Условно-Обязательный N(3) Целое
            /// </summary>
            public string НомИспрКСчФ { get; set; }
            /// <summary>
            /// Условно-Обязательный D(10) Дата
            /// </summary>
            public string ДатаИспрКСчФ { get; set; }
            /// <summary>
            /// Обязательный Перечисление
            /// </summary>
            public string КодВидСд { get; set; }
            /// <summary>
            /// Необязательный T(=3) Строка
            /// </summary>
            public string ОКВ { get; set; }
            /// <summary>
            /// Обязательный N(19.2) Вещественное
            /// </summary>
            public string СтоимТовСчФВс { get; set; }
            /// <summary>
            /// Условно-Обязательный N(19.2) Вещественное
            /// </summary>
            public string СумНДССчФ { get; set; }
            /// <summary>
            /// Необязательный N(19.2) Вещественное
            /// </summary>
            public string РазСтКСчФУм { get; set; }
            /// <summary>
            /// Необязательный N(19.2) Вещественное
            /// </summary>
            public string РазСтКСчФУв { get; set; }
            /// <summary>
            /// Условно-Обязательный N(19.2) Вещественное
            /// </summary>
            public string РазНДСКСчФУм { get; set; }
            /// <summary>
            /// Условно-Обязательный N(19.2) Вещественное
            /// </summary>
            public string РазНДСКСчФУв { get; set; }
            #endregion

            /// <summary>
            /// Получить строку атрибутов в формате XML
            /// </summary>
            /// <returns>Строка атрибутов вида: атрибут="значение"</returns>
            public string ПолучитьАтрибуты()
            {
                string res = "";

                // * - Обязательный
                // ! - Условно-обязательный

                res += (St.ValAttr(НомерПор))      ? "НомерПор=\"" + НомерПор + "\" "                            : "НомерПор=\"0\" "; // *
                res += (St.ValAttr(НомСчФПрод))    ? "НомСчФПрод=\"" + НомСчФПрод + "\" "                        : "НомСчФПрод=\"0\" "; // *
                res += (St.ValAttr(ДатаСчФПрод))   ? "ДатаСчФПрод=\"" + St.GetDateFormat(ДатаСчФПрод) + "\" "    : "ДатаСчФПрод=\"01.01.2000\" "; // *
                res += (St.ValAttr(НомИспрСчФ))    ? "НомИспрСчФ=\"" + НомИспрСчФ + "\" "                        : ""; 
                res += (St.ValAttr(ДатаИспрСчФ))   ? "ДатаИспрСчФ=\"" + St.GetDateFormat(ДатаИспрСчФ) + "\" "    : ""; 
                res += (St.ValAttr(НомКСчФПрод))   ? "НомКСчФПрод=\"" + НомКСчФПрод + "\" "                      : ""; 
                res += (St.ValAttr(ДатаКСчФПрод))  ? "ДатаКСчФПрод=\"" + St.GetDateFormat(ДатаКСчФПрод) + "\" "  : ""; 
                res += (St.ValAttr(НомИспрКСчФ))   ? "НомИспрКСчФ=\"" + НомИспрКСчФ + "\" "                      : ""; 
                res += (St.ValAttr(ДатаИспрКСчФ))  ? "ДатаИспрКСчФ=\"" + St.GetDateFormat(ДатаИспрКСчФ) + "\" "  : "";
                res += (St.ValAttr(КодВидСд))      ? "КодВидСд=\"" + КодВидСд + "\" "                            : "КодВидСд=\"0\" "; // *
                res += (St.ValAttr(ОКВ))           ? "ОКВ=\"" + ОКВ + "\" "                                      : ""; 
                res += (St.ValAttr(СтоимТовСчФВс)) ? "СтоимТовСчФВс=\"" + St.GetDecFormat(СтоимТовСчФВс) + "\" " : "СтоимТовСчФВс=\"0\" "; // *
                res += (St.ValAttr(СумНДССчФ))     ? "СумНДССчФ=\"" + St.GetDecFormat(СумНДССчФ) + "\" "         : ""; 
                res += (St.ValAttr(РазСтКСчФУм))   ? "РазСтКСчФУм=\"" + St.GetDecFormat(РазСтКСчФУм) + "\" "     : ""; 
                res += (St.ValAttr(РазСтКСчФУв))   ? "РазСтКСчФУв=\"" + St.GetDecFormat(РазСтКСчФУв) + "\" "     : ""; 
                res += (St.ValAttr(РазНДСКСчФУм))  ? "РазНДСКСчФУм=\"" + St.GetDecFormat(РазНДСКСчФУм) + "\" "   : ""; 
                res += (St.ValAttr(РазНДСКСчФУв))  ? "РазНДСКСчФУв=\"" + St.GetDecFormat(РазНДСКСчФУв) + "\" "   : ""; 

                return res;
            }
        }

        /// <summary>
        /// Класс подготовки атрибутов узла <ЖУчВыстСчФСтр>
        /// </summary>
        public class атрибутыЖУчВыстСчФСтр : Book
        {
            #region Атрибуты
            /// <summary>
            /// Обязательный N(12) Целое
            /// </summary>
            public string НомерПор { get; set; }
            /// <summary>
            /// Обязательный T(1000) Строка
            /// </summary>
            public string НомСчФПрод { get; set; }
            /// <summary>
            /// Обязательный D(10) Дата
            /// </summary>
            public string ДатаСчФПрод { get; set; }
            /// <summary>
            /// Условно-Обязательный N(3) Целое
            /// </summary>
            public string НомИспрСчФ { get; set; }
            /// <summary>
            /// Условно-Обязательный D(10) Дата
            /// </summary>
            public string ДатаИспрСчФ { get; set; }
            /// <summary>
            /// Условно-Обязательный T(256) Строка
            /// </summary>
            public string НомКСчФПрод { get; set; }
            /// <summary>
            /// Условно-Обязательный D(10) Дата
            /// </summary>
            public string ДатаКСчФПрод { get; set; }
            /// <summary>
            /// Условно-Обязательный N(3) Целое
            /// </summary>
            public string НомИспрКСчФ { get; set; }
            /// <summary>
            /// Условно-Обязательный D(10) Дата
            /// </summary>
            public string ДатаИспрКСчФ { get; set; }
            #endregion

            /// <summary>
            /// Получить строку атрибутов в формате XML
            /// </summary>
            /// <returns>Строка атрибутов вида: атрибут="значение"</returns>
            public string ПолучитьАтрибуты()
            {
                string res = "";

                // * - Обязательный
                // ! - Условно-обязательный

                res += (St.ValAttr(НомерПор))     ? "НомерПор=\"" + НомерПор + "\" "                           : "НомерПор=\"0\" "; // *
                res += (St.ValAttr(НомСчФПрод))   ? "НомСчФПрод=\"" + НомСчФПрод + "\" "                       : "НомСчФПрод=\"0\" "; // *
                res += (St.ValAttr(ДатаСчФПрод))  ? "ДатаСчФПрод=\"" + St.GetDateFormat(ДатаСчФПрод) + "\" "   : "ДатаСчФПрод=\"01.01.2000\" "; // *
                res += (St.ValAttr(НомИспрСчФ))   ? "НомИспрСчФ=\"" + НомИспрСчФ + "\" "                       : "";
                res += (St.ValAttr(ДатаИспрСчФ))  ? "ДатаИспрСчФ=\"" + St.GetDateFormat(ДатаИспрСчФ) + "\" "   : "";
                res += (St.ValAttr(НомКСчФПрод))  ? "НомКСчФПрод=\"" + НомКСчФПрод + "\" "                     : ""; 
                res += (St.ValAttr(ДатаКСчФПрод)) ? "ДатаКСчФПрод=\"" + St.GetDateFormat(ДатаКСчФПрод) + "\" " : ""; 
                res += (St.ValAttr(НомИспрКСчФ))  ? "НомИспрКСчФ=\"" + НомИспрКСчФ + "\" "                     : "";
                res += (St.ValAttr(ДатаИспрКСчФ)) ? "ДатаИспрКСчФ=\"" + St.GetDateFormat(ДатаИспрКСчФ) + "\" " : "";

                return res;
            }
        }

        /// <summary>
        /// Класс подготовки атрибутов узла <СвСчФОтПрод>
        /// </summary>
        public class атрибутыСвСчФОтПрод : Book
        {
            #region Атрибуты
            /// <summary>
            /// Обязательный T(1000) Строка
            /// </summary>
            public string НомСчФОтПрод { get; set; }
            /// <summary>
            /// Обязательный D(10) Дата
            /// </summary>
            public string ДатаСчФОтПрод { get; set; }
            /// <summary>
            /// Необязательный T(=3) Строка
            /// </summary>
            public string ОКВ { get; set; }
            /// <summary>
            /// Необязательный N(19.2) Вещественное
            /// </summary>
            public string СтоимТовСчФВс { get; set; }
            /// <summary>
            /// Необязательный N(19.2) Вещественное
            /// </summary>
            public string СумНДССчФ { get; set; }
            /// <summary>
            /// Необязательный N(19.2) Вещественное
            /// </summary>
            public string РазСтКСчФУм { get; set; }
            /// <summary>
            /// Необязательный N(19.2) Вещественное
            /// </summary>
            public string РазСтКСчФУв { get; set; }
            /// <summary>
            /// Условно-Обязательный N(19.2) Вещественное
            /// </summary>
            public string РазНДСКСчФУм { get; set; }
            /// <summary>
            /// Условно-Обязательный N(19.2) Вещественное
            /// </summary>
            public string РазНДСКСчФУв { get; set; }
            #endregion

            /// <summary>
            /// Получить строку атрибутов в формате XML
            /// </summary>
            /// <returns>Строка атрибутов вида: атрибут="значение"</returns>
            public string ПолучитьАтрибуты()
            {
                string res = "";

                // * - Обязательный
                // ! - Условно-обязательный

                res += (St.ValAttr(НомСчФОтПрод))  ? "НомСчФОтПрод=\"" + НомСчФОтПрод + "\" "                     : ""; //"НомСчФОтПрод=\"0\" "; // *
                res += (St.ValAttr(ДатаСчФОтПрод)) ? "ДатаСчФОтПрод=\"" + St.GetDateFormat(ДатаСчФОтПрод) + "\" " : ""; //"ДатаСчФОтПрод=\"01.01.2000\" "; // *
                res += (St.ValAttr(ОКВ))           ? "ОКВ=\"" + ОКВ + "\" "                                       : "";
                res += (St.ValAttr(СтоимТовСчФВс)) ? "СтоимТовСчФВс=\"" + St.GetDecFormat(СтоимТовСчФВс) + "\" "  : "";
                res += (St.ValAttr(СумНДССчФ))     ? "СумНДССчФ=\"" + St.GetDecFormat(СумНДССчФ) + "\" "          : "";
                res += (St.ValAttr(РазСтКСчФУм))   ? "РазСтКСчФУм=\"" + St.GetDecFormat(РазСтКСчФУм) + "\" "      : "";
                res += (St.ValAttr(РазСтКСчФУв))   ? "РазСтКСчФУв=\"" + St.GetDecFormat(РазСтКСчФУв) + "\" "      : "";
                res += (St.ValAttr(РазНДСКСчФУм))  ? "РазНДСКСчФУм=\"" + St.GetDecFormat(РазНДСКСчФУм) + "\" "    : "";
                res += (St.ValAttr(РазНДСКСчФУв))  ? "РазНДСКСчФУв=\"" + St.GetDecFormat(РазНДСКСчФУв) + "\" "    : "";

                return res;
            }
        }
    }
}
