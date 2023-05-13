using ModelCalculater;
using ModelCalculater.Models;
using UI.Components.Dialogs.MessageDialog;
using UI.Data;
using UI.Utils;

namespace UI.Services
{
    public class ResultDisplayingService
    {
        private readonly DialogService dialogService;

        public ResultDisplayingService(DialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public Task ShowResult(Matrix matrix, FormationProcedureType procedureType)
        {
            return procedureType switch
            {
                FormationProcedureType.Status => ShowStatusResult(matrix),
                FormationProcedureType.ComputationalModel => ShowComputationalModelResult(matrix),
                FormationProcedureType.InformationLinks => ShowInformationLinksResult(matrix),
            };
        }

        private async Task ShowStatusResult(Matrix matrix)
        {
            var result = Calculater.GetTaskType(matrix);
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = result.GetName(),
                SoundName = result.GetSoundName(),
            });
        }

        private async Task ShowComputationalModelResult(Matrix matrix)
        {
            var canCreateCalculationModel = Calculater.CheckPossibilityOfFormingCalculationModel(matrix);
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = canCreateCalculationModel ? LocalizationService.Localization.TaskResultDisplay_TaskIsCorrect : LocalizationService.Localization.TaskResultDisplay_TaskIsIncorrect,
            });
        }

        private async Task ShowInformationLinksResult(Matrix matrix)
        {
            var haveInformationLinks = Calculater.CheckForInformationLinks(matrix);
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = haveInformationLinks ? LocalizationService.Localization.TaskResultDisplay_LinksTakesPlace : LocalizationService.Localization.TaskResultDisplay_NoLinks,
            });
        }
    }
}
