using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using log4net;

namespace YMP.Util
{
    public static class WebImage
    {
        private static ILog log = LogManager.GetLogger("WebImage");

        public static readonly string CachePath = YMPInfo.CachePath;

        public static async Task<BitmapImage> GetImage(string url)
        {
            try
            {
                var cacheFile = GetCachePath(url);
                if (!File.Exists(cacheFile))
                {
                    log.Info("Download Cache : " + url);

                    using (var wc = new WebClient())
                    {
                        await wc.DownloadFileTaskAsync(url, cacheFile);
                    }
                }

                return new BitmapImage(new Uri(cacheFile));
            }
            catch (Exception ex)
            {
                log.Info("GetImage Exception");
                log.Info(ex);
                return new BitmapImage();
            }
        }

        private static string GetCachePath(string url)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string filename = new string(r.Replace(url, "").Reverse().ToArray());

            return Path.Combine(CachePath, filename);
        }

        public static long GetCacheSize()
        {
            log.Info("GetCacheSize");

            var dir = new DirectoryInfo(CachePath);

            if (!dir.Exists)
                return 0;

            long sum = 0;
            foreach (var item in dir.GetFiles())
            {
                sum += item.Length;
            }

            log.Info("Cache Size : " + sum);
            return sum;
        }

        public static void ClearCache()
        {
            log.Info("ClearCache");

            var dir = new DirectoryInfo(CachePath);

            if (!dir.Exists)
                return;

            foreach (var item in dir.GetFiles())
            {
                try
                {
                    item.Delete();
                }
                catch(Exception e)
                {
                    log.Info(e);
                }
            }
        }
    }
}
