using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Util
{
    public static class StringFormat
    {
        public static string ToDurationString(TimeSpan time)
        {
            var front = time.ToString("mm:ss");

            if (time.Hours > 0)
                return $"{time.Hours}:{front}";
            else
                return front;
        }
    }
}
