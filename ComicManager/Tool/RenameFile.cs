using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace ComicManager.Tool
{
    public class RenameFile
    {
        public RenameFile() 
        {
            _vm = (App.Current as App).MainViewModel;
        }
        MainViewModel _vm;

        public async Task<bool> RenameMethod(string filePath,string newerName)
        {
            try
            {
                FileSystem.RenameFile(filePath, newerName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;

        }
       
    }
}
