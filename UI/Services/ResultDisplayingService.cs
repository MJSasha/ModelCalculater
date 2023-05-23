using ModelCalculater.Models;
using UI.Components.Dialogs.EnterTaskDialog;
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

        public async Task ShowResult(Dictionary<string, List<int>> matrix, FormationProcedureType procedureType)
        {
            var result = await dialogService.Show<EnterTaskDialog, EnterTaskDialogParams, EnteredTaskResult>(new EnterTaskDialogParams()
            {
                Title = "Test",
                ShowCriteriaSelector = procedureType == FormationProcedureType.ComputationalModel,
                ColumnsNames = matrix.Keys.ToList(),
            });

            //var matrixWithoutAnyColumns = new Matrix(matrix.Where(m => !definedVariables.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));

            await (procedureType switch
            {
                FormationProcedureType.Status => ShowStatusResult(matrix, result),
                FormationProcedureType.ComputationalModel => ShowComputationalModelResult(matrix, result),
                FormationProcedureType.InformationLinks => ShowInformationLinksResult(matrix, result),
                FormationProcedureType.InformationAboutModel => ShowInformationAboutModel(matrix, result),
            });
        }

        private async Task ShowStatusResult(Dictionary<string, List<int>> matrix, EnteredTaskResult result)
        {
            var matrixWithoutAnyColumns = new Matrix(matrix.Where(m => !result.GivenValues.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));

            var taskType = matrixWithoutAnyColumns.GetTaskType();
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = taskType.GetName(),
                SoundName = taskType.GetSoundName(),
            });
        }

        private async Task ShowComputationalModelResult(Dictionary<string, List<int>> matrix, EnteredTaskResult result)
        {
            Matrix matrixWithoutAnyColumns;
            if (result.Criteria.Any())
            {
                var criteria = result.Criteria.First();
                result.Criteria.Remove(criteria);

                var excludedColumnsNames = result.GivenValues.Concat(result.ValuesToFind.Concat(result.Criteria));

                matrixWithoutAnyColumns = new Matrix(matrix.Where(m => !excludedColumnsNames.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));
            }
            else
            {
                matrixWithoutAnyColumns = new Matrix(matrix.Where(m => !result.GivenValues.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));
            }

            var canCreateCalculationModel = matrixWithoutAnyColumns.CheckPossibilityOfFormingCalculation();
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = canCreateCalculationModel ? LocalizationService.Localization.TaskResultDisplay_TaskIsCorrect : LocalizationService.Localization.TaskResultDisplay_TaskIsIncorrect,
            });
        }

        private async Task ShowInformationLinksResult(Dictionary<string, List<int>> matrix, EnteredTaskResult result)
        {
            var matrixWithoutAnyColumns = new Matrix(matrix.Where(m => !result.GivenValues.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));
            var haveInformationLinks = matrixWithoutAnyColumns.CheckForInformationLinks();
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = haveInformationLinks ? LocalizationService.Localization.TaskResultDisplay_LinksTakesPlace : LocalizationService.Localization.TaskResultDisplay_NoLinks,
            });
        }

        private async Task ShowInformationAboutModel(Dictionary<string, List<int>> matrix, EnteredTaskResult result)
        {
            var matrixWithoutAnyColumns = new Matrix(matrix.Where(m => !result.GivenValues.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = string.Format(LocalizationService.Localization.TaskResultDisplay_ModelInformation_FormattedText, matrixWithoutAnyColumns.Deficit, string.Join(", ", matrixWithoutAnyColumns.LinesWithDeficit.Select(i => i + 1)))
            });
        }
    }
}
