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

            Assert.Equal(TaskType.Estimated ,Calculater.GetTaskType(matrix));
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

            Assert.Equal(TaskType.NoSolution ,Calculater.GetTaskType(matrix));
        }
        [Fact]
        public void GetTaskType_IsOptimization()
        {
            Matrix matrix = new();

            matrix.AddColumns("A", "B", "C", "D");
            matrix.AddRow(1, 1, 0, 0);
            matrix.AddRow(0, 1, 1, 0);
            matrix.AddRow(0, 1, 0, 1);

            Assert.Equal(TaskType.Optimization ,Calculater.GetTaskType(matrix));
        }
    }
}
