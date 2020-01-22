using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Core
{
    public class Music
    {
        public string YoutubeID { get; private set; }
        public string Title { get; private set; }
        public string Artists { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Thumbnail { get; private set; }
    }
}
