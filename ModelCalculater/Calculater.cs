using ModelCalculater.DEfinitions;
using ModelCalculater.Models;

namespace ModelCalculater
{
    public static class Calculater
    {
        public static TaskType GetTaskType(Matrix matrix)
        {
            var rowsWeight = CalculateRowsWeight(matrix);
            var combinations = CreateCombinations(rowsWeight);
            var maxValue = combinations.Max();

            return maxValue switch
            {
                var x when x > 0 => TaskType.NoSolution,
                var x when x == 0 => TaskType.Estimated,
                var x when x < 0 => TaskType.Optimization,
                _ => throw new NotImplementedException(),
            };
        }

        private static int[] CreateCombinations(int[] initialArray, int startIndex = 0, int pair = 0, int equationCount = 1)
        {
            var combinations = new List<int>();
            for (int i = startIndex; i < initialArray.Length; i++)
            {
                var value = pair + initialArray[i] - equationCount;
                combinations.Add(value);
                combinations.AddRange(CreateCombinations(initialArray, i + 1, value, equationCount++));
            }

            return combinations.ToArray();
        }

        private static int[] CalculateRowsWeight(Matrix matrix)
        {
            List<int> rows = new();

            for (int i = 0; i < matrix.Length; i++) rows.Add(matrix.Get(i).Sum());

            return rows.ToArray();
        }
    }
}