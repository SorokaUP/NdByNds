using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;

namespace BookLoadConsole
{
    public class Settings
    {
        const string DefaultBookFormat = "5.08";

        #region Свойства
        /// <summary>
        /// Разделитель
        /// </summary>
        public string Delimiter { get; set; }
        /// <summary>
        /// Строка начала считывания Excel-файла (9 - Книга продаж)
        /// </summary>
        public int RowStart9 { get; set; }
        /// <summary>
        /// Строка начала считывания Excel-файла (8 - Книга покупок)
        /// </summary>
        public int RowStart8 { get; set; }
        /// <summary>
        /// Строка начала считывания Excel-файла (10 - Журнал выставленных СФ)
        /// </summary>
        public int RowStart10 { get; set; }
        /// <summary>
        /// Строка начала считывания Excel-файла (11 - журнал полученных СФ)
        /// </summary>
        public int RowStart11 { get; set; }
        /// <summary>
        /// Флаг перезаписи ИНН / КПП
        /// </summary>
        public bool IsRewriteInnKpp { get; set; }
        /// <summary>
        /// Флаг перенумирования
        /// </summary>
        public bool IsRenumber { get; set; }
        /// <summary>
        /// Флаг записи ошибки. По умолчанию: true
        /// </summary>
        public bool IsWriteMsg { get; set; }
        /// <summary>
        /// Флаг перезаписи номера ГТД
        /// </summary>
        public bool IsRewriteNumberTD { get; set; }
        /// <summary>
        /// Перечень номеров колонок для (8) книги Покупок
        /// </summary>
        public SettCols8 Cols8 { get; set; }
        /// <summary>
        /// Перечень номеров колонок для (9) книги Продаж
        /// </summary>
        public SettCols9 Cols9 { get; set; }
        /// <summary>
        /// Перечень номеров колонок для (10) журнала Выставленных СФ
        /// </summary>
        public SettCols10 Cols10 { get; set; }
        /// <summary>
        /// Перечень номеров колонок для (11) журнала Полученных СФ
        /// </summary>
        public SettCols11 Cols11 { get; set; }
        /// <summary>
        /// 5.05 (01.01.2017) / 5.06 (01.01.2019) / 5.07 (01.10.2020) / 5.08 (01.07.2021)
        /// </summary>
        public string BookFormat { get; set; }
        /// <summary>
        /// Ссылка на директорию Шаблонов Excel (*.xltx)
        /// </summary>
        public string TemplatesPath { get; set; }
        /// <summary>
        /// Лимит записей на пакет обработки SQL
        /// </summary>
        public int LimitRowsPack { get; set; }
        #endregion



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ОСНОВНЫЕ МЕТОДЫ



        /// <summary>
        /// Конструктор класса для переопределения перечня колонок
        /// </summary>
        /// <param name="bookFormat"></param>
        public Settings(string bookFormat = DefaultBookFormat)
        {
            Delimiter = "/";
            IsRewriteInnKpp = true;
            IsRenumber = true;
            IsWriteMsg = true;
            IsRewriteNumberTD = true;
            BookFormat = bookFormat;
            TemplatesPath = "G:\\Work\\TaxDeclaration\\Templates\\";

            Cols8 = new SettCols8(BookFormat);
            Cols9 = new SettCols9(BookFormat);
            Cols10 = new SettCols10(BookFormat);
            Cols11 = new SettCols11(BookFormat);

            RowStart8 = 7;
            RowStart9 = 7;
            RowStart10 = 6;
            RowStart11 = 6;

            LimitRowsPack = 500;
        }

