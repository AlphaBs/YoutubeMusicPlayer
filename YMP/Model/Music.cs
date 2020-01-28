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
        public string Duration { get; set; }
        [JsonProperty]
        public string Thumbnail { get; set; }
        [JsonProperty]
        public string AddDateStr { get; set; }
        public DateTime PublishAt { get; set; }
        public ulong Views { get; set; }

        [JsonIgnore]
        bool isDtParseDone = false;
        [JsonIgnore]
        DateTime dt;

        public DateTime AddDate
        {
            get
            {
                if (!isDtParseDone)
                {
                    dt = DateTime.Parse(AddDateStr);
                    isDtParseDone = true;
                }
                return dt;
            }
            set
            {
                dt = value;
                AddDateStr = dt.ToString("G", DateTimeFormatInfo.InvariantInfo);
            }
        }
    }
}
