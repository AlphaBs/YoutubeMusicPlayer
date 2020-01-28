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
            var front = time.ToString(@"mm\:ss");

            if (time.Hours > 0)
                return $"{time.Hours}:{front}";
            else
                return front;
        }

        public static string ToTimeSpanString(DateTime t)
        {
            var n = DateTime.Now;

            if (n.Year != t.Year)
                return (n.Year - t.Year) + "년 전";
            else if (n.Month != t.Month)
                return (n.Month - t.Month) + "개월 전";
            else if (n.Day != t.Day)
            {
                var days = n.Day - t.Day;
                var weeks = days / 7;
                if (weeks > 0)
                    return weeks + "주 전";
                else
                    return days + "일 전";
            }
            else if (n.Hour != t.Hour)
                return (n.Hour - t.Hour) + "시간 전";
            else
                return "방금";
        }
    }
}
