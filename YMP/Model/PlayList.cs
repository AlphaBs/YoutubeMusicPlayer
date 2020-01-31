using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMP.Model
{
    public class PlayList
    {
        public PlayList(string name, string type, Music[] musics, PlayListMetadata md)
        {
            this.Name = name;
            this.Musics = new List<Music>(musics.Length);

            foreach (var item in musics)
            {
                AddMusic(item);
            }

            this.Type = type;
            this.Metadata = md;
        }

        public PlayListMetadata Metadata { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public int Lenght { get => Musics.Count; }
        public int CurrentMusicIndex { get; set; } = 0;
        public List<Music> Musics { get; private set; }

        public Music GetCurrentusic()
        {
            return Musics[CurrentMusicIndex];
        }

        public Music GetNextMusic()
        {
            if (CurrentMusicIndex >= Lenght - 1)
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

        public void AddMusic(Music music)
        {
            if (music.AddDate == null)
                music.AddDate = DateTime.Now;
            Musics.Add(music);
        }

        public void RemoveMusic(Music music)
        {
            Musics.Remove(music);
        }
    }
}
 