        /// <summary>
        /// Колонки Книги продаж (9)
        /// </summary>
        public class SettCols9
        {
            #region Перечень колонок
            /// <summary>
            /// Порядковый номер
            /// </summary>
            public int ListNum { get; set; }
            /// <summary>
            /// Код вида операции
            /// </summary>
            public int OperTypeCode { get; set; }
            /// <summary>
            /// Номер и дата счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSf { get; set; }
            /// <summary>
            /// Регистрационный номер таможенной декларации
            /// </summary>
            public int NumberTD { get; set; }
            /// <summary>
            /// Код вида товара
            /// </summary>
            public int GoodTypeCode { get; set; }
            /// <summary>
            /// Номер и дата исправления счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSfRed { get; set; }
            /// <summary>
            /// Номер и дата корректировочного счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSfKor { get; set; }
            /// <summary>
            /// Номер и дата исправления корректировочного счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSfKorRed { get; set; }
            /// <summary>
            /// Наименование покупателя
            /// </summary>
            public int KontrName { get; set; }
            /// <summary>
            /// ИНН/КПП покупателя
            /// </summary>
            public int KontrInnKpp { get; set; }
            /// <summary>
            /// Сведения о посреднике (комиссионере, агенте). Наименование посредника
            /// </summary>
            public int AgentName { get; set; }
            /// <summary>
            /// Сведения о посреднике (комиссионере, агенте). ИНН/КПП посредника
            /// </summary>
            public int AgentInnKpp { get; set; }
            /// <summary>
            /// Номер и дата документа, подтверждающего оплату
            /// </summary>
            public int NumAndDateDocConfirmPay { get; set; }
            /// <summary>
            /// Наименование и код валюты
            /// </summary>
            public int NameAndCodeCurrency { get; set; }
            /// <summary>
            /// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре(включая НДС) в валюте счета-фактуры. 
            /// В валюте счета-фактуры"
            /// </summary>
            public int CostCurrencySf { get; set; }
            /// <summary>
            /// Стоимость продаж по счету-фактуре, разница стоимости по корректировочному счету-фактуре(включая НДС) в валюте счета-фактуры. 
            /// В рублях и копейках
            /// </summary>
            public int CostRubKop { get; set; }
            /// <summary>
            /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре(без НДС) в рублях и копейках по ставке. 
            /// 20 процентов
            /// </summary>
            public int CostRubKopWithoutNDS20 { get; set; }
            /// <summary>
            /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре(без НДС) в рублях и копейках по ставке. 
            /// 18 процентов
            /// </summary>
            public int CostRubKopWithoutNDS18 { get; set; }
            /// <summary>
            /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре(без НДС) в рублях и копейках по ставке. 
            /// 10 процентов
            /// </summary>
            public int CostRubKopWithoutNDS10 { get; set; }
            /// <summary>
            /// Стоимость продаж, облагаемых налогом, по счету-фактуре, разница стоимости по корректировочному счету-фактуре(без НДС) в рублях и копейках по ставке. 
            /// 0 процентов
            /// </summary>
            public int CostRubKopWithoutNDS0 { get; set; }
            /// <summary>
            /// Сумма НДС по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках, по ставке. 
            /// 20 процентов
            /// </summary>
            public int SumNDS20 { get; set; }
            /// <summary>
            /// Сумма НДС по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках, по ставке. 
            /// 18 процентов
            /// </summary>
            public int SumNDS18 { get; set; }
            /// <summary>
            /// Сумма НДС по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках, по ставке. 
            /// 10 процентов
            /// </summary>
            public int SumNDS10 { get; set; }
            /// <summary>
            /// Стоимость продаж, освобождаемых от налога, по счету-фактуре, разница стоимости по корректировочному счету-фактуре в рублях и копейках
            /// </summary>
            public int CostSalesWithoutNDS { get; set; }
            /// <summary>
            /// Предупреждающие сообщения
            /// </summary>
            public int WarningMessage { get; set; }
            #endregion

