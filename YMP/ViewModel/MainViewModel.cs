using MaterialDesignThemes.Wpf;
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
            playerPage.BackEvent += delegate
            {
                SetPreviousPage();
            };

            searchPage = new SearchPage();
            searchPage.BackEvent += delegate
            {
                CurrentContent = FrameContent.MusicListPage;
            };

            settingPage = new SettingPage();
            settingPage.BackEvent += delegate
            {
                SetPreviousPage();
            };

            timer = new DispatcherTimer();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        MusicListPage musicList;
        PlayerPage playerPage;
        SearchPage searchPage;
        SettingPage settingPage;

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
                            musicList.UpdateList();
                            DisplayPage = musicList;
                            break;
                        case FrameContent.SearchPage:
                            DisplayPage = searchPage;
                            break;
                        case FrameContent.PlayerPage:
                            DisplayPage = playerPage;
                            break;
                        case FrameContent.SettingPage:
                            DisplayPage = settingPage;
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
        bool isTimeUpdated = false;

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
            set => CurrentTime = TimeSpan.FromSeconds(value);
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

        string _searchQ;
        public string SearchQuery
        {
            get => _searchQ;
            set
            {
                if (_searchQ != value)
                {
                    _searchQ = value;
                    RaiseChanged(nameof(SearchQuery));
                }
            }
        }

        PackIconKind _kind = PackIconKind.PlayArrow;
        public PackIconKind PlayPauseButtonIconKind
        {
            get => _kind;
            set
            {
                if (_kind != value)
                {
                    _kind = value;
                    RaiseChanged(nameof(PlayPauseButtonIconKind));
                }
            }
        }

        private void RaiseChanged(string pn)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pn));
        }

        public void OnClickBrowser()
        {
            if (CurrentContent != FrameContent.SettingPage)
                CurrentContent = FrameContent.PlayerPage;
        }

        #region ICommands

        ICommand prevCommand;
        public ICommand PrevCommand
        {
            get => prevCommand ?? (prevCommand = new RelayCommand(PrevMusic));
        }

        ICommand nextCommand;
        public ICommand NextCommand
        {
            get => nextCommand ?? (nextCommand = new RelayCommand(NextMusic));
        }

        ICommand playpauseCommand;
        public ICommand PlayPauseCommand
        {
            get => playpauseCommand ?? (playpauseCommand = new RelayCommand(PlayPauseMusic));
        }

        ICommand dragStartedCommand;
        public ICommand DragStartedCommand
        {
            get => dragStartedCommand ?? (dragStartedCommand = new RelayCommand(SliderDragStarted));
        }

        ICommand dragCompletedCommand;
        public ICommand DragCompletedCommand
        {
            get => dragCompletedCommand ?? (dragCompletedCommand = new RelayCommand(SliderDragCompleted));
        }

        ICommand loadedCommand;
        public ICommand LoadedCommand
        {
            get => loadedCommand ?? (loadedCommand = new RelayCommand(LoadedWindow));
        }

        ICommand searchCommand;
        public ICommand SearchCommand
        {
            get => searchCommand ?? (searchCommand = new RelayCommand(SearchClick));
        }

        ICommand settingCommand;
        public ICommand OnSettingClickCommand
        {
            get => settingCommand ?? (settingCommand = new RelayCommand(OnSettingClick));
        }

        #endregion

        private void PrevMusic(object o)
        {
            if (YMPCore.PlayList.CurrentPlayList == null)
                return;

            var p = YMPCore.PlayList.CurrentPlayList.GetPreviousMusic();
            YMPCore.Browser.PlayMusic(p);
        }

        private void NextMusic(object o)
        {
            if (YMPCore.PlayList.CurrentPlayList == null)
                return;

            var p = YMPCore.PlayList.CurrentPlayList.GetNextMusic();
            YMPCore.Browser.PlayMusic(p);
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

        public void SliderDragCompleted(object o)
        {
            YMPCore.Browser.SeekTo(CurrentTimeInt);
            isTimeUpdated = true;
            isUserSliding = false;
        }

        public void SearchClick(object o)
        {
            CurrentContent = FrameContent.SearchPage;
            searchPage.Search(SearchQuery);
        }

        public void OnSettingClick(object o)
        {
            CurrentContent = FrameContent.SettingPage;
        }

        public void LoadedWindow(object o)
        {
            CurrentContent = FrameContent.MusicListPage;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (YMPCore.Browser.CurrentMusic != null)
            {
                Title = YMPCore.Browser.CurrentMusic.Title;
                Subtitle = YMPCore.Browser.CurrentMusic.Artists;
                setThumbnail(YMPCore.Browser.CurrentMusic.Thumbnail);
            }

            if (!isTimeUpdated)
            {
                if (!isUserSliding)
                {
                    CurrentTime = YMPCore.Browser.CurrentTime;
                    Duration = YMPCore.Browser.Duration;
                }
            }
            else
                isTimeUpdated = false;

            if (YMPCore.Browser.State == PlayerState.Playing)
                PlayPauseButtonIconKind = PackIconKind.Pause;
            else
                PlayPauseButtonIconKind = PackIconKind.PlayArrow;
        }

        private async void setThumbnail(string url)
        {
            Thumbnail = await WebImage.GetImage(url);
        }
    }
}
