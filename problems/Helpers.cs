using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler
{
    public static class Helpers
    {
        public static List<bool> getSieve(int limit)
        {
            var primes = Enumerable.Repeat(true, limit).ToList();
            primes[0] = primes[1] = false;
            // Eratosthene sieve
            for (int i = 2; i < primes.Count; i++) {
                if (!primes[i]) {
                    continue;
                }
                // primes[i] is prime, remove multiples
                for (int j = 2 * i; j < primes.Count; j += i) {
                    primes[j] = false;
                }
            }
            return primes;
        }
        public static IEnumerable<int> primes(int from, List<bool> sieve) {
            for (int i = from; i < sieve.Count; i++) {
                if (sieve[i]) {
                    yield return i;
                }
            }
        }
        public static int[] getDigits(long x) {
            var digits = new int[10];
            Array.Fill(digits, 0);

            var temp = x;
            while (temp > 0) {
                digits[temp % 10]++;
                temp /= 10;
            }
            return digits;
        }
        public static bool arePermutations(long x, long y)
        {
            return Enumerable.SequenceEqual(getDigits(x), getDigits(y));
        }
        public static bool arePermutations(int[] x, int[] y) {
            return Enumerable.SequenceEqual(x, y);
        }
    }
}