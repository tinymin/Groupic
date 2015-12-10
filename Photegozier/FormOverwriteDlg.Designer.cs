namespace Groupic
{
    partial class FormOverwriteDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOverwriteDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.chkApplyToAll = new System.Windows.Forms.CheckBox();
            this.btnOverwrite = new System.Windows.Forms.Button();
            this.btnNotMove = new System.Windows.Forms.Button();
            this.btnMoveRename = new System.Windows.Forms.Button();
            this.lblSourceFile = new System.Windows.Forms.Label();
            this.lblTargetName = new System.Windows.Forms.Label();
            this.lblNewName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "이 위치에 이름이 같은 파일이 있습니다.";
            // 
            // chkApplyToAll
            // 
            this.chkApplyToAll.AutoSize = true;
            this.chkApplyToAll.Location = new System.Drawing.Point(14, 186);
            this.chkApplyToAll.Name = "chkApplyToAll";
            this.chkApplyToAll.Size = new System.Drawing.Size(116, 16);
            this.chkApplyToAll.TabIndex = 4;
            this.chkApplyToAll.Text = "모든 파일에 적용";
            this.chkApplyToAll.UseVisualStyleBackColor = true;
            // 
            // btnOverwrite
            // 
            this.btnOverwrite.BackColor = System.Drawing.Color.Transparent;
            this.btnOverwrite.Location = new System.Drawing.Point(17, 120);
            this.btnOverwrite.Name = "btnOverwrite";
            this.btnOverwrite.Size = new System.Drawing.Size(160, 50);
            this.btnOverwrite.TabIndex = 1;
            this.btnOverwrite.Text = "덮어쓰기";
            this.btnOverwrite.UseVisualStyleBackColor = false;
            this.btnOverwrite.Click += new System.EventHandler(this.btnOverwrite_Click);
            // 
            // btnNotMove
            // 
            this.btnNotMove.Location = new System.Drawing.Point(211, 120);
            this.btnNotMove.Name = "btnNotMove";
            this.btnNotMove.Size = new System.Drawing.Size(160, 50);
            this.btnNotMove.TabIndex = 2;
            this.btnNotMove.Text = "이동 안 함";
            this.btnNotMove.UseVisualStyleBackColor = true;
            this.btnNotMove.Click += new System.EventHandler(this.btnNotMove_Click);
            // 
            // btnMoveRename
            // 
            this.btnMoveRename.Location = new System.Drawing.Point(396, 120);
            this.btnMoveRename.Name = "btnMoveRename";
            this.btnMoveRename.Size = new System.Drawing.Size(160, 50);
            this.btnMoveRename.TabIndex = 3;
            this.btnMoveRename.Text = "이름 변경하여 이동";
            this.btnMoveRename.UseVisualStyleBackColor = true;
            this.btnMoveRename.Click += new System.EventHandler(this.btnMoveRename_Click);
            // 
            // lblSourceFile
            // 
            this.lblSourceFile.AutoSize = true;
            this.lblSourceFile.Location = new System.Drawing.Point(46, 41);
            this.lblSourceFile.Name = "lblSourceFile";
            this.lblSourceFile.Size = new System.Drawing.Size(65, 12);
            this.lblSourceFile.TabIndex = 5;
            this.lblSourceFile.Text = "기존 파일 :";
            // 
            // lblTargetName
            // 
            this.lblTargetName.AutoSize = true;
            this.lblTargetName.Location = new System.Drawing.Point(29, 63);
            this.lblTargetName.Name = "lblTargetName";
            this.lblTargetName.Size = new System.Drawing.Size(85, 12);
            this.lblTargetName.TabIndex = 6;
            this.lblTargetName.Text = "이동 할 파일 : ";
            // 
            // lblNewName
            // 
            this.lblNewName.AutoSize = true;
            this.lblNewName.Location = new System.Drawing.Point(20, 94);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(93, 12);
            this.lblNewName.TabIndex = 7;
            this.lblNewName.Text = "변경될 파일명 : ";
            // 
            // FormOverwriteDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(859, 212);
            this.Controls.Add(this.lblNewName);
            this.Controls.Add(this.lblTargetName);
            this.Controls.Add(this.lblSourceFile);
            this.Controls.Add(this.btnMoveRename);
            this.Controls.Add(this.btnNotMove);
            this.Controls.Add(this.btnOverwrite);
            this.Controls.Add(this.chkApplyToAll);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOverwriteDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "파일 이동";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkApplyToAll;
        private System.Windows.Forms.Button btnOverwrite;
        private System.Windows.Forms.Button btnNotMove;
        private System.Windows.Forms.Button btnMoveRename;
        private System.Windows.Forms.Label lblSourceFile;
        private System.Windows.Forms.Label lblTargetName;
        private System.Windows.Forms.Label lblNewName;
    }
}