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

		foreach (string line in input.ParseLines())
		{
			var a = line.Split(":", StringSplitOptions.TrimEntries);
			int gameNumber = a[0].ReplaceRemove("Game").ToInt();
			var sets = a[1].Split(";", StringSplitOptions.TrimEntries);

			bool gameWorks = true;
			foreach (string set in sets)
			{
				var moves = set.Split(",", StringSplitOptions.TrimEntries);

				int moveRed = 0;
				int moveGreen = 0;
				int moveBlue = 0;

				foreach (var move in moves)
				{
					var splitMove = move.Split(" ");
					switch (splitMove[1])
					{
						case "blue":
							moveBlue = splitMove[0].ToInt();
							break;
						case "red":
							moveRed = splitMove[0].ToInt();
							break;
						case "green":
							moveGreen = splitMove[0].ToInt();
							break;
					}
				}

				if (moveBlue > totalBlue || moveRed > totalRed || moveGreen > totalGreen)
				{
					gameWorks = false;
				}
			}

			if (gameWorks)
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

		foreach (string line in input.ParseLines())
		{
			var a = line.Split(":", StringSplitOptions.TrimEntries);
			int gameNumber = a[0].ReplaceRemove("Game").ToInt();
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
					switch (splitMove[1])
					{
						case "blue":
							moveBlue = splitMove[0].ToInt();
							break;
						case "red":
							moveRed = splitMove[0].ToInt();
							break;
						case "green":
							moveGreen = splitMove[0].ToInt();
							break;
					}
				}

				if (moveRed > maxRed) { maxRed = moveRed; }
				if (moveGreen > maxGreen) { maxGreen = moveGreen; }
				if (moveBlue > maxBlue) { maxBlue = moveBlue; }
			}

			result += (maxRed * maxGreen * maxBlue);
		}

		return result.ToString();
	}
}
