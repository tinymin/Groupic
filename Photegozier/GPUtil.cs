using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Groupic
{
    class GPUtil
    {
        public static String GetNewTargetName(String targetFile)
        {
            String path = Path.GetDirectoryName(targetFile);
            String fileName = Path.GetFileNameWithoutExtension(targetFile);
            String ext = Path.GetExtension(targetFile);
            String newFilePath;

            while (true)
            {
                fileName += " copy";
                newFilePath = path + "\\" + fileName + ext;

                if (false == File.Exists(newFilePath))
                    return newFilePath.Trim();
            }
        }
    }
}
