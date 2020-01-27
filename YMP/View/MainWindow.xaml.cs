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
using YMP.Model;
using YMP.View.Pages;
using YMP.ViewModel;

namespace YMP.View
{
    public enum FrameContent
    {
        Blank,
        MusicListPage,
        SearchPage,
        PlayerPage
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();

            this.DataContext = viewModel;
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CurrentContent = FrameContent.SearchPage;
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.OnClickBrowser();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            YMPCore.Stop();
        }

        private void slSeeker_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            viewModel.SliderDragStarted(null);
        }

        private void slSeeker_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            viewModel.SliderDragCompleted((int)slSeeker.Value);
        }
    }
}
