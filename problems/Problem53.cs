using System;
using System.Numerics;

namespace ProjectEuler.problems
{
    public class Problem53 : Problem
    {
        public void run()
        {
            /*
            There are exactly ten ways of selecting three from five, 12345:

            123, 124, 125, 134, 135, 145, 234, 235, 245, and 345

            In combinatorics, we use the notation, (5 choose 3)=10

            In general, (n choose r) = n! / (r!(n−r)!), where r ≤ n, n! = n × (n−1) × ... × 3 × 2 × 1, and 0! = 1

            It is not until n = 23, that a value exceeds one-million: (23 choose 10) = 1144066

            How many, not necessarily distinct, values of (n choose r) for 1 ≤ n ≤ 100, are greater than one-million?
            */

            // instead of calculating binomial coefficients through factorials, it's way easier and faster to construct Pascal's triangle
            // we're constrained with n <= 100, so we need to construct 100 rows of values and count those larget than 1M

            BigInteger[,] triangle = new BigInteger[101, 101];
            for (int r = 0; r < 101; r++) {
                for (int c = 0; c < 101; c++) {
                    triangle[r,c] = 0;
                }
            }

            // Since Pascal's triangle is symmetrical, it could be optimized, but who cares
            for (int r = 0; r < 101; r++) {
                for (int c = 0; c <= r; c++) {
                    if (c == 0) {
                        triangle[r,c] = 1;
                    } else {
                        triangle[r,c] = triangle[r-1, c] + triangle[r-1, c-1];
                    }
                }
            }

            int count = 0;
            BigInteger largest = 0;
            int largestR = 0, largestC = 0;
            for (int r = 1; r <= 100; r++) {
                for (int c = 0; c <= r; c++) {
                    if (triangle[r,c] >= 1_000_000) {
                        count++;
                    }
                    if (triangle[r,c] > largest) {
                        largest = triangle[r,c];
                        largestR = r;
                        largestC = c;
                    }
                }
            }
            System.Console.WriteLine($"Number of binomial coefficients for n <= 100 larger than 1 million: {count}");
            System.Console.WriteLine($"Largest binomial coefficient: ({largestR} choose {largestC}) = {largest}");
        }
    }
}