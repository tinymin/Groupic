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
using System.Windows.Forms;

namespace Groupic
{
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
        private List<String> exifExtentionList = new List<String>();
        #endregion


        #region Constructor
        public Groupic()
        {
            InitializeComponent();
            SetExifExtention(exifExtentionList);
        }
        #endregion


        #region EventHandler
        private void Form1_Load(object sender, EventArgs e)
        {
            MakeDLLIfNotExist();
            SetProgramTitle();

            fileListView.GridLines = true;
            fileListView.AllowDrop = true;

            StringBuilder optionVal = new StringBuilder();

            if (true == new FileInfo(iniPath).Exists)
            {
                GetPrivateProfileString("OPTION", "DateFormat", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkDateFormat.Checked = true;

                GetPrivateProfileString("OPTION", "MakeRoot", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkMakeRoot.Checked = true;

                GetPrivateProfileString("OPTION", "UseDash", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkUseDash.Checked = true;

                GetPrivateProfileString("OPTION", "CategoryByMonth", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkCategoryByMonth.Checked = true;

                GetPrivateProfileString("OPTION", "AutoDeleteDoneItem", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkAutoDeleteDoneItem.Checked = true;

                GetPrivateProfileString("OPTION", "SeperateRawFile", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkSepertateRawFile.Checked = true;

                GetPrivateProfileString("OPTION", "ChangeFileName", "0", optionVal, 16, iniPath);
                if ("1" == optionVal.ToString())
                    chkChangeFileName.Checked = true;
            }
            
            SetPreviewFolderPath();
        }

        private void fileListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void fileListView_DragDrop(object sender, DragEventArgs e)
        {
            DragDropThreadFunc(e);
        }

        private void btnCategorizing_Click(object sender, EventArgs e)
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

            foreach(ListViewItem item in fileListView.Items)
            {
                // 이미 작업한 항목은 Skip 한다.
                if (true == item.SubItems[1].Text.Equals("O"))
                    continue;

                sourceFilePath = item.SubItems[2].Text;
                fileInfo = new FileInfo(sourceFilePath);

                if (false == fileInfo.Exists)
                {
                    continue;
                }


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
                if (true == chkChangeFileName.Checked)
                    destFilePath = destDirectory.ToString() + "\\" + GetExifDateFileName(sourceFilePath);
                else
                    destFilePath = destDirectory.ToString() + "\\" + fileInfo.Name;

                destFilePath = destFilePath.Replace("\\\\", "\\");

                // 대상 경로에 동일한 파일이 존재하면 덮어쓰기 다이얼로그를 띄운다.
                if (true == File.Exists(destFilePath))
                {
                    OverwriteResult overWriteResult = OverwriteResult.Cancel;
                    overWriteResult = overwriteDlg.ShowOverwriteDialog(sourceFilePath, destFilePath);

                    switch(overWriteResult)
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
                item.ForeColor = Color.LightGray;
                item.SubItems[1].Text = "O"; // 영문자 O 이다. 숫자 0 아님

                if (true == chkAutoDeleteDoneItem.Checked)
                    item.Remove();
            } // end of foreach

            if (true == isDoneWork)
                MessageBox.Show("작업 완료");
            else if (false == isDoneWork && (skipCnt != fileListView.Items.Count) )
                MessageBox.Show("이미 작업이 완료된 파일들 입니다.");

            isDoneWork = false;
        }

        private void btnDeleteAllFileList_Click(object sender, EventArgs e)
        {
            fileListView.Items.Clear();
        }

        private void btnDeleteSelectedFileList_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in fileListView.SelectedItems)
            {
                item.Remove();
            }

            foreach (ListViewItem item in fileListView.CheckedItems)
            {
                item.Remove();
            }

            ReviseIndexNumber(fileListView);
        }

        private void btnDeleteDoneItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fileListView.Items)
            {
                if (true == item.SubItems[1].Text.Equals("O"))  // 영문자 O 이다. 숫자 0 아님
                    item.Remove();
            }

            ReviseIndexNumber(fileListView);
        }

        private void chkDateFormat_CheckedChanged(object sender, EventArgs e)
        {
            WritePrivateProfileString("OPTION", "DateFormat", (chkDateFormat.Checked==true?1:0).ToString(), iniPath);
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


        #region Methods
        private String GetExifDateFileName(string filePath)
        {
            String dateTime = GetExifDate(filePath);
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

        private void SetExifExtention(List<string> extList)
        {
            extList.Add(".jpg");
            extList.Add(".jpeg");
            extList.Add(".tiff");
            extList.Add(".riff");
            extList.Add(".wav");
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

        private void DragDropThreadFunc(DragEventArgs e)
        {
            String[] files;
            files = (String[])e.Data.GetData(DataFormats.FileDrop);

            foreach (String file in files)
            {
                DirSearch(file);
            }
        }

        void DirSearch(string sDir)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(sDir);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    // 디렉토리 탐색
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        foreach (string f in Directory.GetFiles(d))
                        {
                            addItemToListView(fileListView, f);
                        }
                        DirSearch(d);
                    }

                    // 루트 디렉토리의 파일 처리
                    foreach (string f in Directory.GetFiles(sDir))
                    {
                        addItemToListView(fileListView, f);
                    }
                }
                else
                {
                    // 파일 리스트에 추가
                    addItemToListView(fileListView, sDir);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private Boolean addItemToListView(ListView listView, String path)
        {
            if (null != listView.FindItemWithText(path))
            {
                return false;
            }

            ListViewItem lvi = new ListViewItem((listView.Items.Count + 1).ToString());

            lvi.SubItems.Add(""); // 완료 여부 컬럼 - 처음에는 빈칸을 넣는다.
            lvi.SubItems.Add(path); //파일 경로

            // 기본적으로 파일 생성날짜를 읽어온다.
            // Exif가 존재한다면 아래 로직에서 Exif에서 생성된 날짜를 읽어온다.
            FileInfo fileInfo = new FileInfo(path);
            String fileCreationTime = fileInfo.LastWriteTime.Date.ToString("yyyy-MM-dd").Substring(0, 10);

            // Exif 지원 확장자인 경우 Exif에서 생성된 날짜를 읽어온다.
            if (true == IsExifSupportExtension(path))
            {
                 String exifDate = GetExifDate(path);

                if (false == String.IsNullOrEmpty(exifDate))
                {
                    exifDate = exifDate.Substring(0, 10);
                    fileCreationTime = exifDate;
                }
            }

            lvi.SubItems.Add(fileCreationTime); // 생성 날짜

            long fileSize = fileInfo.Length;
            // 파일 크기
            if (0 == fileSize)
                lvi.SubItems.Add("0 KB");
            else if (1024 > fileSize)
                lvi.SubItems.Add("1 KB");
            else if (1048576 > fileSize)
                lvi.SubItems.Add(String.Format("{0:N}", (double)(fileSize / 1024)) + " KB");
            else
                lvi.SubItems.Add(String.Format("{0:N}", (double)(fileSize / 1024 / 1024)) + " MB");

            listView.Items.Add(lvi);
            return true;
        }

        private String GetExifDate(string path)
        {
            String strDate;

            try
            {
                ExifReader exifData = new ExifReader(path);
                exifData.GetTagValue(ExifTags.DateTimeOriginal, out strDate);
                exifData.Dispose(); // Close file
            }
            catch (ExifLibException ex)
            {
                Console.WriteLine(ex.ToString());
                return String.Empty;
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine(ex.ToString());
                return String.Empty;
            }

            if (null == strDate || String.Empty == strDate)
                return String.Empty;

            strDate = strDate.Replace(":", "-");
            return strDate;
        }

        private bool IsExifSupportExtension(string path)
        {
            String fileExt = Path.GetExtension(path);

            if (true == String.IsNullOrEmpty(fileExt))
                return false;

            return exifExtentionList.Contains(fileExt.ToLower());
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
                String strFileFormat = DateTime.Now.ToString("yyyy-MM-dd__hh-mm-ss");
                strJpgPreview += "/" + strFileFormat + ".jpg";
                strRawPreview += "/" + strFileFormat + ".raw";
            }

            lbJpegPreview.Text = strJpgPreview;
            lbRawPreview.Text = strRawPreview;
        }
        #endregion
    }
}
