using System.Text;
using Shared;
using Shared.Helpers;

namespace Year2023.Day14;

public class Solver : ISolver
{
	private Dictionary<string, Rock[,]> DpCache = new Dictionary<string, Rock[,]>();

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		Rock[,] grid = input.AsGridMatrix((c, x, y) => new Rock(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new Rock('#', x, y));

		RollUp(grid);

		// Count points
		foreach (Rock r in grid)
		{
			if(r.c == 'O')
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

		Rock[,] grid = input.AsGridMatrix((c, x, y) => new Rock(c, x, y));
		grid = grid.ExtendGridMatrix(1, (x, y) => new Rock('#', x, y));

		long target = 1000000000;
		long cycle = 27*7;

		// example1 has cycle 7
		// full has cycle 27
		// TODO add cycle detection

		for (long i = 1; i <= target; i++)
		{
			if(i == 1_000)
			{
				// Jump a head
				i += (target - i) / cycle * cycle;
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
		grid = RotateMatrixCounterClockwise(grid);
		RollUp(grid);
		grid = RotateMatrixCounterClockwise(grid);
		RollUp(grid);
		grid = RotateMatrixCounterClockwise(grid);
		RollUp(grid);
		grid = RotateMatrixCounterClockwise(grid);

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
					for (int k = j-1; k >= 0; k--)
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

	/// <summary>
	/// Taken from https://stackoverflow.com/a/18035050
	/// </summary>
	static Rock[,] RotateMatrixCounterClockwise(Rock[,] oldMatrix)
	{
		Rock[,] newMatrix = new Rock[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
		int newColumn, newRow = 0;
		for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
		{
			newColumn = 0;
			for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
			{
				newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
				newMatrix[newRow, newColumn].x = newRow;
				newMatrix[newRow, newColumn].y = newColumn;
				newColumn++;
			}
			newRow++;
		}
		return newMatrix;
	}

	public static string GridToStr(Rock[,] grid)
	{
		StringBuilder sb = new StringBuilder();
		for (int y = 0; y < grid.GetLength(1); y++)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
			{
				Rock current = grid[x, y];

				sb.Append(current.c);
			}
			sb.AppendLine();
		}

		return sb.ToString();
	}
}
