namespace ModelCalculater.Models
{
    public class Matrix
    {
        public int Length => matrix.Any() ? matrix.Count : 0;
        public int Width => matrix.FirstOrDefault().Value?.Count ?? 0;

        private Dictionary<string, List<int>> matrix;

        public Matrix()
        {
            matrix = new();
        }

        public Matrix(Dictionary<string, List<int>> matrix)
        {
            this.matrix = matrix;
        }

        public void AddColumns(params string[] columnsNames)
        {
            foreach (string columnName in columnsNames)
            {
                matrix.Add(columnName, new List<int>());
            }
        }

        public void AddRow(params int[] row)
        {
            if (row.Length != Length) throw new ArgumentException("The length of the row does not match the number of columns");

            for (int i = 0; i < row.Length; i++)
            {
                matrix.ElementAt(i).Value.Add(row[i]);
            }
        }

        public string[] GetRowVariables(int rowIndex)
        {
            return matrix.Where(c => c.Value[rowIndex] == 1).Select(c => c.Key).ToArray();
        }

        public int[][] Get()
        {
            return matrix.Values.Select(v => v.ToArray()).ToArray();
        }
    }
}
