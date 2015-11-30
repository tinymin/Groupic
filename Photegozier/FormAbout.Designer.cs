namespace Groupic
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.linkBlog = new System.Windows.Forms.LinkLabel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.iconURL = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.linkBlog);
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.iconURL);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.picLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 583);
            this.panel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 447);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "문의 :";
            // 
            // linkBlog
            // 
            this.linkBlog.AutoSize = true;
            this.linkBlog.Location = new System.Drawing.Point(70, 447);
            this.linkBlog.Name = "linkBlog";
            this.linkBlog.Size = new System.Drawing.Size(182, 12);
            this.linkBlog.TabIndex = 10;
            this.linkBlog.TabStop = true;
            this.linkBlog.Text = "http://blog.naver.com/tinymin/";
            this.linkBlog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBlog_LinkClicked);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(239, 356);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(115, 12);
            this.lblVersion.TabIndex = 9;
            this.lblVersion.Text = "Version : [Version]";
            // 
            // iconURL
            // 
            this.iconURL.AutoSize = true;
            this.iconURL.Location = new System.Drawing.Point(290, 512);
            this.iconURL.Name = "iconURL";
            this.iconURL.Size = new System.Drawing.Size(184, 12);
            this.iconURL.TabIndex = 8;
            this.iconURL.TabStop = true;
            this.iconURL.Text = "http://hopstarter.deviantart.com";
            this.iconURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.iconURL_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 399);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(385, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "본 프로그램은 공개용 프로그램입니다.\r\n이 프로그램을 사용하여 발생하는 문제에 대해서는 책임지지 않습니다.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 489);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(530, 72);
            this.label2.TabIndex = 6;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(157, 271);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 73);
            this.label1.TabIndex = 5;
            this.label1.Text = "Groupic";
            // 
            // picLogo
            // 
            this.picLogo.Image = global::Groupic.Properties.Resources.icon;
            this.picLogo.Location = new System.Drawing.Point(161, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(256, 256);
            this.picLogo.TabIndex = 4;
            this.picLogo.TabStop = false;
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(583, 583);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.Text = "Groupic";
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel iconURL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkBlog;

    }
}