            /// <summary>
            /// Конструктор класса для переопределения перечня колонок
            /// </summary>
            /// <param name="SbisFormat"></param>
            public SettCols9(string SbisFormat = DefaultBookFormat)
            {
                if (SbisFormat == "5.05")
                {
                    // !!! Старый формат (01.01.2017) !!!
                    ListNum = 1;
                    OperTypeCode = 2;
                    SellerNumAndDateSf = 3;
                    NumberTD = 0;
                    GoodTypeCode = 0;
                    SellerNumAndDateSfRed = 4;
                    SellerNumAndDateSfKor = 5;
                    SellerNumAndDateSfKorRed = 6;
                    KontrName = 7;
                    KontrInnKpp = 8;
                    AgentName = 9;
                    AgentInnKpp = 10;
                    NumAndDateDocConfirmPay = 11;
                    NameAndCodeCurrency = 12;
                    CostCurrencySf = 13;
                    CostRubKop = 14;
                    CostRubKopWithoutNDS20 = 0;
                    CostRubKopWithoutNDS18 = 15;
                    CostRubKopWithoutNDS10 = 16;
                    CostRubKopWithoutNDS0 = 17;
                    SumNDS20 = 0;
                    SumNDS18 = 18;
                    SumNDS10 = 19;
                    CostSalesWithoutNDS = 20;
                    WarningMessage = 21;
                }

                if (SbisFormat == "5.06" || SbisFormat == "5.07" || SbisFormat == "5.08")
                {
                    // !!! Старый формат (01.01.2019) !!!
                    ListNum = 1;
                    OperTypeCode = 2;
                    SellerNumAndDateSf = 3;
                    NumberTD = 4;
                    GoodTypeCode = 5;
                    SellerNumAndDateSfRed = 6;
                    SellerNumAndDateSfKor = 7;
                    SellerNumAndDateSfKorRed = 8;
                    KontrName = 9;
                    KontrInnKpp = 10;
                    AgentName = 11;
                    AgentInnKpp = 12;
                    NumAndDateDocConfirmPay = 13;
                    NameAndCodeCurrency = 14;
                    CostCurrencySf = 15;
                    CostRubKop = 16;
                    CostRubKopWithoutNDS20 = 17;
                    CostRubKopWithoutNDS18 = 18;
                    CostRubKopWithoutNDS10 = 19;
                    CostRubKopWithoutNDS0 = 20;
                    SumNDS20 = 21;
                    SumNDS18 = 22;
                    SumNDS10 = 23;
                    CostSalesWithoutNDS = 24;
                    WarningMessage = 25;
                }
            }
        }

        /// <summary>
        /// Колонки Книги покупок (8)
        /// </summary>
        public class SettCols8
        {
            #region Перечень колонок
            /// <summary>
            /// Порядковый номер
            /// </summary>
            public int ListNum { get; set; }
            /// <summary>
            /// Код вида операции
            /// </summary>
            public int OperTypeCode { get; set; }
            /// <summary>
            /// Номер и дата счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSf { get; set; }
            /// <summary>
            /// Номер и дата исправления счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSfRed { get; set; }
            /// <summary>
            /// Номер и дата корректировочного счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSfKor { get; set; }
            /// <summary>
            /// Номер и дата исправления корректировочного счета-фактуры продавца
            /// </summary>
            public int SellerNumAndDateSfKorRed { get; set; }
            /// <summary>
            /// Номер и дата документа, подтверждающего уплату налога
            /// </summary>
            public int NumAndDateDocConfirmPay { get; set; }
            /// <summary>
            /// Дата принятия на учет товаров (работ, услуг), имущественных прав
            /// </summary>
            public int DateAcceptGoodOrServices { get; set; }
            /// <summary>
            /// Наименование продавца
            /// </summary>
            public int SellerName { get; set; }
            /// <summary>
            /// ИНН/КПП продавца
            /// </summary>
            public int SellerInnKpp { get; set; }
            /// <summary>
            /// Сведения о посреднике (комиссионере, агенте). Наименование посредника
            /// </summary>
            public int AgentName { get; set; }
            /// <summary>
            /// Сведения о посреднике (комиссионере, агенте). ИНН/КПП посредника
            /// </summary>
            public int AgentInnKpp { get; set; }
            /// <summary>
            /// Регистрационный номер таможенной декларации
            /// </summary>
            public int NumberTD { get; set; }
            /// <summary>
            /// Наименование и код валюты
            /// </summary>
            public int NameAndCodeCurrency { get; set; }
            /// <summary>
            /// Стоимость покупок по счету-фактуре, разница стоимости по корректировочному счету-фактуре (включая НДС) в валюте счета-фактуры
            /// </summary>
            public int CostPaymentOfSf { get; set; }
            /// <summary>
            /// Сумма НДС по счету-фактуре, разница суммы НДС по корректировочному счету-фактуре, принимаемая к вычету в рублях и копейках
            /// </summary>
            public int SumNdsOfSf { get; set; }
            /// <summary>
            /// Предупреждающие сообщения
            /// </summary>
            public int WarningMessage { get; set; }
            #endregion

