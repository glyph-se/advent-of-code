using System.Runtime.CompilerServices;

namespace Shared.Startup;

public class InputConstants
{

	public static readonly string BaseDirectory = GetDirectoryForThisFile() + "\\..\\..\\";

	public static string PuzzleDirectory(int year, int day)
	{
		return $"Year{year}\\Day{day:D2}";
	}

	public static string FullInputPath(int year, int day)
	{
		return BaseDirectory + $"Year{year}\\Day{day:D2}\\full_input";
	}

	public static string Example1InputPath(int year, int day)
	{
		return BaseDirectory + $"Year{year}\\Day{day:D2}\\example1_input";
	}

	public static string Example2InputPath(int year, int day)
	{
		return BaseDirectory + $"Year{year}\\Day{day:D2}\\example2_input";
	}

	public static string Example3InputPath(int year, int day)
	{
		return BaseDirectory + $"Year{year}\\Day{day:D2}\\example3_input";
	}

	private static string GetDirectoryForThisFile([CallerFilePath] string callerFilePath = "")
	{
		return Path.GetDirectoryName(callerFilePath)!;
	}
}
