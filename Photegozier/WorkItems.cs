using ExifLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Groupic
{
    internal class WorkItems
    {
        public event SetToolStripProgressBarValueCallback ProgressBar;

        private List<String> exifExtentionList;
        private HashSet<string> setListViewItems;
        private int prevCount;
        private int currentCount;

        public WorkItems()
        {
            exifExtentionList = new List<String>();
            setListViewItems = new HashSet<string>();
            GenerateExifExtention(exifExtentionList);
        }

        public HashSet<string> ListViewItems { get => setListViewItems; set => setListViewItems = value; }

        internal Boolean IsSameList()
        {
            return currentCount == prevCount;
        }

        internal void IndexingFiles(String[] files)
        {
            prevCount = setListViewItems.Count;

            foreach (String file in files)
            {
                DirSearch(file);
            }

            currentCount = setListViewItems.Count;
        }

        private void DirSearch(string directoryOrFile)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(directoryOrFile);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory) // 디렉토리인 경우
                {
                    // 하위 디렉토리 탐색
                    foreach (string d in Directory.GetDirectories(directoryOrFile))
                    {
                        DirSearch(d);
                    }

                    // 현재 디렉토리의 파일 처리
                    foreach (string file in Directory.GetFiles(directoryOrFile))
                    {
                        setListViewItems.Add(file);
                        //ListViewItem lvi = GenerateListViewItem(listViewItems.Count, file);
                        //Console.WriteLine("루트 디렉토리 : " + file);
                        //if (null != lvi)
                        //{
                        //    listViewItems.Add(lvi);
                        //    counter.DoneCount += 1; // 처리된 갯수 증가
                        //    SetToolStripProgressBarValue(counter.percentage());
                        //}
                    }
                }
                else // 파일인 경우
                {
                    // 파일을 집합(HashSet)에 추가
                    setListViewItems.Add(directoryOrFile);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal List<ListViewItem> ToList()
        {
            List<ListViewItem> items = new List<ListViewItem>();
            Counter counter = new Counter(setListViewItems.Count);
            int idx = 1;
            foreach(string file in setListViewItems)
            {
                items.Add(GenerateListViewItem(idx, file));
                idx++;
                counter.DoneCount += 1; // 처리된 갯수 증가
                ProgressBar(counter.Percentage());
                //SetToolStripProgressBarValue(counter.percentage());
            }

            return items;
        }

        private ListViewItem GenerateListViewItem(int idx, String path)
        {
            ListViewItem lvi = new ListViewItem(idx.ToString());

            lvi.SubItems.Add(""); // 완료 여부 컬럼 - 처음에는 빈칸을 넣는다.
            lvi.SubItems.Add(path); //파일 경로

            // 기본적으로 파일 생성날짜를 읽어온다.
            // Exif가 존재한다면 아래 로직에서 Exif에서 생성된 날짜를 읽어온다.
            FileInfo fileInfo = new FileInfo(path);
            String fileCreationTime = fileInfo.LastWriteTime.Date.ToString("yyyy-MM-dd").Substring(0, 10);

            // Exif 지원 확장자인 경우 Exif에서 생성된 날짜를 읽어온다.
            if (true == IsExifSupportExtension(path))
            {
                String exifDate = GPUtil.GetExifDate(path);

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
            else if (1073741824 > fileSize)
                lvi.SubItems.Add(String.Format("{0:N}", (double)(fileSize / 1048576)) + " MB");
            else
                lvi.SubItems.Add(String.Format("{0:N}", (double)(fileSize / 1073741824)) + " GB");

            return lvi;
        }

        private bool IsExifSupportExtension(string path)
        {
            String fileExt = Path.GetExtension(path);

            if (true == String.IsNullOrEmpty(fileExt))
                return false;

            return exifExtentionList.Contains(fileExt.ToLower());
        }

        private void GenerateExifExtention(List<string> extList)
        {
            extList.Add(".jpg");
            extList.Add(".jpeg");
            extList.Add(".tiff");
            extList.Add(".riff");
            extList.Add(".wav");
        }
    }
}