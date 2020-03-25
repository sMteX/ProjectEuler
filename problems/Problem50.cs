using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler.problems
{
    class Problem50: Problem
    {
        private List<bool> sieve = new List<bool>();

        public void run()
        {
            // The prime 41, can be written as the sum of six consecutive primes:
            //     41 = 2 + 3 + 5 + 7 + 11 + 13

            // This is the longest sum of consecutive primes that adds to a prime below one-hundred.

            // The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953.

            // Which prime, below one-million, can be written as the sum of the most consecutive primes?
            
            int maxStarting = 2;
            int maxPrime = 2;
            int maxCount = 0;

            this.sieve = Helpers.getSieve(1_000_000);

            // basically iterate through all consecutive prime sums that sum up to 1 million
            foreach (var prime in this.primes(2)) {
                int sum = prime;
                int count = 1;

                int localMaxSum = prime;
                int localMaxCount = 1;

                foreach (var nextPrime in this.primes(prime + 1)) {
                    if (sum + nextPrime >= 1_000_000) {
                        break;
                    }
                    sum += nextPrime;
                    count++;
                    // since the sum increases, and so does the count, keep track of the latest (and largest) sum and respective prime count
                    if (this.sieve[sum]) {
                        localMaxSum = sum;
                        localMaxCount = count;
                    }
                }
                // keep track of the longest sum and where it started
                if (localMaxCount > maxCount) {
                    maxCount = localMaxCount;
                    maxPrime = localMaxSum;
                    maxStarting = prime;
                }
            }
            Console.WriteLine("Largest consecutive prime sum below 1 000 000:");
            Console.WriteLine($"Max prime: {maxPrime}, starting prime: {maxStarting}, count: {maxCount}");

        }
        private IEnumerable<int> primes(int from) {
            for (int i = from; i < this.sieve.Count; i++) {
                if (this.sieve[i]) {
                    yield return i;
                }
            }
        }
    }
}
