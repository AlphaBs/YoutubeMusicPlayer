using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Util
{
    public static class Utils
    {
        public static void StartProcess(string p)
        {
            try
            {
                System.Diagnostics.Process.Start(p);
            }
            catch
            {

            }
        }
    }
}
