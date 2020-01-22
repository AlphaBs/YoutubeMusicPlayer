using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Core
{
    public static class YMPCore
    {
        public static void Initialize()
        {
            PlayList = new PlayListManager();
            PlayList.LoadAllPlayLists();
        }

        public static PlayListManager PlayList { get; private set; }
    }
}
