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

        public void Search(string q)
        {
            if (Searching)
                return;

            stkList.Children.Clear();
            Searching = true;
            var th = new Thread(() =>
            {
                ytSearch(q, "");
            });
            th.Start();
        }

        void ytSearch(string q, string pagetoken)
        {
            var r = YMPCore.Youtube.Search(q, pagetoken);

            Dispatcher.Invoke(() =>
            {
                foreach (var item in r)
                {
                    var c = new SearchListItem();
                    c.Music = item;

                    c.ClickEvent += C_ClickEvent;
                    stkList.Children.Add(c);
                }

                Searching = false;
            });
        }

        private void C_ClickEvent(object sender, EventArgs e)
        {
            var ctr = sender as SearchListItem;
            if (ctr == null)
                return;

            YMPCore.Browser.PlayMusic(ctr.Music);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, new EventArgs());
        }
    }
}
