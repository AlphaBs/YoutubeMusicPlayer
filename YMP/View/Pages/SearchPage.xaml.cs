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
        }

        public event EventHandler BackEvent;
        bool searching = false;

        public void Search(string q)
        {
            if (searching)
                return;

            stkList.Children.Clear();
            var th = new Thread(() =>
            {
                searching = true;
                ytSearch(q, "");
                searching = false;
            });
            th.Start();
        }

        void ytSearch(string q, string pagetoken)
        {
            var r = YMPCore.Youtube.Search(q, pagetoken);

            Dispatcher.Invoke(() =>
            {
                foreach (var item in r.Items)
                {
                    var c = new SearchListItem();
                    c.Title = WebUtility.HtmlDecode(item.Snippet.Title);
                    c.Channel = item.Snippet.ChannelTitle;

                    if (item.Snippet.PublishedAt != null)
                        c.Info = StringFormat.ToTimeSpanString(item.Snippet.PublishedAt ?? DateTime.Now);

                    var th = item.Snippet.Thumbnails.Medium;
                    c.ThumbnailUrl = th.Url;

                    c.ClickEvent += C_ClickEvent;
                    stkList.Children.Add(c);
                }
            });
        }

        private void C_ClickEvent(object sender, EventArgs e)
        {
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, new EventArgs());
        }
    }
}
