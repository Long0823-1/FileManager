using SevenZip;
using System.Diagnostics;
using System.IO;

namespace ComicManager
{
    public class ArchiveExtractor
    {
        string sevenZip_x86_x64 = @"C:\Program Files\7-Zip\7z.dll";
        string sevenZip_x86 = @"C:\Program Files (x86)\7-Zip\7z.dll";
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
        private static bool IsImageFile(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".tiff":
                case ".tif":
                case ".heic":
                case ".webp":
                case ".raw":
                case ".svg":
                case ".ico":
                    return true;
                default:
                    return false;
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
                    if(IsImageFile(Path.GetExtension(firstFile.FileName).ToLower()))
                    {
                        break;
                    }
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
                if (File.Exists(sevenZip_x86_x64))
                {
                    SevenZipBase.SetLibraryPath(sevenZip_x86_x64);
                }else
                {
                    SevenZipBase.SetLibraryPath(sevenZip_x86);
                }

                using (var extractor = new SevenZipExtractor(path))
                {
                    if (extractor.ArchiveFileData.Count > 0)
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
            finally
            {
                GC.Collect();
            }

            return "謎エラー";
            
        }
        public void GetThumb(string filePath)
        {
            try
            {
                string result = ExtractArchive(filePath);
                vm.CoverImage = MainViewModel.LoadImage(result); // アーカイブファイルを解凍する
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
    }
}
