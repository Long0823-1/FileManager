using System.Configuration;
using System.Data;
using System.Windows;

namespace ComicManager
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
