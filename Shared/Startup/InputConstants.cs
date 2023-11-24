namespace Shared.Startup;

public class InputConstants
{

	public static readonly string BaseDirectory = Environment.CurrentDirectory + "\\..\\..\\..\\..\\";

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
}
