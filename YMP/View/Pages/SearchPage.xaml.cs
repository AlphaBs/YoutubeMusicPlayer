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
using MaterialDesignThemes.Wpf;

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

            Searching = true;
            stkList.Children.Clear();

            var th = new Thread(() =>
            {
                ytSearch(q, "");
            });
            th.Start();
        }

        void ytSearch(string q, string pagetoken)
        {
            var r = YMPCore.Youtube.Search(q, ref pagetoken);
            var videoIds = YMPCore.Youtube.Videos(r.Item1);
            var playlistIds = YMPCore.Youtube.Playlists(r.Item2);

            Dispatcher.Invoke(() =>
            {
                Searching = false;

                lbNoResult.Visibility = Visibility.Collapsed;
                if (pagetoken == "" && videoIds.Length == 0 && playlistIds.Length == 0)
                {
                    lbNoResult.Visibility = Visibility.Visible;
                    return;
                }

                for (int i = 0; i < playlistIds.Length; i++)
                {
                    var c = new SearchListItem(i);
                    c.Playlist = playlistIds[i];

                    c.ClickEvent += PlayListItemClick;
                    c.AddEvent += PlayListItemAdd;
                    stkList.Children.Add(c);
                }

                for (int i = 0; i < videoIds.Length; i++)
                {
                    var c = new SearchListItem(i);
                    c.Music = videoIds[i];

                    c.ClickEvent += VideoItemClick;
                    c.AddEvent += VideoItemAdd;
                    stkList.Children.Add(c);
                }
            });
        }

        PlayList CurrentShowingPlayList;
        List<SearchListItem> SearchResultCache;

        private void PlayListItemClick(object sender, EventArgs e)
        {
            // playlist

            SearchResultCache = new List<SearchListItem>(stkList.Children.Count);
            foreach (var item in stkList.Children)
            {
                if (item is SearchListItem)
                    SearchResultCache.Add((SearchListItem)item);
            }

            var ctr = sender as SearchListItem;
            if (ctr == null)
                return;

            var pagetoken = "";
            var musics = YMPCore.Youtube.PlaylistItem(ctr.Playlist, ref pagetoken);
            CurrentShowingPlayList = new PlayList(ctr.Playlist.Title, "youtube", musics, ctr.Playlist);

            lbListNameContent.Text = ctr.Playlist.Title;

            stkList.Children.Clear();
            for (int i = 0; i < musics.Length; i++)
            {
                var c = new SearchListItem(i);
                c.Music = musics[i];

                c.ClickEvent += VideoItemClick;
                stkList.Children.Add(c);
            }
        }

        private void PlayListItemAdd(object sender, EventArgs e)
        {
            var ctrl = sender as SearchListItem;
            if (ctrl == null)
                return;

            var pagetoken = "";
            var musics = new List<Music>();
            do
            {
                musics.AddRange(YMPCore.Youtube.PlaylistItem(ctrl.Playlist, ref pagetoken));

            } while (!string.IsNullOrEmpty(pagetoken));

            YMPCore.PlayList.AddPlayList(new PlayList(ctrl.Playlist.Title, "youtube", musics.ToArray(), ctrl.Playlist));
        }

        private void VideoItemClick(object sender, EventArgs e)
        {
            // music

            var ctr = sender as SearchListItem;
            if (ctr == null)
                return;

            if (CurrentShowingPlayList != null)
            {
                CurrentShowingPlayList.CurrentMusicIndex = ctr.Index;
                YMPCore.PlayList.CurrentPlayList = CurrentShowingPlayList;
            }

            YMPCore.Browser.PlayMusic(ctr.Music);
        }

        Music AddMusic;

        private void VideoItemAdd(object sender, EventArgs e)
        {
            var ctrl = sender as SearchListItem;
            if (ctrl == null)
                return;

            AddMusic = ctrl.Music;

            foreach (var item in YMPCore.PlayList.PlayLists)
            {
                liPlaylist.Items.Add(item.Name);
            }

            addDialogHost.IsOpen = true;
        }


        private void btnAddCancle_Click(object sender, RoutedEventArgs e)
        {
            liPlaylist.Items.Clear();
            addDialogHost.IsOpen = false;
        }

        private void liPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = liPlaylist.SelectedItem;

            if (selected == null)
                return;

            var pl = YMPCore.PlayList.GetPlayList(selected.ToString());
            pl.AddMusic(AddMusic);

            liPlaylist.Items.Clear();
            addDialogHost.IsOpen = false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentShowingPlayList == null)
                BackEvent?.Invoke(this, new EventArgs());
            else
            {
                if (CurrentShowingPlayList == YMPCore.PlayList.CurrentPlayList)
                    YMPCore.PlayList.CurrentPlayList = null;
                CurrentShowingPlayList = null;

                stkList.Children.Clear();

                lbListNameContent.Text = "검색결과";

                foreach (var item in SearchResultCache)
                {
                    stkList.Children.Add(item);
                }

                SearchResultCache.Clear();
                SearchResultCache = null;
            }
        }

        private void addDialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {

        }
    }
}
