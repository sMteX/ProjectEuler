using System.Collections;

namespace ProjectEuler.problems
{
    public class Problem58 : Problem
    {
        public void run()
        {
            /*
            
            Starting with 1 and spiralling anticlockwise in the following way, a square spiral with side length 7 is formed.

            37 36 35 34 33 32 31
            38 17 16 15 14 13 30
            39 18  5  4  3 12 29
            40 19  6  1  2 11 28
            41 20  7  8  9 10 27
            42 21 22 23 24 25 26
            43 44 45 46 47 48 49

            It is interesting to note that the odd squares lie along the bottom right diagonal, 
            but what is more interesting is that 8 out of the 13 numbers lying along both diagonals are prime; that is, a ratio of 8/13 ≈ 62%.

            If one complete new layer is wrapped around the spiral above, a square spiral with side length 9 will be formed. 
            If this process is continued, what is the side length of the square spiral for which the ratio of primes 
            along both diagonals first falls below 10%?
 
            */

            BitArray sieve = Helpers.getSieve(1_000_000_000);

            // numbers on the diagonal can be expressed as a function of the square size
            int allCount = 1; // 1 in the middle, starting number
            int primeCount = 0;
            int n = 3;
            bool skipFirst = true; // because in the first iteration (only 1), it's 0 primes, 1 all numbers
            while (skipFirst || primeCount / (double)allCount >= 0.1) {
                if (skipFirst) {
                    skipFirst = false;
                }
                long br = n * n;
                long tr = br - 3*n + 3;
                long tl = br - 2*n + 2;
                long bl = br - n + 1;

                // corners are longs, sieve is indexed by unsigned integers, so check for if the number is in the sieve at all, or if it's larger than int 
                // since we would've come into problem where either of the 4 values are invalid, we need only to check the largest (bottom right)
                // if that one's ok, all 4 are ok
                if (br > uint.MaxValue || br >= sieve.Count ) {
                    System.Console.WriteLine("We need more primes");
                    throw new System.OverflowException();
                }

                allCount += 4;
                if (sieve[(int)br]) {
                    primeCount++;
                }
                if (sieve[(int)tr]) {
                    primeCount++;
                }
                if (sieve[(int)tl]) {
                    primeCount++;
                }
                if (sieve[(int)bl]) {
                    primeCount++;
                }

                n += 2;
            }
            System.Console.WriteLine($"Ratio of primes to all numbers on diagonals dropped below 10% at size {n - 2}");
        }
    }
}