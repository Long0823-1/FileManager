using FileManager;
using System.Runtime.InteropServices;
using System.Windows;
namespace FileManager.Tool
{
    /// <summary>
    /// DownloadNow.xaml の相互作用ロジック
    /// </summary>
    public partial class DownloadNow : Window
    {
        public DownloadNow()
        {
            InitializeComponent();
            _vm = (App.Current as App).MainViewModel;
            this.DataContext = _vm;

        }
        MainViewModel _vm;

        /// <summary>
        /// メニューのハンドル取得
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// メニュー項目の削除
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="uPosition"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        private const int SC_CLOSE = 0xf060;

        /// <summary>
        /// uPositionに設定するのは項目のID
        /// </summary>
        private const int MF_BYCOMMAND = 0x0000;
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper((Window)sender).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            RemoveMenu(hMenu, SC_CLOSE, MF_BYCOMMAND);
        }
        /// <summary>
        /// プログレスバー関連
        /// </summary>
        /// <returns></returns>
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
