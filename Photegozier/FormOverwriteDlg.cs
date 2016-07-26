using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Groupic
{
    public enum OverwriteResult {Overwrite, OverwriteToAll, NotMove, NotMoveToAll, Rename, RenameToAll, Cancel };

    public partial class FormOverwriteDlg : Form
    {
        private string destFile;

        OverwriteResult lastResult = OverwriteResult.Cancel;
        OverwriteResult result = OverwriteResult.Cancel;

        public FormOverwriteDlg()
        {
            InitializeComponent();
            InitStyle();
        }

        private void InitStyle()
        {
            
        }

        public OverwriteResult ShowOverwriteDialog(String sourceFile, String destFilePath)
        {
            this.destFile = destFilePath;
            this.lblSourceFile.Text = String.Format("{0}{1}", "이동 대상 파일 : ",sourceFile);
            this.lblTargetName.Text = String.Format("{0}{1}", "기존 파일 : ", destFilePath);
            this.lblNewName.Text = String.Format("{0}{1}", "변경 될 파일명 : ", GPUtil.GetNewTargetName(destFilePath));

            this.pbThumbOriginal.Image = GPUtil.GetThumbNail(sourceFile, 150, 100);
            this.pbThumbNew.Image = GPUtil.GetThumbNail(destFilePath, 150, 100);

            return ShowOverwriteDialog();
        }

        private OverwriteResult ShowOverwriteDialog()
        {
            result = OverwriteResult.Cancel;

            if (lastResult == OverwriteResult.OverwriteToAll)
            {
                result = OverwriteResult.Overwrite;
            }
            else if (lastResult == OverwriteResult.NotMoveToAll)
            {
                result = OverwriteResult.NotMove;
            }
            else if (lastResult == OverwriteResult.RenameToAll)
            {
                result = OverwriteResult.Rename;
            }
            else
            {
                base.ShowDialog();
            }
            return result;
        }

        private void btnOverwrite_Click(object sender, EventArgs e)
        {
            result = OverwriteResult.Overwrite;
            lastResult = OverwriteResult.Overwrite;
            
            if (true ==  chkApplyToAll.Checked)
                lastResult = OverwriteResult.OverwriteToAll;

            DialogResult = DialogResult.OK;
        }

        private void btnNotMove_Click(object sender, EventArgs e)
        {
            result = OverwriteResult.NotMove;
            lastResult = OverwriteResult.NotMove;

            if (true == chkApplyToAll.Checked)
                lastResult = OverwriteResult.NotMoveToAll;

            DialogResult = DialogResult.OK;
        }

        private void btnMoveRename_Click(object sender, EventArgs e)
        {
            result = OverwriteResult.Rename;
            lastResult = OverwriteResult.Rename;

            if (true == chkApplyToAll.Checked)
                lastResult = OverwriteResult.RenameToAll;

            DialogResult = DialogResult.OK;
        }
    }
}
