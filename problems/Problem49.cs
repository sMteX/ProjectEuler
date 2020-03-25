using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler.problems
{
    class Problem49: Problem
    {
        List<bool> sieve = new List<bool>();

        public void run()
        {
            // The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways: (i) each of the three terms are prime, and, (ii) each of the 4-digit numbers are permutations of one another.
            // There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property, but there is one other 4-digit increasing sequence.
            // What 12-digit number do you form by concatenating the three terms in this sequence?


            this.sieve = Helpers.getSieve(10_000);
            foreach (var a in this.primes(1001)) {
                // loop through all possible distances to subsequent primes, check for 3-length sequencee
                foreach (var d in this.getDistances(a))
                {
                    if (this.conditions(a, d)) {
                        System.Console.WriteLine($"{a}, {a + d}, {a + 2 * d}");
                    }
                }
            }
            // solution: 296962999629
        }

        private bool conditions(int a, int d) {
            // a is prime (we loop through all primes)
            // a + d is prime (d is distance to nearest larger prime)
            
            return a + 2*d < 10_000 && a + 3*d >= 10_000 // this must be true to form a 3-length sequence
                && this.sieve[a + 2*d] // a + 2d must be prime too to form 3-prime sequence
                && arePermutations(a, d); // all 3 must be permutations of one another
        }
        private IEnumerable<int> primes(int from) {
            for (int i = from; i < this.sieve.Count; i++) {
                if (this.sieve[i]) {
                    yield return i;
                }
            }
        }
        private static bool arePermutations(int a, int d)
        {
            return Helpers.arePermutations(a, a + d) && Helpers.arePermutations(a + d, a + 2 * d);
        }

        private IEnumerable<int> getDistances(int prime)
        {
            for (int i = prime + 1; i < this.sieve.Count; i++)
            {
                if (this.sieve[i])
                {
                    yield return i - prime;
                }
            }
        }
    }
}