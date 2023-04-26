using ModelCalculater.DEfinitions;
using ModelCalculater.Models;

namespace ModelCalculater
{
    public static class Calculater
    {
        public static TaskType GetTaskType(Matrix matrix)
        {
            List<int> indexes = new();
            for (int i = 0; i < matrix.Width; i++) indexes.Add(i);
            var combinations = GetCombinations(indexes);
            var maxValue = CalculateMaxValue(combinations, matrix);

            return maxValue switch
            {
                var x when x > 0 => TaskType.NoSolution,
                var x when x == 0 => TaskType.Estimated,
                var x when x < 0 => TaskType.Optimization,
                _ => throw new NotImplementedException(),
            };
        }

        private static int CalculateMaxValue(IEnumerable<int[]> indexesArray, Matrix matrix)
        {
            int maxValue = int.MinValue;
            foreach (var indexes in indexesArray)
            {
                var variables = indexes.SelectMany(i => matrix.GetRowVariables(i)).Distinct().ToList();
                if (maxValue < indexes.Length - variables.Count) maxValue = indexes.Length - variables.Count;
            }
            return maxValue;
        }

        private static IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
              .Range(0, 1 << (data.Length))
              .Select(index => data
                 .Where((v, i) => (index & (1 << i)) != 0)
                 .ToArray())
              .Where(x => x.Any());
        }
    }
}