using Microsoft.AspNetCore.Components;
using ModelCalculater.Models;
using UI.Components.Dialogs.InputDialog;
using UI.Components.Dialogs.MessageDialog;
using UI.Components.Dialogs.ProcedureTypeSelectorDialog;
using UI.Data;
using UI.Services;

namespace UI.Pages
{
    public partial class MatrixPage : IDisposable
    {
        [Inject]
        public DialogService DialogService { get; set; }

        [Inject]
        public MatrixActionsService MatrixActionsService { get; set; }

        [Inject]
        public ResultDisplayingService ResultDisplayingService { get; set; }

        private Dictionary<string, List<int>> matrix = new();
        private Matrix matrixCache;
        private List<string> definedVariables = new();
        private List<string> requiredVariables = new();

        private bool isRedacted = false;
        private bool matrixChanged = false;


        public void Dispose()
        {
            MatrixActionsService.OnClearPressed -= OnClearPressed;
            MatrixActionsService.OnRedactionPressed -= OnRedactButtonPressed;
            MatrixActionsService.OnCalculatePressed -= OnCalculatePressed;
        }

        protected override void OnInitialized()
        {
            MatrixActionsService.OnClearPressed += OnClearPressed;
            MatrixActionsService.OnRedactionPressed += OnRedactButtonPressed;
            MatrixActionsService.OnCalculatePressed += OnCalculatePressed;
        }

        private void AddRow()
        {
            foreach (var col in matrix)
            {
                col.Value.Add(0);
            }

            matrixChanged = true;
            StateHasChanged();
        }

        private async void AddColumn()
        {
            try
            {
                var columnName = await DialogService.Show<InputDialog, InputDialogParams, string>(new InputDialogParams { Title = LocalizationService.Localization.MatrixPage_EnterColumnName_ModalTitle });
                
                if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentException();

                List<int> columnValues = new();
                for (int i = 0; i < (matrix.Values.FirstOrDefault()?.Count ?? 0); i++) columnValues.Add(0);
                matrix.Add(columnName, columnValues);

                matrixChanged = true;
                StateHasChanged();
            }
            catch (ArgumentException)
            {
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = LocalizationService.Localization.MatrixPage_IncorrectColumnName_ModalTitle,
                    Message = LocalizationService.Localization.MatrixPage_IncorrectColumnName_ModalText
                });
            }
            catch { /*ignore*/ }
        }

        private void RemoveColumn(string columnName)
        {
            matrix.Remove(columnName);
            definedVariables.Remove(columnName);
            requiredVariables.Remove(columnName);
            if (matrix.Count == 0) isRedacted = false;

            matrixChanged = true;
            StateHasChanged();
        }

        private void RemoveRow(int rowIndex)
        {
            foreach (var col in matrix)
            {
                col.Value.RemoveAt(rowIndex);
            }
            matrixChanged = true;
        }

        private void ChangeValue(string columnName, int index)
        {
            matrix[columnName][index] = matrix[columnName][index] > 0 ? 0 : 1;

            matrixChanged = true;
            StateHasChanged();
        }

        private string GetColumnColor(string columnName)
        {
            if (requiredVariables.Contains(columnName)) return "background-color: var(--bs-success); color: var(--bs-white)";
            else if (definedVariables.Contains(columnName)) return "background-color: var(--bs-danger); color: var(--bs-white)";
            else return "";
        }

        private async void OnCalculatePressed()
        {
            if (matrixCache == null || matrixChanged)
            {
                matrixCache = new Matrix(matrix.Where(m => !definedVariables.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));
                matrixChanged = false;
            }

            if (matrixCache.Length == 0 || matrixCache.Width == 0)
            {
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = LocalizationService.Localization.MatrixPage_Error_ModalTitle,
                    Message = LocalizationService.Localization.MatrixPage_Error_ModalText,
                });
            }
            else
            {
                try
                {
                    var procedureType = await DialogService.Show<ProcedureTypeSelectorDialog, ProcedureTypeSelectorDialogParams, FormationProcedureType>(new ProcedureTypeSelectorDialogParams
                    {
                        ProcedureType = FormationProcedureType.Status
                    });
                    await ResultDisplayingService.ShowResult(matrixCache, procedureType);
                }
                catch (Exception) { /*ignore*/ }
            }
        }

        private void OnRedactButtonPressed()
        {
            isRedacted = !isRedacted;
            StateHasChanged();
        }

        private void OnColumnPressed(string columnName)
        {
            if (requiredVariables.Contains(columnName)) requiredVariables.Remove(columnName);
            else if (definedVariables.Contains(columnName))
            {
                definedVariables.Remove(columnName);
                requiredVariables.Add(columnName);
            }
            else definedVariables.Add(columnName);

            matrixChanged = true;
            StateHasChanged();
        }

        private void OnClearPressed()
        {
            matrix = new();
            definedVariables = new();
            requiredVariables = new();

            isRedacted = false;
            matrixChanged = true;

            StateHasChanged();
        }
    }
}
