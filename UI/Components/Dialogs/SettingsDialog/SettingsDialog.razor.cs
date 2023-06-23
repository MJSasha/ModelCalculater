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
            if (Params?.AppSettings != null) Params.AppSettings.OnLanguageChanged += SelectLocalization;
        }

        protected override void OnBeforeClose()
        {
            if (Params?.AppSettings != null) Params.AppSettings.OnLanguageChanged -= SelectLocalization;
        }

        private void SelectLocalization()
        {
            localization = Params.AppSettings.Language.GetLocalization();
            StateHasChanged();
        }
    }
}
