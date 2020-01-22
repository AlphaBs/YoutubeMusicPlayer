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
        static string p(string path, bool isdir=false)
        {
            var fullpath = Path.Combine(Environment.CurrentDirectory, path);

            var dirname = "";
            if (isdir)
                dirname = fullpath;
            else
                dirname = Path.GetDirectoryName(fullpath);

            Directory.CreateDirectory(dirname);
            return fullpath;
        }

        public readonly static string
            ProgramName     = "YoutubeMusicPlayer",
            Version         = "2.0-b1",

            WebFrontendPath = p("Web\\index.html"),
            PlaylistPath    = p("playlist", isdir: true),
            LicensePath     = p("LICENSE.txt"),
            SettingPath     = p("setting.json")
            ;
    }
}
