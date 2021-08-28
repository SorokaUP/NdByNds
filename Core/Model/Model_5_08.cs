using System;
using System.Text;

namespace Core.Model
{
    public class Model_5_08 : ModelMaster
    {
        #region Настройки версии схемы
        /// <summary>
        /// Описание книги покупок
        /// </summary>
        public sealed class Map08 : IMap
        {
            public override string Tag { get { return "КнигаПокуп"; } }                
            public override string TagLine { get { return "КнПокСтр"; } }
            public override string PathToFileXSD { get { return @"G:\Work\TaxDeclaration\xsd\8_5.08_Сбис.xsd"; } }
            public override int LineStartReadExcel { get { return 7; } }
            public override string SumTag { get { return "КнПокСтр"; } }
            public override string[] SumFields { get { return new string[]
                { "СтоимПокупВ", "СумНДСВыч" }; } }

            public const byte ПорядковыйНомер = 1;
            public const byte КодВидОпер = 2;
            public const byte НомерИДатаСФПрод = 3;
            public const byte НомерИДатаИспрСФПрод = 4;
            public const byte НомерИДатаКоррСФПрод = 5;
            public const byte НомерИДатаИспрКоррСФПрод = 6;
            public const byte НомерИДатаДокПодтвОпл = 7;
            public const byte ДатаПринятияНаУчетТоваров = 8;
            public const byte НаименованиеПрод = 9;
            public const byte ИннКппПрод = 10;
            public const byte СведОПосредНаименование = 11;
            public const byte СведОПосредИннКпп = 12;
            public const byte НомерТаможДекларации = 13;
            public const byte НаимИКодВалюты = 14;
            public const byte СтоимостьПокВклНДС = 15;
            public const byte СуммаНДС = 16;
        }

        /// <summary>
        /// Описание книги продаж
        /// </summary>
        public sealed class Map09 : IMap
        {
            public override string Tag { get { return "КнигаПрод"; } }
            public override string TagLine { get { return "КнПродСтр"; } }
            public override string PathToFileXSD { get { return @"G:\Work\TaxDeclaration\xsd\9_5.08_Сбис.xsd"; } }
            public override int LineStartReadExcel { get { return 7; } }
            public override string SumTag { get { return "КнПродСтр"; } }
            public override string[] SumFields { get { return new string[]
                { "СтоимПродСФВ", "СтоимПродСФ", "СтоимПродСФ20", "СтоимПродСФ18", "СтоимПродСФ10", "СтоимПродСФ0", "СумНДССФ20", "СумНДССФ18", "СумНДССФ10", "СтоимПродОсв" }; } }

            public const byte ПорядковыйНомер = 1;
            public const byte КодВидОпер = 2;
            public const byte НомерИДатаСФПрод = 3;
            public const byte РегНомТаможДекларации = 4;
            public const byte КодВидТовара = 5;
            public const byte НомерИДатаИспрСФПрод = 6;
            public const byte НомерИДатаКоррСФПрод = 7;
            public const byte НомерИДатаИспрКоррСФПрод = 8;
            public const byte НаименованиеПокуп = 9;
            public const byte ИннКппПокуп = 10;
            public const byte НаименованиеПоср = 11;
            public const byte ИннКппПоср = 12;
            public const byte НомерИДатаДокПодтвОпл = 13;
            public const byte НаимИКодВалюты = 14;
            public const byte СтоимПродВВалютеСФ = 15;
            public const byte СтоимПродВРублях = 16;
            public const byte СуммаПродажБезНДС20 = 17;
            public const byte СуммаПродажБезНДС18 = 18;
            public const byte СуммаПродажБезНДС10 = 19;
            public const byte СуммаПродажБезНДС0 = 20;
            public const byte СуммаНДС20 = 21;
            public const byte СуммаНДС18 = 22;
            public const byte СуммаНДС10 = 23;
            public const byte СуммаНДС0 = 24;
        }

