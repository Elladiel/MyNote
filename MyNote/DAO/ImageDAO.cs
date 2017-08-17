using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.DAO
{
    class ImageManager
    {

        public static String GetImage()
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                return myDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        public static void SaveImage()
        {

        }
    }
}
