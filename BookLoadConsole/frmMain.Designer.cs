namespace BookLoadConsole
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbBookTypes = new System.Windows.Forms.ComboBox();
            this.cbBookFormats = new System.Windows.Forms.ComboBox();
            this.nudNumberKorr = new System.Windows.Forms.NumericUpDown();
            this.tbPathImportExcel = new System.Windows.Forms.TextBox();
            this.btnPathImportExcel = new System.Windows.Forms.Button();
            this.btnClearImportExcel = new System.Windows.Forms.Button();
            this.btnClearImportXml = new System.Windows.Forms.Button();
            this.btnPathImportXml = new System.Windows.Forms.Button();
            this.tbPathImportXml = new System.Windows.Forms.TextBox();
            this.btnClearXsd = new System.Windows.Forms.Button();
            this.btnPathXsd = new System.Windows.Forms.Button();
            this.tbPathImportXsd = new System.Windows.Forms.TextBox();
            this.btnClearExportExcel = new System.Windows.Forms.Button();
            this.btnPathExportExcel = new System.Windows.Forms.Button();
            this.tbPathExportExcel = new System.Windows.Forms.TextBox();
            this.btnClearExportXml = new System.Windows.Forms.Button();
            this.btnPathExportXml = new System.Windows.Forms.Button();
            this.tbPathExportXml = new System.Windows.Forms.TextBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.gbModes = new System.Windows.Forms.GroupBox();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.gbBookType = new System.Windows.Forms.GroupBox();
            this.gbBookFormat = new System.Windows.Forms.GroupBox();
            this.gbNumberKorr = new System.Windows.Forms.GroupBox();
            this.gbPeriod = new System.Windows.Forms.GroupBox();
            this.gbPathImportExcel = new System.Windows.Forms.GroupBox();
            this.gbPathImportXml = new System.Windows.Forms.GroupBox();
            this.gbPathImportXsd = new System.Windows.Forms.GroupBox();
            this.gbPathExportXml = new System.Windows.Forms.GroupBox();
            this.gbPathExportExcel = new System.Windows.Forms.GroupBox();
            this.gbExport = new System.Windows.Forms.GroupBox();
            this.gbImport = new System.Windows.Forms.GroupBox();
            this.gbParameters = new System.Windows.Forms.GroupBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.fbdFolder = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberKorr)).BeginInit();
            this.gbModes.SuspendLayout();
            this.gbBookType.SuspendLayout();
            this.gbBookFormat.SuspendLayout();
            this.gbNumberKorr.SuspendLayout();
            this.gbPeriod.SuspendLayout();
            this.gbPathImportExcel.SuspendLayout();
            this.gbPathImportXml.SuspendLayout();
            this.gbPathImportXsd.SuspendLayout();
            this.gbPathExportXml.SuspendLayout();
            this.gbPathExportExcel.SuspendLayout();
            this.gbExport.SuspendLayout();
            this.gbImport.SuspendLayout();
            this.gbParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbBookTypes
            // 
            this.cbBookTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBookTypes.FormattingEnabled = true;
            this.cbBookTypes.Location = new System.Drawing.Point(6, 19);
            this.cbBookTypes.Name = "cbBookTypes";
            this.cbBookTypes.Size = new System.Drawing.Size(310, 21);
            this.cbBookTypes.TabIndex = 1;
            // 
            // cbBookFormats
            // 
            this.cbBookFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBookFormats.FormattingEnabled = true;
            this.cbBookFormats.Location = new System.Drawing.Point(6, 19);
            this.cbBookFormats.Name = "cbBookFormats";
            this.cbBookFormats.Size = new System.Drawing.Size(310, 21);
            this.cbBookFormats.TabIndex = 3;
            // 
            // nudNumberKorr
            // 
            this.nudNumberKorr.Location = new System.Drawing.Point(6, 19);
            this.nudNumberKorr.Name = "nudNumberKorr";
            this.nudNumberKorr.Size = new System.Drawing.Size(310, 20);
            this.nudNumberKorr.TabIndex = 5;
            // 
            // tbPathImportExcel
            // 
            this.tbPathImportExcel.Location = new System.Drawing.Point(6, 19);
            this.tbPathImportExcel.Name = "tbPathImportExcel";
            this.tbPathImportExcel.ReadOnly = true;
            this.tbPathImportExcel.Size = new System.Drawing.Size(240, 20);
            this.tbPathImportExcel.TabIndex = 7;
            // 
            // btnPathImportExcel
            // 
            this.btnPathImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathImportExcel.Location = new System.Drawing.Point(252, 19);
            this.btnPathImportExcel.Name = "btnPathImportExcel";
            this.btnPathImportExcel.Size = new System.Drawing.Size(29, 20);
            this.btnPathImportExcel.TabIndex = 8;
            this.btnPathImportExcel.Text = "...";
            this.btnPathImportExcel.UseVisualStyleBackColor = true;
            this.btnPathImportExcel.Click += new System.EventHandler(this.btnPathImportExcel_Click);
            // 
            // btnClearImportExcel
            // 
            this.btnClearImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearImportExcel.Location = new System.Drawing.Point(287, 19);
            this.btnClearImportExcel.Name = "btnClearImportExcel";
            this.btnClearImportExcel.Size = new System.Drawing.Size(29, 20);
            this.btnClearImportExcel.TabIndex = 9;
            this.btnClearImportExcel.Text = "X";
            this.btnClearImportExcel.UseVisualStyleBackColor = true;
            this.btnClearImportExcel.Click += new System.EventHandler(this.btnClearImportExcel_Click);
            // 
            // btnClearImportXml
            // 
            this.btnClearImportXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearImportXml.Location = new System.Drawing.Point(287, 19);
            this.btnClearImportXml.Name = "btnClearImportXml";
            this.btnClearImportXml.Size = new System.Drawing.Size(29, 20);
            this.btnClearImportXml.TabIndex = 13;
            this.btnClearImportXml.Text = "X";
            this.btnClearImportXml.UseVisualStyleBackColor = true;
            this.btnClearImportXml.Click += new System.EventHandler(this.btnClearImportXml_Click);
            // 
            // btnPathImportXml
            // 
            this.btnPathImportXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathImportXml.Location = new System.Drawing.Point(252, 19);
            this.btnPathImportXml.Name = "btnPathImportXml";
            this.btnPathImportXml.Size = new System.Drawing.Size(29, 20);
            this.btnPathImportXml.TabIndex = 12;
            this.btnPathImportXml.Text = "...";
            this.btnPathImportXml.UseVisualStyleBackColor = true;
            this.btnPathImportXml.Click += new System.EventHandler(this.btnPathImportXml_Click);
            // 
            // tbPathImportXml
            // 
            this.tbPathImportXml.Location = new System.Drawing.Point(6, 19);
            this.tbPathImportXml.Name = "tbPathImportXml";
            this.tbPathImportXml.ReadOnly = true;
            this.tbPathImportXml.Size = new System.Drawing.Size(240, 20);
            this.tbPathImportXml.TabIndex = 11;
            // 
            // btnClearXsd
            // 
            this.btnClearXsd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearXsd.Location = new System.Drawing.Point(287, 19);
            this.btnClearXsd.Name = "btnClearXsd";
            this.btnClearXsd.Size = new System.Drawing.Size(29, 20);
            this.btnClearXsd.TabIndex = 17;
            this.btnClearXsd.Text = "X";
            this.btnClearXsd.UseVisualStyleBackColor = true;
            this.btnClearXsd.Click += new System.EventHandler(this.btnClearXsd_Click);
            // 
            // btnPathXsd
            // 
            this.btnPathXsd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathXsd.Location = new System.Drawing.Point(252, 19);
            this.btnPathXsd.Name = "btnPathXsd";
            this.btnPathXsd.Size = new System.Drawing.Size(29, 20);
            this.btnPathXsd.TabIndex = 16;
            this.btnPathXsd.Text = "...";
            this.btnPathXsd.UseVisualStyleBackColor = true;
            this.btnPathXsd.Click += new System.EventHandler(this.btnPathXsd_Click);
            // 
            // tbPathImportXsd
            // 
            this.tbPathImportXsd.Location = new System.Drawing.Point(6, 19);
            this.tbPathImportXsd.Name = "tbPathImportXsd";
            this.tbPathImportXsd.ReadOnly = true;
            this.tbPathImportXsd.Size = new System.Drawing.Size(240, 20);
            this.tbPathImportXsd.TabIndex = 15;
            // 
            // btnClearExportExcel
            // 
            this.btnClearExportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearExportExcel.Location = new System.Drawing.Point(287, 19);
            this.btnClearExportExcel.Name = "btnClearExportExcel";
            this.btnClearExportExcel.Size = new System.Drawing.Size(29, 20);
            this.btnClearExportExcel.TabIndex = 25;
            this.btnClearExportExcel.Text = "X";
            this.btnClearExportExcel.UseVisualStyleBackColor = true;
            this.btnClearExportExcel.Click += new System.EventHandler(this.btnClearExportExcel_Click);
            // 
            // btnPathExportExcel
            // 
            this.btnPathExportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathExportExcel.Location = new System.Drawing.Point(252, 19);
            this.btnPathExportExcel.Name = "btnPathExportExcel";
            this.btnPathExportExcel.Size = new System.Drawing.Size(29, 20);
            this.btnPathExportExcel.TabIndex = 24;
            this.btnPathExportExcel.Text = "...";
            this.btnPathExportExcel.UseVisualStyleBackColor = true;
            this.btnPathExportExcel.Click += new System.EventHandler(this.btnPathExportExcel_Click);
            // 
            // tbPathExportExcel
            // 
            this.tbPathExportExcel.Location = new System.Drawing.Point(6, 19);
            this.tbPathExportExcel.Name = "tbPathExportExcel";
            this.tbPathExportExcel.ReadOnly = true;
            this.tbPathExportExcel.Size = new System.Drawing.Size(240, 20);
            this.tbPathExportExcel.TabIndex = 23;
            // 
            // btnClearExportXml
            // 
            this.btnClearExportXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearExportXml.Location = new System.Drawing.Point(287, 19);
            this.btnClearExportXml.Name = "btnClearExportXml";
            this.btnClearExportXml.Size = new System.Drawing.Size(29, 20);
            this.btnClearExportXml.TabIndex = 21;
            this.btnClearExportXml.Text = "X";
            this.btnClearExportXml.UseVisualStyleBackColor = true;
            this.btnClearExportXml.Click += new System.EventHandler(this.btnClearExportXml_Click);
            // 
            // btnPathExportXml
            // 
            this.btnPathExportXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathExportXml.Location = new System.Drawing.Point(252, 19);
            this.btnPathExportXml.Name = "btnPathExportXml";
            this.btnPathExportXml.Size = new System.Drawing.Size(29, 20);
            this.btnPathExportXml.TabIndex = 20;
            this.btnPathExportXml.Text = "...";
            this.btnPathExportXml.UseVisualStyleBackColor = true;
            this.btnPathExportXml.Click += new System.EventHandler(this.btnPathExportXml_Click);
            // 
            // tbPathExportXml
            // 
            this.tbPathExportXml.Location = new System.Drawing.Point(6, 19);
            this.tbPathExportXml.Name = "tbPathExportXml";
            this.tbPathExportXml.ReadOnly = true;
            this.tbPathExportXml.Size = new System.Drawing.Size(240, 20);
            this.tbPathExportXml.TabIndex = 19;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(6, 19);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(130, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(186, 19);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(130, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(142, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "по";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbModes
            // 
            this.gbModes.Controls.Add(this.cbMode);
            this.gbModes.Location = new System.Drawing.Point(12, 12);
            this.gbModes.Name = "gbModes";
            this.gbModes.Size = new System.Drawing.Size(680, 55);
            this.gbModes.TabIndex = 29;
            this.gbModes.TabStop = false;
            this.gbModes.Text = "Режим работы";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(15, 19);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(650, 21);
            this.cbMode.TabIndex = 2;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // gbBookType
            // 
            this.gbBookType.Controls.Add(this.cbBookTypes);
            this.gbBookType.Location = new System.Drawing.Point(6, 19);
            this.gbBookType.Name = "gbBookType";
            this.gbBookType.Size = new System.Drawing.Size(323, 50);
            this.gbBookType.TabIndex = 30;
            this.gbBookType.TabStop = false;
            this.gbBookType.Text = "Тип книги";
            // 
            // gbBookFormat
            // 
            this.gbBookFormat.Controls.Add(this.cbBookFormats);
            this.gbBookFormat.Location = new System.Drawing.Point(6, 75);
            this.gbBookFormat.Name = "gbBookFormat";
            this.gbBookFormat.Size = new System.Drawing.Size(323, 50);
            this.gbBookFormat.TabIndex = 31;
            this.gbBookFormat.TabStop = false;
            this.gbBookFormat.Text = "Формат книги (по налоговой)";
            // 
            // gbNumberKorr
            // 
            this.gbNumberKorr.Controls.Add(this.nudNumberKorr);
            this.gbNumberKorr.Location = new System.Drawing.Point(6, 131);
            this.gbNumberKorr.Name = "gbNumberKorr";
            this.gbNumberKorr.Size = new System.Drawing.Size(323, 50);
            this.gbNumberKorr.TabIndex = 32;
            this.gbNumberKorr.TabStop = false;
            this.gbNumberKorr.Text = "Номер корректировки";
            // 
            // gbPeriod
            // 
            this.gbPeriod.Controls.Add(this.label10);
            this.gbPeriod.Controls.Add(this.dtpFromDate);
            this.gbPeriod.Controls.Add(this.dtpToDate);
            this.gbPeriod.Location = new System.Drawing.Point(6, 187);
            this.gbPeriod.Name = "gbPeriod";
            this.gbPeriod.Size = new System.Drawing.Size(323, 50);
            this.gbPeriod.TabIndex = 33;
            this.gbPeriod.TabStop = false;
            this.gbPeriod.Text = "Период";
            // 
            // gbPathImportExcel
            // 
            this.gbPathImportExcel.Controls.Add(this.tbPathImportExcel);
            this.gbPathImportExcel.Controls.Add(this.btnClearImportExcel);
            this.gbPathImportExcel.Controls.Add(this.btnPathImportExcel);
            this.gbPathImportExcel.Location = new System.Drawing.Point(6, 19);
            this.gbPathImportExcel.Name = "gbPathImportExcel";
            this.gbPathImportExcel.Size = new System.Drawing.Size(323, 50);
            this.gbPathImportExcel.TabIndex = 34;
            this.gbPathImportExcel.TabStop = false;
            this.gbPathImportExcel.Text = "Путь к файлу Excel";
            // 
            // gbPathImportXml
            // 
            this.gbPathImportXml.Controls.Add(this.tbPathImportXml);
            this.gbPathImportXml.Controls.Add(this.btnPathImportXml);
            this.gbPathImportXml.Controls.Add(this.btnClearImportXml);
            this.gbPathImportXml.Location = new System.Drawing.Point(6, 75);
            this.gbPathImportXml.Name = "gbPathImportXml";
            this.gbPathImportXml.Size = new System.Drawing.Size(323, 50);
            this.gbPathImportXml.TabIndex = 35;
            this.gbPathImportXml.TabStop = false;
            this.gbPathImportXml.Text = "Путь к файлу Xml";
            // 
            // gbPathImportXsd
            // 
            this.gbPathImportXsd.Controls.Add(this.tbPathImportXsd);
            this.gbPathImportXsd.Controls.Add(this.btnClearXsd);
            this.gbPathImportXsd.Controls.Add(this.btnPathXsd);
            this.gbPathImportXsd.Location = new System.Drawing.Point(6, 131);
            this.gbPathImportXsd.Name = "gbPathImportXsd";
            this.gbPathImportXsd.Size = new System.Drawing.Size(323, 50);
            this.gbPathImportXsd.TabIndex = 36;
            this.gbPathImportXsd.TabStop = false;
            this.gbPathImportXsd.Text = "Путь к файлу Xsd";
            // 
            // gbPathExportXml
            // 
            this.gbPathExportXml.Controls.Add(this.tbPathExportXml);
            this.gbPathExportXml.Controls.Add(this.btnClearExportXml);
            this.gbPathExportXml.Controls.Add(this.btnPathExportXml);
            this.gbPathExportXml.Location = new System.Drawing.Point(6, 19);
            this.gbPathExportXml.Name = "gbPathExportXml";
            this.gbPathExportXml.Size = new System.Drawing.Size(323, 50);
            this.gbPathExportXml.TabIndex = 37;
            this.gbPathExportXml.TabStop = false;
            this.gbPathExportXml.Text = "Директория выгрузки Xml";
            // 
            // gbPathExportExcel
            // 
            this.gbPathExportExcel.Controls.Add(this.tbPathExportExcel);
            this.gbPathExportExcel.Controls.Add(this.btnPathExportExcel);
            this.gbPathExportExcel.Controls.Add(this.btnClearExportExcel);
            this.gbPathExportExcel.Location = new System.Drawing.Point(6, 75);
            this.gbPathExportExcel.Name = "gbPathExportExcel";
            this.gbPathExportExcel.Size = new System.Drawing.Size(323, 50);
            this.gbPathExportExcel.TabIndex = 38;
            this.gbPathExportExcel.TabStop = false;
            this.gbPathExportExcel.Text = "Директория выгрузки Excel";
            // 
            // gbExport
            // 
            this.gbExport.Controls.Add(this.gbPathExportXml);
            this.gbExport.Controls.Add(this.gbPathExportExcel);
            this.gbExport.Location = new System.Drawing.Point(355, 269);
            this.gbExport.Name = "gbExport";
            this.gbExport.Size = new System.Drawing.Size(337, 135);
            this.gbExport.TabIndex = 39;
            this.gbExport.TabStop = false;
            this.gbExport.Text = "Экспорт";
            // 
            // gbImport
            // 
            this.gbImport.Controls.Add(this.gbPathImportExcel);
            this.gbImport.Controls.Add(this.gbPathImportXml);
            this.gbImport.Controls.Add(this.gbPathImportXsd);
            this.gbImport.Location = new System.Drawing.Point(355, 73);
            this.gbImport.Name = "gbImport";
            this.gbImport.Size = new System.Drawing.Size(337, 190);
            this.gbImport.TabIndex = 40;
            this.gbImport.TabStop = false;
            this.gbImport.Text = "Импорт";
            // 
            // gbParameters
            // 
            this.gbParameters.Controls.Add(this.btnGo);
            this.gbParameters.Controls.Add(this.gbBookType);
            this.gbParameters.Controls.Add(this.gbBookFormat);
            this.gbParameters.Controls.Add(this.gbNumberKorr);
            this.gbParameters.Controls.Add(this.gbPeriod);
            this.gbParameters.Location = new System.Drawing.Point(12, 73);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(337, 331);
            this.gbParameters.TabIndex = 41;
            this.gbParameters.TabStop = false;
            this.gbParameters.Text = "Параметры";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(6, 271);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(323, 50);
            this.btnGo.TabIndex = 34;
            this.btnGo.Text = "Выполнить";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // ofdFile
            // 
            this.ofdFile.FileName = "openFileDialog1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 416);
            this.Controls.Add(this.gbParameters);
            this.Controls.Add(this.gbImport);
            this.Controls.Add(this.gbExport);
            this.Controls.Add(this.gbModes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 455);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Налоговая декларация";
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberKorr)).EndInit();
            this.gbModes.ResumeLayout(false);
            this.gbBookType.ResumeLayout(false);
            this.gbBookFormat.ResumeLayout(false);
            this.gbNumberKorr.ResumeLayout(false);
            this.gbPeriod.ResumeLayout(false);
            this.gbPathImportExcel.ResumeLayout(false);
            this.gbPathImportExcel.PerformLayout();
            this.gbPathImportXml.ResumeLayout(false);
            this.gbPathImportXml.PerformLayout();
            this.gbPathImportXsd.ResumeLayout(false);
            this.gbPathImportXsd.PerformLayout();
            this.gbPathExportXml.ResumeLayout(false);
            this.gbPathExportXml.PerformLayout();
            this.gbPathExportExcel.ResumeLayout(false);
            this.gbPathExportExcel.PerformLayout();
            this.gbExport.ResumeLayout(false);
            this.gbImport.ResumeLayout(false);
            this.gbParameters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbBookTypes;
        private System.Windows.Forms.ComboBox cbBookFormats;
        private System.Windows.Forms.NumericUpDown nudNumberKorr;
        private System.Windows.Forms.TextBox tbPathImportExcel;
        private System.Windows.Forms.Button btnPathImportExcel;
        private System.Windows.Forms.Button btnClearImportExcel;
        private System.Windows.Forms.Button btnClearImportXml;
        private System.Windows.Forms.Button btnPathImportXml;
        private System.Windows.Forms.TextBox tbPathImportXml;
        private System.Windows.Forms.Button btnClearXsd;
        private System.Windows.Forms.Button btnPathXsd;
        private System.Windows.Forms.TextBox tbPathImportXsd;
        private System.Windows.Forms.Button btnClearExportExcel;
        private System.Windows.Forms.Button btnPathExportExcel;
        private System.Windows.Forms.TextBox tbPathExportExcel;
        private System.Windows.Forms.Button btnClearExportXml;
        private System.Windows.Forms.Button btnPathExportXml;
        private System.Windows.Forms.TextBox tbPathExportXml;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.GroupBox gbModes;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.GroupBox gbBookType;
        private System.Windows.Forms.GroupBox gbBookFormat;
        private System.Windows.Forms.GroupBox gbNumberKorr;
        private System.Windows.Forms.GroupBox gbPeriod;
        private System.Windows.Forms.GroupBox gbPathImportExcel;
        private System.Windows.Forms.GroupBox gbPathImportXml;
        private System.Windows.Forms.GroupBox gbPathImportXsd;
        private System.Windows.Forms.GroupBox gbPathExportXml;
        private System.Windows.Forms.GroupBox gbPathExportExcel;
        private System.Windows.Forms.GroupBox gbExport;
        private System.Windows.Forms.GroupBox gbImport;
        private System.Windows.Forms.GroupBox gbParameters;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.OpenFileDialog ofdFile;
        private System.Windows.Forms.FolderBrowserDialog fbdFolder;
    }
}