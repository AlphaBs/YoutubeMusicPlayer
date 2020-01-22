using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YMP.Core
{
    public class Music
    {
        [JsonProperty]
        public string YoutubeID { get; private set; }
        [JsonProperty]
        public string Title { get; private set; }
        [JsonProperty]
        public string Artists { get; private set; }
        [JsonProperty]
        public string Duration { get; private set; }
        [JsonProperty]
        public string Thumbnail { get; private set; }
    }
}