            /// <summary>
            /// Конструктор класса для переопределения перечня колонок
            /// </summary>
            /// <param name="SbisFormat"></param>
            public SettCols8(string SbisFormat = DefaultBookFormat)
            {
                if (SbisFormat == "5.05")
                {
                    // !!! Старый формат (01.01.2017) !!!
                    ListNum = 1;
                    OperTypeCode = 2;
                    SellerNumAndDateSf = 3;
                    SellerNumAndDateSfRed = 4;
                    SellerNumAndDateSfKor = 5;
                    SellerNumAndDateSfKorRed = 6;
                    NumAndDateDocConfirmPay = 7;
                    DateAcceptGoodOrServices = 8;
                    SellerName = 9;
                    SellerInnKpp = 10;
                    AgentName = 11;
                    AgentInnKpp = 12;
                    NumberTD = 13;
                    NameAndCodeCurrency = 14;
                    CostPaymentOfSf = 15;
                    SumNdsOfSf = 16;
                    WarningMessage = 17;
                }

                if (SbisFormat == "5.06" || SbisFormat == "5.07" || SbisFormat == "5.08")
                {
                    // !!! Старый формат (01.01.2019) !!!
                    ListNum = 1;
                    OperTypeCode = 2;
                    SellerNumAndDateSf = 3;
                    SellerNumAndDateSfRed = 4;
                    SellerNumAndDateSfKor = 5;
                    SellerNumAndDateSfKorRed = 6;
                    NumAndDateDocConfirmPay = 7;
                    DateAcceptGoodOrServices = 8;
                    SellerName = 9;
                    SellerInnKpp = 10;
                    AgentName = 11;
                    AgentInnKpp = 12;
                    NumberTD = 13;
                    NameAndCodeCurrency = 14;
                    CostPaymentOfSf = 15;
                    SumNdsOfSf = 16;
                    WarningMessage = 17;
                }
            }
        }

        /// <summary>
        /// Колонки журнала выставленных счетов-фактур (10)
        /// </summary>
        public class SettCols10
        {
            #region Перечень колонок
            /// <summary>
            /// Порядковый номер
            /// </summary>
            public int ListNum { get; set; }
            /// <summary>
            /// Дата выставления
            /// </summary>
            public int DateView { get; set; }
            /// <summary>
            /// Код вида операции
            /// </summary>
            public int OperTypeCode { get; set; }
            /// <summary>
            /// Номер и дата счета-фактуры
            /// </summary>
            public int NumAndDateSf { get; set; }
            /// <summary>
            /// Номер и дата исправления счета-фактуры
            /// </summary>
            public int NumAndDateSfRed { get; set; }
            /// <summary>
            /// Номер и дата корректировочного счета-фактуры
            /// </summary>
            public int NumAndDateSfKor { get; set; }
            /// <summary>
            /// Номер и дата исправления корректировочного счета-фактуры
            /// </summary>
            public int NumAndDateSfKorRed { get; set; }
            /// <summary>
            /// Наименование покупателя
            /// </summary>
            public int KontrName { get; set; }
            /// <summary>
            /// ИНН/КПП покупателя
            /// </summary>
            public int KontrInnKpp { get; set; }
            /// <summary>
            /// Сведения из счетов-фактур, полученных от продавцов. Наименование продавца
            /// </summary>
            public int SellerName { get; set; }
            /// <summary>
            /// Сведения из счетов-фактур, полученных от продавцов. ИНН/КПП продавца
            /// </summary>
            public int SellerInnKpp { get; set; }
            /// <summary>
            /// Сведения из счетов-фактур, полученных от продавцов. Номер и дата счета-фактуры (корректировочного счета-фактуры), полученного от продавца
            /// </summary>
            public int SellerNumAndDateSfKor { get; set; }
            /// <summary>
            /// Наименование и код валюты
            /// </summary>
            public int NameAndCodeCurrency { get; set; }
            /// <summary>
            /// Стоимость товаров (работ,услуг), имущественных прав по счету-фактуре всего
            /// </summary>
            public int CostSumGoodsAndServices { get; set; }
            /// <summary>
            /// В том числе сумма НДС по счету-фактуре
            /// </summary>
            public int CostSumNds { get; set; }
            /// <summary>
            /// Разница стоимости с учетом НДС по корректировочному счету-фактуре. Увеличение
            /// </summary>
            public int DiffCostWithNdsOfSfKorPlus { get; set; }
            /// /// <summary>
            /// Разница стоимости с учетом НДС по корректировочному счету-фактуре. Уменьшение
            /// </summary>
            public int DiffCostWithNdsOfSfKorMinus { get; set; }
            /// <summary>
            /// Разница НДС по корректировочному счету-фактуре. Увеличение
            /// </summary>
            public int DiffCostNdsOfSfKorPlus { get; set; }
            /// <summary>
            /// Разница НДС по корректировочному счету-фактуре. Уменьшение
            /// </summary>
            public int DiffCostNdsOfSfKorMinus { get; set; }
            /// <summary>
            /// Предупреждающие сообщения
            /// </summary>
            public int WarningMessage { get; set; }
            #endregion

