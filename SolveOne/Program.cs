﻿using Shared;
using Shared.Startup;

namespace SolveOne;

internal class Program
{
	static readonly int YEAR = 2023;
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

		while (inputType == InputType.None)
		{
			Console.WriteLine("Choose problem input:");
			Console.WriteLine(" 1. Example 1 data");
			Console.WriteLine(" 2. Full data");
			Console.WriteLine(" 3. Custom data");
			Console.WriteLine(" 9. Change date");
			Console.WriteLine(" 21. Example 2 data");
			Console.Write("> ");

			string? choiceString = Console.ReadLine();

			switch (choiceString)
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
				case "21":
					inputType = InputType.Example2;
					break;
				default:
					PrintError("Invalid choice, try again");
					break;
			}
		}

		string? inputs = await InputService.GetInput(inputType, year, day);

		Console.WriteLine("Running part one...");
		string outputOne = await instance.PartOne(inputs);
		PrintOutput(outputOne);

		Console.WriteLine("Running part two...");
		string outputTwo = await instance.PartTwo(inputs);
		PrintOutput(outputTwo);
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

	private static void PrintOutput(string output)
	{
		Console.Write("Output is: ");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write(output);
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