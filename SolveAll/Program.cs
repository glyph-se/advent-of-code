using System.Reflection;
using Shared;
using Shared.Startup;

namespace RunAll;

internal class Program
{
	static async Task Main(string[] args)
	{
		Console.WriteLine("Running all puzzles");

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
					.Namespace!
					.Split('.');

			int year = int.Parse(namespaces[1].Substring(4));
			int day = int.Parse(namespaces[2].Substring(3));

			Console.Write($"{year}/{day:D2} ");


			string puzzlieDirectory = InputConstants.BaseDirectory + "..\\" + InputConstants.PuzzleDirectory(year, day) + "\\";

			Console.Write("Example1 ");
			await RunInput(solver, puzzlieDirectory + "example1_input");
			Console.Write("Full ");
			await RunInput(solver, puzzlieDirectory + "full_input");

			Console.WriteLine();
		}
	}

	private static async Task RunInput(ISolver solver, string inputPath)
	{
		if (File.Exists(inputPath))
		{
			string input = File.ReadAllText(inputPath);

			string partOneAnswer = inputPath.Replace("_input", "_answer_partone");
			string partTwoAnswer = inputPath.Replace("_input", "_answer_parttwo");

			if (File.Exists(partOneAnswer))
			{
				Console.Write("Part one: ");
				string expected = File.ReadAllText(partOneAnswer);
				string actual = await solver.PartOne(input);

				if (expected.Equals(actual))
				{
					Console.Write("OK  ");
				}
				else
				{
					Console.Write("FAIL");
				}
			}

			Console.Write("  ");

			if (File.Exists(partTwoAnswer))
			{
				Console.Write("Part two: ");

				string expected = File.ReadAllText(partTwoAnswer);
				string actual = await solver.PartTwo(input);

				if (expected.Equals(actual))
				{
					Console.Write("OK  ");
				}
				else
				{
					Console.Write("FAIL");
				}
			}

			Console.Write("  ");
		}
	}
}