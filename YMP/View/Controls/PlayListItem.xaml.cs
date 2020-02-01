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
    /// PlayListItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayListItem : UserControl
    {
        public PlayListItem(int index)
        {
            InitializeComponent();
            this.Index = index;
        }

        public int Index { get; private set; }
        public event EventHandler Click;

        Music _music;
        public Music Music
        {
            get => _music;
            set
            {
                if (_music != value)
                {
                    _music = value;
                    Title = _music.Title;
                    SubTitle = _music.Artists;
                    Length = StringFormat.ToDurationString(_music.Duration);
                    SetThumbnailAsync(_music.Thumbnail);
                }
            }
        }

        PlayList _playlist;
        public PlayList Playlist
        {
            get => _playlist;
            set
            {
                if (_playlist != value)
                {
                    _playlist = value;
                    Title = _playlist.Name;
                    SubTitle = $"곡 {_playlist.Lenght}개";
                    Thumbnail = (BitmapImage)FindResource("folder");
                    Length = "재생목록";
                }
            }
        }

        public string Title
        {
            get => lbTitle.Text;
            set => lbTitle.Text = value;
        }

        public string SubTitle
        {
            get => lbArtist.Text;
            set => lbArtist.Text = value;
        }

        public BitmapImage Thumbnail
        {
            get => (BitmapImage)imgThumbnail.Source;
            set => imgThumbnail.Source = value;
        }

        public async void SetThumbnailAsync(string url)
        {
            Thumbnail = await Util.WebImage.GetImage(url);
        }

        public string Length
        {
            get => lbDuration.Content.ToString();
            set => lbDuration.Content = value;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(this, new EventArgs());
        }
    }
}
