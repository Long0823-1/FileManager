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
        public Task<ObservableCollection<string>> FilesList(string path = @$"Z:\ダウン\本\mu\ふ\ふ\の\ふ\エロ漫画")
        {
            vm.Path = path;
            vm.FilesList = new ObservableCollection<string>();
            var files = Directory.GetFileSystemEntries(path);
            foreach (var file in files)
            {
                vm.FilesList.Add(Path.GetFileName(file));
            }
            return null;
        }
    }
}
