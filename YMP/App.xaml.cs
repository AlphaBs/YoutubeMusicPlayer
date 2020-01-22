using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YMP.Core;
using YMP.UI;

namespace YMP
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            YMPCore.Initialize();

            new MainWindow().Show();
            new CEFTestWindow().Show();
        }
    }
}
