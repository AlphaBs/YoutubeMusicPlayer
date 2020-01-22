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
using YMP.Core;
using YMP.UI.Controls;
using YMP.Util;

namespace YMP.UI.Pages
{
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

        PlayList CurrentPlayList = null;

        private void ShowAllPlayLists()
        {
            stkList.Children.Clear();
            lbListNameContent.Text = "플레이리스트";
            btnReturnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlaylistNote;

            var list = YMPCore.PlayList.PlayLists;
            for (int i = 0; i < list.Count; i++)
            {
                AddPlaylistItem(list[i], i);
            }
        }

        private void ShowPlayListMusic(PlayList pl)
        {
            stkList.Children.Clear();
            CurrentPlayList = pl;
            lbListNameContent.Text = pl.Name;
            btnReturnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.KeyboardBackspace;

            for (int i = 0; i<pl.Lenght; i++)
            {
                AddMusicItem(pl.Musics[i], i);
            }
        }

        private void AddMusicItem(Music music, int index)
        {
            var item = new PlayListItem();

            item.Title = music.Title;
            item.SubTitle = music.Artists;
            item.Thumbnail = Base64Image.GetImage(music.Thumbnail);
            item.Length = music.Duration;
            item.IndexNumber = index;

            item.Click += (sender, e) =>
            {
                var pli = (PlayListItem)sender;
                // PLAY MUSIC
            };

            stkList.Children.Add(item);
        }

        private void AddPlaylistItem(PlayList list, int index)
        {
            var item = new PlayListItem();

            item.Title = list.Name;
            item.SubTitle = $"곡 {list.Lenght}개";
            item.Thumbnail = (BitmapImage)FindResource("folder");
            item.Length = "";
            item.IndexNumber = index;

            item.Click += (sender, e) =>
            {
                var pli = (PlayListItem)sender;
                ShowPlayListMusic(YMPCore.PlayList.GetPlayList(pli.IndexNumber));
            };

            stkList.Children.Add(item);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPlayList != null)
                ShowAllPlayLists();
        }
    }
}
