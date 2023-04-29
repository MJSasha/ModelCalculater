using Microsoft.AspNetCore.Components;
using ModelCalculater;
using ModelCalculater.Models;
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
            //await SecureStorage.Default.SetAsync("oauth_token", "secret-oauth-token-value");
            //string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
            try
            {
                var columnName = await DialogService.Show<InputDialog, InputDialogParams, string>(new InputDialogParams { Title = "Enter column name" });
                List<int> columnValues = new();
                for (int i = 0; i < (matrix.Values.FirstOrDefault()?.Count ?? 0); i++) columnValues.Add(0);
                matrix.Add(columnName, columnValues);
                StateHasChanged();
            }
            catch (ArgumentException)
            {
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = "Incorrect column name",
                    Message = "Incorrect column name. Perhaps a column with this name already exists"
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
                    Title = "Error",
                    Message = "You have to specify the matrix",
                });
            }
            else
            {
                var result = Calculater.GetTaskType(matrixWithoutDefineVariables);
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = "Result",
                    Message = result.GetName(),
                    SoundName = result.GetSoundName(),
                });
            }
        }
    }
}
