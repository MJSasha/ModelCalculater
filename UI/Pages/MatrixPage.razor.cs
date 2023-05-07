using Microsoft.AspNetCore.Components;
using ModelCalculater;
using ModelCalculater.Models;
using Plugin.Maui.Audio;
using UI.Components.Dialogs.InputDialog;
using UI.Components.Dialogs.MessageDialog;
using UI.Services;
using UI.Utils;

namespace UI.Pages
{
    public partial class MatrixPage : IDisposable
    {
        [Inject]
        public DialogService DialogService { get; set; }

        [Inject]
        public MatrixActionsService MatrixActionsService { get; set; }

        private Dictionary<string, List<int>> matrix = new();
        private List<string> definedVariables = new();
        private List<string> requiredVariables = new();

        private bool isRedacted = false;


        public void Dispose()
        {
            MatrixActionsService.OnClearPressed -= ClearMatrix;
            MatrixActionsService.OnRedactionPressed -= OnRedactButtonPressed;
            MatrixActionsService.OnCalculatePressed -= CalculateMatrix;
        }

        protected override void OnInitialized()
        {
            MatrixActionsService.OnClearPressed += ClearMatrix;
            MatrixActionsService.OnRedactionPressed += OnRedactButtonPressed;
            MatrixActionsService.OnCalculatePressed += CalculateMatrix;
        }

        private void AddRow()
        {
            foreach (var col in matrix)
            {
                col.Value.Add(0);
            }
            StateHasChanged();
        }

        private void ClearMatrix()
        {
            matrix = new();
            isRedacted = false;
            StateHasChanged();
        }

        private void OnRedactButtonPressed()
        {
            isRedacted = !isRedacted;
            StateHasChanged();
        }

        private void RemoveColumn(string columnName)
        {
            matrix.Remove(columnName);
            if (matrix.Count == 0) isRedacted = false;
            StateHasChanged();
        }

        private void RemoveRow(int rowIndex)
        {
            foreach (var col in matrix)
            {
                col.Value.RemoveAt(rowIndex);
            }
        }

        private void ChangeValue(string columnName, int index)
        {
            matrix[columnName][index] = matrix[columnName][index] > 0 ? 0 : 1;
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
            StateHasChanged();
        }

        private string GetColumnColor(string columnName)
        {
            if (requiredVariables.Contains(columnName)) return "background-color: var(--bs-success); color: var(--bs-white)";
            else if (definedVariables.Contains(columnName)) return "background-color: var(--bs-danger); color: var(--bs-white)";
            else return "";
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

        private async void CalculateMatrix()
        {
            var matrixWithoutDefineVariables = new Matrix(matrix.Where(m => !definedVariables.Contains(m.Key)).ToDictionary(s => s.Key, s => s.Value));

            if (matrixWithoutDefineVariables.Length == 0 || matrixWithoutDefineVariables.Width == 0)
            {
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = LocalizationService.Localization.MatrixPage_Error_ModalTitle,
                    Message = LocalizationService.Localization.MatrixPage_Error_ModalText,
                });
            }
            else
            {
                var result = Calculater.GetTaskType(matrixWithoutDefineVariables);
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = LocalizationService.Localization.MatrixPage_Result_ModalTitle,
                    Message = result.GetName(),
                    SoundName = result.GetSoundName(),
                });
            }
        }
    }
}
