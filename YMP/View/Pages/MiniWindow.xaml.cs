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
using System.Windows.Shapes;
using YMP.Model;

namespace YMP.View.Pages
{
    /// <summary>
    /// MiniWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MiniWindow : Window
    {
        public MiniWindow()
        {
            InitializeComponent();
        }

        public event EventHandler ExitProgram;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExitProgram?.Invoke(this, new EventArgs());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}