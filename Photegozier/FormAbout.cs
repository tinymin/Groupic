using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Groupic
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void iconURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://hopstarter.deviantart.com");
        }

        private void linkBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://blog.naver.com/PostList.nhn?blogId=tinymin&from=postList&categoryNo=55");
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version; // 현재 실행되는 어셈블리..dll의 버전 가져오기
            lblVersion.Text = "Version : " + ver.ToString();
        }
    }
}
