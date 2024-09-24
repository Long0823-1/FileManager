using ComicManager.Tool;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using static ComicManager.MainViewModel;

namespace ComicManager
{
    public class GetFilesList
    {
        public GetFilesList()
        {
            vm = (App.Current as App).MainViewModel;
            //vm.FilesList.CollectionChanged += FilesList_CollectionChanged;

        }
        MainViewModel vm;
        public async Task FilesList(string path, bool force = false)
        {
            Loading loading = new Loading();
            loading.Show();
            var get = await Task.Run(() =>
             {
                 bool result = false;
                 // 前の情報を格納

                 if (force)
                 {
                     GetFilesListMethod(path);

                     result = true;
                 }
                 else if (path == vm.PreviousPath)
                 {
                     vm.FilesList = vm.PreviousFilesList;
                     vm.Path = vm.PreviousPath;
                     result = true;
                 }
                 else
                 {
                     GetFilesListMethod(path);

                     result = true;
                 }

                 return result;
             });

            if (get)
            {
                loading.Close();
            }
            else
            {
                loading.Close();
                MessageBox.Show("Error", "ロードに失敗しました", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void GetFilesListMethod(string path)
        {
            vm.PreviousFilesList = vm.FilesList;
            vm.PreviousPath = vm.Path;
            vm.Path = path;
            //　一時的に格納
            ObservableCollection<FilesListClass> _tempList = new ObservableCollection<FilesListClass>();

            var files = Directory.GetFileSystemEntries(path);
            string filename = string.Empty;
            foreach (var file in files)
            {
                Debug.WriteLine(file);
                filename = Path.GetFileName(file); //ファイル名だけ一旦ここに格納

                vm.loadingFileName = "ロード中\n" + filename; // Loading中のファイル名を表示

                _tempList.Add(new FilesListClass { filePath = file, fileName = filename, dateTime = File.GetCreationTime(file).ToString("yyyy/MM/dd（dddd）"), maxBytes = "" }); //配列にクラスを格納
            }
            ObservableCollection<FilesListClass> orderBy = new ObservableCollection<FilesListClass>(_tempList.OrderBy(x => x.filePath));
            orderBy.Insert(0, (new() { fileName = "..", filePath = @"\.." }));
            vm.FilesList = orderBy;
        }
    }
}
