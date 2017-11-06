using ExifLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Groupic
{
    class GPUtil
    {
        public static String GetExifDate(string path)
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
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.ToString());
                return String.Empty;
            }

            if (null == strDate || String.Empty == strDate)
                return String.Empty;

            strDate = strDate.Replace(":", "-");
            return strDate;
        }

        public static String GetNewTargetName(String targetFile)
        {
            String path = Path.GetDirectoryName(targetFile);
            String fileName = Path.GetFileNameWithoutExtension(targetFile);
            String ext = Path.GetExtension(targetFile);

            while (true)
            {
                fileName += " copy";
                String newFilePath = path + "\\" + fileName + ext;

                if (false == File.Exists(newFilePath))
                    return newFilePath.Trim();
            }
        }

        public static Bitmap GetResizeImage(String filePath, int thumbWidth, int thumbHeight)
        {
            List<String> supportExtensions = new List<String>(new[] {"BMP", "GIF", "EXIF", "JPG", "PNG", "TIFF" });
            
            if ( false == supportExtensions.Contains(new FileInfo(filePath).Extension.ToUpper().Trim().Replace(".", "")) )
                return null;

            Bitmap image = new Bitmap(filePath);

            var newImage = new Bitmap(thumbWidth, thumbHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, thumbWidth, thumbHeight);
            Bitmap bmp = new Bitmap(newImage);

            image.Dispose();
            newImage.Dispose();

            return bmp;
        }

        public static Bitmap GetThumbNail(String filePath, int thumbWidth, int thumbHeight)
        {
            Bitmap thumbNail = GPUtil.GetResizeImage(filePath, 150, 100);

            // Return default image
            if (null == thumbNail)
                return Properties.Resources.noPreview;

            return thumbNail;
        }
    }
}
