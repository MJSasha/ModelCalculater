using System.Text.Json.Serialization;
using UI.Data;

namespace UI
{
    public class AppSettings
    {
        public bool PalySound { get; set; } = true;
        public Language Language { get => language; set { language = value; OnLanguageChanged?.Invoke(); } }

        [JsonIgnore]
        public Action OnLanguageChanged { get; set; }

        private Language language = Language.English;
	}
}
