using ExifLib;
using Groupic;
using Groupic.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace Groupic
{
    delegate void SetToolStripProgressBarValueCallback(int value);
    delegate void SetListViewItemsCallback(List<ListViewItem> listViewItems);

    public partial class Groupic : Form
    {
        #region DllImport
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(    // GetIniValue 를 위해
            String section,
            String key,
            String def,
            StringBuilder retVal,
            int size,
            String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // SetIniValue를 위해
            String section,
            String key,
            String val,
            String filePath);
        #endregion
        





        #region Data
        private String iniPath = ".\\setting.ini";
        private Counter counter = new Counter();
        private List<ListViewItem> listViewItems = new List<ListViewItem>();
        private WorkItems workItems = new WorkItems();
        #endregion






        #region Constructor
        public Groupic()
        {
            InitializeComponent();
        }
        #endregion
        





        #region EventHandler
        private void Form1_Load(object sender, EventArgs e)
        {
            MakeDLLIfNotExist();
            SetProgramTitle();

            fileListView.GridLines = true;
            fileListView.AllowDrop = true;

            workItems.ProgressBar += new SetToolStripProgressBarValueCallback(SetToolStripProgressBarValue);

            // INI에서 옵션 로드
            LoadINIOption(iniPath);
            
            SetPreviewFolderPath();
        }

        private void fileListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void fileListView_DragDrop(object sender, DragEventArgs e)
        {
            Thread thread = new Thread(() => DragDropThreadFunc(e));
            thread.Start();
        }

        private void btnCategorizing_Click(object sender, EventArgs e)
        {
            DoCategories();
        }

        private void btnDeleteAllFileList_Click(object sender, EventArgs e)
        {
            fileListView.Items.Clear();
        }

        private void btnDeleteSelectedFileList_Click(object sender, EventArgs e)
        {
            DeleteSelectedFileList(fileListView);
        }

        private void btnDeleteDoneItem_Click(object sender, EventArgs e)
        {
            DeleteDoneItems(fileListView);
        }

        private void toolStripAbout_Click(object sender, EventArgs e)
        {
            FormAbout dlgAbout = new FormAbout();
            dlgAbout.ShowDialog();
        }

        private void toolStripQuit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion
        
        
        
        
        
        
        #region EventHandlerForOption
        private void chkDateFormat_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "DateFormat", (chkDateFormat.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }

        private void chkMakeRoot_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "MakeRoot", (chkMakeRoot.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }

        private void chkUseDash_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "UseDash", (chkUseDash.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }

        private void chkCategoryByMonth_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "CategoryByMonth", (chkCategoryByMonth.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }

        private void chkAutoDeleteDoneItem_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "AutoDeleteDoneItem", (chkAutoDeleteDoneItem.Checked == true ? 1 : 0).ToString(), iniPath);
        }

        private void chkSepertateRawFile_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "SeperateRawFile", (chkSepertateRawFile.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }

        private void chkChangeFileName_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "ChangeFileName", (chkChangeFileName.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }

        private void chkDelRawIfJpgNotExist_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "DeleteRawIfJpgNotExist", (chkDelRawIfJpgNotExist.Checked == true ? 1 : 0).ToString(), iniPath);
            SetPreviewFolderPath();
        }
        #endregion






        #region Methods
        private void SetListViewItems(List<ListViewItem> listViewItems)
        {
            if (true == this.fileListView.InvokeRequired)
            {
                SetListViewItemsCallback d = new SetListViewItemsCallback(SetListViewItems);
                this.Invoke(d, new object[] { listViewItems });
            }
            else
            {
                this.fileListView.Items.Clear();
                this.fileListView.Items.AddRange(listViewItems.ToArray());
            }
            
        }

        private void SetToolStripProgressBarValue(int value)
        {
            if (true == this.toolStripProgressBar1.GetCurrentParent().InvokeRequired)
            {
                SetToolStripProgressBarValueCallback d = new SetToolStripProgressBarValueCallback(SetToolStripProgressBarValue);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.toolStripProgressBar1.Value = value;
            }
        }

        private void DeleteDoneItems(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (true == item.SubItems[1].Text.Equals("O"))  // 영문자 O 이다. 숫자 0 아님
                    item.Remove();
            }

            ReviseIndexNumber(listView);
        }

        private void DeleteSelectedFileList(ListView listView)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                item.Remove();
            }

            foreach (ListViewItem item in listView.CheckedItems)
            {
                item.Remove();
            }

            ReviseIndexNumber(listView);
        }

        private void MarkingCompleteItem(ListViewItem item)
        {
            item.ForeColor = Color.LightGray;
            item.SubItems[1].Text = "O"; // 영문자 O 이다. 숫자 0 아님
        }

        private void ProcessFileDelete(List<FileInfo> deleteFileList)
        {
            DialogResult answer = MessageBox.Show("주의!!\r\n\r\n[예]를 선택하면 JPG가 존재하지 않는 RAW파일이 삭제됩니다.\r\n[아니오]를 선택하면 [__Delete]폴더로 파일들이 이동 됩니다.\r\n[취소]를 선택하면 아무 작업도 하지 않습니다.\r\n\r\n★주의!!!★ [예]를 선택하면 파일 복구가 불가능합니다. 확실한 경우만 눌러주세요.", "JPG 파일이 없으면 RAW 파일 삭제", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);

            // 삭제 처리
            if (DialogResult.Yes == answer)
            {
                foreach (FileInfo file in deleteFileList)
                    file.Delete();
            }
            else if (DialogResult.No == answer) // 파일 이동 처리
            {
                foreach (FileInfo file in deleteFileList)
                {
                    String delFolder = file.Directory + "\\__Delete";
                    DirectoryInfo di = Directory.CreateDirectory(delFolder);

                    if (false == di.Exists)
                        continue;

                    file.MoveTo(delFolder + "\\" + file.Name);
                }
            }
            else // 취소 선택시
                ; // 아무것도 안함
        }

        private List<String> GetJpgFileList(ListView listView)
        {
            List<String> jpgList = new List<String>();

            foreach (ListViewItem item in listView.Items)
            {
                FileInfo fileInfo = new FileInfo(item.SubItems[2].Text);
                if (true == fileInfo.Extension.ToLower().Equals(".jpg"))
                    jpgList.Add(fileInfo.FullName);
            }

            return jpgList;
        }

        private void DoCategories()
        {
            if (0 >= fileListView.Items.Count)
            {
                MessageBox.Show("작업할 파일이 없습니다.");
                return;
            }

            DirectoryInfo destDirectory;
            FileInfo fileInfo;

            String sourceFilePath;
            String destFilePath;
            bool isDoneWork = false;
            int skipCnt = 0;
            FormOverwriteDlg overwriteDlg = new FormOverwriteDlg();
            RawFileList rawFileList = new RawFileList();
            List<String> jpgFileList = null;
            List<FileInfo> deleteFileList = new List<FileInfo>();

            // [JPG 파일이 없으면 RAW 파일 삭제] 옵션 선택시 RAW파일과 경로 및 파일명 비교를 위해 JPG 파일 리스트를 미리 가져온다
            if (true == chkDelRawIfJpgNotExist.Checked)
                jpgFileList = GetJpgFileList(fileListView);

            foreach (ListViewItem item in fileListView.Items)
            {
                // 이미 작업한 항목은 Skip 한다.
                if (true == item.SubItems[1].Text.Equals("O"))
                    continue;

                sourceFilePath = item.SubItems[2].Text;
                fileInfo = new FileInfo(sourceFilePath);

                // 파일이 존재하지 않으면 skip
                if (false == fileInfo.Exists)
                    continue;

                // [JPG 파일이 없으면 RAW 파일 삭제] 옵션 선택시 RAW파일인 경우 삭제 리스트에 넣는다.
                // 위 옵션을 켰더라도 RAW 파일만 목록에 있는 경우도 있다. 이 때는 RAW파일만 정리가 목적이므로 이 옵션을 수행하지 않는다.
                if (null != jpgFileList)
                {
                    if (true == chkDelRawIfJpgNotExist.Checked && rawFileList.IsContain(fileInfo.Extension))
                    {
                        String lookUpName = fileInfo.FullName.Replace(fileInfo.Extension, "") + ".jpg";
                        String lookResult = jpgFileList.Find(jpg => lookUpName.ToLower().Equals(jpg.ToLower()));

                        if (true == String.IsNullOrEmpty(lookResult))
                        {
                            deleteFileList.Add(fileInfo);
                            MarkingCompleteItem(item);
                            continue;
                        }
                    }
                }

                // 아래 {}블럭은 작업 완료 후에 더 이상 필요없는 값들이므로{}블럭 처리하여 지역변수로 세팅한 것이다.
                // 그러면 블럭 이후에는 메모리를 반환되는 장점이 있어 의도적인 처리이다.
                {
                    StringBuilder destDirectoryName = new StringBuilder(fileInfo.DirectoryName);
                    if (true == chkMakeRoot.Checked)
                        destDirectoryName.Append("\\Category");

                    destDirectoryName.Append("\\");


                    String destFolderName;

                    // 파일 생성날짜를 가져온다. (폴더명으로 쓰기 위함)
                    destFolderName = item.SubItems[3].Text;

                    // 사진을 월단위로 분류 한다.
                    if (true == chkCategoryByMonth.Checked)
                        destFolderName = destFolderName.Substring(0, 7);

                    // 옵션에 따라 날짜형식에 "-"를 포함하지 않는다.
                    if (false == chkUseDash.Checked)
                        destFolderName = destFolderName.Replace("-", "");

                    // 옵션에 따라 날짜 형식을 4자리 또는 2자리로 사용한다.
                    if (false == chkDateFormat.Checked)
                        destFolderName = destFolderName.Substring(2);

                    destDirectoryName.Append(destFolderName);


                    if (true == chkSepertateRawFile.Checked)
                    {
                        if (true == rawFileList.IsContain(fileInfo.Extension))
                            destDirectoryName.Append("\\Raw");
                    }

                    destDirectory = new DirectoryInfo(destDirectoryName.ToString());


                    // 대상 디렉토리가 존재하지 않는 경우에는 생성한다.
                    if (false == destDirectory.Exists)
                    {
                        destDirectory.Create();
                    }
                }

                // 대상 파일 경로를 생성.
                destFilePath = destDirectory.ToString() + "\\";
                if (true == chkChangeFileName.Checked)
                    destFilePath += GetExifDateFileName(sourceFilePath);
                else
                    destFilePath += fileInfo.Name;

                destFilePath = destFilePath.Replace("\\\\", "\\");
                

                // 대상 경로에 동일한 파일이 존재하면 덮어쓰기 다이얼로그를 띄운다.
                try
                {
                    if (true == File.Exists(destFilePath))
                    {
                        OverwriteResult overWriteResult = OverwriteResult.Cancel;
                        overWriteResult = overwriteDlg.ShowOverwriteDialog(sourceFilePath, destFilePath);

                        switch (overWriteResult)
                        {
                            case OverwriteResult.Overwrite: // 덮어쓰기
                                File.Delete(destFilePath);
                                break;
                            case OverwriteResult.NotMove: // 이동 안 함
                                skipCnt++;
                                continue;
                            case OverwriteResult.Rename: // 이름변경하여 이동
                                destFilePath = GPUtil.GetNewTargetName(destFilePath);
                                break;
                            case OverwriteResult.Cancel: //취소
                                return;
                        }
                    }

                    File.Move(sourceFilePath, destFilePath);
                    isDoneWork = true;
                    MarkingCompleteItem(item);
                    workItems.removeItem(sourceFilePath);
                } catch (IOException ex)
                {
                    MessageBox.Show(String.Format("다른 곳에서 파일이 사용 중입니다.\n\n{0}", destFilePath), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (true == chkAutoDeleteDoneItem.Checked)
                    item.Remove();
            } // end of foreach

            // 삭제할 파일이 존재하는 경우에 대한 처리
            if (0 < deleteFileList.Count)
            {
                ProcessFileDelete(deleteFileList);
            }

            if (true == isDoneWork)
                MessageBox.Show("작업 완료", "Groupic");
            else if (false == isDoneWork && (skipCnt != fileListView.Items.Count))
                MessageBox.Show("이미 작업이 완료된 파일들 입니다.", "Groupic");

            isDoneWork = false;
        }

        private void LoadINIOption(String filePath)
        {
            StringBuilder optionVal = new StringBuilder();

            if (true == new FileInfo(filePath).Exists)
            {
                // 옵션 - Category 폴더 하위에 분류
                GetPrivateProfileString("OPTION", "DateFormat", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkDateFormat.Checked = true;

                // 옵션 - 4자리 연도 사용
                GetPrivateProfileString("OPTION", "MakeRoot", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkMakeRoot.Checked = true;

                // 옵션 - "-" 사용
                GetPrivateProfileString("OPTION", "UseDash", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkUseDash.Checked = true;

                // 옵션 - 월별 분류
                GetPrivateProfileString("OPTION", "CategoryByMonth", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkCategoryByMonth.Checked = true;

                // 옵션 - 작업 후 완료된 항목 목록에서 자동 삭제
                GetPrivateProfileString("OPTION", "AutoDeleteDoneItem", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkAutoDeleteDoneItem.Checked = true;

                // 옵션 - RAW 파일을 하위 폴더에 저장
                GetPrivateProfileString("OPTION", "SeperateRawFile", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkSepertateRawFile.Checked = true;

                // 옵션 - 파일명을 날짜 정보로 변경
                GetPrivateProfileString("OPTION", "ChangeFileName", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkChangeFileName.Checked = true;

                // 옵션 - JPG 파일이 없으면 RAW 파일 삭제
                GetPrivateProfileString("OPTION", "DeleteRawIfJpgNotExist", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkDelRawIfJpgNotExist.Checked = true;
            }
        }

        private String GetExifDateFileName(string filePath)
        {
            String dateTime = GPUtil.GetExifDate(filePath);
            FileInfo fi = new FileInfo(filePath);

            if (true == String.IsNullOrEmpty(dateTime)) // Exif 정보가 없는 경우
            {
                // 마지막 수정일을 파일명으로 사용
                dateTime = fi.LastWriteTime.ToString("yyyy-mm-dd__HH-mm-ss");
            }
            else // Exif 정보 있음
            {
                dateTime = dateTime.Replace(" ", "__");
            }

            dateTime += fi.Extension;

            return dateTime.Trim();
        }

        private void SetProgramTitle()
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version; // 현재 실행되는 어셈블리..dll의 버전 가져오기
            this.Text = "Groupic v" + ver.ToString();
        }

        private void MakeDLLIfNotExist()
        {
            // 프로그램 실행시 DLL 파일을 생성한다. 
            String strFilePath = Application.ExecutablePath.Replace("/", "\\");
            strFilePath = strFilePath.Substring(0, strFilePath.LastIndexOf('\\'));
            strFilePath += "\\ExifLib.dll";

            FileInfo fileInfo = new FileInfo(strFilePath);
            if (fileInfo.Exists == false)
            {
                byte[] aryData = Resources.ExifLib;
                FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.CreateNew);
                fileStream.Write(aryData, 0, aryData.Length);
                fileStream.Close();
            }
        }

        private void SetToolStripProgressBar(int value)
        {
            toolStripProgressBar1.Value = value;
        }

        private void DragDropThreadFunc(DragEventArgs e)
        {
            counter.reset();

            String[] files;
            files = (String[])e.Data.GetData(DataFormats.FileDrop);
            counter.CalcurateTotalCount(files);
            workItems.IndexingFiles(files);

            if (false == workItems.IsSameList())
            {
                listViewItems = workItems.ToList();
            }

            SetListViewItems(listViewItems);
        }

        // 리스트뷰의 인덱스번호를 새로 보정한다.
        private void ReviseIndexNumber(ListView listView)
        {
            int cnt = 1;
            foreach (ListViewItem item in listView.Items)
            {
                item.SubItems[0].Text = cnt.ToString(); ;
                cnt++;
            }
        }

        private void SetPreviewFolderPath()
        {
            lbRawPreview.Visible = false;

            if (true == chkSepertateRawFile.Checked)
                lbRawPreview.Visible = true;

            String strPreview = "./";
            String strDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (true == chkCategoryByMonth.Checked) // 월별 분류
            {
                strDate = strDate.Substring(0, 7);
            }

            if (false == chkDateFormat.Checked) // 연도 2자리 사용
            {
                strDate = strDate.Substring(2);
            }

            if (false == chkUseDash.Checked)
            {
                strDate = strDate.Replace("-", "");
            }

            if (true == chkMakeRoot.Checked)
            {
                strPreview += "Category/";
            }

            strPreview += strDate;

            String strJpgPreview = "[  JPG  ]  " + strPreview;
            String strRawPreview = "[  RAW  ]  " + strPreview + "/RAW";

            if (true == chkChangeFileName.Checked)
            {
                String strFileFormat = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
                strJpgPreview += "/" + strFileFormat + ".jpg";
                strRawPreview += "/" + strFileFormat + ".raw";
            }

            lbJpegPreview.Text = strJpgPreview;
            lbRawPreview.Text = strRawPreview;
        }
        #endregion
    }
}
