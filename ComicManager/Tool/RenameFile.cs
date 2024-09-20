using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Collections;
using static ComicManager.MainViewModel;
using System.Windows;

namespace ComicManager.Tool
{
    public class RenameFile
    {
        public RenameFile() 
        {
            _vm = (App.Current as App).MainViewModel;
        }
        MainViewModel _vm;

        private string renameSuccess = string.Empty;
        private string renameError = string.Empty;

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

        public async Task LeftAddStr(IList FilesListView,string str)
        {
            if (FilesListView.Count > 0)
            {
                bool renameResult = false;
                string newerName = string.Empty;
                string filePath = string.Empty;
                RenameFile rename = new RenameFile();

                foreach (var item in FilesListView)
                {
                    filePath = (item as FilesListClass).filePath;
                    newerName = str + Path.GetFileName(filePath);

                    renameResult = await rename.RenameMethod(filePath, newerName);
                    if (renameResult)
                    {
                        renameSuccess += newerName + "\n";
                        //MessageBox.Show($"リネームに成功しました！\n{newerName}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        renameError += newerName + "\n";
                        //MessageBox.Show($"リネームに失敗しました\n{newerName}", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (renameSuccess != string.Empty)
                {
                    MessageBox.Show($"リネームに成功しました！\n{renameSuccess}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (renameError != string.Empty)
                {
                    MessageBox.Show($"リネームに失敗しました\n{renameError}", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public async Task DeleteStr(IList FilesListView, string str)
        {
            if (FilesListView.Count > 0)
            {
                bool renameResult = false;
                string newerName = string.Empty;
                string filePath = string.Empty;
                RenameFile rename = new RenameFile();

                foreach (var item in FilesListView)
                {
                    filePath = (item as FilesListClass).filePath;

                    // リプレースで対象の文字列を消す
                    newerName = Path.GetFileName(filePath).Replace(str, "");

                    renameResult = await rename.RenameMethod(filePath, newerName);
                    if (renameResult)
                    {
                        renameSuccess += newerName + "\n";
                    }
                    else
                    {
                        renameError += newerName + "\n";
                    }
                }
                if (renameSuccess != string.Empty)
                {
                    MessageBox.Show($"リネームに成功しました！\n{renameSuccess}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (renameError != string.Empty)
                {
                    MessageBox.Show($"リネームに失敗しました\n{renameError}", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
