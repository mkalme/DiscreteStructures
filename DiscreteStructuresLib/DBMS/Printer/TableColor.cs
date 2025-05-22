namespace DiscreteStructuresLib.DBMS
{
    public struct TableColor
    {
        public ConsoleColor BorderColor { get; set; }
        public ConsoleColor HeaderColor { get; set; }
        public ConsoleColor ContentColor { get; set; }
        public ConsoleColor NullColor { get; set; }

        public static TableColor YellowDarkTheme => new()
        {
            BorderColor = ConsoleColor.DarkGray,
            HeaderColor = ConsoleColor.Yellow,
            ContentColor = ConsoleColor.White,
            NullColor = ConsoleColor.DarkCyan
        };
        public static TableColor SimpleWhite => new()
        {
            BorderColor = ConsoleColor.White,
            HeaderColor = ConsoleColor.White,
            ContentColor = ConsoleColor.White,
            NullColor = ConsoleColor.White
        };
    }
}
