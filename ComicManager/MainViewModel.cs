using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicManager
{
    public class MainViewModel:Prism.Mvvm.BindableBase
    {
        public MainViewModel() 
        {

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
            get => _Path;
            set=>SetProperty(ref  _Path, value,nameof(Path));   
        }

        private ObservableCollection<string> _FilesList;
        public ObservableCollection<string> FilesList
        {
            get => _FilesList;
            set=> SetProperty(ref _FilesList, value,nameof(FilesList));
        }
    }
}
