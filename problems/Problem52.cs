using System.Collections.Generic;

namespace ProjectEuler.problems
{
    public class Problem52 : Problem
    {
        public void run()
        {
            /*
            It can be seen that the number, 125874, and its double, 251748, contain exactly the same digits, but in a different order.

            Find the smallest positive integer, x, such that 2x, 3x, 4x, 5x, and 6x, contain the same digits.
            */

            for (long x = 1; x < long.MaxValue / 6; x++) {
                var digits = new List<int[]>();
                bool valid = true;
                for (int i = 2; i <= 6; i++) {
                    digits.Add(Helpers.getDigits(i * x));
                    if (i > 2) {
                        // check if last 2 multiples are permutations - if they're not, no reason to keep extracting
                        if (!Helpers.arePermutations(digits[digits.Count - 1], digits[digits.Count - 2])) {
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid) {
                    System.Console.WriteLine($"Smallest positive integer is {x}. Multiples:");
                    for (int i = 2; i <= 6; i++) {
                        System.Console.WriteLine($"{i}x = {i * x}");
                    }
                    break;
                }
            }
        }
    }
}