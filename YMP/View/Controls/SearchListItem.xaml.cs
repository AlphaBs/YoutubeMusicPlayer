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

        public string Title
        {
            get => lbTitle.Text;
            set => lbTitle.Text = value.Replace("_", "__");
        }

        public string Subtitle
        {
            get => lbSubtitle.Text;
            set => lbSubtitle.Text = value.Replace("_", "__");
        }

        public BitmapImage Thumbnail
        {
            get => (BitmapImage)imgThumbnail.Source;
            set => imgThumbnail.Source = value;
        }
    }
}
