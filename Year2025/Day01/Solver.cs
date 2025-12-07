namespace Year2025.Day01;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		int pos = 50;

		long result = 0;

		foreach (string line in input.AsLines())
		{
			string dir = line.Substring(0, 1);
			int dist = line.Substring(1).ToInt();

			if (dir == "R")
			{
				pos += dist;
			}
			else if (dir == "L")
			{
				pos -= dist;
			}

			pos = pos % 100;

			if (pos == 0)
			{
				result++;
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		int pos = 50;

		long result = 0;

		foreach (string line in input.AsLines())
		{
			string dir = line.Substring(0, 1);
			int dist = line.Substring(1).ToInt();

			if (dir == "R")
			{
				for (int i = 0; i < dist; i++)
				{
					pos++;
					if (pos == 100)
					{
						result++;
						pos = 0;
					}
				}
			}
			else if (dir == "L")
			{
				for (int i = 0; i < dist; i++)
				{
					pos--;
					if (pos == 0)
					{
						result++;
					}
					if (pos == -1)
					{
						pos = 99;
					}
				}
			}
		}

		return result.ToString();
	}
}
