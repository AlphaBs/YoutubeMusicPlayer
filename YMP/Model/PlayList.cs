using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace YMP.Model
{
    public class PlayList
    {
        private static ILog log = LogManager.GetLogger("PlayList");

        public PlayList(string name, string type, Music[] musics, int count, PlayListMetadata md)
        {
            this.Name = name;
            this.Musics = new List<Music>(count);
            Lenght = count;

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
        public int Lenght { get; private set; }
        [JsonIgnore]
        public int LoadedMusicIndex { get => Musics.Count - 1; }
        [JsonIgnore]
        public Action<PlayList> NextLoadFunc { get; set; }
        [JsonIgnore]
        public int CurrentMusicIndex { get; private set; } = 0;

        public Music GetMusic(int index)
        {
            CurrentMusicIndex = index;
            while (index > LoadedMusicIndex)
            {
                log.Info($"index out of LoadedMusicIndex : {index}/{LoadedMusicIndex}");
                if (!LoadNextMusics())
                    return null;
            }

            log.Info("GetMusic : " + index);
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

            return GetMusic(CurrentMusicIndex);
        }

        public Music GetPreviousMusic()
        {
            if (CurrentMusicIndex <= 0)
                CurrentMusicIndex = Lenght - 1;
            else
                CurrentMusicIndex--;

            return GetMusic(CurrentMusicIndex);
        }

        public void AddMusic(Music music)
        {
            if (music.AddDate == null)
                music.AddDate = DateTime.Now;
            Musics.Add(music);

            if (LoadedMusicIndex >= Lenght)
                Lenght++;
        }

        public void RemoveMusic(Music music)
        {
            Musics.Remove(music);

            if (LoadedMusicIndex <= Lenght)
                Lenght--;
        }

        public void RemoveMusic(int index)
        {
            Musics.RemoveAt(index);

            if (LoadedMusicIndex <= Lenght)
                Lenght--;
        }

        private bool LoadNextMusics()
        {
            if (NextLoadFunc == null)
                return false;

            log.Info("Calling NextLoadFunc");
            NextLoadFunc(this);
            return true;
        }
    }
}
 