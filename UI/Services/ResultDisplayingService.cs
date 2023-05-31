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
                Title = LocalizationService.Localization.TaskResultDisplay_EnterTaskTitle,
                ShowCriteriaSelector = procedureType == FormationProcedureType.ComputationalModel,
                ShowTwoInputTasks = procedureType == FormationProcedureType.InformationLinks,
                ColumnsNames = matrix.Keys.ToList(),
            });

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
            var matrixWithoutAnyColumns = GetMatrixWithoutAnyColumns(matrix, result.GivenValues);

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
            List<string> excludedColumnsNames;
            List<string> toFind;
            Matrix matrixWithoutAnyColumns;
            if (result.Criteria.Any())
            {
                var criteria = result.Criteria.First();
                result.Criteria.Remove(criteria);
                toFind = new() { criteria };

                excludedColumnsNames = result.GivenValues.Concat(result.ValuesToFind.Concat(result.Criteria)).ToList();
                matrixWithoutAnyColumns = GetMatrixWithoutAnyColumns(matrix, excludedColumnsNames);
            }
            else
            {
                excludedColumnsNames = result.GivenValues;
                toFind = result.ValuesToFind;
                matrixWithoutAnyColumns = GetMatrixWithoutAnyColumns(matrix, result.GivenValues);
            }

            var canCreateCalculationModel = matrixWithoutAnyColumns.CheckPossibilityOfFormingCalculation();
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = string.Format(canCreateCalculationModel ? LocalizationService.Localization.TaskResultDisplay_TaskIsCorrect_FormattedText : LocalizationService.Localization.TaskResultDisplay_TaskIsIncorrect_FormattedText,
                string.Join(", ", excludedColumnsNames),
                string.Join(", ", toFind)),
            });
        }

        private async Task ShowInformationLinksResult(Dictionary<string, List<int>> matrix, EnteredTaskResult result)
        {
            var matrixWithoutAnyColumns = GetMatrixWithoutAnyColumns(matrix, result.GivenValues);
            var haveInformationLinks = matrixWithoutAnyColumns.CheckForInformationLinks();
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = haveInformationLinks ? LocalizationService.Localization.TaskResultDisplay_LinksTakesPlace : LocalizationService.Localization.TaskResultDisplay_NoLinks,
            });
        }

        private async Task ShowInformationAboutModel(Dictionary<string, List<int>> matrix, EnteredTaskResult result)
        {
            var matrixWithoutAnyColumns = GetMatrixWithoutAnyColumns(matrix, result.GivenValues);
            await dialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
            {
                Title = LocalizationService.Localization.TaskResultDisplay_Result_ModalTitle,
                Message = string.Format(LocalizationService.Localization.TaskResultDisplay_ModelInformation_FormattedText,
                    matrixWithoutAnyColumns.Deficit,
                    string.Join(", ", matrixWithoutAnyColumns.LinesWithDeficit.Select(i => i + 1)))
            });
        }

        private Matrix GetMatrixWithoutAnyColumns(Dictionary<string, List<int>> matrix, IEnumerable<string> columnsToExclude)
        {
            return new Matrix(matrix.Where(m => !columnsToExclude.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));
        }
    }
}
