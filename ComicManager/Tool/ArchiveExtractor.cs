using SevenZip;
using SkiaSharp;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace ComicManager
{
    public class ArchiveExtractor
    {
        string sevenZip_x86_x64 = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive")
, @"Program Files\7-Zip\7z.dll");
        string sevenZip_x86 = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive")
, @"Program Files (x86)\7-Zip\7z.dll");
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

        /// <summary>
        /// 画像かどうかを判別
        /// </summary>
        /// <param name="extension">拡張子</param>
        /// <returns>画像かどうかをbool型で返す</returns>
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

        /// <summary>
        /// 画像ファイルがアーカイブ内に有るかどうかをチェック
        /// </summary>
        /// <param name="extractor">SevenZipExtractor</param>
        /// <returns>ファイルの場所を数値で返す（0から）</returns>
        private static int FileExists(SevenZipExtractor extractor)
        {
            int i = 0;
            var firstFile = extractor.ArchiveFileData[i];
            while (!Path.HasExtension(firstFile.FileName))
            {
                firstFile = extractor.ArchiveFileData[i];

                // ファイルだった場合、即時に抜ける
                if (Path.HasExtension(firstFile.FileName))
                {
                    // 拡張子を見て、画像ファイルかどうかを判断
                    if (IsImageFile(Path.GetExtension(firstFile.FileName).ToLower()))
                    {
                        break;
                    }
                }

                Debug.WriteLine(firstFile.FileName);
                i++;
            }
            return i;
        }
        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <return>サムネイルのある場所を返す</returns>
        private string ExtractArchive(string path)
        {
            CreateDirectory(); // とりあえずキャッシュディレクトリを作る

            try
            {
                if (File.Exists(sevenZip_x86_x64))
                {
                    SevenZipBase.SetLibraryPath(sevenZip_x86_x64);
                }
                else
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
                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
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
        /// <summary>
        /// pdfからjpgに変換
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>サムネイルのある場所を返す</returns>
        public string PdfToJpg(string filePath)
        {
            try
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] fileData = new byte[new FileInfo(filePath).Length];
                stream.Read(fileData, 0, fileData.Length);
                string base64 = Convert.ToBase64String(fileData);

                string outputDir = System.IO.Path.Combine(CachePath, System.IO.Path.GetFileNameWithoutExtension(filePath));
                Directory.CreateDirectory(outputDir);

                var image = PDFtoImage.Conversion.ToImage(base64, 0);
                var skImage = SKImage.FromBitmap(image);
                string outputPath = System.IO.Path.Combine(outputDir, $"cover.jpg");

                using (var saveStream = File.Create(outputPath))
                {
                    var encodedData = skImage.Encode(SKEncodedImageFormat.Jpeg, 80); // Jpegに変換
                    encodedData.SaveTo(saveStream); // ファイルに保存
                }

                Debug.WriteLine($"Image saved: {outputPath}");


                return outputPath;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return filePath;
            }
        }
        /// <summary>
        /// ここからすべてが始まる
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public async void GetThumb(string filePath)
        {
            try
            {
                string result = string.Empty;
                if (Path.GetExtension(filePath).ToLower() == ".pdf")
                {
                    result = PdfToJpg(filePath); // pdfからjpgに変換
                }
                else if (Path.GetExtension(filePath).ToLower()
                switch
                {
                    ".mp4" or ".m4v" or ".mov" or ".mkv" or ".wmv" or ".ts" => true,
                    ".heic" or ".webp" or ".png" or ".jpg" or ".zip" or ".rar" or ".7z" => false
                })
                {
                    string path = string.Empty;
                    path = @""".\ffmpeg-7.1-essentials_build\bin\ffmpeg.exe""";

                    string outputDir = System.IO.Path.Combine(CachePath, System.IO.Path.GetFileNameWithoutExtension(filePath));

                    if (File.Exists(@".\.ThumbSaveOn"))
                    {
                        outputDir = System.IO.Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
                    }

                    string output = Path.Combine(outputDir, "cover.png");
                    Directory.CreateDirectory(outputDir);

                    if (!File.Exists(output))
                    {
                        var startInfo = new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = path,
                            Arguments = @$" -i ""{filePath}"" -vf thumbnail=60 -frames:v 1 ""{output}""",
                            UseShellExecute = false,
                            CreateNoWindow = true,

                        };
                        var isEnd = System.Diagnostics.Process.Start(startInfo);

                        result = await CoverImageIsExists(output);

                    }
                    else
                    {
                        result = output;
                    }

                }
                else
                {
                    result = ExtractArchive(filePath); // アーカイブから画像を解凍
                }

                vm.CoverImage = LoadImage(result); // 画像をStreamに変換
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        private async static Task<string> CoverImageIsExists(string output)
        {
            return await Task.Run(async () =>
            {
                while (!File.Exists(output)) ;
                try
                {
                    LoadImage(output);
                }
                catch (System.IO.IOException)
                {
                    return await CoverImageIsExists(output);
                }
                return output;
            });

        }


    }
}
