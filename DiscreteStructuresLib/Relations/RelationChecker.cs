namespace DiscreteStructuresLib.Relations
{
    public static class RelationChecker
    {
        public static bool IsReflexive(ISet<string> set, ISet<Pair> relation) 
        {
            foreach (string element in set) 
            {
                Pair pair = new(element);
                if(!relation.Contains(pair)) return false;
            }

            return true;
        }

        public static bool IsSymmetrical(ISet<Pair> relation)
        {
            foreach (Pair pair in relation)
            {
                if(!relation.Contains(pair.MakeOpposite())) return false;
            }

            return true;
        }

        public static bool IsTransitive(ISet<Pair> relation)
        {
            foreach (Pair i in relation)
            {
                if (i.IsSymmetrical()) continue;

                foreach (Pair j in relation) 
                {
                    if (j.IsSymmetrical() || j.Equals(i) || j.A != i.B) continue;
                    if (!relation.Contains(new Pair(i.A, j.B))) return false;
                }
            }

            return true;
        }
    }
}
