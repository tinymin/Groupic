using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Groupic
{
    class Counter
    {
        private int totalCount;
        private int doneCount;

        public Counter() { }

        public Counter(int count)
        {
            this.totalCount = count;
        }

        public int TotalCount { get => totalCount; set => totalCount = value; }
        public int DoneCount { get => doneCount; set => doneCount = value; }

        public int Percentage()
        {
            Console.WriteLine(doneCount + ", " + totalCount + " = " + (int)(doneCount / (double)totalCount * 100));
            return (int)(doneCount / (double)totalCount * 100);
        }

        internal void reset()
        {
            totalCount = 0;
            doneCount = 0;
        }

        internal void CalcurateTotalCount(string[] files)
        {
            HashSet<string> set = new HashSet<string>();
            foreach(string file in files)
            {
                FileAttributes attr = File.GetAttributes(file);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory) //디렉토리이면
                {
                    // 디렉토리 내의 모든 파일 갯수를 구한다.
                    int cnt = Directory.GetFiles(file, "*.*", SearchOption.AllDirectories).Length;
                    totalCount += cnt;
                }
                else // 파일이면
                {
                    totalCount++;
                }
            }
        }
    }
}
