using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Util
{
    public static class MsiQuery
    {
        [DllImport("msi.dll", SetLastError = true)]
        public static extern INSTALLSTATE MsiQueryProductState(string product);

        public enum INSTALLSTATE
        {
            NOTUSED = -7,  // component disabled
            BADCONFIG = -6,  // configuration data corrupt
            INCOMPLETE = -5,  // installation suspended or in progress
            SOURCEABSENT = -4,  // run from source, source is unavailable
            MOREDATA = -3,  // return buffer overflow
            INVALIDARG = -2,  // invalid function argument
            UNKNOWN = -1,  // unrecognized product or feature
            BROKEN = 0,  // broken
            ADVERTISED = 1,  // advertised feature
            REMOVED = 1,  // component being removed (action state, not settable)
            ABSENT = 2,  // uninstalled (or action state absent but clients remain)
            LOCAL = 3,  // installed on local drive
            SOURCE = 4,  // run from source, CD or net
            DEFAULT = 5,  // use default, local or source
        }
    }
}
