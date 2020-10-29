﻿using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class L
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var A = Console.ReadLine().Split(" ").Select(int.Parse)
                .Select(x => x == 0 ? new S(1, 0, 0) : new S(0, 1, 0)).ToArray();

            var lst = new LazySegmentTree<S, bool>(A, new Oracle());
            for (var i = 0; i < Q; i++)
            {
                var TLR = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (T, L, R) = (TLR[0], TLR[1], TLR[2]);
                L--;
                if (T == 1) lst.Apply(L, R, true);
                else Console.WriteLine(lst.Query(L, R).Inversion);
            }
        }

        public readonly struct S
        {
            public readonly long Zero;
            public readonly long One;
            public readonly long Inversion;

            public S(long zero, long one, long inversion) => (Zero, One, Inversion) = (zero, one, inversion);
        }

        public class Oracle : IOracle<S, bool>
        {
            public S MonoidIdentity { get; } = new S(0, 0, 0);

            public S Operate(in S a, in S b) =>
                new S(a.Zero + b.Zero, a.One + b.One, a.Inversion + b.Inversion + a.One * b.Zero);

            public bool MapIdentity { get; } = false;
            public S Map(in bool f, in S x) => f ? new S(x.One, x.Zero, x.One * x.Zero - x.Inversion) : x;

            public bool Compose(in bool f, in bool g) => f && !g || !f && g;
        }
    }
}