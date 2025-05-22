using System.Text;

namespace DiscreteStructuresLib.DBMS
{
    public class Relation
    {
        public string Name { get; set; }
        public string[,] Table { get; set; }

        public Relation(string name, string[,] table)
        {
            Name = name;
            Table = table;
        }

        public Relation Union(Relation other) 
        {
            if (Table.GetLength(0) != other.Table.GetLength(0)) throw new Exception("Domains are not compatible.");

            string[,] table = new string[Table.GetLength(0), Table.GetLength(1) + other.Table.GetLength(1) - 1];

            for (int i = 0; i < Table.GetLength(1); i++) 
            {
                CopyRow(table, i, Table, i);
            }

            for (int i = 0; i < other.Table.GetLength(0); i++)
            {
                CopyRow(table, i + Table.GetLength(1), other.Table, i + 1);
            }

            table = RemoveDuplicateEntries(table);
            return new Relation(string.Empty, table);
        }
        public Relation Intersts(Relation other)
        {
            if (Table.GetLength(0) != other.Table.GetLength(0)) throw new Exception("Domains are not compatible.");

            Dictionary<int, string> firstHashes = new();
            List<int> firstIndexes = new() { 0 };
            CreateUniqueSet(Table, firstHashes, firstIndexes);

            Dictionary<int, string> othertHashes = new();
            List<int> otherIndexes = new();
            CreateUniqueSet(other.Table, othertHashes, otherIndexes);
            HashSet<string> uniqueOtherRows = new(othertHashes.Values);

            for (int i = 1; i < firstIndexes.Count; i++) 
            {
                string row = firstHashes[firstIndexes[i]];
                if (!uniqueOtherRows.Contains(row)) firstIndexes.RemoveAt(i--);
            }

            string[,] table = new string[Table.GetLength(0), firstIndexes.Count];
            for (int i = 0; i < table.GetLength(1); i++)
            {
                CopyRow(table, i, Table, firstIndexes[i]);
            }

            return new Relation(string.Empty, table);
        }
        public Relation Difference(Relation other)
        {
            if (Table.GetLength(0) != other.Table.GetLength(0)) throw new Exception("Domains are not compatible.");

            Dictionary<int, string> firstHashes = new();
            List<int> firstIndexes = new() { 0 };
            CreateUniqueSet(Table, firstHashes, firstIndexes);

            Dictionary<int, string> othertHashes = new();
            List<int> otherIndexes = new();
            CreateUniqueSet(other.Table, othertHashes, otherIndexes);
            HashSet<string> uniqueOtherRows = new(othertHashes.Values);

            for (int i = 1; i < firstIndexes.Count; i++)
            {
                string row = firstHashes[firstIndexes[i]];
                if (uniqueOtherRows.Contains(row)) firstIndexes.RemoveAt(i--);
            }

            string[,] table = new string[Table.GetLength(0), firstIndexes.Count];
            for (int i = 0; i < table.GetLength(1); i++)
            {
                CopyRow(table, i, Table, firstIndexes[i]);
            }

            return new Relation(string.Empty, table);
        }

