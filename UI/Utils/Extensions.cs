using ModelCalculater.DEfinitions;
using System.Reflection;
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
            var folder = LocalizationService.CurrentLanguage.GetFolderName();

			return taskType switch
            {
                TaskType.NoSolution => $"{folder}/no-solutions.mp3",
                TaskType.Estimated => $"{folder}/estimated.mp3",
                TaskType.Optimization => $"{folder}/optimization.mp3",
                _ => throw new NotImplementedException(),
            };
        }

        public static ILocalization GetLocalization(this Language language)
        {
            return language switch
            {
                Language.Russian => new RuLocalization(),
                Language.English => new EnLocalization(),
                Language.Chinese => new ChLocalization(),
                _ => throw new NotImplementedException(),
            };
        }

        public static string GetName(this Language language) => language.GetType()?
            .GetMember(language.ToString())?
            .First()?
            .GetCustomAttribute<LocalizationPropsAttribute>()?
            .DisplayName ?? nameof(language);

        public static string GetFolderName(this Language language) => language.GetType()?
            .GetMember(language.ToString())?
            .First()?
            .GetCustomAttribute<LocalizationPropsAttribute>()?
            .FolderName ?? nameof(language);

        public static string GetName(this FormationProcedureType procedureType)
        {
            return LocalizationService.Localization.GetName(procedureType);
        }
    }
}
