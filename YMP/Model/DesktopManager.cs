using System;
using System.Drawing;
using System.Windows.Forms;
using YMP.Util;

/*
 * This code is from Code Project (https://www.codeproject.com/Articles/856020/Draw-Behind-Desktop-Icons-in-Windows-plus)
 * By Gerald Degeneve, 23 Dec 2014.
 * License : CPOL (https://www.codeproject.com/info/cpol10.aspx)
 */

namespace YMP.Model
{
    public class DesktopManager
    {
        public static Form CreateDesktopForm(Control child, int scrInx)
        {
            var scr = Screen.AllScreens[scrInx].Bounds;

            var f = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                Width = scr.Width,
                Height = scr.Height,
                Top = scr.X,
                Left = scr.Y
            };

            child.Dock = DockStyle.Fill;
            child.Location = new Point(0, 0);
            f.Controls.Add(child);
            f.Show();

            return f;
        }

        public static bool SetDesktopChildForm(Form f)
        {
            var handle = f.Handle;
            var progman = NativeMethods.FindWindow("Progman", null);

            var result = IntPtr.Zero;
            NativeMethods.SendMessageTimeout(progman,
                                      0x052C,
                                      new IntPtr(0),
                                      IntPtr.Zero,
                                      NativeMethods.SendMessageTimeoutFlags.SMTO_NORMAL,
                                      1000,
                                      out result);

            IntPtr workerw = IntPtr.Zero;
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = NativeMethods.FindWindowEx(tophandle,
                                               IntPtr.Zero,
                                               "SHELLDLL_DefView",
                                               IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    workerw = NativeMethods.FindWindowEx(IntPtr.Zero,
                                                  tophandle,
                                                  "WorkerW",
                                                  IntPtr.Zero);
                }

                return true;
            }), IntPtr.Zero);

            var formHandle = f.Handle;
            NativeMethods.ShowWindow(workerw, 0);
            NativeMethods.SetParent(formHandle, progman);

            return true;
        }
    }
}
