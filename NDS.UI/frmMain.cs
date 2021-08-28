﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using System.Threading;

namespace NDS.UI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            cbMode.Items.Clear();
            foreach (ModeType item in Enum.GetValues(typeof(ModeType)))
                cbMode.Items.Add(item);
            cbMode.SelectedIndex = 0;

            cbRoundMode.Items.Clear();
            foreach (RoundType item in Enum.GetValues(typeof(RoundType)))
                cbRoundMode.Items.Add(item);
            cbRoundMode.SelectedIndex = 0;

            cbBookType.Items.Clear();
            foreach (BookType item in Enum.GetValues(typeof(BookType)))
                cbBookType.Items.Add(item);
            cbBookType.SelectedIndex = 0;

            cbVersionSbis.Items.Clear();
            foreach (VersionSbis item in Enum.GetValues(typeof(VersionSbis)))
                cbVersionSbis.Items.Add(item);
            cbVersionSbis.SelectedIndex = 0;

            //cbMode_SelectedIndexChanged(null, null);
            //cbMode.SelectedIndex = 0;            
            //cbBookType.SelectedIndex = 2;
            lbInputPath.Items.Add(@"C:\Files\NDS\08.xlsx");
            tbPathExport.Text = @"C:\Files\NDS";
            //cbRoundMode.SelectedIndex = 0;
            //btnGo_Click(null, null);

            SetGoStyle(true);
        }

        private void btnClearImportExcel_Click(object sender, EventArgs e)
        {
            lbInputPath.Items.Clear();
        }

        private void btnClearExport_Click(object sender, EventArgs e)
        {
            tbPathExport.Text = "";
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbPathImport.Text = "Путь к файлу " + (cbMode.SelectedItem is ModeType.ExcelToXml ? "Excel" : "XML");
        }

        Thread run = null;
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (run == null || !run.IsAlive)
            {
                Go();
                SetGoStyle(false);
            }
            else
            {
                Abort();
                SetGoStyle(true);
            }
        }

        private void SetGoStyle(bool isFirstState)
        {
            if (isFirstState)
            {
                btnGo.Text = "Выполнить";
                btnGo.BackColor = SystemColors.Control;
                btnGo.ForeColor = Color.Black;
            }
            else
            {
                btnGo.Text = "ПРЕРВАТЬ";
                btnGo.BackColor = Color.DarkRed;
                btnGo.ForeColor = Color.White;
            }
        }

        private void Abort()
        {
            if (run != null && run.IsAlive)
                run.Abort();
        }

        private void Go()
        {
            if (cbMode.SelectedIndex < 0)
            {
                MessageBox.Show("Не выбран режим работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cbBookType.SelectedIndex < 0)
            {
                MessageBox.Show("Не выбран тип книги / журнала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (lbInputPath.Items.Count == 0)
            {
                MessageBox.Show("Не выбран файл.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tbPathExport.Text == "" && cbMode.SelectedItem is ModeType.ExcelToXml)
            {
                MessageBox.Show("Не указан путь выгрузки (экспорта).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cbVersionSbis.SelectedIndex < 0)
            {
                MessageBox.Show("Не выбрана версия СБИС.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //--------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------

            pbProgress.Step = Core.Helper.LogLines;
            Callback callback = new Callback(LogAdd, StepProgress);
            string[] importFilePaths = new string[lbInputPath.Items.Count];
            for (int i = 0; i < lbInputPath.Items.Count; i++)
            {
                importFilePaths[i] = lbInputPath.Items[i].ToString();
            }
            ModeType modeType = (ModeType)cbMode.SelectedItem;
            BookType bookType = (BookType)cbBookType.SelectedItem;
            VersionSbis versionSbis = (VersionSbis)cbVersionSbis.SelectedItem;
            RoundType roundType = (RoundType)cbRoundMode.SelectedItem;
            byte numberCorr = (byte)nudNumberKorr.Value;
            string pathExport = tbPathExport.Text;

            run = new Thread(() => {
                Execute(
                    callback,
                    modeType,
                    bookType,
                    versionSbis,
                    roundType,
                    numberCorr,
                    importFilePaths,
                    pathExport);
            });
            run.Start();
        }

        private void Execute(Callback callback, ModeType modeType, BookType bookType, VersionSbis versionSbis, RoundType roundType, byte numberCorr, string[] importFilePaths, string pathExport)
        {
            DateTime startJob = DateTime.Now;
            Core.Core.Execute(
                modeType,
                bookType,
                importFilePaths,
                versionSbis,
                roundType,
                numberCorr,
                pathExport,
                callback);
            TimeSpan TotalTime = DateTime.Now.Subtract(startJob);
            this.Invoke(new MethodInvoker(() =>
            {
                tbLog.Text += Environment.NewLine + $"Итоговое время: {TotalTime.TimeFormat()}";
                MessageBox.Show("Выполнение завершено. Не забудьте ознакомиться с логом в области справа." + (callback.errorQnt > 0 ? " ИМЕЮТСЯ ОШИБКИ." : ""), "Выполнено");
                SetGoStyle(true);
            }));
        }

        private void LogAdd(string message)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                tbLog.Text += Environment.NewLine + $"{message}";
            }));
        }

        private void StepProgress(int value, int max)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (!pbProgress.Maximum.Equals(max))
                    pbProgress.Maximum = max;
                pbProgress.Value = value;
            }));
        }

        private class Callback : Core.Model.ICallback
        {
            public delegate void DLog(string message);
            public delegate void DProgress(int value, int max);
            private DLog log = null;
            private DProgress progress = null;
            public int errorQnt { get; set; }
            public Callback(DLog ilog, DProgress iprogress)
            {
                this.log = ilog;
                this.progress = iprogress;
                this.errorQnt = 0;
            }

            public void OnFailed(string message)
            {
                errorQnt++;
                log($"{message}");
            }

            public void OnMessage(string message, bool isRewriteLine = false)
            {
                //TODO: Доделать перезапись строки в ЛОГЕ
                log($"{message}");
            }

            public void OnSuccess(string message)
            {
                log($"{message}");
            }

            public void OnProgress(int value, int max)
            {
                progress(value, max);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbPathExport.Text != "")
                System.Diagnostics.Process.Start("explorer", tbPathExport.Text);
        }

        private void btnPathImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = (cbMode.SelectedItem is ModeType.ExcelToXml) ? "*.xlsx|*.xlsx|*.xls|*.xls" : "*.xml|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in ofd.FileNames)
                {
                    lbInputPath.Items.Add(item);
                }
            }
            ofd.Dispose();
        }

        private void btnPathExport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                tbPathExport.Text = fbd.SelectedPath + "\\";
            fbd.Dispose();
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите очистить область логирования?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                tbLog.Text = "";
        }

        private void сохранитьВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый файл (*.txt)|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.Write(tbLog.Text);
                    sw.Close();
                }
                try
                {
                    FileInfo fi = new FileInfo(sfd.FileName);
                    if (MessageBox.Show("Файл сохранен. Открыть папку с файлом?", "Готово", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        System.Diagnostics.Process.Start("explorer", fi.DirectoryName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            sfd.Dispose();
        }

        private void tbLog_TextChanged(object sender, EventArgs e)
        {
            //Application.DoEvents();
        }
    }
}
