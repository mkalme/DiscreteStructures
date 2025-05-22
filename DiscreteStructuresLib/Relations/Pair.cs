using System.Diagnostics.CodeAnalysis;

namespace DiscreteStructuresLib
{
    public struct Pair
    {
        public string A { get; set; }
        public string B { get; set; }

        public static Pair Empty => new(string.Empty, string.Empty);

        public static implicit operator Pair((string, string) pair) => new(pair.Item1, pair.Item2);

        public Pair(string a)
        {
            A = a;
            B = a;
        }
        public Pair(string a, string b) 
        {
            A = a;
            B = b;
        }

        public Pair MakeOpposite() => new(B, A);
        public bool IsSymmetrical() => B == A;
        public bool IsEmpty() => string.IsNullOrEmpty(A) && string.IsNullOrEmpty(B);

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B);
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null || obj is not Pair pair) return false;
            return A == pair.A && B == pair.B;
        }
    }
}