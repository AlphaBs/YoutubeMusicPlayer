using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Core
{
    public class YoutubeJSBound
    {
        public void OnReady()
        {
            Console.WriteLine("onready");
        }

        public void OnStateChange(int data)
        {
            Console.WriteLine("onstatechange {0}", data);
        }

        public void OnPlaybackQualityChange(string data)
        {
            Console.WriteLine("qualala {0}", data);
        }

        public void OnError(int data)
        {
            Console.WriteLine("err {0}", data);
        }
    }
}
