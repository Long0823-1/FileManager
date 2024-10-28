using FileManager;
using FileManager.Tool;
using Microsoft.VisualBasic.FileIO;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows;
using static FileManager.MainViewModel;

namespace FileManager.Tool
{
    public class RenameFile
    {
        public RenameFile()
        {
            // ここでApp.xaml.csで初期化しているMainViewModelを代入。
            _vm = (Application.Current as App).MainViewModel;
        }

        // 初期化していないMainViewModel
        MainViewModel _vm;

        // 成功、失敗したファイル名を格納
        string renameSuccess = string.Empty;
        string renameError = string.Empty;

        public bool RenameMethod(string filePath, string newerName)
        {
            try
            {
                _vm.loadingFileName = filePath;

                // VB.NETのクラスを参照
                FileSystem.RenameFile(filePath, newerName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;

        }
        public async void LeftAddStr(IList FilesListView, string str)
        {
            Loading loading = new Loading();
            loading.Show();

            var result = await Task.Run(() =>
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
                        _vm.loadingFileName = "変換中：" + newerName;

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
                }
                return true;
            });

            if (result)
            {
                loading.Close();
                ResultMessage(); // 結果を表示
            }
        }

        public async void RightAddStr(IList FilesListView, string str)
        {
            Loading loading = new Loading();
            loading.Show();

            var result = await Task.Run(() =>
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
                        newerName = Path.GetFileNameWithoutExtension(filePath) + str + Path.GetExtension(filePath);
                        _vm.loadingFileName = "変換中：" + newerName;

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
                }
                return true;
            });

            if (result)
            {
                loading.Close();
                ResultMessage(); // 結果を表示
            }
        }

        public async void DeleteStr(IList FilesListView, string str)
        {
            Loading loading = new Loading();
            loading.Show();

            var result = await Task.Run(() =>
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
                        _vm.loadingFileName = "変換中：" + newerName;

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
                }
                return true;
            });
            if (result)
            {
                loading.Close();
                ResultMessage(); // 結果を表示

            }

        }
        private async void GetFilesList(bool force = false)
        {
            GetFilesList getFiles = new GetFilesList();
            await getFiles.FilesList(_vm.Path, force);
        }

        private void ResultMessage()
        {
            if (renameSuccess != string.Empty)
            {
                MessageBox.Show($"リネームに成功しました！\n", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (renameError != string.Empty)
            {
                MessageBox.Show($"リネームに失敗しました\n{renameError}", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            GetFilesList(true);
        }

        public async void ExchangeStr(IList FilesListView, string str, string replaceStr)
        {
            Loading loading = new Loading();
            loading.Show();

            if (FilesListView.Count > 0)
            {
                var result = await Task.Run(() =>
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
                        _vm.loadingFileName = "変換中：" + newerName;

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


                    return true;
                });
                if (result)
                {
                    loading.Close();
                    ResultMessage(); // 結果を表示
                }
            }
        }

    }
}
