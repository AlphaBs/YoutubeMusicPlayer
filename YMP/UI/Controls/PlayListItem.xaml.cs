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

namespace YMP.UI.Controls
{
    /// <summary>
    /// PlayListItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayListItem : UserControl
    {
        public PlayListItem()
        {
            InitializeComponent();
        }

        public event EventHandler Click;

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
