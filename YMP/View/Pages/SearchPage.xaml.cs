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
            CurrentNextPageButton = getNextPageButton();
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

        Button CurrentNextPageButton;
        string CurrentNextPageToken = "";
        string CurrentQuery = "";

        private Button getNextPageButton()
        {
            var btn = new Button();
            btn.Click += NextBtn_Click;
            btn.Content = "다음 페이지";
            btn.Style = (Style)FindResource("MaterialDesignFlatButton");
            return btn;
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentShowingPlayList == null)
                Search(CurrentQuery, false);
            else
                ShowPlayListItems(CurrentShowingPlayList.Metadata, false);
        }

        public void Search(string q, bool refresh)
        {
            if (Searching)
                return;

            Searching = true;
            lbInputPlease.Visibility = Visibility.Collapsed;

            if (CurrentShowingPlayList != null)
                btnBack_Click(this, null);

            CurrentQuery = q;
            if (refresh)
            {
                CurrentNextPageToken = "";
                stkList.Children.Clear();
                svList.ScrollToHome();
            }

            var th = new Thread(() =>
            {
                ytSearch(q);
            });
            th.Start();
        }

        void ytSearch(string q)
        {
            Music[] videoIds = new Music[0];
            PlayListMetadata[] playlistIds = new PlayListMetadata[0];

            // Search one video by video id
            if (q.Length > 4 && q.Substring(0, 4) == "#id=")
            {
                var id = q.Substring(4);
                videoIds = YMPCore.Youtube.Videos(new string[] { id });
            }
            // Search videos by query string
            else
            {
                var r = YMPCore.Youtube.Search(q, ref CurrentNextPageToken);
                videoIds = YMPCore.Youtube.Videos(r.Item1);
                playlistIds = YMPCore.Youtube.Playlists(r.Item2);
            }

            // UI
            Dispatcher.Invoke(() =>
            {
                Searching = false;

                lbNoResult.Visibility = Visibility.Collapsed;
                if (string.IsNullOrEmpty(CurrentNextPageToken) && videoIds.Length == 0 && playlistIds.Length == 0)
                {
                    lbNoResult.Visibility = Visibility.Visible;
                    return;
                }

                for (int i = 0; i < playlistIds.Length; i++)
                {
                    AddPlayList(i, playlistIds[i]);
                }

                for (int i = 0; i < videoIds.Length; i++)
                {
                    AddVideo(i, videoIds[i]);
                }

                stkList.Children.Remove(CurrentNextPageButton);
                if (!string.IsNullOrEmpty(CurrentNextPageToken))
                    stkList.Children.Add(CurrentNextPageButton);
            });
        }

        private void AddPlayList(int index, PlayListMetadata playlist)
        {
            var c = new SearchListItem(index);
            c.Playlist = playlist;

            c.ClickEvent += PlayListItemClick;
            c.AddEvent += PlayListItemAdd;
            stkList.Children.Add(c);
        }

        private void AddVideo(int index, Music music)
        {
            var c = new SearchListItem(index);
            c.Music = music;

            c.ClickEvent += VideoItemClick;
            c.AddEvent += VideoItemAdd;
            stkList.Children.Add(c);
        }

        PlayList CurrentShowingPlayList;
        List<SearchListItem> SearchResultCache;

        private void PlayListItemClick(object sender, EventArgs e)
        {
            // playlist

            var ctrl = sender as SearchListItem;
            if (ctrl == null)
                return;

            ShowPlayListItems(ctrl.Playlist, true);
        }

        private void ShowPlayListItems(PlayListMetadata playlist, bool refresh)
        {
            if (refresh)
            {
                SearchResultCache = new List<SearchListItem>(stkList.Children.Count);
                foreach (var item in stkList.Children)
                {
                    if (item is SearchListItem)
                        SearchResultCache.Add((SearchListItem)item);
                }
                CurrentNextPageToken = "";
                stkList.Children.Clear();
                svList.ScrollToHome();
                lbListNameContent.Text = playlist.Title;
            }

            var musics = YMPCore.Youtube.PlaylistItem(playlist, ref CurrentNextPageToken);

            if (refresh)
                CurrentShowingPlayList = new PlayList(playlist.Title, "youtube", musics, playlist);

            for (int i = 0; i < musics.Length; i++)
            {
                var c = new SearchListItem(i);
                c.Music = musics[i];

                c.ClickEvent += VideoItemClick;
                stkList.Children.Add(c);
            }

            stkList.Children.Remove(CurrentNextPageButton);
            if (!string.IsNullOrEmpty(CurrentNextPageToken))
                stkList.Children.Add(CurrentNextPageButton);
        }

        private void PlayListItemAdd(object sender, EventArgs e)
        {
            var ctrl = sender as SearchListItem;
            if (ctrl == null)
                return;

            Searching = true;
            stkList.IsEnabled = false;

            var th = new Thread(() =>
            {
                var pagetoken = "";
                var musics = new List<Music>();
                do
                {
                    musics.AddRange(YMPCore.Youtube.PlaylistItem(ctrl.Playlist, ref pagetoken));

                } while (!string.IsNullOrEmpty(pagetoken));

                YMPCore.PlayList.AddPlayList(new PlayList(ctrl.Playlist.Title, "youtube", musics.ToArray(), ctrl.Playlist));

                Dispatcher.Invoke(() =>
                {
                    Searching = false;
                    stkList.IsEnabled = true;
                });
            });
            th.Start();
        }

        private void VideoItemClick(object sender, EventArgs e)
        {
            // music

            var ctr = sender as SearchListItem;
            if (ctr == null)
                return;

            if (CurrentShowingPlayList != null)
            {
                YMPCore.PlayList.CurrentPlayList = CurrentShowingPlayList;
                var music = CurrentShowingPlayList.GetMusic(ctr.Index);
                YMPCore.Browser.PlayMusic(music);
            }
            else
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
    }
}
