using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Navigation;
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
        }
        MainViewModel viewModel;
        ArchiveExtractor extractor;

        private void FileName_Click(object sender, RoutedEventArgs e)
        {

        }

        bool isOrderBy = true;
        private void CreateTime_Click(object sender, RoutedEventArgs e)
        {
            if(isOrderBy)
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderBy(x => x.dateTime));
                viewModel.FilesList = OrderBy;
                isOrderBy = false;
            }else
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

                if(dlg.ShowDialog() == CommonFileDialogResult.Ok)
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
                if((((sender as ListView).SelectedItem) as FilesListClass) != null)
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
                string fileNameOnly = strEntry.Text + Path.GetFileName(filePath);
                string Directory = Path.GetDirectoryName(filePath);
                string newerPath = Path.Combine(Directory, fileNameOnly);

                File.Move(filePath, newerPath);

                GetFilesList getFilesList = new GetFilesList();
                await getFilesList.FilesList(viewModel.Path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }
    }
}