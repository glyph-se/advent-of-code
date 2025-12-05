using Shared;

namespace Year2025.Day04;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('.', x, y));

		result = RemoveableRolls(grid, false);

		return result.ToString();
	}



	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new CharPoint('.', x, y));

		long removedThisRound = 0;

		do
		{
			removedThisRound = RemoveableRolls(grid, true);

			result += removedThisRound;

		} while (removedThisRound > 0);

		return result.ToString();
	}

	private static long RemoveableRolls(CharPoint[,] grid, bool removeRoll)
	{
		long result = 0;

		for (int y = 1; y < grid.LengthY(); y++)
		{
			for (int x = 1; x < grid.LengthX(); x++)
			{
				if (grid[x, y].c != '@')
				{
					continue;
				}

				var neighbors = GridHelpers.AllDirs()
					.Select(dir => grid[x + dir.dx, y + dir.dy])
					.Where(p => p.c == '@')
					.Count();

				if (neighbors < 4)
				{
					result++;

					if (removeRoll)
					{
						grid[x, y].c = '.';
					}
				}
			}
		}

		return result;
	}
}
