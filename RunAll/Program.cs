using System.Reflection;
using AdventOfCode;

namespace RunAll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<ISolver> allSolvers = Assembly.Load("AdventOfCode")
                .GetTypes()
                .Where(t => t.IsClass)
                .Where(t => typeof(ISolver).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t))
                .OfType<ISolver>();

            foreach (ISolver solver in allSolvers)
            {
                string[] namespaces = solver
                    .GetType()
                    .Namespace
                    .Split('.');

                int year = int.Parse(namespaces[1].Substring(4));
                int day = int.Parse(namespaces[2].Substring(3));

                Console.WriteLine($"Running {year} / {day}");
            }
        }
    }
}