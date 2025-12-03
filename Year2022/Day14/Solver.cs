using Shared;

namespace Year2022.Day14
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			int result = 0;

			char[,] grid = new char[800, 200];

			for (int x = 0; x < 800; x++)
			{
				for (int y = 0; y < 200; y++)
				{
					grid[x, y] = '.';
				}
			}
			grid[500, 0] = '+';

			foreach (string rockPath in input.AsLines())
			{
				(int x, int y) prev = (0, 0);
				foreach (string rock in rockPath.Split(" -> "))
				{
					var coords = rock.Split(',');
					(int x, int y) current = (int.Parse(coords[0]), int.Parse(coords[1]));

					if (prev == (0, 0))
					{
						prev = current;
						continue;
					}

					for (int i = current.x; i <= prev.x; i++)
					{
						grid[i, current.y] = '#';
					}
					for (int i = prev.x; i <= current.x; i++)
					{
						grid[i, current.y] = '#';
					}
					for (int i = current.y; i <= prev.y; i++)
					{
						grid[current.x, i] = '#';
					}
					for (int i = prev.y; i <= current.y; i++)
					{
						grid[current.x, i] = '#';
					}

					prev = current;
				}
			}

			void FallSand()
			{
				(int x, int y) sandStart = (500, 0);
				while (true)
				{
					(int x, int y) sand = sandStart;
					while (true)
					{
						if (sand.y == 199)
						{
							// Sand will fall out, stop here
							return;
						}
						if (grid[sand.x, sand.y + 1] == '.')
						{
							sand.y += 1;
						}
						else if (grid[sand.x - 1, sand.y + 1] == '.')
						{
							sand.y += 1;
							sand.x -= 1;
						}
						else if (grid[sand.x + 1, sand.y + 1] == '.')
						{
							sand.y += 1;
							sand.x += 1;
						}
						else
						{
							// Sand is at rest, continue with next
							grid[sand.x, sand.y] = 'o';
							result++;
							break;
						}
					}
				}
			}

			FallSand();

			return result.ToString();
		}

		private void PrintGrid(char[,] grid)
		{
			for (int y = 0; y < 200; y++)
			{
				for (int x = 400; x < 800; x++)
				{
					Console.Write(grid[x, y]);
				}
				Console.WriteLine();
			}
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			int result = 0;
			int maxY = 0;

			char[,] grid = new char[800, 200];

			for (int x = 0; x < 800; x++)
			{
				for (int y = 0; y < 200; y++)
				{
					grid[x, y] = '.';
				}
			}
			grid[500, 0] = '+';

			foreach (string rockPath in input.AsLines())
			{
				(int x, int y) prev = (0, 0);
				foreach (string rock in rockPath.Split(" -> "))
				{
					var coords = rock.Split(',');
					(int x, int y) current = (int.Parse(coords[0]), int.Parse(coords[1]));

					if (current.y > maxY)
					{
						maxY = current.y;
					}

					if (prev == (0, 0))
					{
						prev = current;
						continue;
					}

					for (int i = current.x; i <= prev.x; i++)
					{
						grid[i, current.y] = '#';
					}
					for (int i = prev.x; i <= current.x; i++)
					{
						grid[i, current.y] = '#';
					}
					for (int i = current.y; i <= prev.y; i++)
					{
						grid[current.x, i] = '#';
					}
					for (int i = prev.y; i <= current.y; i++)
					{
						grid[current.x, i] = '#';
					}

					prev = current;
				}
			}

			for (int x = 0; x < 800; x++)
			{
				grid[x, maxY + 2] = '#';
			}

			void FallSand()
			{
				(int x, int y) sandStart = (500, 0);
				while (true)
				{
					(int x, int y) sand = sandStart;
					while (true)
					{
						if (grid[sand.x, sand.y + 1] == '.')
						{
							sand.y += 1;
						}
						else if (grid[sand.x - 1, sand.y + 1] == '.')
						{
							sand.y += 1;
							sand.x -= 1;
						}
						else if (grid[sand.x + 1, sand.y + 1] == '.')
						{
							sand.y += 1;
							sand.x += 1;
						}
						else
						{
							if (sand == sandStart)
							{
								// Sand can't fall more;
								result++;
								return;
							}
							grid[sand.x, sand.y] = 'o';
							result++;
							break;
						}
					}
				}
			}

			FallSand();

			return result.ToString();
		}
	}
}
