namespace DiscreteStructuresLib.DBMS
{
    public class TablePrinter
    {
        public TableBorder Border { get; set; } = TableBorder.ThickUnicode;
        public TableColor Color { get; set; } = TableColor.YellowDarkTheme;

        public string NullValue { get; set; } = "NULL";

        public void PrintToConsole(string[,] table) 
        {
            int[] columnWidths = CalculateColumnWidths(table);

            PrintRowSeperator(columnWidths, Border.HeaderTop);
            AddRow(0, Color.HeaderColor);
            PrintRowSeperator(columnWidths, Border.HeaderBottom);

            for (int i = 1; i < table.GetLength(1); i++)
            {
                AddRow(i, Color.ContentColor);
                if (i < table.GetLength(1) - 1) PrintRowSeperator(columnWidths, Border.ContentMiddle);
            }

            PrintRowSeperator(columnWidths, Border.ContentBottom);

            void AddRow(int row, ConsoleColor foreColor)
            {
                WriteToConsole(Border.TableWalls[0].ToString(), Color.BorderColor);

                for (int column = 0; column < table.GetLength(0); column++)
                {
                    string cell = table[column, row];
                    ConsoleColor foreground = foreColor;

                    if (string.IsNullOrEmpty(cell)) 
                    {
                        cell = NullValue;
                        foreground = Color.NullColor;
                    }

                    WriteToConsole($" {cell}{new string(' ', columnWidths[column] - cell.Length - 1)}", foreground);

                    if (column < columnWidths.Length - 1) WriteToConsole(Border.TableWalls[1].ToString(), Color.BorderColor);
                }

                WriteLineToConsole(Border.TableWalls[0].ToString(), Color.BorderColor);
            }
        }

        private int[] CalculateColumnWidths(string[,] table) 
        {
            int[] output = new int[table.GetLength(0)];

            for (int column = 0; column < table.GetLength(0); column++)
            {
                for (int row = 0; row < table.GetLength(1); row++)
                {
                    string cell = table[column, row];

                    int cellLength = string.IsNullOrEmpty(cell) ? NullValue.Length : cell.Length;
                    if (output[column] < cellLength + 2) output[column] = cellLength + 2;
                }
            }

            return output;
        }
        
        private void PrintRowSeperator(int[] columnWidth, string characters) 
        {
            WriteToConsole(characters[0].ToString(), Color.BorderColor);

            for (int i = 0; i < columnWidth.Length; i++)
            {
                WriteToConsole(new string(characters[2], columnWidth[i]), Color.BorderColor);
                if (i < columnWidth.Length - 1) WriteToConsole(characters[1].ToString(), Color.BorderColor);
            }

            WriteLineToConsole(characters[3].ToString(), Color.BorderColor);
        }

        private static void WriteLineToConsole(string text, ConsoleColor foregroundColor) 
        {
            ConsoleColor prev = Console.ForegroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
            Console.ForegroundColor = prev;
        }
        private static void WriteToConsole(string text, ConsoleColor foregroundColor) 
        {
            ConsoleColor prev = Console.ForegroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
            Console.ForegroundColor = prev;
        }
    }
}
