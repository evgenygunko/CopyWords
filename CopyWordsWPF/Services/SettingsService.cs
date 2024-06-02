namespace CopyWordsWPF.Services
{
    public interface ISettingsService
    {
        string GetAnkiSoundsFolder();

        void SetAnkiSoundsFolder(string path);

        string GetMp3gainPath();

        void SetMp3gainPath(string path);

        bool UseMp3gain { get; set; }

        bool UseSlovardk { get; set; }

        string GetDanRusDictionaryFolder();

        void SetDanRusDictionaryFolder(string path);

        void Save();
    }

    public class SettingsService : ISettingsService
    {
        public bool UseMp3gain
        {
            get => Properties.Settings.Default.UseMp3gain;
            set => Properties.Settings.Default.UseMp3gain = value;
        }

        public bool UseSlovardk
        {
            get => Properties.Settings.Default.UseSlovardk;
            set => Properties.Settings.Default.UseSlovardk = value;
        }

        public string GetAnkiSoundsFolder() => Properties.Settings.Default.AnkiSoundsFolder;

        public void SetAnkiSoundsFolder(string path) => Properties.Settings.Default.AnkiSoundsFolder = path;

        public string GetMp3gainPath() => Properties.Settings.Default.Mp3gainPath;

        public void SetMp3gainPath(string path) => Properties.Settings.Default.Mp3gainPath = path;

        public string GetDanRusDictionaryFolder() => Properties.Settings.Default.DanRusDictionaryFolder;

        public void SetDanRusDictionaryFolder(string path) => Properties.Settings.Default.DanRusDictionaryFolder = path;

        public void Save() => Properties.Settings.Default.Save();
    }
}
