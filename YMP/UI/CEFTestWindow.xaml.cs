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
using System.Windows.Shapes;
using CefSharp;
using YMP.Core;

namespace YMP.UI
{
    /// <summary>
    /// CEFTestWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CEFTestWindow : Window
    {
        public CEFTestWindow()
        {
            InitializeComponent();
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                tbUrl.Text = browser.Browser.Address;
            });
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            browser.Browser.Load(tbUrl.Text);
        }
    }
}
