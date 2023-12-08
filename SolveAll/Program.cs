using System.Reflection;
using Shared;
using Shared.Startup;

namespace SolveAll;

internal class Program
{
	static async Task Main(string[] args)
	{
		Console.Write("--------------------------------------------------------------------------------\n");
		Console.Write("|                                      ALL                                     |\n");
		Console.Write("--------------------------------------------------------------------------------\n");

		IEnumerable<ISolver> allSolvers = Assembly.Load("Year2023")
				.GetTypes()
				.Where(t => t.IsClass)
				.Where(t => typeof(ISolver).IsAssignableFrom(t))
				.OrderBy(t => t.FullName)
				.Select(t => Activator.CreateInstance(t))
				.OfType<ISolver>();

		foreach (ISolver solver in allSolvers)
		{
			string[] namespaces = solver
					.GetType()
					.Namespace!
					.Split('.');

			int year = int.Parse(namespaces[0].Substring(4));
			int day = int.Parse(namespaces[1].Substring(3));

			Console.Write($"{year}/{day:D2} ");


			Console.Write("Example1 ");
			await RunInput(solver, InputConstants.Example1InputPath(year, day));
			Console.Write("Full ");
			await RunInput(solver, InputConstants.FullInputPath(year, day));

			Console.WriteLine();
		}
	}

	private static async Task RunInput(ISolver solver, string inputPath)
	{
		if (!File.Exists(inputPath))
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("N/A ");
			Console.ResetColor();
			return;
		}

		string input = await InputService.ReadFileAsync(inputPath);

		string partOneAnswer = inputPath.Replace("_input", "_answer_partone");
		string partTwoAnswer = inputPath.Replace("_input", "_answer_parttwo");

		if (File.Exists(partOneAnswer))
		{
			Console.Write("Part one: ");
			string expected = await InputService.ReadFileAsync(partOneAnswer);
			string actual = await solver.PartOne(input);

			if (expected.Equals(actual))
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("OK  ");
				Console.ResetColor();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("FAIL");
				Console.ResetColor();
				Console.Write($" {expected} vs {actual}");
			}
		}

		Console.Write("  ");

		if (File.Exists(partTwoAnswer))
		{
			Console.Write("Part two: ");

			string expected = await InputService.ReadFileAsync(partTwoAnswer);
			string actual = await solver.PartTwo(input);

			if (expected.Equals(actual))
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("OK  ");
				Console.ResetColor();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("FAIL");
				Console.ResetColor();
				Console.Write($" {expected} vs {actual}");
			}
		}

		Console.Write("  ");
	}
}