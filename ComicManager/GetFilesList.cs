using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComicManager
{
    public class GetFilesList
    {
        public GetFilesList() 
        {
            vm = (App.Current as App).MainViewModel;
        }
        MainViewModel vm;
        public Task<ObservableCollection<string>> FilesList(string Path = @$"Z:\ダウン\本\mu\ふ\ふ\の\ふ\エロ漫画")
        {
            vm.Path = Path;
            vm.FilesList = new ObservableCollection<string>();
            var files = Directory.GetFileSystemEntries(Path);
            foreach (var file in files)
            {
                vm.FilesList.Add(file);
            }
            return null;
        }
    }
}
