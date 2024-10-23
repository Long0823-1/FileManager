using ComicManager.Tool;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static ComicManager.MainViewModel;

namespace ComicManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            viewModel = (App.Current as App).MainViewModel;
            this.DataContext = viewModel;
            extractor = new ArchiveExtractor();
            viewModel.CoverImage = LoadImage(@"images\none.png");
            GetFilesList(@"C:\");
        }

        MainViewModel viewModel;
        ArchiveExtractor extractor;

        bool isOrderByName = true;
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

        private void FilesListView_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Path.HasExtension(filePath))
            {
                FileOpen();
            }
            else
            {
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

        private void FileOpen(bool isExplorer = false)
        {
            string path = string.Empty;

            if (isExplorer)
            {
                path = @"C:\Windows\explorer.exe";
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            FileOpen();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", $"エラーが発生しました\n{ex.Message}");
            }
            finally
            {
                viewModel.Path = viewModel.Path;
                GetFilesList(Path.GetFullPath(filePath));

            }
        }

        private void FileNameCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Path.GetFileNameWithoutExtension(filePath));
        }
        private void FileDirectoryOpen_Click(object sender, RoutedEventArgs e)
        {
            FileOpen(true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Conv_Click(object sender, RoutedEventArgs e)
        {
            if (LeftStr.IsChecked == true)
            {
                LeftStr_Click();
            }
            else if (RightStr.IsChecked == true)
            {
                //loading.Show();
                //RightStr_Click();
            }
            else if (DeleteStr.IsChecked == true)
            {
                DeleteStr_Click();
            }
            else if (ExchangeStr.IsChecked == true)
            {
                ExchangeStr_Click();
            }

            searchBox.Text = string.Empty;
        }
        private ObservableCollection<FilesListClass> _FilesList = new ObservableCollection<FilesListClass>();
        private bool onetime = true;
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
                var filteredFiles = new ObservableCollection<FilesListClass>(_FilesList.Where(fileName => fileName.fileName.Contains(searchText)));
                viewModel.FilesList = filteredFiles;
            }

        }

        private async void FFmpeg_Download_Click(object sender, RoutedEventArgs e)
        {
            await GetContents("https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip", "FFMPEG");

        }

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
                MessageBox.Show("しっぱい");
            }
            this.IsEnabled = true;
        }

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
    }
}