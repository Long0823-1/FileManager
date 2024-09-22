using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ComicManager
{
    public class MainViewModel : Prism.Mvvm.BindableBase
    {
        public MainViewModel()
        {

        }

        /// <summary>
        /// ファイルの詳細情報
        /// </summary>
        public class FilesListClass()
        {
            public string filePath { get; set; }
            public string fileName { get; set; }
            public string maxBytes { get; set; }
            public string dateTime { get; set; }
        }
        public static BitmapImage LoadImage(string imagePath)
        {
            var bitmap = new BitmapImage();
            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // メモリにロード
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            return bitmap;
        }

        private BitmapImage _CoverImage = new BitmapImage();
        public BitmapImage CoverImage
        {
            get
            {
                return _CoverImage;
            }
            set => SetProperty(ref _CoverImage, value, nameof(CoverImage));
        }
        private string _Path = @"C:\";
        private string[] _PathSplit;

        /// <summary>
        /// 現在のパスを格納
        /// </summary>
        public string Path
        {
            get
            {
                _Path = System.IO.Path.GetFullPath(_Path);
                return _Path;
            }
            set => SetProperty(ref _Path, value, nameof(Path));
        }

        /// <summary>
        /// 最初に格納する
        /// </summary>

        private ObservableCollection<FilesListClass> _FilesList = new ObservableCollection<FilesListClass>();
        public ObservableCollection<FilesListClass> FilesList
        {
            get => _FilesList;
            set => SetProperty(ref _FilesList, value, nameof(FilesList));
        }

        /// <summary>
        /// 前の配列を格納
        /// </summary>
        private ObservableCollection<FilesListClass> _PreviousFilesList = new ObservableCollection<FilesListClass>();
        public ObservableCollection<FilesListClass> PreviousFilesList
        {
            get => _PreviousFilesList;
            set => SetProperty(ref _PreviousFilesList, value, nameof(PreviousFilesList));
        }

        private string _PreviousPath = string.Empty;
        public string PreviousPath
        {
            get => _PreviousPath;
            set=>SetProperty(ref _PreviousPath, value, nameof(PreviousPath));
        }

        private bool _isLoading = true;

        public bool isLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value, nameof(isLoading));
        }

        private string _loadingFileName = "ロード中";

        public string loadingFileName
        {
            get => _loadingFileName;
            set => SetProperty(ref _loadingFileName, value, nameof(loadingFileName));
        }

        // private string OrderByMode = 
    }
}
