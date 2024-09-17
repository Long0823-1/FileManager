using SevenZip;
using System.Diagnostics;
using System.IO;

namespace ComicManager
{
    public class ArchiveExtractor
    {
        public ArchiveExtractor()
        {
            vm = (App.Current as App).MainViewModel;
        }
        MainViewModel vm;
        private string CachePath = $@"{Path.GetTempPath()}\ComicManager\caches";

        private void CreateDirectory()
        {
            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }
            
        }
        private int FileExists(SevenZipExtractor extractor)
        {
            int i = 0;
            var firstFile = extractor.ArchiveFileData[i];
            while(!Path.HasExtension(firstFile.FileName))
            {
                firstFile = extractor.ArchiveFileData[i];

                // ファイルだった場合、即時に抜ける
                if(Path.HasExtension(firstFile.FileName))
                {
                    break;             
                }

                Debug.WriteLine(firstFile.FileName);
                i++;
            }
            return i;
        }

        private string ExtractArchive(string path)
        {
            CreateDirectory(); // とりあえずディレクトリを作る

            try
            {
                SevenZipBase.SetLibraryPath(@"C:\Program Files\7-Zip\7z.dll");
                using (var extractor = new SevenZipExtractor(path))
                {
                    if(extractor.ArchiveFileData.Count > 0)
                    {
                        // ファイルかどうかを再帰的にチェック
                        var result = FileExists(extractor);

                        string outputDirectory = Path.Combine(CachePath, Path.GetFileNameWithoutExtension(path));
                        Directory.CreateDirectory(outputDirectory);

                        string outputFile = Path.Combine(outputDirectory, "Cover.png");
                        using (FileStream fs = new FileStream(outputFile,FileMode.Create))
                        {
                            extractor.ExtractFile(result, fs);
                            fs.Close();
                        }
                        
                        return outputFile;

                    }
                    else
                    {
                        Debug.WriteLine("Not Found!");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return path;
            }

            return "謎エラー";
            
        }
        public void GetThumb(string filePath)
        {
            try
            {
                vm.CoverImage = ExtractArchive(filePath); // アーカイブファイルを解凍する
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
    }
}
