using Newtonsoft.Json;
using System;
using System.IO;
using log4net;

namespace YMP.Model
{
    public class Setting
    {
        private static ILog log = LogManager.GetLogger("Setting");

        #region Setting IO

        private Setting() { }

        public static Setting LoadSetting(string path)
        {
            log.Info("Loading Setting " + path);

            Setting setting;
            try
            {
                if (File.Exists(path))
                {
                    log.Info("Deserialilzing Setting");
                    setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(path));
                }
                else
                    setting = new Setting();
            }
            catch (Exception ex)
            {
                log.Info(ex);
                setting = new Setting();
            }

            setting.SettingPath = path;
            return setting;
        }

        public void SaveSetting()
        {
            log.Info("Saving Settings");

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
