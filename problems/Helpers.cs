using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectEuler.problems
{
    public static class Helpers
    {
        public const string DEFAULT_SIEVE_FILE = "problems/primeSieve.dat";

        public static void createSieveFile(int limit, string fileName = DEFAULT_SIEVE_FILE) {
            // tired of trying to find an effective prime sieve, let's bruteforce this shit
            // basically just runs an Eratosthenes sieve (which is pretty slow), and STORES THE RESULT in a file
            var primes = new BitArray(limit, true);
            primes[0] = primes[1] = false;
            // Eratosthene sieve
            int sqrt = (int)Math.Sqrt((double)limit);
            for (int i = 2; i < sqrt; i++) {
                if (!primes[i]) {
                    continue;
                }
                // primes[i] is prime, remove multiples
                for (int j = 2 * i; j >= 0 && j < primes.Count; j += i) {
                    primes[j] = false;
                }
            }

            int nBytes = primes.Length / 8;
            if (primes.Length % 8 != 0) {
                nBytes++;
            }

            var bytes = new byte[nBytes];
            primes.CopyTo(bytes, 0);

            File.WriteAllBytes(fileName, bytes);
        }
        public static BitArray readSieveFile(string fileName = DEFAULT_SIEVE_FILE) {
            if (!File.Exists(fileName)) {
                throw new ArgumentException("File doesn't exist");
            }
            byte[] bytes = File.ReadAllBytes(fileName);
            return new BitArray(bytes);
        }
        public static BitArray getSieve(int limit, string file = DEFAULT_SIEVE_FILE) {
            if (!File.Exists(file)) {
                throw new ArgumentException("File doesn't exist");
            }
            BitArray bigSieve = readSieveFile(file);
            if (limit > bigSieve.Length) {
                throw new ArgumentOutOfRangeException("Not enough precalculated values, need a bigger sieve");
            }
            BitArray target = new BitArray(limit);
            for (int i = 0; i < limit; i++) {
                target[i] = bigSieve[i];
            }
            return target;
        }
        public static IEnumerable<int> primes(int from, BitArray sieve) {
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