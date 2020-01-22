using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Core
{
    public class PlayList
    {
        public PlayList(string name, Music[] musics)
        {
            this.Name = name;
            this.Musics = musics;
            CurrentMusicIndex = 0;
            Lenght = musics.Length;
        }

        public string Name { get; private set; }

        public int Lenght { get; private set; }
        public int CurrentMusicIndex { get; private set; }
        public Music[] Musics { get; private set; }

        public Music GetCurrentusic()
        {
            return Musics[CurrentMusicIndex];
        }

        public Music GetNextMusic()
        {
            if (CurrentMusicIndex >= Lenght)
                CurrentMusicIndex = 0;
            else
                CurrentMusicIndex++;

            return Musics[CurrentMusicIndex];
        }

        public Music GetPreviousMusic()
        {
            if (CurrentMusicIndex <= 0)
                CurrentMusicIndex = Lenght - 1;
            else
                CurrentMusicIndex--;

            return Musics[CurrentMusicIndex];
        }
    }
}
