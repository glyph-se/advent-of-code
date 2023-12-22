using System.Diagnostics;
using Shared;
using Shared.Helpers;
using Part = Shared.Helpers.CharPoint;

namespace Year2023.Day03;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		Part?[,] grid = input.AsGridMatrix((c, x, y) => CreatePart(c, x, y));
		grid = grid.ExtendGridMatrixWithNull(1);


		for (int y = 1; y < grid.GetLength(0); y++)
		{
			for (int x = 1; x < grid.GetLength(1); x++)
			{
				var cell = grid[x, y];
				if (cell == null) continue;

				int number = 0;
				bool adjacent = false;
				if (cell.c.IsDigit())
				{
					number = cell.c.ToString().ToInt();

					foreach (var dirs in GridHelpers.AllDirs())
					{
						var check = grid[x + dirs.dx, y + dirs.dy];
						if (check != null && !check.c.IsDigit())
						{
							adjacent = true;
						}
					}

					// Get rest of number
					for (int nx = x + 1; true; nx++)
					{
						var nCell = grid[nx, y];

						if (nCell == null) break;

						if (nCell.c.IsDigit())
						{
							number *= 10;
							number += nCell.c.ToString().ToInt();
						}
						else
						{
							break;
						}

						foreach (var dirs in GridHelpers.AllDirs())
						{
							var check = grid[nx + dirs.dx, y + dirs.dy];
							if (check != null && !check.c.IsDigit())
							{
								adjacent = true;
							}
						}

						x++;
					}
				}

				if (adjacent)
				{
					result += number;
				}
			}
		}


		return result.ToString();
	}

	private Part? CreatePart(char c, int x, int y)
	{
		if (c == '.')
			return null;
		return new Part(c, x, y);
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		Part?[,] grid = input.AsGridMatrix((c, x, y) => CreatePart(c, x, y));
		grid = grid.ExtendGridMatrixWithNull(1);

		HashSet<Part> visited = new HashSet<Part>();

		for (int y = 1; y < grid.GetLength(0); y++)
		{
			for (int x = 1; x < grid.GetLength(1); x++)
			{
				var cell = grid[x, y];
				if (cell == null) continue;

				if (cell.c == '*')
				{
					int n1 = 0;
					int n2 = 0;

					foreach (var dirs in GridHelpers.AllDirs())
					{
						var checkForNumber = grid[x + dirs.dx, y + dirs.dy];
						if(checkForNumber == null)
							continue;
						if (visited.Contains(checkForNumber))
							continue;

						if (checkForNumber != null && checkForNumber.c.IsDigit())
						{
							// Find start of number by walking left
							int numberStart = 0;
							for (int startX = checkForNumber.x; true; startX--)
							{
								Part? checkForStart = grid[startX, checkForNumber.y];

								if (checkForStart == null || !checkForStart.c.IsDigit())
								{
									// Number starts on startX + 1
									numberStart = startX + 1;
									break;
								}
							}

							Part start = grid[numberStart, checkForNumber.y]!;

							int number = start.c.ToString().ToInt();
							visited.Add(start);

							// Get rest of number
							for (int nx = start.x + 1; true; nx++)
							{
								var nCell = grid[nx, checkForNumber.y];

								if (nCell == null) break;

								if (nCell.c.IsDigit())
								{
									number *= 10;
									number += nCell.c.ToString().ToInt();
									visited.Add(nCell);
								}
								else
								{
									break;
								}
							}

							if (n1 == 0)
							{
								n1 = number;
							}
							else if (n2 == 0)
							{
								n2 = number;
							}
							else
							{
								throw new Exception("Should not be 2 neighbouring numbers to a *");
							}
						}
					}

					result += n1 * n2;
				}
			}
		}
		return result.ToString();
	}
}