        /// <summary>
        /// Описание журнала выставленных СФ
        /// </summary>
        public sealed class Map10 : IMap
        {
            public override string Tag { get { return "ЖУчВыстСчФ"; } }
            public override string TagLine { get { return "ЖУчВыстСчФСтр"; } }
            public override string PathToFileXSD { get { return @"G:\Work\TaxDeclaration\xsd\10_5.08_Сбис.xsd"; } }
            public override int LineStartReadExcel { get { return 6; } }
            public override string SumTag { get { return "СвСчФОтПрод"; } }
            public override string[] SumFields { get { return new string[]
                { "СтоимТовСчФВс", "СумНДССчФ", "РазСтКСчФУм", "РазСтКСчФУв", "РазНДСКСчФУм", "РазНДСКСчФУв" }; } }

            public const byte ПорядковыйНомер = 1;
            public const byte ДатаВыставления = 2;
            public const byte КодВидОпер = 3;
            public const byte НомерИДатаСФ = 4;
            public const byte НомерИДатаИспрСФ = 5;
            public const byte НомерИДатаКоррСФ = 6;
            public const byte НомерИДатаИспрКоррСФ = 7;
            public const byte НаименованиеПокуп = 8;
            public const byte ИннКппПокуп = 9;
            public const byte ПосредНаименованиеПрод = 10;
            public const byte ПосредИннКппПрод = 11;
            public const byte ПосредНомерИДатаСФОтПрод = 12;
            public const byte НаимИКодВалюты = 13;
            public const byte СтоимПоСФ = 14;
            public const byte СтоимНДС = 15;
            public const byte РазницаВклНДСУменьшение = 16;
            public const byte РазницаВклНДСУвеличение = 17;
            public const byte РазницаНДСУменьшение = 18;
            public const byte РазницаНДСУвеличение = 19;
        }

        /// <summary>
        /// Описание журнала полученных СФ
        /// </summary>
        public sealed class Map11 : IMap
        {
            public override string Tag { get { return "ЖУчПолучСчФ"; } }
            public override string TagLine { get { return "ЖУчПолучСчФСтр"; } }
            public override string PathToFileXSD { get { return @"G:\Work\TaxDeclaration\xsd\11_5.08_Сбис.xsd"; } }
            public override int LineStartReadExcel { get { return 6; } }
            public override string SumTag { get { return "ЖУчПолучСчФСтр"; } }
            public override string[] SumFields { get { return new string[]
                { "СтоимТовСчФВс", "СумНДССчФ", "РазСтКСчФУм", "РазСтКСчФУв", "РазНДСКСчФУм", "РазНДСКСчФУв" }; } }

            public const byte ПорядковыйНомер = 1;
            public const byte ДатаПолучения = 2;
            public const byte КодВидОпер = 3;
            public const byte НомерИДатаСФ = 4;
            public const byte НомерИДатаИспрСФ = 5;
            public const byte НомерИДатаКоррСФ = 6;
            public const byte НомерИДатаИспрКоррСФ = 7;
            public const byte НаименованиеПрод = 8;
            public const byte ИннКппПрод = 9;
            public const byte ПосредНаименованиеКомиссионера = 10;
            public const byte ПосредИннКппКомиссионера = 11;
            public const byte КодВидСделки = 12;
            public const byte НаимИКодВалюты = 13;
            public const byte СтоимПоСФ = 14;
            public const byte СтоимНДС = 15;
            public const byte РазницаВклНДСУменьшение = 16;
            public const byte РазницаВклНДСУвеличение = 17;
            public const byte РазницаНДСУменьшение = 18;
            public const byte РазницаНДСУвеличение = 19;
        }
        #endregion

        public Model_5_08(BookType bookType, byte correctNum) 
            : base(bookType, correctNum, "5.08", new Map08(), new Map09(), new Map10(), new Map11())
        {
            res = new StringBuilder();
        }

