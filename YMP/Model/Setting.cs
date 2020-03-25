using Newtonsoft.Json;
using System;
using System.IO;

namespace YMP.Model
{
    public class Setting
    {
        #region Setting IO

        private Setting() { }

        public static Setting LoadSetting(string path)
        {
            Setting setting;
            try
            {
                if (File.Exists(path))
                    setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(path));
                else
                    setting = new Setting();
            }
            catch (Exception ex)
            {
                setting = new Setting();
            }

            setting.SettingPath = path;
            return setting;
        }

        public void SaveSetting()
        {
            string contents = JsonConvert.SerializeObject(this);
            Directory.CreateDirectory(Path.GetDirectoryName(this.SettingPath));
            File.WriteAllText(this.SettingPath, contents);
        }

        #endregion

        [JsonIgnore]
        public string SettingPath { get; private set; }

        [JsonProperty]
        public bool AutoSwitchPlayer { get; set; } = true;
        [JsonProperty]
        public int DefaultBrowser { get; set; } = (int) BrowserControllerKind.FrameAPI;
    }
}
