using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YMP.Model;

namespace YMP.View.Pages
{
    /// <summary>
    /// MiniWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MiniWindow : Window
    {
        public MiniWindow(object context)
        {
            InitializeComponent();
            this.DataContext = context;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var wid = SystemParameters.WorkArea.Width;
            var hei = SystemParameters.WorkArea.Height;

            var margin = 30;

            this.Left = wid - margin - this.Width;
            this.Top = hei - margin - this.Height;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            YMPCore.Main.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
