using System.Diagnostics;
using Shared;
using Shared.Startup;

namespace SolveOne;

internal class Program
{
	static readonly int YEAR = 2025;
	static readonly int DAY = 4;

	public static async Task Main(string[] args)
	{
		PrintHeader(YEAR, DAY);

		await SolveOneDayAsync(YEAR, DAY);

		PrintFooter();
	}

	private static async Task SolveOneDayAsync(int year, int day)
	{
		ISolver? instance = GetSolverInstance(year, day);

		if (instance == null)
		{
			PrintError("Solver not found!");
			return;
		}

		InputType inputType = InputType.None;
		string? choiceString = null;

		while (inputType == InputType.None)
		{
			PrintChoiceMenu();

			choiceString = Console.ReadLine();

			switch (choiceString?.TrimEnd(['a', 'b']))
			{
				case "1":
					inputType = InputType.Example1;
					break;
				case "2":
					inputType = InputType.Full;
					break;
				case "3":
					inputType = InputType.Custom;
					break;
				case "9":
					while (true)
					{
						PrintWarning("Change date");
						Console.WriteLine("Year?");
						Console.Write("> ");
						string? newYear = Console.ReadLine();
						Console.WriteLine("Day?");
						Console.Write("> ");
						string? newDay = Console.ReadLine();
						if (int.TryParse(newYear, out int parsedNewYear) && int.TryParse(newDay, out int parsedNewDay))
						{
							PrintWarning($"Changing to {parsedNewYear}/{parsedNewDay}");
							await SolveOneDayAsync(parsedNewYear, parsedNewDay);
						}
					}
				case "22":
					inputType = InputType.Example2;
					break;
				case "23":
					inputType = InputType.Example3;
					break;
				default:
					PrintError("Invalid choice, try again");
					break;
			}
		}

		string? inputs = await InputService.GetInput(inputType, year, day);

		string? outputOne = null;
		string? outputTwo = null;

		Stopwatch stopwatch = new Stopwatch();

		if (!choiceString!.EndsWith('b'))
		{
			Console.WriteLine("Running part one...");
			stopwatch.Restart();
			outputOne = await instance.PartOne(inputs);
			PrintOutput(outputOne, stopwatch.ElapsedMilliseconds);
		}

		if (!choiceString!.EndsWith('a'))
		{
			Console.WriteLine("Running part two...");
			stopwatch.Restart();
			outputTwo = await instance.PartTwo(inputs);
			PrintOutput(outputTwo, stopwatch.ElapsedMilliseconds);
		}

		CopyOutputToClipboard(outputOne, outputTwo);
	}

	private static void CopyOutputToClipboard(string? outputOne, string? outputTwo)
	{
		if (!(string.IsNullOrWhiteSpace(outputTwo) || outputTwo.Equals("0")))
		{
			WindowsClipboard.SetText(outputTwo);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("Copied part two to clipboard");
			Console.ResetColor();
		}
		else if (!(string.IsNullOrWhiteSpace(outputOne) || outputOne.Equals("0")))
		{
			WindowsClipboard.SetText(outputOne);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("Copied part one to clipboard");
			Console.ResetColor();
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("Nothing to copy");
			Console.ResetColor();
		}
		Console.WriteLine();
	}

	private static void PrintChoiceMenu()
	{
		Console.Write("Choose problem input:");
		Console.Write("\t\t\t\t");
		Console.Write("Advanced:");
		Console.WriteLine();
		Console.Write(" 1. Example 1 data");
		Console.Write("\t\t\t\t");
		Console.Write(" 9. Change date");
		Console.WriteLine();

		Console.Write(" 2. Full data");
		Console.Write("\t\t\t\t\t");
		Console.Write(" 22. Example 2 data");
		Console.WriteLine();
		Console.Write(" 3. Custom data");
		Console.Write("\t\t\t\t\t");
		Console.Write(" 23. Example 3 data");
		Console.WriteLine();

		Console.WriteLine();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Suffix with 'a' or 'b' for only part one / part two");
		Console.ResetColor();
		Console.WriteLine();
		Console.WriteLine();

		Console.Write("> ");
	}

	private static void PrintWarning(string text)
	{
		Console.WriteLine();
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(text);
		Console.ResetColor();
		Console.WriteLine();
	}

	private static void PrintFooter()
	{
		Console.Write("--------------------------------------------------------------------------------\n");
	}

	private static void PrintOutput(string output, long elapsedTime)
	{
		Console.Write("Output is: ");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write(output);
		Console.ResetColor();
		Console.WriteLine();
		Console.ForegroundColor= ConsoleColor.DarkGray;
		Console.Write($"{elapsedTime} ms");
		Console.ResetColor();
		Console.WriteLine();
		Console.WriteLine();
	}

	private static ISolver? GetSolverInstance(int year, int day)
	{
		Type? solverType = Type.GetType($"Year{year}.Day{day:D2}.Solver, Year{year}");

		if (solverType == null)
		{
			PrintError("Check constants (type not found)!");
			return null;
		}

		ISolver? solverInstance = Activator.CreateInstance(solverType) as ISolver;

		if (solverInstance == null)
		{
			PrintError("Check constants (constructor failed)!");
			return null;
		}

		return solverInstance;
	}

	private static void PrintError(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(message);
		Console.ResetColor();
	}

	private static void PrintHeader(int year, int day)
	{
		Console.Write("---------------------------------------------------------------------------------\n");
		Console.Write("|                              Advent of Code                                   |\n");
		Console.Write("---------------------------------------------------------------------------------\n");
		Console.Write("Year: ");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write(year);
		Console.ResetColor();
		Console.Write(" ");
		Console.Write("Day: ");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write(day);
		Console.ResetColor();
		Console.WriteLine();
		Console.WriteLine();
	}
}