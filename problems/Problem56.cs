using System.Numerics;

namespace ProjectEuler.problems
{
    public class Problem56 : Problem
    {
        public void run()
        {
            /*
            A googol (10^100) is a massive number: one followed by one-hundred zeros; 100^100 is almost unimaginably large: one followed by two-hundred zeros. 
            Despite their size, the sum of the digits in each number is only 1.

            Considering natural numbers of the form, a^b, where a, b < 100, what is the maximum digital sum?
            */

            long maxSum = 0;
            BigInteger max = 0;
            int maxA = 0;
            int maxB = 0;

            for (int a = 1; a < 100; a++) {
                for (int b = 1; b < 100; b++) {
                    var result = BigInteger.Pow(a, b);
                    long sum = digitSum(result);
                    if (sum > maxSum) {
                        maxSum = sum;
                        maxA = a;
                        maxB = b;
                        max = result;
                    }
                }
            }
            System.Console.WriteLine($"Max digit sum: {maxA}^{maxB} = {max.ToString("N0")} => {maxSum}");
        }
        private long digitSum(BigInteger x) {
            string s = x.ToString();
            long r = 0;
            foreach (var c in s) {
                r += (int)char.GetNumericValue(c);
            }
            return r;
        }
    }
}