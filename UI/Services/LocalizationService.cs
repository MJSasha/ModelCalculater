using UI.Data;
using UI.Localization;
using UI.Utils;

namespace UI.Services
{
	public static class LocalizationService
	{
		public static ILocalization Localization { get; private set; }
		public static Language CurrentLanguage { get; private set; }

		private static AppSettings _appSettings;

		public static void Init(AppSettings appSettings)
		{
			_appSettings = appSettings;
			_appSettings.OnLanguageChanged += SelectLocalization;
			SelectLocalization();
		}

		private static void SelectLocalization()
		{
			CurrentLanguage = _appSettings.Language;
			Localization = _appSettings.Language.GetLocalization();
		}
	}
}
