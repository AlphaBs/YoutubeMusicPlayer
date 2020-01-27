using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using YMP.Command;
using YMP.Model;
using YMP.Util;
using YMP.View;
using YMP.View.Pages;

namespace YMP.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            musicList = new MusicListPage();
            playerPage = new PlayerPage();
            searchPage = new SearchPage();
            timer = new DispatcherTimer();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        MusicListPage musicList;
        PlayerPage playerPage;
        SearchPage searchPage;

        DispatcherTimer timer;

        FrameContent _frameContent = FrameContent.Blank;
        FrameContent PreviousContent = FrameContent.Blank;
        public FrameContent CurrentContent
        {
            get => _frameContent;
            set
            {
                if (_frameContent != value)
                {
                    PreviousContent = _frameContent;
                    _frameContent = value;

                    switch (value)
                    {
                        case FrameContent.MusicListPage:
                            DisplayPage = musicList;
                            break;
                        case FrameContent.SearchPage:
                            DisplayPage = searchPage;
                            break;
                        case FrameContent.PlayerPage:
                            DisplayPage = playerPage;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void SetPreviousPage()
        {
            CurrentContent = PreviousContent;
        }

        Page _displayPage;
        public Page DisplayPage
        {
            get => _displayPage;
            set
            {
                if (!Equals(_displayPage, value))
                {
                    _displayPage = value;
                    RaiseChanged(nameof(DisplayPage));
                }
            }
        }

        bool isUserSliding = false;

        TimeSpan _currentTime;
        TimeSpan _duration;
        public TimeSpan CurrentTime
        {
            get => _currentTime;
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;

                    if (!isUserSliding)
                    {
                        RaiseChanged(nameof(CurrentTimeInt));
                        RaiseChanged(nameof(Position));
                    }
                }
            }
        }
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;

                    if (!isUserSliding)
                    {
                        RaiseChanged(nameof(DurationInt));
                        RaiseChanged(nameof(Position));
                    }
                }
            }
        }

        public string Position
        {
            get => $"{StringFormat.ToDurationString(_currentTime)} / {StringFormat.ToDurationString(_duration)}";
        }

        public int CurrentTimeInt
        {
            get => (int)_currentTime.TotalSeconds;
        }

        public int DurationInt
        {
            get => (int)_duration.TotalSeconds;
        }

        BitmapImage _thumbnail;
        public BitmapImage Thumbnail
        {
            get => _thumbnail;
            set
            {
                if (_thumbnail != value)
                {
                    _thumbnail = value;
                    RaiseChanged(nameof(Thumbnail));
                }
            }
        }

        string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaiseChanged(nameof(Title));
                }
            }
        }

        string _subtitle;
        public string Subtitle
        {
            get => _subtitle;
            set
            {
                if (_subtitle != value)
                {
                    _subtitle = value;
                    RaiseChanged(nameof(Subtitle));
                }
            }
        }

        private void RaiseChanged(string pn)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pn));
        }

        public void OnClickBrowser()
        {
            CurrentContent = FrameContent.PlayerPage;
        }

        ICommand prevCommand;
        public ICommand PrevCommand
        {
            get => prevCommand ?? (prevCommand = new RelayCommand(PrevMusic));
        }

        private void PrevMusic(object o)
        {
            var p = YMPCore.PlayList.CurrentPlayList.GetPreviousMusic();
            YMPCore.Browser.PlayMusic(p);
        }

        ICommand nextCommand;
        public ICommand NextCommand
        {
            get => nextCommand ?? (nextCommand = new RelayCommand(NextMusic));
        }

        private void NextMusic(object o)
        {
            var p = YMPCore.PlayList.CurrentPlayList.GetNextMusic();
            YMPCore.Browser.PlayMusic(p);
        }

        ICommand playpauseCommand;
        public ICommand PlayPauseCommand
        {
            get => playpauseCommand ?? (playpauseCommand = new RelayCommand(PlayPauseMusic));
        }

        private void PlayPauseMusic(object o)
        {
            var state = YMPCore.Browser.State;
            if (state == PlayerState.Playing)
                YMPCore.Browser.Pause();
            else
                YMPCore.Browser.Play();
        }

        public void SliderDragStarted(object o)
        {
            isUserSliding = true;
        }

        public void SliderDragCompleted(int o)
        {
            YMPCore.Browser.SeekTo(o);
            isUserSliding = false;
        }

        ICommand loadedCommand;
        public ICommand LoadedCommand
        {
            get => loadedCommand ?? (loadedCommand = new RelayCommand(LoadedWindow));
        }

        private void LoadedWindow(object o)
        {
            CurrentContent = FrameContent.MusicListPage;
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = YMPCore.Browser.CurrentTime;
            Duration = YMPCore.Browser.Duration;
        }
    }
}
