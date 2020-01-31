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
        public string DurationStr { get; set; }
        [JsonProperty]
        public string Thumbnail { get; set; }
        [JsonProperty]
        public string AddDateStr { get; set; }

        public DateTime PublishAt { get; set; }
        public ulong Views { get; set; }

        DateTime _addDate = DateTime.MinValue;
        public DateTime AddDate 
        {
            get
            {
                if (_addDate == DateTime.MinValue)
                    _addDate = DateTime.Parse(AddDateStr);
                return _addDate;
            }
            set
            {
                _addDate = value;
                AddDateStr = _addDate.ToString("G");
            }
        }

        TimeSpan _duration = TimeSpan.Zero;
        public TimeSpan Duration 
        {
            get
            {
                if (_duration == TimeSpan.Zero)
                    _duration = TimeSpan.Parse(DurationStr);
                return _duration;
            }
            set
            {
                _duration = value;
                DurationStr = _duration.ToString("G");
            }
        }
    }
}
