using System;
using YMP.Model;
using System.Windows.Forms;
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

            notify = new NotifyIcon()
            {
                Icon = Properties.Resources.logo,
                Visible = true,
                Text = "Youtube Music Player"
            };

            var context = new ContextMenu();

            var showWindow = new MenuItem();
            showWindow.Text = "프로그램 열기";
            showWindow.Click += ShowWindow_Click;

            var showMini = new MenuItem();
            showMini.Text = "미니윈도우 열기";
            showMini.Click += ShowMini_Click;

            var exit = new MenuItem();
            exit.Text = "종료";
            exit.Click += Exit_Click;

            context.MenuItems.AddRange(new MenuItem[]
            {
                showWindow,
                showMini,
                exit
            });

            notify.ContextMenu = context;
            notify.MouseClick += Notify_MouseClick;
        }

        private void Notify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                window.Show();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("정말로 프로그램을 종료할까요?", "YMP", MessageBoxButtons.YesNo, MessageBoxIcon.None);
            if (result == DialogResult.Yes)
                YMPCore.Stop();
        }

        private void ShowMini_Click(object sender, EventArgs e)
        {
            window.Show();
        }

        private void ShowWindow_Click(object sender, EventArgs e)
        {
            YMPCore.Main.Show();
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
