using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Groupic
{
    public partial class FormProgressBar : Form
    {
        public FormProgressBar()
        {
            InitializeComponent();
        }

        private void FormProgressBar_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (i++ < 100)
            {
                progressBar1.PerformStep();
                Thread.Sleep(100);
            }
        }

    }
}
