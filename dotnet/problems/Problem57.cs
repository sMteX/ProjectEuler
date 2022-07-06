using System.Numerics;

namespace ProjectEuler.problems
{
    public class Problem57 : Problem
    {
        private class Fraction {
            public BigInteger N { get; set; }
            public BigInteger D { get; set; }

            public Fraction(BigInteger numerator, BigInteger denominator)
            {
                this.N = numerator;
                this.D = denominator;
            }
            public Fraction(BigInteger value) {
                this.N = value;
                this.D = 1;
            }
            public Fraction(int n, Fraction d) {
                // used in recursive creation of the continued fraction (1 / (....))
                this.N = n * d.D;
                this.D = d.N;
                this.simplify();
            }

            private void simplify() {
                BigInteger gcd = GCD(this.N, this.D);
                this.N /= gcd;
                this.D /= gcd;
            }

            public static Fraction operator+(int x, Fraction f) {
                return new Fraction(x) + f;
            }
            public static Fraction operator+(Fraction first, Fraction second) {
                Fraction f = new Fraction(first.N * second.D + second.N * first.D, first.D * second.D);
                f.simplify();
                return f;
            }
            public override string ToString() {
                return $"{this.N}/{this.D}";
            }
            private static BigInteger GCD(BigInteger a, BigInteger b) {
                if (b == 0) {
                    return a;
                }
                return GCD(b, a % b);
            }
        }
        public void run()
        {
            /*
            It is possible to show that the square root of two can be expressed as an infinite continued fraction.

            sqrt(2) = 1 + 1/(2 + 1/(2 + 1/(2 + …)))

            By expanding this for the first four iterations, we get:

            1 + 1/2 = 3/2 = 1.5

            1 + 1/(2 + 1/2) = 7/5 = 1.4
            1 + 1/(2 + 1/(2 + 1/2)) = 17/12 = 1.41666…
            1 + 1/(2 + 1/(2 + 1/(2 + 1/2))) = 41/29 = 1.41379…


            The next three expansions are 99/70, 239/169, and 577/408, but the eighth expansion, 1393/985, 
                is the first example where the number of digits in the numerator exceeds the number of digits in the denominator.

            In the first one-thousand expansions, how many fractions contain a numerator with more digits than the denominator?
            */


            // My solution - horrendously slow (like half minute maybe?)
            // int count = 0;
            // for (int it = 1; it <= 1000; it++) {
            //     Fraction f = new Fraction(2); // at the end of the fraction
            //     for (int i = it; i > 0; i--) {
            //         if (i == 1) {
            //             f = 1 + new Fraction(1, f);
            //         } else {
            //             f = 2 + new Fraction(1, f);
            //         }
            //     }
            //     if (digitLength(f.N) > digitLength(f.D)) {
            //         count++;
            //     }
            // }

            // Stolen solution
            // this guy noticed a pattern in the iterations and used that to calculate the iterations - thousands of times faster
            // n_k+1 = n_k + 2*d_k
            // d_k+1 = n_k+1 - d_k
            int count = 0;
            
            BigInteger n = 3;
            BigInteger d = 2;
            
            // from 1 because the first is already set = 3/2
            for (int i = 1; i < 1000; i++) {
                n += 2 * d;
                d = n - d;
                if (digitLength(n) > digitLength(d))
                    count++;
            }
            System.Console.WriteLine($"In first 1000 expansions of sqrt(2), numerator had more digits than denominator {count} times");
        }
        private int digitLength(BigInteger x) {
            return x.ToString().Length;
        }
    }
}