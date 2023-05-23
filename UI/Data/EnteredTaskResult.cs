namespace UI.Data
{
    public class EnteredTaskResult
    {
        public List<string> GivenValues { get; set; } = new();
        public List<string> ValuesToFind { get; set; } = new();
        public List<string> Criteria { get; set; } = new();
    }
}
