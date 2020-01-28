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
using System.Windows.Navigation;
using System.Windows.Shapes;
using YMP.Model;
using YMP.Util;
using YMP.View.Controls;
using System.Net;

namespace YMP.View.Pages
{
    /// <summary>
    /// SearchPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            this.SizeChanged += delegate
            {
                stkList.Width = this.ActualWidth - 20;
            };
        }

        public event EventHandler BackEvent;
        bool _searching = false;
        public bool Searching
        {
            get => _searching;
            set
            {
                _searching = value;
                pbLoad.Visibility = _searching ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Search(string q)
        {
            if (Searching)
                return;

            stkList.Children.Clear();
            Searching = true;
            var th = new Thread(() =>
            {
                ytSearch(q, "");
            });
            th.Start();
        }

        void ytSearch(string q, string pagetoken)
        {
            var r = YMPCore.Youtube.Search(q, pagetoken);
            var videoIds = YMPCore.Youtube.Videos(r.Item1);
            var playlistIds = YMPCore.Youtube.Playlists(r.Item2);

            Dispatcher.Invoke(() =>
            {
                foreach (var item in playlistIds)
                {
                    var c = new SearchListItem();
                    c.Playlist = item;

                    c.ClickEvent += C_ClickEvent1;
                    stkList.Children.Add(c);
                }

                foreach (var item in videoIds)
                {
                    var c = new SearchListItem();
                    c.Music = item;

                    c.ClickEvent += C_ClickEvent;
                    stkList.Children.Add(c);
                }

                Searching = false;
            });
        }

        private void C_ClickEvent1(object sender, EventArgs e)
        {
            // playlist
        }

        private void C_ClickEvent(object sender, EventArgs e)
        {
            // music

            var ctr = sender as SearchListItem;
            if (ctr == null)
                return;

            YMPCore.Browser.PlayMusic(ctr.Music);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, new EventArgs());
        }
    }
}
