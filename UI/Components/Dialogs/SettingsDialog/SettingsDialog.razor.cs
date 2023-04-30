using UI.Localization;
using UI.Utils;

namespace UI.Components.Dialogs.SettingsDialog
{
    public partial class SettingsDialog
    {
        private ILocalization localization;

        protected override void OnShow()
        {
            SelectLocalization();
            if (Parameters?.AppSettings != null) Parameters.AppSettings.OnLanguageChanged += SelectLocalization;
        }

        protected override void OnClose()
        {
            if (Parameters?.AppSettings != null) Parameters.AppSettings.OnLanguageChanged -= SelectLocalization;
        }

        private void SelectLocalization()
        {
            localization = Parameters.AppSettings.Language.GetLocalization();
            StateHasChanged();
        }
    }
}
