using System.Collections.ObjectModel;
using System.IO;
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
        public async Task FilesList(string path)
        {
            await Task.Run(() =>
            {
                ObservableCollection<FilesListClass> tempList = new ObservableCollection<FilesListClass>();

                var files = Directory.GetFileSystemEntries(path);

                foreach (var file in files)
                {
                    tempList.Add(new FilesListClass { filePath = file, fileName = Path.GetFileName(file), dateTime = File.GetCreationTime(file).ToString("yyyy/MM/dd（dddd）"),maxBytes ="" });
                }
                ObservableCollection<FilesListClass> orderBy = new ObservableCollection<FilesListClass>(tempList.OrderBy(x => x.filePath));
                orderBy.Insert(0,(new() { fileName = @"\..", filePath = @"\.." }));
                vm.FilesList = orderBy;
            });

        }
    }
}
