using Newtonsoft.Json;
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

        [JsonProperty]
        public PlayListMetadata Metadata { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string Type { get; private set; }
        [JsonProperty]
        private List<Music> Musics { get; set; }

        [JsonIgnore]
        public int Lenght { get => Musics.Count; }
        [JsonIgnore]
        public int CurrentMusicIndex { get; private set; } = 0;

        public Music GetMusic(int index)
        {
            CurrentMusicIndex = index;
            return Musics[index];
        }

        public Music[] GetMusics()
        {
            return Musics.ToArray();
        }

        public Music GetCurrentMusic()
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
 