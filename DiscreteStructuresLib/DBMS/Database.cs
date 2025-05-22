namespace DiscreteStructuresLib.DBMS
{
    public class Database
    {
        public IDictionary<string, Relation> Relations { get; set; }

        public Database() 
        {
            Relations = new Dictionary<string, Relation>();
        }

        public static Database FromFile(string path) 
        {
            Database output = new();

            string text = File.ReadAllText(path);
            string[] split = text.Split("#", StringSplitOptions.RemoveEmptyEntries);

            foreach (string segment in split) 
            {
                string[] lines = segment.Split("\n", StringSplitOptions.TrimEntries).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                string name = lines[0];
                string[] columsHeaders = lines[1].Split(",");

                string[,] table = new string[columsHeaders.Length, lines.Length - 1];
                Relation relation = new(name, table);
                SetRow(relation, 0, columsHeaders);

                for (int i = 2; i < lines.Length; i++)
                {
                    SetRow(relation, i - 1, lines[i].Split(","));
                }

                output.Relations.Add(name, relation);
            }

            void SetRow(Relation relation, int rowIndex, string[] cells)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    relation.Table[i, rowIndex] = cells[i];
                }
            }

            return output;
        }
    }
}
