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
using YMP.Core;
using YMP.UI.Pages;

namespace YMP.UI
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
        MusicListPage musicList;
        PlayerPage playerPage;
        SearchPage searchPage;

        FrameContent CurrentContent = FrameContent.Blank;
        FrameContent PreviousContent = FrameContent.Blank;

        public MainWindow()
        {
            InitializeComponent();

            musicList = new MusicListPage();
            playerPage = new PlayerPage();
            searchPage = new SearchPage();

            // default page
            SetFrameContent(FrameContent.MusicListPage);
        }

        public void SetFrameContent(FrameContent c)
        {
            if (c == CurrentContent)
                return;

            PreviousContent = CurrentContent;
            CurrentContent = c;

            switch (c)
            {
                case FrameContent.MusicListPage:
                    frmContent.Content = musicList;
                    break;
                case FrameContent.SearchPage:
                    frmContent.Content = searchPage;
                    break;
                case FrameContent.PlayerPage:
                    frmContent.Content = playerPage;
                    break;
                default:
                    break;
            }
        }

        public void SetPreviousContent()
        {
            SetFrameContent(PreviousContent);
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetFrameContent(FrameContent.PlayerPage);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            YMPCore.Stop();
        }
    }
}
