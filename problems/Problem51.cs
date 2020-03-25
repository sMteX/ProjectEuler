using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler.problems
{
    public class Problem51 : Problem
    {
        List<bool> sieve = new List<bool>();

        public void run()
        {
            /*
            By replacing the 1st digit of the 2-digit number *3, it turns out that six of the nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.

            By replacing the 3rd and 4th digits of 56**3 with the same digit, this 5-digit number is the first example having seven primes among the ten generated numbers, yielding the family: 56003, 56113, 56333, 56443, 56663, 56773, and 56993. Consequently 56003, being the first member of this family, is the smallest prime with this property.

            Find the smallest prime which, by replacing part of the number (not necessarily adjacent digits) with the same digit, is part of an eight prime value family.
            */

            /* Remarks:
                - if first digit, it can't be replaced by zero (that leaves 9 more possibilities which allow for 8-prime family)
                - last digit can't be replaced (would have to be only odd numbers in order to even allow being prime, which leaves only 5 possibilities)
                - any other combination of wildcards can be possible probably
                - we're looking for the smallest prime in an 8-prime family
            */

            this.sieve = Helpers.getSieve(1_000_000);

            List<int> resultFamily = new List<int>();

            foreach (var prime in Helpers.primes(11, this.sieve)) {
                // we know the numbers in the family must be primes, so we can start searching from the smallest primes
                // at least I think
                bool found = false;
                foreach (var mask in getMasks(numberLength(prime))) {
                    var family = getFamily(prime, mask);
                    var primes = filterPrimes(family);
                    if (primes.Count == 8) {
                        resultFamily = primes;
                        found = true;
                        break;
                    }
                }
                if (found) {
                    break;
                }
            }
            System.Console.WriteLine($"8 prime family: {String.Join(", ", resultFamily)}, smallest: {resultFamily[0]}");
        }

        private int numberLength(int x) {
            int t = x;
            int n = 0;
            while (t > 0) {
                t /= 10;
                n++;
            }
            return n;
        }
        private List<int> filterPrimes(List<int> family) {
            return family.FindAll(f => this.sieve[f]);
        }
        private int replace(int number, List<bool> mask, int replacement) {
            var digits = new List<int>();
            int x = number;
            while (x > 0) {
                digits.Add(x % 10);
                x /= 10;
            }
            // numbers are REVERSED (1's are [0], 10's are [1] etc.)
            // it's easier to reconstruct the number this way though
            for (int i = 0; i < mask.Count; i++) {
                if (mask[i]) {
                    digits[digits.Count - 1 - i] = replacement;
                }
            }
            int r = 0;
            for (int i = 0; i < digits.Count; i++) {
                r += digits[i] * (int)Math.Pow(10, i);
            }
            return r;
        }
        private List<int> getFamily(int number, List<bool> mask) {
            // generate all possible numbers that result from replacing parts of the number according to the mask
            var family = new List<int>();
            int start = (mask[0]) ? 1 : 0; // if the first digit is to be replaced, it can't be 0

            for (int i = start; i <= 9; i++) {
                family.Add(this.replace(number, mask, i));
            }

            return family;
        }
        private IEnumerable<List<bool>> getMasks(int length) {
            // <1 - 2^(length-1))
            int end = (int)Math.Pow(2, length - 1);
            for (int i = 1; i < end; i++) {
                var bitArray = new BitArray(BitConverter.GetBytes(i));
                var list = new List<bool>();
                
                foreach (bool bit in bitArray) {
                    list.Add(bit);
                }

                // 32 bits, from LEAST SIGNIFICANT
                list = list.Take(length - 1).ToList();
                // now contains "length - 1" least significant bits, reverse, add false to the end
                list.Reverse();
                list.Add(false);
                yield return list;
            }
        }
    }
}