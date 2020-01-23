using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.WinForms;

namespace YMP.UI
{
    /// <summary>
    /// CEFBrowser.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CEFBrowser : UserControl
    {
        public CEFBrowser()
        {
            InitializeComponent();

            var host = new System.Windows.Forms.Integration.WindowsFormsHost();
            browser = new ChromiumWebBrowser("about:blank");
            host.Child = browser;
            this.grd.Children.Add(host);
        }

        ChromiumWebBrowser browser;

        public ChromiumWebBrowser Browser
        {
            get => browser;
        }
    }
}