        #region Начало / Конец документа
        public override string GetHeader()
        {
            return (
                //$"<?xml version=\"1.0\" encoding=\"windows-1251\" ?>" +
                $"<Файл {"ИдФайл".AsAttr(fileName)} {"ВерсПрог".AsAttr(this.GetType().Name)} {"ВерсФорм".AsAttr(versionName)}>" +
                $"<Документ {"Индекс".AsAttr(GenBookIndex())} {"НомКорр".AsAttr(correctNum)} {$"ПризнСвед{(int)bookType}".AsAttr("0")}>" +
                (
                (bookType is BookType.Book08) ? $"<{map08.Tag} {"СумНДСВсКПк".AsAttr("0")}>" :
                (bookType is BookType.Book09) ? $"<{map09.Tag}>" :
                (bookType is BookType.Book10) ? $"<{map10.Tag}>" :
                (bookType is BookType.Book11) ? $"<{map11.Tag}>" : 
                "")).ClearTrash();
        }

        public override string GetFooter()
        {
            return ((
               (bookType is BookType.Book08) ? $"</{map08.Tag}>" :
               (bookType is BookType.Book09) ? $"</{map09.Tag}>" :
               (bookType is BookType.Book10) ? $"</{map10.Tag}>" :
               (bookType is BookType.Book11) ? $"</{map11.Tag}>" :
               "") + "</Документ></Файл>").ClearTrash();
        }
        #endregion

        //====================================================================================================

        #region  08. Книга Покупок
        public override string GetBodyBook08(object[] data) 
        {
            try
            {
                res.Clear();
                CheckAndEditBook08(ref data);

                res.Add($"<{map08.TagLine}");
                res.Add("НомерПор".AsAttr(GetNumberLine()));
                res.Add(data[Map08.НомерИДатаСФПрод]?.ToString().AsAttrDocNumDate("НомСчФПрод", "ДатаСчФПрод"));
                res.Add(data[Map08.НомерИДатаИспрСФПрод]?.ToString().AsAttrDocNumDate("НомИспрСчФ", "ДатаИспрСчФ"));
                res.Add(data[Map08.НомерИДатаКоррСФПрод]?.ToString().AsAttrDocNumDate("НомКСчФПрод", "ДатаКСчФПрод"));
                res.Add(data[Map08.НомерИДатаИспрКоррСФПрод]?.ToString().AsAttrDocNumDate("НомИспрКСчФ", "ДатаИспрКСчФ"));
                res.Add("СтоимПокупВ".AsAttr(data.AsDec(Map08.СтоимостьПокВклНДС)));
                res.Add("СумНДСВыч".AsAttr(data.AsDec(Map08.СуммаНДС)));
                res.Add($">");

                res.Add("КодВидОпер".AsSingleTag(data[Map08.КодВидОпер]));
                res.Add(GenerateDocSubmit(data, Map08.НомерИДатаДокПодтвОпл));
                res.Add("ДатаУчТов".AsSingleTag(data[Map08.ДатаПринятияНаУчетТоваров] ?? "", Feature.НеОбязательно));
                res.Add(GenerateSved("СвПрод", data, Map08.ИннКппПрод));
                res.Add(GenerateGTD(data, Map08.НомерТаможДекларации));

                res.Add($"</{map08.TagLine}>");
            }
            catch (Exception e)
            {
                Helper.Log($"Провал формирования строки {map08.TagLine}: {e.Message}", LogMode.Ошибка);
                res.Clear();
            }
            
            return res.ToString().ClearTrash();
        }
        private void CheckAndEditBook08(ref object[] data)
        {
            // При КОД ОПЕРАЦИИ 18 - должны быть заполнены графы 3 и 5
            if ((data[Map08.КодВидОпер]?.ToString().Trim()).Equals("18"))
            {
                string dataSf = data[Map08.НомерИДатаСФПрод]?.ToString();
                string dataCorrSf = data[Map08.НомерИДатаКоррСФПрод]?.ToString();

                if (string.IsNullOrEmpty(dataSf) && !string.IsNullOrEmpty(dataCorrSf))
                    data[Map08.НомерИДатаСФПрод] = dataCorrSf;
                if (!string.IsNullOrEmpty(dataSf) && string.IsNullOrEmpty(dataCorrSf))
                    data[Map08.НомерИДатаКоррСФПрод] = dataSf;
            }
        }
        #endregion
        
