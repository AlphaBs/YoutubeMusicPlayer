using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace YMP.Model
{
    public class PlayListManager
    {
        public PlayList CurrentPlayList { get; set; }
        public PlayList RecentPlayList { get; private set; }
        public List<PlayList> PlayLists { get; private set; }

        public List<PlayList> LoadAllPlayLists()
        {
            var files = new DirectoryInfo(YMPInfo.PlaylistPath).GetFiles();
            PlayLists = new List<PlayList>(files.Length);

            foreach (var item in files)
            {
                try
                {
                    var filecontent = File.ReadAllText(item.FullName, Encoding.UTF8);
                    var obj = JsonConvert.DeserializeObject<PlayList>(filecontent);

                    if (obj.Name != item.Name)
                        continue;

                    if (obj.Type == "recent")
                        RecentPlayList = obj;

                    PlayLists.Add(obj);
                }
                catch (JsonSerializationException jex)
                {

                }
            }

            return PlayLists;
        }

        public void SavePlayList(PlayList list)
        {
            var path = Path.Combine(YMPInfo.PlaylistPath, list.Name);
            var content = JsonConvert.SerializeObject(list);

            File.WriteAllText(path, content, Encoding.UTF8);
        }

        public void SaveAllPlayLists()
        {
            foreach (var item in PlayLists)
            {
                SavePlayList(item);
            }
        }

        public PlayList CreateNewPlaylist(string name)
        {
            var obj = new PlayList(name, "", new Music[0], new PlayListMetadata());
            PlayLists.Add(obj);
            return obj;
        }

        public PlayList GetPlayList(int index)
        {
            return PlayLists[index];
        }

        public PlayList GetPlayList(string name)
        {
            foreach (var item in PlayLists)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public void AddPlayList(PlayList pl)
        {
            PlayLists.Add(pl);
        }

        public void RemovePlayList(int index)
        {
            PlayLists.RemoveAt(index);

            var path = Path.Combine(YMPInfo.PlaylistPath, PlayLists[index].Name);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void RemovePlayList(string name)
        {
            PlayLists.Remove(GetPlayList(name));

            var path = Path.Combine(YMPInfo.PlaylistPath, name);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
