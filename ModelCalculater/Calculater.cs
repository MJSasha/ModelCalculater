using ModelCalculater.DEfinitions;
using ModelCalculater.Models;

namespace ModelCalculater
{
    public static class Calculater
    {
        public static TaskType GetTaskType(Matrix matrix)
        {
            List<int> indexes = Enumerable.Range(0, matrix.Width).ToList();
            var combinations = GetCombinations(indexes);
            var maxValue = CalculateMaxValue(combinations, matrix);

            return maxValue switch
            {
                var x when x > 0 => TaskType.NoSolution,
                var x when x == 0 => TaskType.Estimated,
                var x when x < 0 => TaskType.Optimization,
                _ => throw new NotImplementedException()
            };
        }

        private static int CalculateMaxValue(IEnumerable<int[]> indexesArray, Matrix matrix)
        {
            int maxValue = int.MinValue;
            foreach (var indexes in indexesArray)
            {
                if (maxValue > 0) break;
                var variables = indexes.SelectMany(i => matrix.GetRowVariables(i)).Distinct().ToList();
                var missingVariablesCount = indexes.Length - variables.Count;
                if (maxValue < missingVariablesCount) maxValue = missingVariablesCount;
            }
            return maxValue;
        }

        private static IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> source)
        {
            var data = source.ToArray();
            var length = data.Length;

            for (int i = 1; i < 1 << length; i++) yield return Enumerable.Range(0, length).Where(b => (i & 1 << b) != 0).Select(b => data[b]).ToArray();
        }
    }
}
