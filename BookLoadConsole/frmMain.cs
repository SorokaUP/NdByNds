using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BookLoadConsole
{
    public partial class frmMain : Form
    {
        List<ComboboxData> Modes = new List<ComboboxData>();
        List<ComboboxData> BookTypes = new List<ComboboxData>();
        List<ComboboxData> BookFormats = new List<ComboboxData>();

        public string returnMode = "";
        public Book returnBook = new Book();

        public frmMain()
        {
            InitializeComponent();

            Modes.Add(new ComboboxData { Id = 1, Code = "exchk",     Name = "Проверка файла Excel" });
            Modes.Add(new ComboboxData { Id = 2, Code = "ex2xml",    Name = "Экспорт данных Excel файла в Xml" });
            Modes.Add(new ComboboxData { Id = 3, Code = "dbfb2xml",  Name = "Сформировать XML из базы данных" });
            Modes.Add(new ComboboxData { Id = 4, Code = "dbfb2ex",   Name = "Сформировать Excel из базы данных" });
            Modes.Add(new ComboboxData { Id = 5, Code = "dbfb2dbss", Name = "Импорт в базу данных SQLServer из Firebird" });
            Modes.Add(new ComboboxData { Id = 6, Code = "ex2dbss",   Name = "Импорт в базу данных SQLServer из Excel" });
            Modes.Add(new ComboboxData { Id = 7, Code = "vl",        Name = "Проверка Xml файла по Xsd схеме" });
            cbMode.DataSource = Modes;
            cbMode.DisplayMember = "Name";
            cbMode.ValueMember = "Code";
            cbMode.SelectedIndex = -1;

            BookTypes.Add(new ComboboxData { Id = 8,  Name = "8 - Книга покупок" });
            BookTypes.Add(new ComboboxData { Id = 9,  Name = "9 - Книга продаж" });
            BookTypes.Add(new ComboboxData { Id = 10, Name = "10 - Журнал учета выставленных счетов-фактур" });
            BookTypes.Add(new ComboboxData { Id = 11, Name = "11 - Журнал учета полученных счетов-фактур" });
            cbBookTypes.DataSource = BookTypes;
            cbBookTypes.DisplayMember = "Name";
            cbBookTypes.ValueMember = "Id";
            cbBookTypes.SelectedIndex = -1;

            BookFormats.Add(new ComboboxData { Id = 507, Name = "5.07" });
            BookFormats.Add(new ComboboxData { Id = 506, Name = "5.06" });
            BookFormats.Add(new ComboboxData { Id = 505, Name = "5.05" });
            cbBookFormats.DataSource = BookFormats;
            cbBookFormats.DisplayMember = "Name";
            cbBookFormats.ValueMember = "Name";
            cbBookFormats.SelectedIndex = -1;

            HideAllControl();
            btnGo.Visible = false;
        }

        private class ComboboxData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideAllControl();
            switch (cbMode.SelectedValue)
            {
                case "exchk":
                    gbPathImportExcel.Visible = true;
                    gbBookType.Visible = true;
                    gbBookFormat.Visible = true;
                    break;

                case "ex2xml":
                    gbPathImportExcel.Visible = true;
                    gbBookType.Visible = true;
                    gbBookFormat.Visible = true;
                    gbNumberKorr.Visible = true;
                    gbPathExportXml.Visible = true;
                    break;

                case "dbfb2xml":
                    gbBookType.Visible = true;
                    gbBookFormat.Visible = true;
                    gbNumberKorr.Visible = true;
                    gbPeriod.Visible = true;
                    gbPathExportXml.Visible = true;
                    break;

                case "dbfb2ex":
                    gbBookType.Visible = true;
                    gbBookFormat.Visible = true;
                    gbPeriod.Visible = true;
                    gbPathExportXml.Visible = true;
                    break;

                case "vl":
                    gbPathImportXml.Visible = true;
                    gbPathImportXsd.Visible = true;
                    break;

                case "dbfb2dbss":
                    break;

                case "ex2dbss":
                    break;
            }
        }

        private void HideAllControl()
        {
            foreach (GroupBox gb in gbParameters.Controls.OfType<GroupBox>())
                gb.Visible = false;
            foreach (GroupBox gb in gbImport.Controls.OfType<GroupBox>())
                gb.Visible = false;
            foreach (GroupBox gb in gbExport.Controls.OfType<GroupBox>())
                gb.Visible = false;
            btnGo.Visible = true;
        }

        private void ErrorShow(string msg, string group)
        {
            MessageBox.Show("Не указан " + msg + " в разделе \"" + group + "\"");
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string importPathExcel = tbPathImportExcel.Text;
            string importPathXml = tbPathImportXml.Text;
            string importPathXsd = tbPathImportXsd.Text;
            string exportPathExcel = tbPathExportExcel.Text;
            string exportPathXml = tbPathExportXml.Text;

            int bookType = -1;
            try { bookType = Int32.Parse(cbBookTypes.SelectedValue.ToString()); }
            catch { bookType = -1; }

            string bookFormat = "";
            try { bookFormat = cbBookFormats.SelectedValue.ToString(); }
            catch { bookFormat = ""; }
            int numberKorr = (int)nudNumberKorr.Value;

            DateTime fromDate = dtpFromDate.Value;       
            DateTime toDate = dtpToDate.Value;

            //------------------------------------------------------------------------------------
            Book book = new Book();


            switch (cbMode.SelectedValue.ToString())
            {
                case "exchk":
                    if (String.IsNullOrEmpty(importPathExcel)) { ErrorShow("путь к Excel файлу", "Импорт"); return; }
                    book.FilePathExcel = importPathExcel;
                    if (bookType < 0) { ErrorShow("тип книги", "Параметры"); return; }
                    book.BookType = bookType;
                    if (String.IsNullOrEmpty(bookFormat)) { ErrorShow("формат книги", "Параметры"); return; }
                    book.St.BookFormat = bookFormat;

                    //book.ChecExcelkData();
                    break;

                case "ex2xml":
                    if (String.IsNullOrEmpty(importPathExcel)) { ErrorShow("путь к Excel файлу", "Импорт"); return; }
                    book.FilePathExcel = importPathExcel;
                    if (bookType < 0) { ErrorShow("тип книги", "Параметры"); return; }
                    book.BookType = bookType;
                    if (String.IsNullOrEmpty(bookFormat)) { ErrorShow("формат книги", "Параметры"); return; }
                    book.St.BookFormat = bookFormat;
                    if (numberKorr < 0) { ErrorShow("номер корректировки", "Параметры"); return; }
                    book.NumberKorr = numberKorr;
                    if (String.IsNullOrEmpty(exportPathXml)) { ErrorShow("путь к Xml файлу", "Экспорт"); return; }
                    book.ExportPathXml = exportPathXml;

                    //book.ExcelToXml();
                    break;

                case "dbfb2xml":
                    if (bookType < 0) { ErrorShow("тип книги", "Параметры"); return; }
                    book.BookType = bookType;
                    if (String.IsNullOrEmpty(bookFormat)) { ErrorShow("формат книги", "Параметры"); return; }
                    book.St.BookFormat = bookFormat;
                    if (numberKorr < 0) { ErrorShow("номер корректировки", "Параметры"); return; }
                    book.NumberKorr = numberKorr;
                    if (fromDate < toDate) { ErrorShow("период (дата окончания превышает дату начала)", "Параметры"); return; }
                    book.Fb.FbExDateFrom = fromDate;
                    book.Fb.FbExDateTo = toDate;
                    if (String.IsNullOrEmpty(exportPathXml)) { ErrorShow("путь к Xml файлу", "Экспорт"); return; }
                    book.ExportPathXml = exportPathXml;

                    //book.DataBaseToXml();
                    break;

                case "dbfb2ex":
                    if (bookType < 0) { ErrorShow("тип книги", "Параметры"); return; }
                    book.BookType = bookType;
                    if (String.IsNullOrEmpty(bookFormat)) { ErrorShow("формат книги", "Параметры"); return; }
                    book.St.BookFormat = bookFormat;
                    if (fromDate < toDate) { ErrorShow("период (дата окончания превышает дату начала)", "Параметры"); return; }
                    book.Fb.FbExDateFrom = fromDate;
                    book.Fb.FbExDateTo = toDate;
                    if (String.IsNullOrEmpty(exportPathExcel)) { ErrorShow("путь к Excel файлу", "Экспорт"); return; }
                    book.ExportPathExcel = exportPathExcel;

                    //book.DataBaseToExcel();
                    break;

                case "vl":
                    if (String.IsNullOrEmpty(importPathXml)) { ErrorShow("путь к Xml файлу", "Импорт"); return; }
                    book.FilePathXml = importPathXml;
                    if (String.IsNullOrEmpty(importPathXsd)) { ErrorShow("путь к Xsd файлу", "Импорт"); return; }
                    book.FilePathXsd = importPathXsd;

                    //book.ValidateXmlForXsd();
                    break;

                case "dbfb2dbss":
                    break;

                case "ex2dbss":
                    break;

                default:
                    MessageBox.Show("Не определен режим работы!");
                    return;
            }

            returnMode = cbMode.SelectedValue.ToString();
            returnBook = book;
            this.DialogResult = DialogResult.OK;
        }

        #region Мусор
        private void btnClearImportExcel_Click(object sender, EventArgs e)
        {
            tbPathImportExcel.Text = "";
        }

        private void btnClearImportXml_Click(object sender, EventArgs e)
        {
            tbPathImportXml.Text = "";
        }

        private void btnClearXsd_Click(object sender, EventArgs e)
        {
            tbPathImportXsd.Text = "";
        }

        private void btnClearExportXml_Click(object sender, EventArgs e)
        {
            tbPathExportXml.Text = "";
        }

        private void btnClearExportExcel_Click(object sender, EventArgs e)
        {
            tbPathExportExcel.Text = "";
        }

        private void btnPathImportExcel_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                tbPathImportExcel.Text = ofdFile.FileName;
            }
        }

        private void btnPathImportXml_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                tbPathImportXml.Text = ofdFile.FileName;
            }
        }

        private void btnPathXsd_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                tbPathImportXsd.Text = ofdFile.FileName;
            }
        }

        private void btnPathExportXml_Click(object sender, EventArgs e)
        {
            if (fbdFolder.ShowDialog() == DialogResult.OK)
            {
                tbPathExportXml.Text = fbdFolder.SelectedPath;
            }
        }

        private void btnPathExportExcel_Click(object sender, EventArgs e)
        {
            if (fbdFolder.ShowDialog() == DialogResult.OK)
            {
                tbPathExportExcel.Text = fbdFolder.SelectedPath;
            }
        }
        #endregion
    }
}
