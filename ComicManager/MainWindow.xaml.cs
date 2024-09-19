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

        private void FileName_Click(object sender, RoutedEventArgs e)
        {

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
            catch (Exception)
            {

            }
            finally
            {
            }

        }

        private string renameSuccess = string.Empty;
        private string renameError = string.Empty;
        private async void LeftStr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LeftAddStr();

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

        private async Task LeftAddStr()
        {
            if (FilesListView.SelectedItems.Count > 0)
            {
                bool renameResult = false;
                string newerName = string.Empty;
                string filePath = string.Empty;
                RenameFile rename = new RenameFile();

                foreach (var item in FilesListView.SelectedItems)
                {
                    filePath = (item as FilesListClass).filePath;
                    newerName = strEntry.Text + Path.GetFileName(filePath);

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
        private async Task DeleteStr()
        {
            if (FilesListView.SelectedItems.Count > 0)
            {
                bool renameResult = false;
                string newerName = string.Empty;
                string filePath = string.Empty;
                RenameFile rename = new RenameFile();

                foreach (var item in FilesListView.SelectedItems)
                {
                    filePath = (item as FilesListClass).filePath;

                    // リプレースで対象の文字列を消す
                    newerName = Path.GetFileName(filePath).Replace(strEntry.Text, "");

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
        private async void DeleteStr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               await DeleteStr();
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
    }
}