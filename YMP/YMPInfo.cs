using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YMP
{
    public class YMPInfo
    {
        static string initFile(string path)
        {
            var fullpath = Path.Combine(Environment.CurrentDirectory, path);

            var dirname = Path.GetDirectoryName(fullpath);

            Directory.CreateDirectory(dirname);
            return fullpath;
        }

        static string initDirc(string path)
        {
            var fullpath = Path.Combine(Environment.CurrentDirectory, path);

            var dirname = fullpath;
            Directory.CreateDirectory(dirname);
            return fullpath;
        }

        // Debug
        public static bool Debug = false;

        // This is nothing really woa f
        public static bool MBungMode = false;

        public readonly static string
            ProgramName     = "YoutubeMusicPlayer",
            Version         = "2.0-b2",

            WebFrontendPath = initFile("Web\\index.html"),
            PlaylistPath    = initDirc("playlist"),
            LicensePath     = initFile("LICENSE.txt"),
            SettingPath     = initFile("setting.json"),
            CachePath       = initDirc("ytcache"),
            
            VC2015Url       = "https://www.microsoft.com/ko-kr/download/confirmation.aspx?id=48145",
            ProjectUrl      = "https://github.com/AlphaBs/YoutubeMusicPlayer"
            ;
    }
}
