using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Model
{
    public class PlayListMetadata
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Thumbnail { get; set; }
        public long Count { get; set; }
    }
}
