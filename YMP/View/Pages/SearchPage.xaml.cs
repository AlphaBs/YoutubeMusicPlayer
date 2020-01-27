﻿using System;
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

            var r = YMPCore.Youtube.Search("IZONE", "");
            foreach (var item in r.Items)
            {
                var i = new SearchListItem();
                i.Title = item.Snippet.Title;
                i.Subtitle = item.Snippet.ChannelTitle;
                stkList.Children.Add(i);
            }
        }
    }
}
