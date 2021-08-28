namespace UI
{
    partial class frmMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbParameters = new System.Windows.Forms.GroupBox();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbVersionSbis = new System.Windows.Forms.ComboBox();
            this.btnOpenFolderExport = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.gbPathExport = new System.Windows.Forms.GroupBox();
            this.tbPathExport = new System.Windows.Forms.TextBox();
            this.btnClearExport = new System.Windows.Forms.Button();
            this.btnPathExport = new System.Windows.Forms.Button();
            this.gbPathImport = new System.Windows.Forms.GroupBox();
            this.lbInputPath = new System.Windows.Forms.ListBox();
            this.btnClearImport = new System.Windows.Forms.Button();
            this.btnPathImport = new System.Windows.Forms.Button();
            this.gbBookType = new System.Windows.Forms.GroupBox();
            this.cbBookType = new System.Windows.Forms.ComboBox();
            this.gbNumberKorr = new System.Windows.Forms.GroupBox();
            this.nudNumberKorr = new System.Windows.Forms.NumericUpDown();
            this.gbModes = new System.Windows.Forms.GroupBox();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.pParams = new System.Windows.Forms.Panel();
            this.pLog = new System.Windows.Forms.Panel();
            this.msLog = new System.Windows.Forms.MenuStrip();
            this.очиститьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьВФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbParameters.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbPathExport.SuspendLayout();
            this.gbPathImport.SuspendLayout();
            this.gbBookType.SuspendLayout();
            this.gbNumberKorr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberKorr)).BeginInit();
            this.gbModes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.pParams.SuspendLayout();
            this.pLog.SuspendLayout();
            this.msLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbParameters
            // 
            this.gbParameters.Controls.Add(this.pbProgress);
            this.gbParameters.Controls.Add(this.groupBox1);
            this.gbParameters.Controls.Add(this.btnOpenFolderExport);
            this.gbParameters.Controls.Add(this.btnGo);
            this.gbParameters.Controls.Add(this.gbPathExport);
            this.gbParameters.Controls.Add(this.gbPathImport);
            this.gbParameters.Controls.Add(this.gbBookType);
            this.gbParameters.Controls.Add(this.gbNumberKorr);
            this.gbParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbParameters.Location = new System.Drawing.Point(0, 50);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(330, 390);
            this.gbParameters.TabIndex = 45;
            this.gbParameters.TabStop = false;
            this.gbParameters.Text = "[ Параметры ]";
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(6, 291);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(318, 23);
            this.pbProgress.Step = 1;
            this.pbProgress.TabIndex = 41;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbVersionSbis);
            this.groupBox1.Location = new System.Drawing.Point(6, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 50);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Версия СБИС";
            // 
            // cbVersionSbis
            // 
            this.cbVersionSbis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVersionSbis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersionSbis.DropDownWidth = 200;
            this.cbVersionSbis.FormattingEnabled = true;
            this.cbVersionSbis.Items.AddRange(new object[] {
            "Математическое округление (5.005 -> 5.01)",
            "Отбрасывание дробной части (5.009 -> 5.00)"});
            this.cbVersionSbis.Location = new System.Drawing.Point(6, 19);
            this.cbVersionSbis.Name = "cbVersionSbis";
            this.cbVersionSbis.Size = new System.Drawing.Size(306, 21);
            this.cbVersionSbis.TabIndex = 2;
            // 
            // btnOpenFolderExport
            // 
            this.btnOpenFolderExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolderExport.Location = new System.Drawing.Point(179, 341);
            this.btnOpenFolderExport.Name = "btnOpenFolderExport";
            this.btnOpenFolderExport.Size = new System.Drawing.Size(145, 40);
            this.btnOpenFolderExport.TabIndex = 38;
            this.btnOpenFolderExport.Text = "Открыть папку выгрузки (экспорта)";
            this.btnOpenFolderExport.UseVisualStyleBackColor = true;
            this.btnOpenFolderExport.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnGo.Location = new System.Drawing.Point(6, 341);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(167, 40);
            this.btnGo.TabIndex = 34;
            this.btnGo.Text = "Выполнить";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // gbPathExport
            // 
            this.gbPathExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPathExport.Controls.Add(this.tbPathExport);
            this.gbPathExport.Controls.Add(this.btnClearExport);
            this.gbPathExport.Controls.Add(this.btnPathExport);
            this.gbPathExport.Location = new System.Drawing.Point(6, 179);
            this.gbPathExport.Name = "gbPathExport";
            this.gbPathExport.Size = new System.Drawing.Size(318, 50);
            this.gbPathExport.TabIndex = 37;
            this.gbPathExport.TabStop = false;
            this.gbPathExport.Text = "Путь выгрузки (экспорта)";
            // 
            // tbPathExport
            // 
            this.tbPathExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPathExport.Location = new System.Drawing.Point(6, 19);
            this.tbPathExport.Name = "tbPathExport";
            this.tbPathExport.ReadOnly = true;
            this.tbPathExport.Size = new System.Drawing.Size(233, 20);
            this.tbPathExport.TabIndex = 7;
            // 
            // btnClearExport
            // 
            this.btnClearExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearExport.Location = new System.Drawing.Point(280, 19);
            this.btnClearExport.Name = "btnClearExport";
            this.btnClearExport.Size = new System.Drawing.Size(29, 20);
            this.btnClearExport.TabIndex = 9;
            this.btnClearExport.Text = "X";
            this.btnClearExport.UseVisualStyleBackColor = true;
            this.btnClearExport.Click += new System.EventHandler(this.btnClearExport_Click);
            // 
            // btnPathExport
            // 
            this.btnPathExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPathExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathExport.Location = new System.Drawing.Point(245, 19);
            this.btnPathExport.Name = "btnPathExport";
            this.btnPathExport.Size = new System.Drawing.Size(29, 20);
            this.btnPathExport.TabIndex = 8;
            this.btnPathExport.Text = "••";
            this.btnPathExport.UseVisualStyleBackColor = true;
            this.btnPathExport.Click += new System.EventHandler(this.btnPathExport_Click);
            // 
            // gbPathImport
            // 
            this.gbPathImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPathImport.Controls.Add(this.lbInputPath);
            this.gbPathImport.Controls.Add(this.btnClearImport);
            this.gbPathImport.Controls.Add(this.btnPathImport);
            this.gbPathImport.Location = new System.Drawing.Point(6, 75);
            this.gbPathImport.Name = "gbPathImport";
            this.gbPathImport.Size = new System.Drawing.Size(318, 98);
            this.gbPathImport.TabIndex = 35;
            this.gbPathImport.TabStop = false;
            this.gbPathImport.Text = "Путь к файлу";
            // 
            // lbInputPath
            // 
            this.lbInputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInputPath.FormattingEnabled = true;
            this.lbInputPath.HorizontalScrollbar = true;
            this.lbInputPath.Location = new System.Drawing.Point(6, 19);
            this.lbInputPath.Name = "lbInputPath";
            this.lbInputPath.Size = new System.Drawing.Size(233, 69);
            this.lbInputPath.TabIndex = 10;
            // 
            // btnClearImport
            // 
            this.btnClearImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearImport.Location = new System.Drawing.Point(280, 19);
            this.btnClearImport.Name = "btnClearImport";
            this.btnClearImport.Size = new System.Drawing.Size(29, 20);
            this.btnClearImport.TabIndex = 9;
            this.btnClearImport.Text = "X";
            this.btnClearImport.UseVisualStyleBackColor = true;
            this.btnClearImport.Click += new System.EventHandler(this.btnClearImportExcel_Click);
            // 
            // btnPathImport
            // 
            this.btnPathImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPathImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPathImport.Location = new System.Drawing.Point(245, 19);
            this.btnPathImport.Name = "btnPathImport";
            this.btnPathImport.Size = new System.Drawing.Size(29, 20);
            this.btnPathImport.TabIndex = 8;
            this.btnPathImport.Text = "••";
            this.btnPathImport.UseVisualStyleBackColor = true;
            this.btnPathImport.Click += new System.EventHandler(this.btnPathImport_Click);
            // 
            // gbBookType
            // 
            this.gbBookType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBookType.Controls.Add(this.cbBookType);
            this.gbBookType.Location = new System.Drawing.Point(6, 19);
            this.gbBookType.Name = "gbBookType";
            this.gbBookType.Size = new System.Drawing.Size(167, 50);
            this.gbBookType.TabIndex = 30;
            this.gbBookType.TabStop = false;
            this.gbBookType.Text = "Тип книги / журнала";
            // 
            // cbBookType
            // 
            this.cbBookType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBookType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBookType.DropDownWidth = 200;
            this.cbBookType.FormattingEnabled = true;
            this.cbBookType.Items.AddRange(new object[] {
            "08 - Книга Покупок ",
            "09 - Книга Продаж",
            "10 - Журнал Выставленных счетов-фактур",
            "11 - Журнал Полученных счетов-фактур"});
            this.cbBookType.Location = new System.Drawing.Point(6, 19);
            this.cbBookType.Name = "cbBookType";
            this.cbBookType.Size = new System.Drawing.Size(154, 21);
            this.cbBookType.TabIndex = 1;
            // 
            // gbNumberKorr
            // 
            this.gbNumberKorr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbNumberKorr.Controls.Add(this.nudNumberKorr);
            this.gbNumberKorr.Location = new System.Drawing.Point(179, 19);
            this.gbNumberKorr.Name = "gbNumberKorr";
            this.gbNumberKorr.Size = new System.Drawing.Size(145, 50);
            this.gbNumberKorr.TabIndex = 32;
            this.gbNumberKorr.TabStop = false;
            this.gbNumberKorr.Text = "Номер корректировки";
            // 
            // nudNumberKorr
            // 
            this.nudNumberKorr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumberKorr.Location = new System.Drawing.Point(6, 19);
            this.nudNumberKorr.Name = "nudNumberKorr";
            this.nudNumberKorr.Size = new System.Drawing.Size(133, 20);
            this.nudNumberKorr.TabIndex = 5;
            // 
            // gbModes
            // 
            this.gbModes.Controls.Add(this.cbMode);
            this.gbModes.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbModes.Location = new System.Drawing.Point(0, 0);
            this.gbModes.Name = "gbModes";
            this.gbModes.Size = new System.Drawing.Size(330, 50);
            this.gbModes.TabIndex = 42;
            this.gbModes.TabStop = false;
            this.gbModes.Text = "[ Режим работы ]";
            // 
            // cbMode
            // 
            this.cbMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "01 - Декларация из Excel файла",
            "02 - Проверка Excel файла",
            "03 - Проверка XML файла по схеме СБИС",
            "04 - Проверка XML файла на суммы"});
            this.cbMode.Location = new System.Drawing.Point(6, 19);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(318, 21);
            this.cbMode.TabIndex = 2;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.Color.White;
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(0, 24);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(444, 416);
            this.tbLog.TabIndex = 39;
            this.tbLog.WordWrap = false;
            this.tbLog.TextChanged += new System.EventHandler(this.tbLog_TextChanged);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.pParams);
            this.scMain.Panel1MinSize = 330;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.pLog);
            this.scMain.Size = new System.Drawing.Size(778, 440);
            this.scMain.SplitterDistance = 330;
            this.scMain.TabIndex = 46;
            // 
            // pParams
            // 
            this.pParams.Controls.Add(this.gbParameters);
            this.pParams.Controls.Add(this.gbModes);
            this.pParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pParams.Location = new System.Drawing.Point(0, 0);
            this.pParams.Name = "pParams";
            this.pParams.Size = new System.Drawing.Size(330, 440);
            this.pParams.TabIndex = 0;
            // 
            // pLog
            // 
            this.pLog.Controls.Add(this.tbLog);
            this.pLog.Controls.Add(this.msLog);
            this.pLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLog.Location = new System.Drawing.Point(0, 0);
            this.pLog.Name = "pLog";
            this.pLog.Size = new System.Drawing.Size(444, 440);
            this.pLog.TabIndex = 0;
            // 
            // msLog
            // 
            this.msLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.очиститьToolStripMenuItem,
            this.сохранитьВФайлToolStripMenuItem});
            this.msLog.Location = new System.Drawing.Point(0, 0);
            this.msLog.Name = "msLog";
            this.msLog.Size = new System.Drawing.Size(444, 24);
            this.msLog.TabIndex = 40;
            this.msLog.Text = "menuStrip1";
            // 
            // очиститьToolStripMenuItem
            // 
            this.очиститьToolStripMenuItem.Name = "очиститьToolStripMenuItem";
            this.очиститьToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.очиститьToolStripMenuItem.Text = "Очистить";
            this.очиститьToolStripMenuItem.Click += new System.EventHandler(this.очиститьToolStripMenuItem_Click);
            // 
            // сохранитьВФайлToolStripMenuItem
            // 
            this.сохранитьВФайлToolStripMenuItem.Name = "сохранитьВФайлToolStripMenuItem";
            this.сохранитьВФайлToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.сохранитьВФайлToolStripMenuItem.Text = "Сохранить в файл";
            this.сохранитьВФайлToolStripMenuItem.Click += new System.EventHandler(this.сохранитьВФайлToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 440);
            this.Controls.Add(this.scMain);
            this.MainMenuStrip = this.msLog;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Налоговая декларация (формат СБИС)";
            this.gbParameters.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbPathExport.ResumeLayout(false);
            this.gbPathExport.PerformLayout();
            this.gbPathImport.ResumeLayout(false);
            this.gbBookType.ResumeLayout(false);
            this.gbNumberKorr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberKorr)).EndInit();
            this.gbModes.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.pParams.ResumeLayout(false);
            this.pLog.ResumeLayout(false);
            this.pLog.PerformLayout();
            this.msLog.ResumeLayout(false);
            this.msLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbParameters;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.GroupBox gbBookType;
        private System.Windows.Forms.ComboBox cbBookType;
        private System.Windows.Forms.GroupBox gbNumberKorr;
        private System.Windows.Forms.NumericUpDown nudNumberKorr;
        private System.Windows.Forms.GroupBox gbModes;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.GroupBox gbPathExport;
        private System.Windows.Forms.TextBox tbPathExport;
        private System.Windows.Forms.Button btnClearExport;
        private System.Windows.Forms.Button btnPathExport;
        private System.Windows.Forms.GroupBox gbPathImport;
        private System.Windows.Forms.Button btnClearImport;
        private System.Windows.Forms.Button btnPathImport;
        private System.Windows.Forms.Button btnOpenFolderExport;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.Panel pParams;
        private System.Windows.Forms.Panel pLog;
        private System.Windows.Forms.MenuStrip msLog;
        private System.Windows.Forms.ToolStripMenuItem очиститьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьВФайлToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbVersionSbis;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.ListBox lbInputPath;
    }
}

