using FileManager;
using System.Runtime.InteropServices;
using System.Windows;

namespace FileManager.Tool
{
    /// <summary>
    /// Loading.xaml の相互作用ロジック
    /// </summary>
    public partial class Loading : Window
    {
        
        public Loading()
        {
            InitializeComponent();
            vm = (App.Current as App).MainViewModel;
            this.DataContext = vm;

            this.ResizeMode = ResizeMode.NoResize;

            Bar.IsIndeterminate = true;
        }

        MainViewModel vm;

        private void Window_Closed(object sender, EventArgs e)
        {
        }
    }
}
