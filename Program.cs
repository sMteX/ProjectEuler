using System;

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

            int currentProblem = 55;

            var type = Type.GetType($"ProjectEuler.Problem{currentProblem}");
            if (type == null) {
                type = Type.GetType($"ProjectEuler.problems.Problem{currentProblem}");
            } 
            if (type == null) {
                System.Console.WriteLine("Problem doesn't exist");
            } else {
                var problem = (Problem)Activator.CreateInstance(type);
                problem.run();
            }
        }
    }
}