            public SettCols10(string SbisFormat = DefaultBookFormat)
            {
                if (SbisFormat == "5.06" || SbisFormat == "5.07" || SbisFormat == "5.08")
                {
                    // !!! Старый формат (01.01.2019) !!!
                    ListNum = 1;
                    DateView = 2;
                    OperTypeCode = 3;
                    NumAndDateSf = 4;
                    NumAndDateSfRed = 5;
                    NumAndDateSfKor = 6;
                    NumAndDateSfKorRed = 7;
                    KontrName = 8;
                    KontrInnKpp = 9;
                    SellerName = 10;
                    SellerInnKpp = 11;
                    SellerNumAndDateSfKor = 12;
                    NameAndCodeCurrency = 13;
                    CostSumGoodsAndServices = 14;
                    CostSumNds = 15;
                    DiffCostWithNdsOfSfKorMinus = 16;
                    DiffCostWithNdsOfSfKorPlus = 17;
                    DiffCostNdsOfSfKorMinus = 18;
                    DiffCostNdsOfSfKorPlus = 19;
                    WarningMessage = 20;
                }
            }
        }

        /// <summary>
        /// Колонки журнала полученных счетов-фактур (11)
        /// </summary>
        public class SettCols11
        {
            #region Перечень колонок
            /// <summary>
            /// Порядковый номер
            /// </summary>
            public int ListNum { get; set; }
            /// <summary>
            /// Дата получения
            /// </summary>
            public int DateReceiving { get; set; }
            /// <summary>
            /// Код вида операции
            /// </summary>
            public int OperTypeCode { get; set; }
            /// <summary>
            /// Номер и дата счета-фактуры
            /// </summary>
            public int NumAndDateSf { get; set; }
            /// <summary>
            /// Номер и дата исправления счета-фактуры
            /// </summary>
            public int NumAndDateSfRed { get; set; }
            /// <summary>
            /// Номер и дата корректировочного счета-фактуры
            /// </summary>
            public int NumAndDateSfKor { get; set; }
            /// <summary>
            /// Номер и дата исправления корректировочного счета-фактуры
            /// </summary>
            public int NumAndDateSfKorRed { get; set; }
            /// <summary>
            /// Наименование продавца
            /// </summary>
            public int SellerName { get; set; }
            /// <summary>
            /// ИНН/КПП продавца
            /// </summary>
            public int SellerInnKpp { get; set; }
            /// <summary>
            /// Сведения о посреднической дейстельности, указываемые коммисионером (агентом). Наименование субкомиссионера (субагента)
            /// </summary>
            public int AgentName { get; set; }
            /// <summary>
            /// Сведения о посреднической дейстельности, указываемые коммисионером (агентом). ИНН/КПП субкомиссионера (субагента)
            /// </summary>
            public int AgentInnKpp { get; set; }
            /// <summary>
            /// Сведения о посреднической дейстельности, указываемые коммисионером (агентом). Код вида сделки
            /// </summary>
            public int DealCode { get; set; }
            /// <summary>
            /// Наименование и код валюты
            /// </summary>
            public int NameAndCodeCurrency { get; set; }
            /// <summary>
            /// Стоимость товаров (работ,услуг), имущественных прав по счету-фактуре всего
            /// </summary>
            public int CostSumGoodsAndServices { get; set; }
            /// <summary>
            /// В том числе сумма НДС по счету-фактуре
            /// </summary>
            public int CostSumNds { get; set; }
            /// <summary>
            /// Разница стоимости с учетом НДС по корректировочному счету-фактуре. Увеличение
            /// </summary>
            public int DiffCostWithNdsOfSfKorPlus { get; set; }
            /// /// <summary>
            /// Разница стоимости с учетом НДС по корректировочному счету-фактуре. Уменьшение
            /// </summary>
            public int DiffCostWithNdsOfSfKorMinus { get; set; }
            /// <summary>
            /// Разница НДС по корректировочному счету-фактуре. Увеличение
            /// </summary>
            public int DiffCostNdsOfSfKorPlus { get; set; }
            /// <summary>
            /// Разница НДС по корректировочному счету-фактуре. Уменьшение
            /// </summary>
            public int DiffCostNdsOfSfKorMinus { get; set; }
            /// <summary>
            /// Предупреждающие сообщения
            /// </summary>
            public int WarningMessage { get; set; }
            #endregion

