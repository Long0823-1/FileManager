using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;

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

        private string _CoverImage = string.Empty;
        public string CoverImage
        {
            get => _CoverImage;
            set => SetProperty(ref _CoverImage, value,nameof(CoverImage));
        }
        private string _Path = @"C:\";
        public string Path
        {
            get 
            {
                GetFilesList getFiles = new GetFilesList();
                getFiles.FilesList(_Path);
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
    }
}
