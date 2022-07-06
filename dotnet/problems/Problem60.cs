using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler.problems
{
    public class Problem60 : Problem
    {
        public void run()
        {
            /*
            The primes 3, 7, 109, and 673, are quite remarkable. By taking any two primes and concatenating them in any order the result will always be prime. For example, taking 7 and 109, both 7109 and 1097 are prime. The sum of these four primes, 792, represents the lowest sum for a set of four primes with this property.

            Find the lowest sum for a set of five primes for which any two primes concatenate to produce another prime.
            */
            BitArray sieve = Helpers.getSieve(Helpers.CURRENT_MAX_SIZE);
            int limit = 16_000;
            int maxLimit = Helpers.CURRENT_MAX_SIZE;
            bool found = false;

            int lowestSum = int.MaxValue;
            List<int> lowestPrimes = null;

            // 3-nested list of combinations for different n - precalculated
            // each combination is a list of indexes (another list)
            List<List<List<int>>> combinations = new List<List<List<int>>> {
                Helpers.getCombinations(2, 2).ToList(),
                Helpers.getCombinations(3, 2).ToList(),
                Helpers.getCombinations(4, 2).ToList(),
                Helpers.getCombinations(5, 2).ToList()
            };

            // we need early returns/skips - once any 2 primes don't make
            while (limit < maxLimit && !found) {
                // start from 3, because while 2 is a prime, if concatenated at the end, will result in even number = not a prime
                List<int> primes = Helpers.primes(3, limit, sieve).ToList();
                
                for (int i = 0; i < primes.Count - 4; i++) {
                    int p1 = primes[i];
                    for (int j = i + 1; j < primes.Count - 3; j++) {
                        int p2 = primes[j];
                        if (!allPrime(generateCombinations(combinations, p1, p2), sieve)) {
                            // combinations of p1 and p2 already aren't primes, no need to check further with this combination
                            continue;
                        }
                        for (int k = j + 1; k < primes.Count - 2; k++) {
                            int p3 = primes[k];
                            if (!allPrime(generateCombinations(combinations, p1, p2, p3), sieve)) {
                                continue;
                            }
                            for (int l = k + 1; l < primes.Count - 1; l++) {
                                int p4 = primes[l];
                                if (!allPrime(generateCombinations(combinations, p1, p2, p3, p4), sieve)) {
                                    continue;
                                }
                                for (int m = l + 1; m < primes.Count; m++) {
                                    int p5 = primes[m];
                                    if (!allPrime(generateCombinations(combinations, p1, p2, p3, p4, p5), sieve)) {
                                        continue;
                                    }
                                    int sum = p1 + p2 + p3 + p4 + p5;
                                    if (sum < lowestSum) {
                                        lowestSum = sum;
                                        lowestPrimes = new List<int> { p1, p2, p3, p4, p5 };
                                    }
                                }
                            }
                        }
                    }
                }
                found = (lowestPrimes != null);
                if (!found && limit < maxLimit / 2) {
                    limit *= 2;
                }
            }
            System.Console.WriteLine($"Lowest sum: {lowestSum} in primes: {string.Join(", ", lowestPrimes)}");

            // with infinite amount of time, this would work
            // while (limit < maxLimit && !found) {
            //     List<int> primes = Helpers.primes(2, limit, sieve).ToList();
            //     foreach (var combination in Helpers.getCombinations(primes.Count, 5)) {
            //         List<int> current5 = pick(primes, combination);
            //         List<long> allConcatenations = generateCombinations(current5);
            //         bool allPrime = allConcatenations.All(p => {
            //             if (p >= Helpers.CURRENT_MAX_SIZE) {
            //                 throw new System.ArgumentOutOfRangeException("We need bigger sieve");
            //             }
            //             return sieve[(int)p];
            //         });
            //         if (allPrime) {
            //             int sum = current5.Aggregate(0, (acc, cur) => acc + cur);
            //             if (sum < lowestSum) {
            //                 lowestSum = sum;
            //                 lowestPrimes = current5;
            //             }
            //         }
            //     }
            //     found = (lowestPrimes != null);
            //     if (!found && limit < maxLimit / 2) {
            //         limit *= 2;
            //     }
            // }
        }
        // private List<int> pick(List<int> primes, List<int> combination) {
        //     List<int> r = new List<int>();
        //     foreach (var i in combination) {
        //         r.Add(primes[i]);
        //     }
        //     return r;
        // }
        private bool allPrime (List<long> primes, BitArray sieve) {
            return primes.All(p => {
                if (p >= Helpers.CURRENT_MAX_SIZE) {
                    return false;
                }
                return sieve[(int)p];
            });
        }
        private List<long> generateCombinations(List<List<List<int>>> combinations, params int[] primes) {
            // takes a list of 5 primes, returns all their pairs and both concatenations
            // 5 primes => 10 combinations => 20 new concatenations which all must be prime
            List<long> result = new List<long>();
            foreach (var combination in combinations[primes.Length - 2]) {
                int p1 = primes[combination[0]];
                int p2 = primes[combination[1]];
                
                result.Add(long.Parse(p1.ToString() + p2.ToString()));
                result.Add(long.Parse(p2.ToString() + p1.ToString()));
            }
            return result;
        }
    }
}