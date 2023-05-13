namespace ModelCalculaterTest
{
    public class CalculaterTests
    {
        [Fact]
        public void GetTaskType_IsEstimated()
        {
            Matrix matrix = new();

            matrix.AddColumns("A", "B", "C", "D");
            matrix.AddRow(1, 0, 0, 0);
            matrix.AddRow(0, 1, 0, 0);
            matrix.AddRow(0, 0, 0, 1);
            matrix.AddRow(0, 0, 1, 0);

            Assert.Equal(TaskType.Estimated, Calculater.GetTaskType(matrix));
        }
        [Fact]
        public void GetTaskType_IsNoSolution()
        {
            Matrix matrix = new();

            matrix.AddColumns("A", "B", "C", "D");
            matrix.AddRow(1, 0, 0, 0);
            matrix.AddRow(1, 0, 0, 0);
            matrix.AddRow(0, 1, 0, 1);
            matrix.AddRow(1, 0, 0, 0);

            Assert.Equal(TaskType.NoSolution, Calculater.GetTaskType(matrix));
        }
        [Fact]
        public void GetTaskType_IsOptimization()
        {
            Matrix matrix = new();

            matrix.AddColumns("A", "B", "C", "D");
            matrix.AddRow(1, 1, 0, 0);
            matrix.AddRow(0, 1, 1, 0);
            matrix.AddRow(0, 1, 0, 1);

            Assert.Equal(TaskType.Optimization, Calculater.GetTaskType(matrix));
        }
        [Fact]
        public void GetTaskType_BigMatrix()
        {
            Matrix matrix = new();

            matrix.AddColumns("A", "B", "C", "D", "E", "F", "G", "H", "I", "J");
            matrix.AddRow(1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            matrix.AddRow(1, 1, 0, 0, 0, 0, 0, 0, 0, 0);
            matrix.AddRow(1, 1, 1, 0, 0, 0, 0, 0, 0, 0);
            matrix.AddRow(1, 1, 1, 1, 0, 0, 0, 0, 0, 0);
            matrix.AddRow(1, 1, 1, 1, 1, 0, 0, 0, 0, 0);
            matrix.AddRow(1, 1, 1, 1, 1, 1, 0, 0, 0, 0);
            matrix.AddRow(1, 1, 1, 1, 1, 1, 1, 0, 0, 0);
            matrix.AddRow(1, 1, 1, 1, 1, 1, 1, 1, 0, 0);
            matrix.AddRow(1, 1, 1, 1, 1, 1, 1, 1, 1, 0);
            matrix.AddRow(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            Assert.Equal(TaskType.Estimated, Calculater.GetTaskType(matrix));
        }

        [Theory]
        [MemberData(nameof(GetCombinationsData))]
        public void TestGetCombinations<T>(IEnumerable<T> source, IEnumerable<T[]> expected)
        {
            var result = Calculater.GetCombinations(source);

            Assert.Equal(expected, result);
        }

        private static IEnumerable<object[]> GetCombinationsData()
        {
            yield return new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1 }, new[] { 2 }, new[] { 1, 2 }, new[] { 3 }, new[] { 1, 3 }, new[] { 2, 3 }, new[] { 1, 2, 3 } } };
            yield return new object[] { new[] { 'A', 'B', 'C', 'D' }, new[] { new[] { 'A' }, new[] { 'B' }, new[] { 'A', 'B' }, new[] { 'C' }, new[] { 'A', 'C' }, new[] { 'B', 'C' }, new[] { 'A', 'B', 'C' }, new[] { 'D' }, new[] { 'A', 'D' }, new[] { 'B', 'D' }, new[] { 'A', 'B', 'D' }, new[] { 'C', 'D' }, new[] { 'A', 'C', 'D' }, new[] { 'B', 'C', 'D' }, new[] { 'A', 'B', 'C', 'D' } } };
        }
    }
}
