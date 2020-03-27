using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectEuler.problems;

namespace ProjectEuler
{
    class Program
    {
        static void test() {
            // System.Console.WriteLine("Pre-making primes");
            // Helpers.createSieveFile(Helpers.DEFAULT_SIEVE_FILE, 2_000_000_000);
        }
        static void Main(string[] args)
        {
            // test();
            // return;

            // Note to primeSieve.dat = currently calculated for 0 - 2_000_000_000
            int latestProblem = 49;
            Type latest = null, type;
            while ((type = Type.GetType($"ProjectEuler.problems.Problem{latestProblem}")) != null) {
                latest = type;
                latestProblem++;
            }

            var problem = (Problem)Activator.CreateInstance(latest);
            problem.run();
        }
    }
}
