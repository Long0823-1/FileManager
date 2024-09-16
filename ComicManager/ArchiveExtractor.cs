using SevenZip;
using System.IO;

namespace ComicManager
{
    public class ArchiveExtractor
    {
        public ArchiveExtractor()
        {

        }
        private string CatchePath = @"C:\ComicManager\caches";
        private void ExtractArchive(string path)
        {
            SevenZipBase.SetLibraryPath(@".\7z.dll");
            using (var extractor = new SevenZipExtractor(path))
            {
                extractor.ExtractArchiveAsync(CatchePath + @$"\{Path.GetFileNameWithoutExtension(path)}");
            }
        }
        public string GetThumb()
        {

        }
    }
}
