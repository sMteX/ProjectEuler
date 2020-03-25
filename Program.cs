using System;
using ProjectEuler.problems;

namespace ProjectEuler
{
    class Program
    {
        static void test() {

        }
        static void Main(string[] args)
        {
            // test();
            // return;

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
