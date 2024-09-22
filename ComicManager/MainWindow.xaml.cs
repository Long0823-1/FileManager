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

        private async void GetFilesList(string _Path,bool force = false)
        {
            GetFilesList getFiles = new GetFilesList();
            await getFiles.FilesList(_Path,force);
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

        private void FileOpen()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = filePath,
                UseShellExecute = true,
                CreateNoWindow = true,
            };
            System.Diagnostics.Process.Start(startInfo);
        }

        private void FilesListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Debug.WriteLine("double");
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
            else if(DeleteStr.IsChecked == true)
            {
                DeleteStr_Click();
            }
            else if(ExchangeStr.IsChecked == true)
            {
                ExchangeStr_Click();
            }
        }
    }
}