        public Relation EqualJoin(Relation other, Func<int, int, bool> func) 
        {
            List<(int, int)> rows = new List<(int, int)>() { (0, 0) };

            for (int mainRow = 1; mainRow < Table.GetLength(1); mainRow++)
            {
                for (int otherRow = 1; otherRow < other.Table.GetLength(1); otherRow++)
                {
                    if (func(mainRow, otherRow)) rows.Add((mainRow, otherRow));
                }
            }

            string[,] table = new string[Table.GetLength(0) + other.Table.GetLength(0), rows.Count];

            for (int i = 0; i < rows.Count; i++) 
            {
                SetRowDouble(i, rows[i].Item1, rows[i].Item2);
            }

            void SetRowDouble(int outputRow, int mainInputRow, int otherInputRow)
            {
                for (int i = 0; i < Table.GetLength(0); i++)
                {
                    table[i, outputRow] = Table[i, mainInputRow];
                }

                for (int i = 0; i < other.Table.GetLength(0); i++)
                {
                    table[Table.GetLength(0) + i, outputRow] = other.Table[i, otherInputRow];
                }
            }

            table = RemoveDuplicateEntries(table);
            return new Relation(string.Empty, table);
        }
        public Relation CartesianProduct(Relation other) 
        {
            int columns = Table.GetLength(0) + other.Table.GetLength(0);
            int rows = (Table.GetLength(1) - 1) * (other.Table.GetLength(1) - 1) + 1;

            string[,] table = new string[columns, rows];

            SetRowDouble(0, 0, 0);

            for (int mainRow = 1; mainRow < Table.GetLength(1); mainRow++)
            {
                for (int otherRow = 1; otherRow < other.Table.GetLength(1); otherRow++)
                {
                    int outputRow = (mainRow - 1) * (other.Table.GetLength(1) - 1) + otherRow;
                    SetRowDouble(outputRow, mainRow, otherRow);
                }
            }

            void SetRowDouble(int outputRow, int mainInputRow, int otherInputRow)
            {
                for (int i = 0; i < Table.GetLength(0); i++)
                {
                    table[i, outputRow] = Table[i, mainInputRow];
                }

                for (int i = 0; i < other.Table.GetLength(0); i++)
                {
                    table[Table.GetLength(0) + i, outputRow] = other.Table[i, otherInputRow];
                }
            }

            table = RemoveDuplicateEntries(table);
            return new Relation(string.Empty, table);
        }
        public Relation Select(Func<int, bool> selector) 
        {
            List<int> rows = new List<int>(Table.GetLength(1));
            for (int i = 1; i < Table.GetLength(1); i++) 
            {
                if (selector(i)) rows.Add(i);
            }

            int columns = Table.GetLength(0);
            int rowCount = rows.Count + 1;

            string[,] table = new string[columns, rowCount];
            SetRowFromIndex(0, 0);

            for (int i = 0; i < rows.Count; i++) 
            {
                SetRowFromIndex(i + 1, rows[i]);
            }

            void SetRowFromIndex(int outputRowIndex, int inputRowIndex) 
            {
                for (int column = 0; column < columns; column++) 
                {
                    table[column, outputRowIndex] = Table[column, inputRowIndex];
                }
            }

            table = RemoveDuplicateEntries(table);
            return new Relation(string.Empty, table);
        }
        public Relation Present(params int[] header) 
        {
            string[,] table = new string[header.Length, Table.GetLength(1)];

            for (int i = 0; i < header.Length; i++) 
            {
                CopyColumn(i, header[i]);
            }

            void CopyColumn(int outputIndex, int inputIndex) 
            {
                for (int rows = 0; rows < Table.GetLength(1); rows++) 
                {
                    table[outputIndex, rows] = Table[inputIndex, rows];
                }
            }

            table = RemoveDuplicateEntries(table);
            return new Relation(string.Empty, table);
        }

        private static string[,] RemoveDuplicateEntries(string[,] table) 
        {
            Dictionary<int, string> rowHashes = new();
            List<int> rowIndexes = new() { 0 };
            CreateUniqueSet(table, rowHashes, rowIndexes);

            if (table.GetLength(1) == rowIndexes.Count - 1) return table;

            string[,] output = new string[table.GetLength(0), rowIndexes.Count];

            for (int i = 0; i < rowIndexes.Count; i++) 
            {
                CopyRow(output, i, table, rowIndexes[i]);
            }

            return output;
        }

        private static void CreateUniqueSet(string[,] table, IDictionary<int, string> outputSet, IList<int> outputRows) 
        {
            ISet<string> uniqueHashes = new HashSet<string>();

            StringBuilder builder = new StringBuilder();
            for (int row = 1; row < table.GetLength(1); row++)
            {
                for (int column = 0; column < table.GetLength(0); column++)
                {
                    builder.Append(table[column, row]);
                }

                string rowSum = builder.ToString();
                if (uniqueHashes.Add(rowSum))
                {
                    outputSet.Add(row, rowSum);
                    outputRows.Add(row);
                }

                builder.Clear();
            }
        }

        private static void CopyRow(string[,] output, int outputRow, string[,] input, int inputRow) 
        {
            for (int column = 0; column < output.GetLength(0); column++)
            {
                output[column, outputRow] = input[column, inputRow];
            }
        }
    }
}
