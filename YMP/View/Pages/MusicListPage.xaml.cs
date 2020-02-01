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
using YMP.Model;
using YMP.View.Controls;
using YMP.Util;

namespace YMP.View.Pages
{
    enum SortMode
    {
        Time,
        Name,
        Custom
    }

    /// <summary>
    /// MusicListPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MusicListPage : Page
    {
        public MusicListPage()
        {
            InitializeComponent();

            ShowAllPlayLists();
        }

        SortMode PlayListSortMode = SortMode.Custom;
        PlayList CurrentPlayList = null;

        private void ShowAllPlayLists()
        {
            stkList.Children.Clear();
            lbListNameContent.Text = "플레이리스트";
            btnReturnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlaylistNote;
            EnableCreate(true);

            var playlists = YMPCore.PlayList.PlayLists;
            for (int i = 0; i < playlists.Count; i++)
            {
                var item = new PlayListItem(i);
                item.Playlist = playlists[i];
                item.Click += PlayListItem_Click;
                stkList.Children.Add(item);
            }
        }
        
        private void PlayListItem_Click(object sender, EventArgs e)
        {
            var ctrl = sender as PlayListItem;
            if (ctrl == null)
                return;

            ShowPlayListMusic(ctrl.Playlist);
        }

        private void ShowPlayListMusic(PlayList pl)
        {
            stkList.Children.Clear();

            CurrentPlayList = pl;
            lbListNameContent.Text = pl.Name;
            btnReturnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.KeyboardBackspace;
            EnableCreate(false);

            var musics = pl.GetMusics();
            for (int i = 0; i < musics.Length; i++)
            {
                var item = new PlayListItem(i);
                item.Music = musics[i];
                item.Click += MusicItem_Click;
                stkList.Children.Add(item);
            }
        }

        private void MusicItem_Click(object sender, EventArgs e)
        {
            var ctrl = sender as PlayListItem;
            var musicIndex = ctrl.Index;

            YMPCore.PlayList.CurrentPlayList = CurrentPlayList;
            YMPCore.Browser.PlayMusic(CurrentPlayList.GetMusic(musicIndex));
        }

        public void UpdateList()
        {
            if (CurrentPlayList != null)
            {
                var list = new List<PlayListItem>(stkList.Children.Count);
                foreach (var item in stkList.Children)
                {
                    if (item is FrameworkElement)
                        list.Add((PlayListItem)item);
                }

                IEnumerable<PlayListItem> result;

                switch (PlayListSortMode)
                {
                    case SortMode.Time:
                        result = list.OrderBy(x => CurrentPlayList.GetMusic(x.Index).AddDate);
                        break;
                    case SortMode.Name:
                        result = list.OrderBy(x => CurrentPlayList.GetMusic(x.Index).Title);
                        break;
                    case SortMode.Custom:
                    default:
                        result = list;
                        break;
                }

                stkList.Children.Clear();
                foreach (var item in result)
                {
                    stkList.Children.Add(item);
                }
            }
            else
                ShowAllPlayLists();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPlayList != null)
                ShowAllPlayLists();
        }

        private void DialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            var input = tbNewName.Text;
            tbNewName.Clear();

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!(bool)eventArgs.Parameter)
                return;

            YMPCore.PlayList.CreateNewPlaylist(input);
            ShowAllPlayLists();
        }

        private void EnableCreate(bool value)
        {
            btnAddPlayList.IsEnabled = value;
            btnAddPlayList.Visibility = v(value);
        }

        private void btnSortByName_Click(object sender, RoutedEventArgs e)
        {
            PlayListSortMode = SortMode.Name;
            UpdateList();
        }

        private void btnSortByTime_Click(object sender, RoutedEventArgs e)
        {
            PlayListSortMode = SortMode.Time;
            UpdateList();
        }

        private void btnSortByCustom_Click(object sender, RoutedEventArgs e)
        {
            PlayListSortMode = SortMode.Custom;
            UpdateList();
        }

        private void btnChangeSort_Click(object sender, RoutedEventArgs e)
        {
            // TODO : change list order by user
        }

        private Visibility v(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
