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

        public bool RenameMethod(string filePath,string newerName)
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

        public void LeftAddStr(IList FilesListView,string str)
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

                    renameResult = rename.RenameMethod(filePath, newerName);
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
                ResultMessage(); // 結果を表示
            }
        }
        public void DeleteStr(IList FilesListView, string str)
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

                    renameResult = rename.RenameMethod(filePath, newerName);
                    if (renameResult)
                    {
                        renameSuccess += newerName + "\n";
                    }
                    else
                    {
                        renameError += newerName + "\n";
                    }
                }
                ResultMessage(); // 結果を表示
            }
        }

        private void ResultMessage()
        {
            if (renameSuccess != string.Empty)
            {
                MessageBox.Show($"リネームに成功しました！\n{renameSuccess}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (renameError != string.Empty)
            {
                MessageBox.Show($"リネームに失敗しました\n{renameError}", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExchangeStr(IList FilesListView, string str,string replaceStr)
        {
            if (FilesListView.Count > 0)
            {
                // 初期化
                bool renameResult = false;
                string newerName = string.Empty;
                string filePath = string.Empty;
                RenameFile rename = new RenameFile();


                // ListViewで選択されたItemを一つずつ処理

                foreach (var item in FilesListView)
                {
                    filePath = (item as FilesListClass).filePath; // 生のファイルパス

                    // リプレースで対象の文字列を消す
                    newerName = Path.GetFileName(filePath).Replace(str, replaceStr);

                    renameResult = rename.RenameMethod(filePath, newerName); // 処理の結果を格納

                    if (renameResult)
                    {
                        renameSuccess += newerName + "\n";
                    }
                    else
                    {
                        renameError += newerName + "\n";
                    }
                }
                

                ResultMessage(); // 結果を表示
            }
        }

    }
}
