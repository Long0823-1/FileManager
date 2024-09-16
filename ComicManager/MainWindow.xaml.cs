using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            GetFilesList getFiles = new GetFilesList();
            getFiles.FilesList();
        }
        MainViewModel viewModel;

        private void FileName_Click(object sender, RoutedEventArgs e)
        {

        }
        bool isOrderBy = true;
        private void CreateTime_Click(object sender, RoutedEventArgs e)
        {
            if(isOrderBy)
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderBy(x => x.dateTime).OrderBy(x => x.fileName));
                viewModel.FilesList = OrderBy;
                isOrderBy = false;
            }else
            {
                ObservableCollection<FilesListClass> OrderBy = new ObservableCollection<FilesListClass>(viewModel.FilesList.OrderByDescending(x => x.dateTime).OrderByDescending(x => x.fileName));
                viewModel.FilesList = OrderBy;
                isOrderBy = true;
            }
        }
    }
}