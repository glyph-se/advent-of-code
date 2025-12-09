namespace Year2023.Day14;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		Rock[,] grid = input.ParseGridMatrix((c, x, y) => new Rock(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new Rock('#', x, y));

		RollUp(grid);

		// Count points
		foreach (Rock r in grid)
		{
			if (r.c == 'O')
			{
				int score = grid.GetLength(1) - r.y - 1;
				result += score;
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		Rock[,] grid = input.ParseGridMatrix((c, x, y) => new Rock(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new Rock('#', x, y));

		long target = 1000000000;

		var cycles = CycleHelpers.FindCycle(grid => CycleOne(grid), grid, grid => grid.AsString());

		for (long i = 1; i <= target; i++)
		{
			if (i == cycles.start)
			{
				long repetitions = (target - cycles.start) / cycles.length;
				i += repetitions * cycles.length;
			}
			grid = CycleOne(grid);
		}

		// Count points
		foreach (Rock r in grid)
		{
			if (r.c == 'O')
			{
				int score = grid.GetLength(1) - r.y - 1;
				result += score;
			}
		}

		return result.ToString();
	}

	private Rock[,] CycleOne(Rock[,] grid)
	{
		RollUp(grid);
		grid = grid.RotateMatrixCounterClockwise();
		RollUp(grid);
		grid = grid.RotateMatrixCounterClockwise();
		RollUp(grid);
		grid = grid.RotateMatrixCounterClockwise();
		RollUp(grid);
		grid = grid.RotateMatrixCounterClockwise();

		return grid;
	}

	private static void RollUp(Rock[,] grid)
	{
		// Go through each column
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			// Go through each row in the column
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				var rock = grid[i, j];

				if (rock.c == 'O')
				{
					// Move upwards
					for (int k = j - 1; k >= 0; k--)
					{
						var candidate = grid[i, k];
						if (candidate.c == '#' || candidate.c == 'O')
						{
							// Hit a wall, abort.
							break;
						}

						if (candidate.c == '.')
						{
							// Move
							int oldY = candidate.y;
							candidate.y = rock.y;
							rock.y = oldY;

							grid[i, candidate.y] = candidate;
							grid[i, rock.y] = rock;
						}

					}
				}
			}
		}
	}

	public class Rock(char c, int x, int y) : CharPoint(c, x, y)
	{
	}
}
