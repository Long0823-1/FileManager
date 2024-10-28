using FileManager;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // これ大事
        public MainViewModel MainViewModel = new MainViewModel();
    }
}
