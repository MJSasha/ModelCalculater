namespace ModelCalculater.Models
{
    public class Matrix
    {
        public int Length => matrix.Count;

        private Dictionary<string, List<int>> matrix;

        public Matrix()
        {
            matrix = new();
        }

        public void AddColumn(string columnName)
        {
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
            if (row.Length != this.Length) throw new ArgumentException("The length of the row does not match the number of columns");

            int i = 0;
            foreach (var column in matrix) column.Value.Add(row[i++]);
        }

        public string[] GetColumnsNames()
        {
            return matrix.Keys.ToArray();
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
