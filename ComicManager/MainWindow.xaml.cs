using FileManager;
using FileManager.Tool;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static FileManager.MainViewModel;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // AppクラスのMainViewModelを使用
            viewModel = (App.Current as App).MainViewModel;

            this.DataContext = viewModel;
            extractor = new ArchiveExtractor();
            viewModel.CoverImage = ArchiveExtractor.LoadImage(@"images\none.png");
            
            GetFilesList(@"C:\");
        }

        // ViewModel
        MainViewModel viewModel;

        // 解凍専門クラス
        ArchiveExtractor extractor;

        bool isOrderByName = true;

        /// <summary>
        /// ヘッダーをクリックされた時に並び替えする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileName_Click(object sender, RoutedEventArgs e)
        {
            if (isOrderByName)
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderBy(x => x.filePath));
                viewModel.FilesList = OrderBy;
                isOrderByName = false;
            }
            else
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderByDescending(x => x.filePath));
                viewModel.FilesList = OrderBy;
                isOrderByName = true;
            }
        }

        bool isOrderBy = true;

        /// <summary>
        /// ヘッダーをクリックされた時に並び替えする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateTime_Click(object sender, RoutedEventArgs e)
        {
            if (isOrderBy)
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderBy(x => x.dateTime));
                viewModel.FilesList = OrderBy;
                isOrderBy = false;
            }
            else
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderByDescending(x => x.dateTime));
                viewModel.FilesList = OrderBy;
                isOrderBy = true;
            }
        }

        /// <summary>
        /// フォルダパスをユーザー側で指定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderOpen_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog())
            {
                dlg.IsFolderPicker = true;
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string selectedPath = dlg.FileName;
                    GetFilesList(Path.GetFullPath(selectedPath));

                }
            }
        }

        private string filePath;

        /// <summary>
        /// 選択しているファイルが変わるたびにサムネを表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((((sender as ListView).SelectedItem) as FilesListClass) != null)
                {
                    filePath = (((sender as ListView).SelectedItem) as FilesListClass).filePath;
                    extractor.GetThumb(filePath);

                    Debug.WriteLine(filePath);
                }
            }
            catch (Exception) { }
            finally
            {

            }

        }


        private void LeftStr_Click()
        {
            try
            {
                RenameFile renameFile = new RenameFile();

                renameFile.LeftAddStr(FilesListView.SelectedItems, strEntry.Text);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

        private void RightStr_Click()
        {
            try
            {
                RenameFile renameFile = new RenameFile();

                renameFile.RightAddStr(FilesListView.SelectedItems, strEntry.Text);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }
        private void ExchangeStr_Click()
        {
            try
            {
                RenameFile renameFile = new RenameFile();

                renameFile.ExchangeStr(FilesListView.SelectedItems, strEntry.Text, ExchangeStrEntry.Text);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }


        private void DeleteStr_Click()
        {
            try
            {
                RenameFile renameFile = new RenameFile();
                renameFile.DeleteStr(FilesListView.SelectedItems, strEntry.Text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// ファイルリストを取得
        /// </summary>
        /// <param name="_Path">ファイルパス</param>
        /// <param name="force">強制的に更新するかを指定</param>
        private async void GetFilesList(string _Path, bool force = false)
        {
            if(File.Exists(@".\.ThumbSaveOn"))
            {
                saveItemIsChecked = true;
                ThumbSave.IsChecked = true;
            }
            
            GetFilesList getFiles = new GetFilesList();
            await getFiles.FilesList(_Path, force);

        }

        /// <summary>
        /// ファイルをダブルクリックしたとき、どのような処理をするかを決めるメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesListView_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Path.HasExtension(filePath))
            {
                FileOpen();
            }
            else
            {
                // 一つ階層を下がる際は「\\..」を指定
                // （PathクラスのGetFullPath関数を使えば、整ったパス名に変換してくれる）
                if (filePath == "\\..")
                {
                    string fullPath = viewModel.Path + filePath;
                    GetFilesList(Path.GetFullPath(fullPath));
                }
                else
                {
                    GetFilesList(Path.GetFullPath(filePath));
                }
            }
        }

        /// <summary>
        /// 実際にファイルを開く際のメソッド
        /// </summary>
        /// <param name="isExplorer">ファイルがあるディレクトリを開くかどうかを指定</param>
        private void FileOpen(bool isExplorer = false)
        {
            string path = string.Empty;

            if (isExplorer)
            {
                path = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), @"Windows\explorer.exe");
            }
            else
            {
                path = filePath;
            }

            var startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = path,
                Arguments = $" /select,\"{filePath}\"",
                UseShellExecute = true,
                CreateNoWindow = true,
            };
            System.Diagnostics.Process.Start(startInfo);
        }

        /// <summary>
        /// 右クリックした際のファイルを開くボタン用メソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            FileOpen();
        }

        /// <summary>
        /// 右クリックした際のファイル削除用のボタンのメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました\n{ex.Message}");
            }
            finally
            {
                viewModel.Path = viewModel.Path;
                GetFilesList(Path.GetFullPath(filePath));

            }
        }

        /// <summary>
        /// ファイル名をクリップボードにコピー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNameCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Path.GetFileNameWithoutExtension(filePath));
        }

        /// <summary>
        /// 右クリックした際のファイルのあるディレクトリを開くボタン用メソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDirectoryOpen_Click(object sender, RoutedEventArgs e)
        {
            FileOpen(true);
        }

        /// <summary>
        /// Windowのクローズ時に実行するメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ファイル名を一括処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conv_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)LeftStr.IsChecked)
            {
                LeftStr_Click();
            }
            else if ((bool)RightStr.IsChecked)
            {
                RightStr_Click();
            }
            else if ((bool)DeleteStr.IsChecked)
            {
                DeleteStr_Click();
            }
            else if ((bool)ExchangeStr.IsChecked)
            {
                ExchangeStr_Click();
            }

            searchBox.Text = string.Empty;
        }
        private ObservableCollection<FilesListClass> _FilesList = new ObservableCollection<FilesListClass>();
        private bool onetime = true;
        
        /// <summary>
        /// 検索窓内の文字列が変わるたびに検索結果を反映
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text;

            if (onetime)
            {
                _FilesList = viewModel.FilesList;
                onetime = false;
            }

            if (searchText == "")
            {
                viewModel.FilesList = _FilesList;
                onetime = true;
            }
            else
            {
                ObservableCollection<FilesListClass> filteredFiles = new ObservableCollection<FilesListClass>(_FilesList.Where(fileName => fileName.fileName.Contains(searchText)));
                viewModel.FilesList = filteredFiles;
            }

        }

        private async void FFmpeg_Download_Click(object sender, RoutedEventArgs e)
        {
            // ここはurlが変わることがないため、直接指定
            await GetContents("https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip", "FFMPEG");
        }


        /// <summary>
        /// ファイルダウンローダー
        /// </summary>
        /// <param name="url">URLを格納</param>
        /// <param name="filename">ロード中の際に表示するファイル名</param>
        /// <returns>なし</returns>
        private async Task GetContents(string url, string filename)
        {
            FileDownloader dl = new FileDownloader();
            this.IsEnabled = false;

            if (await dl.GetContent(url))
            {
                MessageBox.Show($"{filename}のダウンロードに成功しました");
            }
            else
            {
                MessageBox.Show("失敗");
            }
            this.IsEnabled = true;
        }

        /// <summary>
        /// 7-Zipのダウンロード先サイトを開くメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SevenZip_Download_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("自動でダウンロードが始まります。\n完了後、手動でインストールしてください。", "インフォメーション", MessageBoxButton.OK, MessageBoxImage.Information);

            var startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "https://www.7-zip.org/a/7z2408-x64.exe",
                UseShellExecute = true,
                CreateNoWindow = true,
            };
            System.Diagnostics.Process.Start(startInfo);
        }

        private bool saveItemIsChecked = false;

        private void Thumb_Save_Click(object sender, RoutedEventArgs e)
        {
            var thumb_Save_MenuItem = (sender as MenuItem);

            if (!saveItemIsChecked)
            {
                saveItemIsChecked = true;
                File.Create(@".\.ThumbSaveOn");
            }
            else
            {
                saveItemIsChecked = false;
                File.Delete(@".\.ThumbSaveOn");
            }


        }

        /// <summary>
        /// 文字入れ替えの文字列を相互に交換するボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Str_Change_Button(object sender, RoutedEventArgs e)
        {
            var temp = strEntry.Text;
            strEntry.Text = ExchangeStrEntry.Text;
            ExchangeStrEntry.Text = temp;   
        }
    }
}