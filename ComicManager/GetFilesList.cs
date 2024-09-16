using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static ComicManager.MainViewModel;

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
            vm.FilesList = new ObservableCollection<FilesListClass>();
            var files = Directory.GetFileSystemEntries(path);
            foreach (var file in files)
            {
                vm.FilesList.Add(new FilesListClass { filePath = file, fileName = Path.GetFileName(file) ,dateTime = File.GetCreationTime(file).ToString("yyyy/MM/dd（dddd）")});
            }
            ObservableCollection<FilesListClass> orderedByTime = new ObservableCollection<FilesListClass>(vm.FilesList.OrderByDescending(x => x.dateTime).OrderByDescending(x=> x.fileName));
            vm.FilesList = orderedByTime;
            return null;
        }
    }
}