        #region 09. Книга Продаж
        public override string GetBodyBook09(object[] data)
        {
            try
            {
                res.Clear();
                CheckAndEditBook09(ref data);

                res.Add($"<{map09.TagLine}");
                res.Add("НомерПор".AsAttr(GetNumberLine()));
                res.Add(data[Map09.НомерИДатаСФПрод]?.ToString().AsAttrDocNumDate("НомСчФПрод", "ДатаСчФПрод", ';'));
                res.Add(data[Map09.НомерИДатаИспрСФПрод]?.ToString().AsAttrDocNumDate("НомИспрСчФ", "ДатаИспрСчФ", ';'));
                res.Add(data[Map09.НомерИДатаКоррСФПрод]?.ToString().AsAttrDocNumDate("НомКСчФПрод", "ДатаКСчФПрод", ';'));
                res.Add(data[Map09.НомерИДатаИспрКоррСФПрод]?.ToString().AsAttrDocNumDate("НомИспрКСчФ", "ДатаИспрКСчФ", ';'));
                res.Add("ОКВ".AsAttr(data.ValSecond(Map09.НаимИКодВалюты, ';').AsDec()));
                res.Add("СтоимПродСФВ".AsAttr(data.AsDec(Map09.СтоимПродВВалютеСФ)));
                res.Add("СтоимПродСФ".AsAttr(data.AsDec(Map09.СтоимПродВРублях)));
                res.Add("СтоимПродСФ20".AsAttr(data.AsDec(Map09.СуммаПродажБезНДС20)));
                res.Add("СтоимПродСФ18".AsAttr(data.AsDec(Map09.СуммаПродажБезНДС18)));
                res.Add("СтоимПродСФ10".AsAttr(data.AsDec(Map09.СуммаПродажБезНДС10)));
                res.Add("СтоимПродСФ0".AsAttr(data.AsDec(Map09.СуммаПродажБезНДС0)));
                res.Add("СумНДССФ20".AsAttr(data.AsDec(Map09.СуммаНДС20)));
                res.Add("СумНДССФ18".AsAttr(data.AsDec(Map09.СуммаНДС18)));
                res.Add("СумНДССФ10".AsAttr(data.AsDec(Map09.СуммаНДС10)));
                res.Add("СтоимПродОсв".AsAttr(data.AsDec(Map09.СуммаНДС0)));
                res.Add($">");

                res.Add("КодВидОпер".AsSingleTag(data[Map09.КодВидОпер]));
                res.Add(GenerateGTD(data, Map09.РегНомТаможДекларации));
                res.Add("КодВидТовар".AsSingleTag(data[Map09.КодВидТовара]));
                res.Add(GenerateDocSubmit(data, Map09.НомерИДатаДокПодтвОпл));
                res.Add(GenerateSved("СвПокуп", data, Map09.ИннКппПокуп));
                res.Add(GenerateSved("СвПос", data, Map09.ИннКппПоср));
                res.Add($"</{map09.TagLine}>");
            }
            catch (Exception e)
            {
                Helper.Log($"Провал формирования строки {map09.TagLine}: {e.Message}", LogMode.Ошибка);
                res.Clear();
            }

            return res.ToString().ClearTrash();
        }
        private void CheckAndEditBook09(ref object[] data)
        {
            // При КОД ОПЕРАЦИИ 18 - должны быть заполнены графы 3 и 7
            if ((data[Map09.НомерИДатаСФПрод]?.ToString()[0]).Equals('-'))
            {
                data[Map09.НомерИДатаСФПрод] = "0000" + data[Map09.НомерИДатаСФПрод];
            }
            if ((data[Map09.КодВидОпер]?.ToString().Trim()).Equals("18"))
            {
                string dataSf = data[Map09.НомерИДатаСФПрод]?.ToString();
                string dataCorrSf = data[Map09.НомерИДатаКоррСФПрод]?.ToString();

                if (string.IsNullOrEmpty(dataSf) && !string.IsNullOrEmpty(dataCorrSf))
                    data[Map09.НомерИДатаСФПрод] = dataCorrSf;
                if (!string.IsNullOrEmpty(dataSf) && string.IsNullOrEmpty(dataCorrSf))
                    data[Map09.НомерИДатаКоррСФПрод] = dataSf;
            }
        }
        #endregion
        
