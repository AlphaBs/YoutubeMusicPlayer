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

            foreach (var item in YMPCore.PlayList.PlayLists)
            {
                AddPlaylistItem(item);
            }
        }

        private void ShowPlayListMusic(PlayList pl)
        {
            stkList.Children.Clear();
            CurrentPlayList = pl;
            lbListNameContent.Text = pl.Name;
            btnReturnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.KeyboardBackspace;
            EnableCreate(false);

            foreach (var item in pl.Musics)
            {
                AddMusicItem(item);
            }
        }

        private async void AddMusicItem(Music music)
        {
            var item = new PlayListItem();

            item.Title = music.Title;
            item.SubTitle = music.Artists;
            item.Thumbnail = await WebImage.GetImage(music.Thumbnail);
            item.Length = StringFormat.ToDurationString(music.Duration);
            item.Tag = stkList.Children.Count;

            item.Click += (sender, e) =>
            {
                var pli = (PlayListItem)sender;
                var click_music = (int)pli.Tag;

                YMPCore.PlayList.CurrentPlayList = CurrentPlayList;
                CurrentPlayList.CurrentMusicIndex = click_music;
                YMPCore.Browser.PlayMusic(CurrentPlayList.Musics[click_music]);
            };

            stkList.Children.Add(item);
        }

        private void AddPlaylistItem(PlayList list)
        {
            var item = new PlayListItem();

            item.Title = list.Name;
            item.SubTitle = $"곡 {list.Lenght}개";
            item.Thumbnail = (BitmapImage)FindResource("folder");
            item.Length = "";
            item.Tag = list;

            item.Click += (sender, e) =>
            {
                var pli = (PlayListItem)sender;
                ShowPlayListMusic((PlayList)pli.Tag);
            };

            stkList.Children.Add(item);
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
                        result = list.OrderBy(x => CurrentPlayList.Musics[(int)(x.Tag)].AddDate);
                        break;
                    case SortMode.Name:
                        result = list.OrderBy(x => CurrentPlayList.Musics[(int)(x.Tag)].Title);
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
