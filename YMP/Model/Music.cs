using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YMP.Model
{
    public class Music
    {
        [JsonProperty]
        public string YoutubeID { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Artists { get; set; }
        [JsonProperty]
        public TimeSpan Duration { get; set; }
        [JsonProperty]
        public string Thumbnail { get; set; }
        [JsonProperty]
        public DateTime? AddDate { get; set; }
        [JsonProperty]
        public DateTime PublishAt { get; set; }
        [JsonProperty]
        public ulong Views { get; set; }
    }
}
