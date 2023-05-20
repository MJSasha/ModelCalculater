using ModelCalculater.Definitions;

namespace ModelCalculater.Models
{
    public class Matrix
    {
        public int Length => matrix.Any() ? matrix.Count : 0;
        public int Width => matrix.FirstOrDefault().Value?.Count ?? 0;
        public int Deficit { get; set; }
        public int[]? LinesWithDeficit { get; set; }

        private Dictionary<string, List<int>> matrix;

        public Matrix(Dictionary<string, List<int>> matrix)
        {
            this.matrix = matrix;
            Deficit = CalculateDeficit();
        }

        public TaskType GetTaskType()
        {
            return Deficit switch
            {
                var x when x > 0 => TaskType.NoSolution,
                var x when x == 0 => TaskType.Estimated,
                var x when x < 0 => TaskType.Optimization,
                _ => throw new NotImplementedException()
            };
        }

        public bool CheckPossibilityOfFormingCalculation()
        {
            return Deficit == 0;
        }

        public bool CheckForInformationLinks()
        {
            return Deficit > 0;
        }

        private int CalculateDeficit()
        {
            List<int> indexes = Enumerable.Range(0, Width).ToList();
            var combinations = GetCombinations(indexes);
            return CalculateMaxValue(combinations);
        }

        private int CalculateMaxValue(IEnumerable<int[]> indexesArray)
        {
            int maxValue = int.MinValue;
            foreach (var indexes in indexesArray)
            {
                if (maxValue > 0) break;
                var variables = indexes.SelectMany(i => GetRowVariables(i)).Distinct().ToList();
                var missingVariablesCount = indexes.Length - variables.Count;
                if (maxValue < missingVariablesCount)
                {
                    maxValue = missingVariablesCount;
                    LinesWithDeficit = indexes;
                }
            }
            return maxValue;
        }

        private IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> source)
        {
            var data = source.ToArray();
            var length = data.Length;

            for (int i = 1; i < 1 << length; i++) yield return Enumerable.Range(0, length).Where(b => (i & 1 << b) != 0).Select(b => data[b]).ToArray();
        }

        private string[] GetRowVariables(int rowIndex)
        {
            return matrix.Where(c => c.Value[rowIndex] == 1).Select(c => c.Key).ToArray();
        }
    }
}