        #region 10. Журнал Выставленных СФ
        public override string GetBodyBook10(object[] data)
        {
            try
            {
                res.Clear();
                res.Add($"<{map10.TagLine}");
                res.Add("НомерПор".AsAttr(GetNumberLine()));
                res.Add(data[Map10.НомерИДатаСФ]?.ToString().AsAttrDocNumDate("НомСчФПрод", "ДатаСчФПрод"));
                res.Add(data[Map10.НомерИДатаИспрСФ]?.ToString().AsAttrDocNumDate("НомИспрСчФ", "ДатаИспрСчФ"));
                res.Add(data[Map10.НомерИДатаКоррСФ]?.ToString().AsAttrDocNumDate("НомКСчФПрод", "ДатаКСчФПрод"));
                res.Add(data[Map10.НомерИДатаИспрКоррСФ]?.ToString().AsAttrDocNumDate("НомИспрКСчФ", "ДатаИспрКСчФ"));
                res.Add($">");                

                res.Add("КодВидОпер".AsSingleTag(data[Map10.КодВидОпер]));
                res.Add(GenerateSved("СвПокуп", data, Map10.ИннКппПокуп));

                res.Add($"<СвСчФОтПрод");                
                res.Add(data[Map10.ПосредНомерИДатаСФОтПрод]?.ToString().AsAttrDocNumDate("НомСчФОтПрод", "ДатаСчФОтПрод"));
                res.Add("СтоимТовСчФВс".AsAttr(data.AsDec(Map10.СтоимПоСФ)));
                res.Add("СумНДССчФ".AsAttr(data.AsDec(Map10.СтоимНДС)));
                res.Add("РазСтКСчФУм".AsAttr(data.AsDec(Map10.РазницаВклНДСУменьшение)));
                res.Add("РазСтКСчФУв".AsAttr(data.AsDec(Map10.РазницаВклНДСУвеличение)));
                res.Add("РазНДСКСчФУм".AsAttr(data.AsDec(Map10.РазницаНДСУменьшение)));
                res.Add("РазНДСКСчФУв".AsAttr(data.AsDec(Map10.РазницаНДСУвеличение)));
                res.Add($">");

                res.Add(GenerateSved("СвПрод", data, Map10.ПосредИннКппПрод));

                res.Add($"</СвСчФОтПрод>");
                res.Add($"</{map10.TagLine}>");
            }
            catch (Exception e)
            {
                Helper.Log($"Провал формирования строки {map10.TagLine}: {e.Message}", LogMode.Ошибка);
                res.Clear();
            }

            return res.ToString().ClearTrash();
        }
        #endregion

