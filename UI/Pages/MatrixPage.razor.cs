using Microsoft.AspNetCore.Components;
using UI.Components.Dialogs.InputDialog;
using UI.Components.Dialogs.MessageDialog;
using UI.Services;

namespace UI.Pages
{
    public partial class MatrixPage : IDisposable
    {
        [Inject]
        public DialogService DialogService { get; set; }

        [Inject]
        public MatrixActionsService matrixActionsService { get; set; }

        private Dictionary<string, List<int>> matrix = new();
        private List<string> definedVariables = new();
        private List<string> requiredVariables = new();


        public void Dispose()
        {
            matrixActionsService.OnClearPressed -= ClearMatrix;
        }

        protected override void OnInitialized()
        {
            matrixActionsService.OnClearPressed += ClearMatrix;
        }

        private void AddRow()
        {
            foreach (var col in matrix)
            {
                col.Value.Add(0);
            }
            StateHasChanged();
        }

        private async void AddColumn()
        {
            try
            {
                var columnName = await DialogService.Show<InputDialog, InputDialogParams, string>(new InputDialogParams { Title = "Enter column name" });
                List<int> columnValues = new();
                for (int i = 0; i < (matrix.Values.FirstOrDefault()?.Count ?? 0); i++) columnValues.Add(0);
                matrix.Add(columnName, columnValues);
                StateHasChanged();
            }
            catch
            {
                await DialogService.Show<MessageDialog, MessageDialogParams, object>(new MessageDialogParams
                {
                    Title = "Incorrect column name",
                    Message = "Incorrect column name. Perhaps a column with this name already exists"
                });
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
            if (requiredVariables.Contains(columnName)) return "background-color: var(--bs-success)";
            else if (definedVariables.Contains(columnName)) return "background-color: var(--bs-danger)";
            else return "";
        }

        private void ClearMatrix()
        {
            matrix = new();
            StateHasChanged();
        }
    }
}
