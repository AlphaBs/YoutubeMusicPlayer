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
using CefSharp.WinForms;
using YMP.Model;

namespace YMP.View.Pages
{
    /// <summary>
    /// PlayerPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            InitializeComponent();
            this.cefBrowser.AttachChild(YMPCore.Browser.InitializeChromiumBrowser());

            SetBtnRepeatIconKind();
        }

        public event EventHandler BackEvent;

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, new EventArgs());
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (YMPCore.Browser.Repeat)
                YMPCore.Browser.Repeat = false;
            else
                YMPCore.Browser.Repeat = true;

            SetBtnRepeatIconKind();
        }

        private void SetBtnRepeatIconKind()
        {
            if (YMPCore.Browser.Repeat)
                btnRepeatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.RepeatOff;
            else
                btnRepeatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Repeat;
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        System.Windows.Forms.Form desktopForm;
        private void btnDesktop_Click(object sender, RoutedEventArgs e)
        {
            if (desktopForm == null)
            {
                desktopForm = DesktopManager.CreateDesktopForm(YMPCore.Browser.Browser, 0);
                DesktopManager.SetDesktopChildForm(desktopForm);
            }
            else
            {
                this.cefBrowser.AttachChild(YMPCore.Browser.Browser);
                desktopForm.Close();
                desktopForm = null;
            }
        }
    }
}
