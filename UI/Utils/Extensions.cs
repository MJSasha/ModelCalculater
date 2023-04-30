using ModelCalculater.DEfinitions;
using UI.Data;
using UI.Localization;
using UI.Services;

namespace UI.Utils
{
    public static class Extensions
    {
        public static string GetName(this TaskType taskType)
        {
            return LocalizationService.Localization.GetName(taskType);
		}

        public static string GetSoundName(this TaskType taskType)
        {
            var folder = LocalizationService.CurrentLanguage switch
            {
                Language.Russian => "ru",
                Language.English => "en",
                _ => throw new NotImplementedException(),
            };

			return taskType switch
            {
                TaskType.NoSolution => $"{folder}/no-solutions.mp3",
                TaskType.Estimated => $"{folder}/estimated.mp3",
                TaskType.Optimization => $"{folder}/optimization.mp3",
                _ => throw new NotImplementedException(),
            };
        }

        public static string GetName(this Language language)
        {
            return language switch
            {
                Language.Russian => "Русский",
                Language.English => "English",
                _ => throw new NotImplementedException(),
            };
        }

        public static ILocalization GetLocalization(this Language language)
        {
            return language switch
            {
                Language.Russian => new RuLocalization(),
                Language.English => new EnLocalization(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
