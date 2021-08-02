﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            var appIcon = ConfigurationManager.AppSettings["AppIcon"] ?? "icon.ico";
            var iconPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appIcon));
            if (File.Exists(iconPath))
            {
                var icon = new System.Drawing.Icon(iconPath);
                this.Icon = icon;
                this.NotifyIcon.Icon = icon;
            }
            var appName = ConfigurationManager.AppSettings["AppName"];
            if (!string.IsNullOrEmpty(appName))
            {
                this.Text = appName;
                this.NotifyIcon.Text = appName;
            }

            this.Visible = false;
            this.FormClosing += Form_FormClosing;
            this.LogShowing += Form_LogShowing;

            Task.Run(() => HostManager.Start());
            foreach (var url in HostManager.GetUrls().Reverse())
            {
                var item = new ToolStripMenuItem
                {
                    Name = url,
                    Text = $"    {url}",
                    Size = new System.Drawing.Size(322, 38),
                    Tag = url,
                };
                item.Click += ToolStripMenuItem_Open_Click;
                this.ContextMenuStrip_NotifyIcon.Items.Insert(1, item);
            }
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;

                return;
            }
        }
        private void Form_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            PreviewKeyDown_(sender, e);
        }
        private void Form_LogShowing(object sender, Logging.LogMessage e)
        {
            this.TextBox.AppendText(e.Message);
        }

        private void TextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            PreviewKeyDown_(sender, e);
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void ToolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                Process.Start("explorer", item.Tag.ToString());
        }
        private void ToolStripMenuItem_Restart_Click(object sender, EventArgs e)
        {
            HostManager.Close();
            Task.Run(async () =>
            {
                this.TextBox.Clear();
                this.TextBox.AppendText($"WebHost restarting……{Environment.NewLine}");
                await Task.Delay(3000);
                this.TextBox.AppendText($"WebHost restarts success.{Environment.NewLine}{Environment.NewLine}");
                HostManager.Start();
            });
        }
        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private event EventHandler<Logging.LogMessage> LogShowing;

        internal void ShowLog(Logging.LogMessage log)
        {
            LogShowing?.Invoke(this, log);
        }

        void PreviewKeyDown_(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                this.TextBox.AppendText($"{Environment.NewLine}Shutting down...");
                System.Threading.Thread.Sleep(1000);
                Exit();
            }
        }

        void Exit()
        {
            HostManager.Close();
            Application.Exit();
        }
    }
}
