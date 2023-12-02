using Shared;
using Year2023.Common;

namespace Year2023.Day02;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		int totalRed = 12;
		int totalGreen = 13;
		int totalBlue = 14;

		foreach (string line in input.AsLines())
		{
			var a = line.Split(":", StringSplitOptions.TrimEntries);
			int gameNumber = int.Parse(a[0].Replace("Game", ""));
			var sets = a[1].Split(";", StringSplitOptions.TrimEntries);

			bool gameWorks = true;
			foreach(string set in sets)
			{
				var moves = set.Split(",", StringSplitOptions.TrimEntries);

				int moveRed = 0;
				int moveGreen = 0;
				int moveBlue = 0;
				
				foreach(var move in moves)
				{
					var splitMove = move.Split(" ");
					if (splitMove[1] == "blue")
					{
						moveBlue = int.Parse(splitMove[0]);
					}
					if (splitMove[1] == "red")
					{
						moveRed = int.Parse(splitMove[0]);
					}
					if (splitMove[1] == "green")
					{
						moveGreen = int.Parse(splitMove[0]);
					}
				}

				if(moveBlue > totalBlue || moveRed > totalRed || moveGreen > totalGreen)
				{
					gameWorks = false;
				}
			}

			if(gameWorks)
			{
				result += gameNumber;
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			var a = line.Split(":", StringSplitOptions.TrimEntries);
			int gameNumber = int.Parse(a[0].Replace("Game", ""));
			var sets = a[1].Split(";", StringSplitOptions.TrimEntries);

			int maxRed = 0;
			int maxGreen = 0;
			int maxBlue = 0;

			foreach (string set in sets)
			{
				var moves = set.Split(",", StringSplitOptions.TrimEntries);

				int moveRed = 0;
				int moveGreen = 0;
				int moveBlue = 0;

				foreach (var move in moves)
				{
					var splitMove = move.Split(" ");
					if (splitMove[1] == "blue")
					{
						moveBlue = int.Parse(splitMove[0]);
					}
					if (splitMove[1] == "red")
					{
						moveRed = int.Parse(splitMove[0]);
					}
					if (splitMove[1] == "green")
					{
						moveGreen = int.Parse(splitMove[0]);
					}
				}

				if(moveRed > maxRed) { maxRed = moveRed; }
				if(moveGreen > maxGreen) {  maxGreen = moveGreen; }
				if(moveBlue > maxBlue) {  maxBlue = moveBlue; }
			}

			result += (maxRed * maxGreen * maxBlue);
		}

		return result.ToString();
	}
}
