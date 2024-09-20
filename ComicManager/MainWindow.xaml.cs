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


        private async void LeftStr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RenameFile renameFile = new RenameFile();
                
                await renameFile.LeftAddStr(FilesListView.SelectedItems,strEntry.Text);

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
                await renameFile.DeleteStr(FilesListView.SelectedItems, strEntry.Text);
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