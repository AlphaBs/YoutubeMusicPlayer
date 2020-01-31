using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static string ToFrendlyString(DateTime t)
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

        public static TimeSpan FromISO8601Str(string input)
        {
            const string hReg = @"[0-9]+H";
            const string mReg = @"[0-9]+M";
            const string sReg = @"[0-9]+S";

            int h = reg(input, hReg);
            int m = reg(input, mReg);
            int s = reg(input, sReg);

            return new TimeSpan(h, m, s);
        }

        static int reg(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                string intStr = Regex.Match(match.Value, @"\d").Value;

                int resultInt = 0;
                if (int.TryParse(intStr, out resultInt))
                    return resultInt;
                else
                    return 0;
            }
            else
                return 0;
        }
    }
}
