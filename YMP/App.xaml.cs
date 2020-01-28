using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using YMP.Model;
using YMP.View;

namespace YMP
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static Mutex CurrentMutex;
        const string mutexId = "AE805CDF-D5FA-49F4-B679-80E70183284C";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isSuccess;
            CurrentMutex = new Mutex(true, mutexId, out isSuccess);
            if (!isSuccess)
            {
                MessageBox.Show("Youtube Music Player 가 이미 실행중입니다.");
                Environment.Exit(0);
            }

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            YMPCore.Initialize();
        }
    }
}
