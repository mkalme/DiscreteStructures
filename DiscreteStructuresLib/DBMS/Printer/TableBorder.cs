namespace DiscreteStructuresLib.DBMS
{
    public struct TableBorder
    {
        public string HeaderTop { get; set; }
        public string HeaderBottom { get; set; }
        public string ContentMiddle { get; set; }
        public string TableWalls { get; set; }
        public string ContentBottom { get; set; }

        public static TableBorder ThickUnicode => new()
        {
            HeaderTop =     "╔╤═╗",
            HeaderBottom =  "╠╪═╣",
            ContentMiddle = "╟┼─╢",
            ContentBottom = "╚╧═╝",
            TableWalls =    "║│"
        };
        public static TableBorder ThickOutsideUnicode => new()
        {
            HeaderTop =     "╔══╗",
            HeaderBottom =  "╠══╣",
            ContentMiddle = "║  ║",
            ContentBottom = "╚══╝",
            TableWalls =    "║ "
        };
        public static TableBorder ThinAscii => new()
        {
            HeaderTop =     "    ",
            HeaderBottom =  "  - ",
            ContentMiddle = "    ",
            ContentBottom = "    ",
            TableWalls =    "  "
        };
        public static TableBorder ThickAscii => new()
        {
            HeaderTop =     "++-+",
            HeaderBottom =  "++-+",
            ContentMiddle = "|| |",
            ContentBottom = "++-+",
            TableWalls =    "||"
        };
    }
}
