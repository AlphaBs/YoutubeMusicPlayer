using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace YMP.View
{
    public class TrayIcon
    {
        private static ILog log = LogManager.GetLogger("TrayIcon");

        public System.Windows.Forms.NotifyIcon notify;
        public System.Windows.Window window;

        public TrayIcon(System.Windows.Window _window)
        {
            log.Info("Creating TrayIcon");

            this.window = _window;

            notify = new System.Windows.Forms.NotifyIcon()
            {
                Icon = Properties.Resources.logo,
                Visible = true,
                Text = "Youtube Music Player"
            };

            notify.Click += Notify_Click;
        }

        private void Notify_Click(object sender, EventArgs e)
        {
            window.Show();
        }

        public void Close()
        {
            log.Info("Closing TrayIcon");

            notify.Visible = false;
            notify.Icon = null;
            notify.Dispose();
        }
    }
}
