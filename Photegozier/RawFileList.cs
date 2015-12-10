using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Groupic
{
    class RawFileList
    {
        private static List<String> _rawList = new List<String>();

        public RawFileList()
        {
            MakeRawList(_rawList);
        }
        
        public Boolean IsContain(String search)
        {
            if (null == _rawList)
                return false;

            return _rawList.Contains(search.ToLower());
        }

        private void MakeRawList(List<String> list)
        {
            list.Add(".raf");
            list.Add(".crw");
            list.Add(".cr2");
            list.Add(".cr3");
            list.Add(".srw");
            list.Add(".tif");
            list.Add(".k25");
            list.Add(".kdc");
            list.Add(".dcs");
            list.Add(".dcr");
            list.Add(".drf");
            list.Add(".mrw");
            list.Add(".nef");
            list.Add(".nrw");
            list.Add(".orf");
            list.Add(".dng");
            list.Add(".ptx");
            list.Add(".pef");
            list.Add(".arw");
            list.Add(".srf");
            list.Add(".sr2");
            list.Add(".x3f");
            list.Add(".erf");
            list.Add(".mef");
            list.Add(".mos");
            list.Add(".rw2");
            list.Add(".cap");
            list.Add(".tif");
            list.Add(".iiq");
            list.Add(".r3d");
            list.Add(".fff");
            list.Add(".pxn");
            list.Add(".bay");
            list.Add(".xmp");
        }
    }
}
