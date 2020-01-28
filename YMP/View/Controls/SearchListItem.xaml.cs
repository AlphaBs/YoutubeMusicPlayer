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

                    var published = StringFormat.ToTimeSpanString(m.PublishAt);
                    var views = m.Views.ToString("n0");
                    Info = published + " / " + views + "회";

                    ThumbnailUrl = m.Thumbnail;
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
                    ThumbnailUrl = pl.Thumbnail;
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

        string url;
        public string ThumbnailUrl
        {
            get => url;
            set
            {
                url = value;
                imgThumbnail.Source = new BitmapImage(new Uri(url));
            }
        }

        public int ThumbnailWidth
        {
            get => (int)imgThumbnail.Width;
            set => imgThumbnail.Width = value;
        }

        public int ThumbnailHeight
        {
            get => (int)imgThumbnail.Height;
            set => imgThumbnail.Height = value;
        }

        private void imgThumbnail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickEvent?.Invoke(this, new EventArgs());
        }
    }
}
