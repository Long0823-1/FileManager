using ComicManager.Tool;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
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
                    viewModel.Path = dlg.FileName;
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


        private async void LeftStr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RenameFile renameFile = new RenameFile();
                
                renameFile.LeftAddStr(FilesListView.SelectedItems,strEntry.Text);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                GetFilesList getFilesList = new GetFilesList();
                await getFilesList.FilesList(viewModel.Path);
            }

        }
        private async void ExchangeStr_Click(object sender, RoutedEventArgs e)
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
            finally
            {
                GetFilesList getFilesList = new GetFilesList();
                await getFilesList.FilesList(viewModel.Path);
            }

        }


        private async void DeleteStr_Click(object sender, RoutedEventArgs e)
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
            finally
            {
                GetFilesList getFilesList = new GetFilesList();
                await getFilesList.FilesList(viewModel.Path);
            }
        }

        private void FilesListView_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.WriteLine("Preview");
            if(Path.HasExtension(filePath))
            {
                FileOpen();
            }
            else
            {
                if(filePath == "\\..")
                {
                    viewModel.Path = viewModel.Path + filePath;
                }else
                {
                    viewModel.Path = filePath;
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
            }
        }
    }
}