using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Windows.Threading;

namespace ComicManager.Tool
{
    public class FileDownloader
    {

        public FileDownloader()
        {
            _vm = (App.Current as App).MainViewModel;
        }
        MainViewModel _vm;
        
        public async Task<bool> GetContent(string Url)
        {
            DownloadNow dlNow = new DownloadNow();
            dlNow.Show();

            var result = await Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient())
                {
                    string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0";
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                    /* if(Assets != "")
                     {
                         client.DefaultRequestHeaders.Add("Accept", Assets);
                     }*/

                    _vm.DownloadUrlName = Path.GetFileName(Url);
                    using (HttpResponseMessage response = await client.GetAsync(new Uri(Url), HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            byte[] buffer = new byte[8192];
                            int bytesRead;
                            long totalBytesRead = 0;
                            long? totalBytes = response.Content.Headers.ContentLength;
                            MemoryStream ms = new MemoryStream();
                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await ms.WriteAsync(buffer, 0, bytesRead);

                                totalBytesRead += bytesRead;

                                if (totalBytes.HasValue)
                                {
                                    double percentage = (double)totalBytesRead / totalBytes.Value * 100;

                                    _vm.DownloadPercentage = $"{(int)percentage}%";
                                    _vm.TotalBytes = (long)totalBytes;
                                    _vm.TotalBytesRead = (int)percentage;

                                    Debug.WriteLine($"Downloaded {totalBytesRead} of {totalBytes} bytes. {percentage:F2}% complete...");
                                }
                            }
                            try
                            {
                                ZipFile.ExtractToDirectory(ms, @".\", true);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }
                        }
                    }
                }
            });
            dlNow.Close();
            return result;
        }
    }
}