        #region 11. Журнал Полученных СФ
        public override string GetBodyBook11(object[] data)
        {
            try
            {
                res.Clear();
                res.Add($"<{map11.TagLine}");
                res.Add("НомерПор".AsAttr(GetNumberLine()));
                res.Add(data[Map11.НомерИДатаСФ]?.ToString().AsAttrDocNumDate("НомСчФПрод", "ДатаСчФПрод"));
                res.Add(data[Map11.НомерИДатаИспрСФ]?.ToString().AsAttrDocNumDate("НомИспрСчФ", "ДатаИспрСчФ"));
                res.Add(data[Map11.НомерИДатаКоррСФ]?.ToString().AsAttrDocNumDate("НомКСчФПрод", "ДатаКСчФПрод"));
                res.Add(data[Map11.НомерИДатаИспрКоррСФ]?.ToString().AsAttrDocNumDate("НомИспрКСчФ", "ДатаИспрКСчФ"));
                res.Add("КодВидСд".AsAttr(data[Map11.КодВидСделки]?.ToString()));
                res.Add("СтоимТовСчФВс".AsAttr(data.AsDec(Map11.СтоимПоСФ)));
                res.Add("СумНДССчФ".AsAttr(data.AsDec(Map11.СтоимНДС)));
                res.Add("РазСтКСчФУм".AsAttr(data.AsDec(Map11.РазницаВклНДСУменьшение)));
                res.Add("РазСтКСчФУв".AsAttr(data.AsDec(Map11.РазницаВклНДСУвеличение)));
                res.Add("РазНДСКСчФУм".AsAttr(data.AsDec(Map11.РазницаНДСУменьшение)));
                res.Add("РазНДСКСчФУв".AsAttr(data.AsDec(Map11.РазницаНДСУвеличение)));
                res.Add($">");

                res.Add("КодВидОпер".AsSingleTag(data[Map11.КодВидОпер]));
                res.Add(GenerateSved("СвПрод", data, Map11.ИннКппПрод));
                res.Add(GenerateSved("СвКомис", data, Map11.ПосредИннКппКомиссионера));

                res.Add($"</{map11.TagLine}>");
            }
            catch (Exception e)
            {
                Helper.Log($"Провал формирования строки {map11.TagLine}: {e.Message}", LogMode.Ошибка);
                res.Clear();
            }

            return res.ToString().ClearTrash();
        }
        #endregion
        
        //====================================================================================================

        #region Вспомогательные
        /// <summary>
        /// Сведения о tagName
        /// </summary>
        private string GenerateSved(string tagName, object[] data, byte i)
        {
            //Пример: 7719022542/774950001

            StringBuilder res = new StringBuilder();
            if (data[i] != null)
            {
                string[] arr = data[i].ToString().Split('/');
                string inn = arr?[0];
                string s;
                if (inn != null)
                {
                    if (inn.Length.Equals(10) && arr.Length > 1)
                    {
                        s = $"<СведЮЛ {"ИННЮЛ".AsAttr(inn ?? "")} {"КПП".AsAttr(arr[1] ?? "")} />";
                    }
                    else
                    {
                        s = $"<СведИП {"ИННФЛ".AsAttr(inn ?? "")} />";
                    }
                    if (!string.IsNullOrEmpty(s))
                    {
                        res.Add(tagName.AsSingleTag(s));
                    }
                }
            }
            return res.ToString();
        }
        /// <summary>
        /// Сведения о регистрационном номере декларации на товары или о товаре, подлежащем прослеживаемости
        /// </summary>
        private string GenerateGTD(object[] data, byte i)
        {
            //Пример: 10002010/120718/0048474;10002010/120718/0048474 

            StringBuilder res = new StringBuilder();
            if (data[i] != null)
            {
                foreach (string gtd in data[i].ToString().Split(';'))
                {
                    if (!string.IsNullOrEmpty(gtd))
                    {
                        if (gtd.CheckGTD())
                        {
                            res.Add($"<СвРегНом {"РегНомПросл".AsAttr(gtd)} />");
                        }
                    }
                }
            }
            return res.ToString().ClearTrash();
        }
        /// <summary>
        /// Сведения о документе, подтверждающем уплату налога
        /// </summary>
        private string GenerateDocSubmit(object[] data, byte i)
        {
            //Пример: ПР_2053;15.04.2020,ПР_2255; 23.04.2020,ПР_3185; 26.06.2020

            StringBuilder res = new StringBuilder();
            string[] arrOwner = data.ValArray(i, ',');
            if (arrOwner != null)
            {
                foreach (string item in arrOwner)
                {
                    string[] arrParent = item.Split(';');
                    if (arrParent.Length > 1)
                    {
                        string docNum = arrParent[0].ClearTrash();
                        string docDate = arrParent[1];
                        res.Add($"<ДокПдтвУпл {"ДатаДокПдтвУпл".AsAttr(docDate)} {"НомДокПдтвУпл".AsAttr(docNum)} />");
                    }
                }

            }
            return res.ToString().ClearTrash();
        }
        #endregion
    }
}