namespace UI.Components.Dialogs.EnterTaskDialog
{
    public class EnterTaskDialogParams
    {
        public string Title { get; set; }
        public List<string> ColumnsNames { get; set; }
        public bool ShowCriteriaSelector { get; set; }
        public bool ShowTwoInputTasks { get; set; }
    }
}
