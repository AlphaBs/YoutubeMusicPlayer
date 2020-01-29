using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace YMP.Util
{
    public static class VCChecker
    {
        public static bool CheckVCRuntimeInstall()
        {
            var r = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\14.0\VC\Runtimes\X86");

            return GetData<int>(r, "Major") >= 14
                && GetData<int>(r, "Installed") != 0;
        }

        public static T GetData<T>(RegistryKey key, string name)
        {
            if (key != null)
            {
                var v = key.GetValue(name);
                return (T)v;
            }

            return default;
        }
    }
}
