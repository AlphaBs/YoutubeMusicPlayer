using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using YMP.Model;
using YMP.Util;
using log4net;

namespace YMP
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static Mutex CurrentMutex;
        const string mutexId = "AE805CDF-D5FA-49F4-B679-80E70183284C";

        private static ILog log = LogManager.GetLogger("App");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isSuccess;
            CurrentMutex = new Mutex(true, mutexId, out isSuccess);
            if (!isSuccess)
            {
                MessageBox.Show("Youtube Music Player 가 이미 실행중입니다.");
                Environment.Exit(0);
            }

            log.Info("Start YMP " + YMPInfo.Version);

            if (!e.Args.Contains("noDependencyCheck"))
            {
                if (!VCChecker.CheckVCRuntimeInstall())
                {
                    MessageBox.Show("Visual Studio 2015용 Visual C++ 재배포 가능 패키지가 설치되어 있지 않습니다.\n확인버튼을 누르면 설치패이지로 이동됩니다.");
                    Utils.StartProcess(YMPInfo.VC2015Url);
                    Environment.Exit(0);
                }
            }

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            YMPCore.Initialize();
        }
    }
}
