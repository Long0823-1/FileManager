using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ComicManager
{
    public class MainViewModel:Prism.Mvvm.BindableBase
    {
        public MainViewModel() 
        {

        }

        public class FilesListClass()
        {
            public string filePath {  get; set; }
            public string fileName { get; set; }
            public string maxBytes {  get; set; }
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
            set => SetProperty(ref _CoverImage, value,nameof(CoverImage));
        }
        private string _Path = @"C:\";
        private string[] _PathSplit;
        public string Path
        {
            get 
            {
                GetFilesList getFiles = new GetFilesList();
                getFiles.FilesList(_Path);
                //_PathSplit = _Path.Split(@"\");

                return _Path;
            }
            set=>SetProperty(ref  _Path, value,nameof(Path));   
        }

        private ObservableCollection<FilesListClass> _FilesList;
        public ObservableCollection<FilesListClass> FilesList
        {
            get => _FilesList;
            set=> SetProperty(ref _FilesList, value,nameof(FilesList));
        }

       // private string OrderByMode = 
    }
}
