
namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.TextBox = new System.Windows.Forms.TextBox();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ContextMenuStrip_NotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Browse = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Restart = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStrip_NotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBox
            // 
            this.TextBox.BackColor = System.Drawing.SystemColors.Desktop;
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TextBox.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.TextBox.Location = new System.Drawing.Point(0, 0);
            this.TextBox.Margin = new System.Windows.Forms.Padding(0);
            this.TextBox.Multiline = true;
            this.TextBox.Name = "TextBox";
            this.TextBox.ReadOnly = true;
            this.TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox.ShortcutsEnabled = false;
            this.TextBox.Size = new System.Drawing.Size(800, 450);
            this.TextBox.TabIndex = 0;
            this.TextBox.TabStop = false;
            this.TextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBox_PreviewKeyDown);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.ContextMenuStrip_NotifyIcon;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "NotifyIcon";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // ContextMenuStrip_NotifyIcon
            // 
            this.ContextMenuStrip_NotifyIcon.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ContextMenuStrip_NotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Browse,
            this.ToolStripSeparator,
            this.ToolStripMenuItem_Restart,
            this.ToolStripMenuItem_Exit});
            this.ContextMenuStrip_NotifyIcon.Name = "ContextMenuStrip";
            this.ContextMenuStrip_NotifyIcon.ShowCheckMargin = true;
            this.ContextMenuStrip_NotifyIcon.Size = new System.Drawing.Size(266, 124);
            // 
            // ToolStripMenuItem_Browse
            // 
            this.ToolStripMenuItem_Browse.Enabled = false;
            this.ToolStripMenuItem_Browse.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ToolStripMenuItem_Browse.Name = "ToolStripMenuItem_Browse";
            this.ToolStripMenuItem_Browse.Size = new System.Drawing.Size(265, 38);
            this.ToolStripMenuItem_Browse.Text = "浏览";
            // 
            // ToolStripSeparator
            // 
            this.ToolStripSeparator.Name = "ToolStripSeparator";
            this.ToolStripSeparator.Size = new System.Drawing.Size(262, 6);
            // 
            // ToolStripMenuItem_Restart
            // 
            this.ToolStripMenuItem_Restart.Name = "ToolStripMenuItem_Restart";
            this.ToolStripMenuItem_Restart.Size = new System.Drawing.Size(265, 38);
            this.ToolStripMenuItem_Restart.Text = "重启WebHost";
            this.ToolStripMenuItem_Restart.Click += new System.EventHandler(this.ToolStripMenuItem_Restart_Click);
            // 
            // ToolStripMenuItem_Exit
            // 
            this.ToolStripMenuItem_Exit.Name = "ToolStripMenuItem_Exit";
            this.ToolStripMenuItem_Exit.Size = new System.Drawing.Size(265, 38);
            this.ToolStripMenuItem_Exit.Text = "退出";
            this.ToolStripMenuItem_Exit.Click += new System.EventHandler(this.ToolStripMenuItem_Exit_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Liyanjie.Desktop.WebHost";
            this.Load += new System.EventHandler(this.Form_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form_PreviewKeyDown);
            this.ContextMenuStrip_NotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TextBox;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_NotifyIcon;
#pragma warning restore CS0108 // 成员隐藏继承的成员；缺少关键字 new
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Browse;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Restart;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Exit;
    }
}

