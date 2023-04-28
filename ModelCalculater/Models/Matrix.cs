namespace ModelCalculater.Models
{
    public class Matrix
    {
        public int Length => matrix.Count;
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

        public void AddColumn(string columnName)
        {
            List<int> column = new();
            for (int i = 0; i < Width; i++) column.Add(0);
            matrix.Add(columnName, new List<int>());
        }

        public void AddColumn(string columnName, List<int> values)
        {
            List<int> column = new();
            for (int i = values.Count - 1; i < Width; i++) column.Add(0);
            matrix.Add(columnName, new List<int>());
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

            int i = 0;
            foreach (var column in matrix) column.Value.Add(row[i++]);
        }

        public string[] GetColumnsNames()
        {
            return matrix.Keys.ToArray();
        }

        public string[] GetRowVariables(int rowIndex)
        {
            return matrix.Where(c => c.Value[rowIndex] == 1).Select(c => c.Key).ToArray();
        }

        public int[][] Get()
        {
            return matrix.Values.Select(v => v.ToArray()).ToArray();
        }

        public int[] Get(int i)
        {
            return matrix.Values.Select(v => v[i]).ToArray();
        }
    }
}
