using BlazorModalDialogs.Components;
using UI.Localization;
using UI.Utils;

namespace UI.Components.Dialogs.SettingsDialog
{
    public partial class SettingsDialog : Dialog<SettingsDialogParams, AppSettings>
    {
        private ILocalization localization;

        protected override void OnAfterShow()
        {
            SelectLocalization();
            if (Parameters?.AppSettings != null) Parameters.AppSettings.OnLanguageChanged += SelectLocalization;
        }

        protected override void OnBeforeClose()
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