            public SettCols11(string SbisFormat = DefaultBookFormat)
            {
                if (SbisFormat == "5.06" || SbisFormat == "5.07" || SbisFormat == "5.08")
                {
                    // !!! Старый формат (01.01.2019) !!!
                    ListNum = 1;
                    DateReceiving = 2;
                    OperTypeCode = 3;
                    NumAndDateSf = 4;
                    NumAndDateSfRed = 5;
                    NumAndDateSfKor = 6;
                    NumAndDateSfKorRed = 7;
                    SellerName = 8;
                    SellerInnKpp = 9;
                    AgentName = 10;
                    AgentInnKpp = 11;
                    DealCode = 12;
                    NameAndCodeCurrency = 13;
                    CostSumGoodsAndServices = 14;
                    CostSumNds = 15;
                    DiffCostWithNdsOfSfKorMinus = 16;
                    DiffCostWithNdsOfSfKorPlus = 17;
                    DiffCostNdsOfSfKorMinus = 18;
                    DiffCostNdsOfSfKorPlus = 19;
                    WarningMessage = 20;
                }
            }
        }



        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ



        #region ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        /// <summary>
        /// Получить дату в формате "дд.мм.гггг" из строки - строкой
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetDateFormat(string s)
        {
            string res = "";
            try
            {
                try
                {
                    // Если пришел старый формат строки: 17514/2811-18;18.10.2018
                    res = (s.Length == 10) ? s : s.Substring(s.IndexOf(";", 10));
                }
                catch
                {
                    try
                    {
                        res = DateTime.Parse(s).ToString("dd.MM.yyyy");
                    }
                    catch { }
                }
                
                // Проверка валидности даты 
                DateTime test = DateTime.ParseExact(res, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                res = "";
            }
            return res;
        }

        /// <summary>
        /// Получить вещественное число в формате 19.2
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetDecFormat(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            string res = "";
            try
            {
                // Прежний вариант (округляем)
                s = s.Replace(".", ",").Replace(" ", "");
                decimal dec = Decimal.Parse(s); 
                dec = Math.Round(dec, 2); // 5.06
                //dec = Math.Round(dec, 4); // 5.07+
                res = dec.ToString().Replace(",", ".").Replace(" ", "");
                //-----------------------------------------------------------------------------

                // Текущий вариант (отбрасываем)
                //s = s.Replace(",", ".").Replace(" ", "");
                //int pos = s.IndexOf('.'); // Позиция разделителя
                //if (pos > 0)
                //{
                //    // Получаем всю дробную часть (без разделителя)
                //    string ns = s.Substring(pos + 1, s.Length - pos - 1);
                //    // В случае, если в строке более 2 символов, отбираем только 2
                //    if (ns.Length > 2)
                //        s = s.Substring(0, pos + 3);
                //}
                //res = s;
                //-----------------------------------------------------------------------------
            }
            catch
            {
                res = "";
            }
            return res;
        }

        public string GetDecFormatOld(string s, bool isNotNull = false)
        {
            if (String.IsNullOrEmpty(s))
                return ((isNotNull) ? "0" : s);

            string res = "";
            try
            {
                s = s.Replace(".", ",").Replace(" ", "");
                decimal dec = Decimal.Parse(s);
                dec = Math.Round(dec, 2);
                res = dec.ToString().Replace(",", ".").Replace(" ", "");
            }
            catch
            {
                res = "";
            }
            return res;
        }

        public decimal GetDecFromString(string s)
        {
            if (String.IsNullOrEmpty(s))
                return 0;

            decimal res = 0;
            try
            {
                s = s.Replace(".", ",").Replace(" ", "");
                decimal dec = Decimal.Parse(s);
                dec = Math.Round(dec, 2);
                res = dec;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        /// <summary>
        /// Разделение строки на основе разделителя (Строго 2 значения)
        /// </summary>
        /// <param name="s">Исходная строка</param>
        /// <param name="d">Разделитель строки - не обязательный</param>
        /// <returns>Массив результатов типа string</returns>
        public string[] SeparationTwo(string s, string d = "")
        {
            string[] arr = new string[2];
            if (d == "") d = Delimiter;
            int PosDelimiter = 0;
            try { PosDelimiter = s.IndexOf(d); }
            catch { PosDelimiter = 0; }
            int LenDelimiter = d.Length;

            // Разбиваем строку на части
            arr[0] = (PosDelimiter > 0) ? s.Substring(0, PosDelimiter) : ((String.IsNullOrEmpty(s)) ? "" : s);
            try
            {
                arr[1] = (PosDelimiter > 0) ? s.Substring(PosDelimiter + LenDelimiter, s.Length - PosDelimiter - LenDelimiter) : "";
            }
            catch { arr[1] = ""; }

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].Trim();
            }

            return arr;
        }

        /// <summary>
        /// Разделение строки на основе разделителя (Множество)
        /// </summary>
        /// <param name="s">Исходная строка</param>
        /// <param name="d">Разделитель строки - не обязательный</param>
        /// <returns>Массив результатов типа string</returns>
        public string[] SeparationMulty(string s, string d = "")
        {
            if (d == "") d = Delimiter;
            int count = s.Split(Char.Parse(d)).Length;
            string[] arr = new string[count];
            int PosDelimiter = 0;

            if (count == 0) return arr;
            for (int i = 0; i < count; i++)
            {
                PosDelimiter = s.IndexOf(d);
                if (PosDelimiter > 0)
                {
                    arr[i] = s.Substring(0, PosDelimiter);
                    s = s.Substring(PosDelimiter + 1, (s.Length - PosDelimiter - 1));
                }
                else
                    arr[i] = s;
            }

            return arr;
        }

        /// <summary>
        /// Валидация значения атрибута
        /// </summary>
        /// <param name="s">Значение атрибута</param>
        public bool ValAttr(string s)
        {
            return (String.IsNullOrEmpty(s) || s == "0") ? false : true;
        }

        /// <summary>
        /// Подмена символа " на \"
        /// </summary>
        /// <param name="s">Строка</param>
        public string SFormat(string s)
        {
            return s.Replace("\"", "&quot;");
        }

        /// <summary>
        /// Конвертер object[] в string[]
        /// </summary>
        /// <param name="Cells"></param>
        /// <returns></returns>
        public string[] GetCellStrings(object[] Cells, int colCount = 0)
        {
            int cnt = (colCount > 0) ? colCount : Cells.Length;
            string[] res = new string[cnt + 1];
            res[0] = ""; // Служебное поле

            for (int i = 1; i <= cnt; i++)
            {
                res[i] = Convert.ToString(Cells[i - 1]).Replace("\n", "").Replace("\r", "");
            }

            return res;
        }

        /// <summary>
        /// Перевод DataTable в List strings[]
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public List<string[]> ConvertToListStr(DataTable Data)
        {
            List<string[]> res = new List<string[]>();

            for (int i = 0; i < Data.Rows.Count; i++) //по всем строкам
            {
                string[] rowStr = GetCellStrings(Data.Rows[i].ItemArray);
                res.Add(rowStr);
            }

            return res;
        }

        /// <summary>
        /// Исправляем ошибки Номера и даты счета-фактуры
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetCorrectNumAndDate(string s)
        {
            string alienDelimiter = " от ";
            string delimiter = ";";
            s = s.Replace(alienDelimiter, delimiter);

            // В случае, если в строке присутствует более двух разделителей, то это означает,
            // что документ пришел из внешних систем, включая в название дату и был выгружен 
            // 1С с дублированием даты. В таком случае берем вторую дату, а первую убираем
            if (CountWords(s, delimiter) > 1)
            {
                string[] arr = s.Split(';');
                s = arr[0] + ";" + arr[2];
            }

            return s;
        }

        /// <summary>
        /// Количество вхождений подстроки в строке
        /// </summary>
        /// <param name="s">Строка</param>
        /// <param name="s0">Искомое слово/символ</param>
        /// <returns></returns>
        private int CountWords(string s, string s0)
        {
            int count = (s.Length - s.Replace(s0, "").Length) / s0.Length;
            return count;
        }
        #endregion
    }
}
