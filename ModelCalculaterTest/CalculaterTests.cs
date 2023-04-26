namespace ModelCalculaterTest
{
    public class CalculaterTests
    {
        [Fact]
        public void Test1()
        {
            Matrix matrix = new();

            matrix.AddColumns("A", "B", "C", "D");
            matrix.AddRow(0, 1, 0, 1);
            matrix.AddRow(1, 0, 1, 0);
            matrix.AddRow(0, 1, 0, 1);
            matrix.AddRow(1, 0, 1, 0);

            Assert.Equal(TaskType.Estimated ,Calculater.GetTaskType(matrix));
        }
    }
}
