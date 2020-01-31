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
using YMP.Util;

namespace YMP.View.Controls
{
    /// <summary>
    /// SearchListItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchListItem : UserControl
    {
        public SearchListItem()
        {
            InitializeComponent();
        }

        public event EventHandler ClickEvent;
        public event EventHandler AddEvent;

        Music m;
        public Music Music
        {
            get => m;
            set
            {
                if (m != value)
                {
                    m = value;
                    Title = m.Title;
                    Channel = m.Artists;

                    var published = StringFormat.ToFrendlyString(m.PublishAt);
                    var views = m.Views.ToString("n0");
                    Info = published + " / " + views + "회";

                    SetImageUrl(m.Thumbnail);
                    Duration = StringFormat.ToDurationString(m.Duration);
                }
            }
        }

        PlayListMetadata pl;
        public PlayListMetadata Playlist
        {
            get => pl;
            set
            {
                if (pl != value)
                {
                    pl = value;
                    Title = pl.Title;
                    Channel = pl.Creator;
                    Info = "재생목록";
                    SetImageUrl(pl.Thumbnail);
                    Duration = pl.Count + "개";
                }
            }
        }

        public string Title
        {
            get => lbTitle.Text;
            set => lbTitle.Text = value;
        }

        public string Channel
        {
            get => lbChannel.Text;
            set => lbChannel.Text = value;
        }

        public string Info
        {
            get => lbInfo.Text;
            set => lbInfo.Text = value;
        }

        public async void SetImageUrl(string url)
        {
            imgThumbnail.Source = await WebImage.GetImage(url);
        }

        public string Duration
        {
            get => lbDuration.Text;
            set => lbDuration.Text = value;
        }

        private void lbTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickEvent?.Invoke(this, new EventArgs());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEvent?.Invoke(this, new EventArgs());
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (Music != null)
                Utils.StartProcess(YMPCore.Youtube.GetVideoUrl(Music.YoutubeID));

            if (Playlist != null)
                Utils.StartProcess(YMPCore.Youtube.GetPlayListUrl(Playlist.ID));
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
