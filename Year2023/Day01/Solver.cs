namespace Year2023.Day01;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.ParseLines())
		{
			int? nr1 = null;
			int? nr2 = null;

			foreach (char a in line.ToCharArray())
			{

				if (int.TryParse(a.ToString(), out int i))
				{
					if (nr1 == null) nr1 = i;
					nr2 = i;
				}
			}

			try
			{
				result += (nr1.ToString() + nr2.ToString()).ToInt();
			}
			catch
			{
				// This will happen on example 2 data
				// Do nothing in that case
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.ParseLines())
		{
			int? nr1 = null;
			int? nr2 = null;

			string replacedLine = line
				.Replace("one", "one1one")
				.Replace("two", "two2two")
				.Replace("three", "three3three")
				.Replace("four", "four4four")
				.Replace("five", "five5five")
				.Replace("six", "six6six")
				.Replace("seven", "seven7seven")
				.Replace("eight", "eight8eight")
				.Replace("nine", "nine9nine");

			foreach (char a in replacedLine.ToCharArray())
			{
				if (int.TryParse(a.ToString(), out int i))
				{
					if (nr1 == null) nr1 = i;
					nr2 = i;
				}
			}

			result += (nr1.ToString() + nr2.ToString()).ToInt();
		}

		return result.ToString();
	}
}
