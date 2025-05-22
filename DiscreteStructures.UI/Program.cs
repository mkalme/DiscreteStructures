using DiscreteStructuresLib;
using DiscreteStructuresLib.DBMS;
using DiscreteStructuresLib.Relations;

namespace DiscreteStructures.UI
{
    internal class Program
    {
        private static readonly string FOLDER_PATH = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\dbms";

        static void Main(string[] args)
        {
            //CheckClasses();

            Database database = Database.FromFile($"{FOLDER_PATH}\\relations.dbms");

            Relation v = database.Relations["VADĪTĀJI"];
            Relation i = database.Relations["IZPILDĪTĀJI"];

            TablePrinter printer = new() 
            {
                Border = TableBorder.ThickUnicode,
                Color = TableColor.YellowDarkTheme
            };

            Console.WriteLine();

            Relation t1 = v.CartesianProduct(i);

            printer.PrintToConsole(t1.Table);

            Relation t2 = t1.Select(i => t1.Table[0, i] == t1.Table[3, i]);
            Relation t3 = t2.Select(i => t2.Table[2, i] == "Vītols");
            Relation t4 = t3.Present(4);

            printer.PrintToConsole(t4.Table);

            Console.ReadLine();
        }

        private static void CheckClasses() 
        {
            const string c = "c", _2 = "2", a = "a", b = "b";
            ISet<string> set = new HashSet<string>() { c, _2, a, b };
            ISet<Pair> relation = new HashSet<Pair>() { (a,b),(_2,c),(_2,b),(a,a),(c,c),(b,c),(a,c) };

            Console.WriteLine(RelationChecker.IsReflexive(set, relation));
            Console.WriteLine(RelationChecker.IsSymmetrical(relation));
            Console.WriteLine(RelationChecker.IsTransitive(relation));
        }
    }
}