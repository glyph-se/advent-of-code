using Shared;
using Shared.Helpers;

namespace Year2022.Day18
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			int result = 0;

			bool[,,] grid = new bool[21, 21, 21];

			foreach (var line in input.AsLines())
			{
				var split = line.Split(',').Select(s => int.Parse(s));

				grid[split.ElementAt(0), split.ElementAt(1), split.ElementAt(2)] = true;
			}

			for (int x = 0; x <= 19; x++)
			{
				for (int y = 0; y <= 19; y++)
				{
					for (int z = 0; z <= 19; z++)
					{
						if (!grid[x, y, z])
						{
							// no cube here
							continue;
						}

						(int, int, int)[] dirs = { (0, 0, 1), (0, 1, 0), (1, 0, 0), (0, 0, -1), (0, -1, 0), (-1, 0, 0) };

						foreach ((int xDiff, int yDiff, int zDiff) in dirs)
						{
							if (x + xDiff < 0 || y + yDiff < 0 || z + zDiff < 0)
							{
								result++;
								continue;
							}

							if (!grid[x + xDiff, y + yDiff, z + zDiff])
							{
								result++;
							}
						}
					}
				}
			}

			return result.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			int result = 0;

			bool?[,,] grid = new bool?[21, 21, 21];

			foreach (var line in input.AsLines())
			{
				var split = line.Split(',').Select(s => int.Parse(s));

				grid[split.ElementAt(0), split.ElementAt(1), split.ElementAt(2)] = true;
			}


			Point start = new Point(0, 0, 0);

			grid[start.x, start.y, start.z] = false;

			Queue<Point> expansion = new();
			expansion.Enqueue(start);

			List<Point> airCubes = new();

			bool[,,] airGrid = new bool[21, 21, 21];
			while (expansion.Count != 0)
			{
				Point p = expansion.Dequeue();

				if (p.x < 0 || p.x > 19 || p.y < 0 || p.y > 19 || p.z < 0 || p.z > 19)
				{
					continue;
				}

				(int, int, int)[] dirs = { (0, 0, 1), (0, 1, 0), (1, 0, 0), (0, 0, -1), (0, -1, 0), (-1, 0, 0) };

				foreach ((int xDiff, int yDiff, int zDiff) in dirs)
				{
					if (p.x + xDiff < 0 || p.y + yDiff < 0 || p.z + zDiff < 0)
					{
						continue;
					}

					if (grid[p.x + xDiff, p.y + yDiff, p.z + zDiff] == null)
					{
						// null means nothing now
						Point next = new Point(p.x + xDiff, p.y + yDiff, p.z + zDiff);

						// false means air
						grid[p.x + xDiff, p.y + yDiff, p.z + zDiff] = false;

						expansion.Enqueue(next);
					}
				}
			}

			for (int x = 0; x <= 19; x++)
			{
				for (int y = 0; y <= 19; y++)
				{
					for (int z = 0; z <= 19; z++)
					{
						bool? cube = grid[x, y, z];

						if (cube != true)
						{
							// no cube here
							continue;
						}

						(int, int, int)[] dirs = { (0, 0, 1), (0, 1, 0), (1, 0, 0), (0, 0, -1), (0, -1, 0), (-1, 0, 0) };

						foreach ((int xDiff, int yDiff, int zDiff) in dirs)
						{
							if (x + xDiff < 0 || y + yDiff < 0 || z + zDiff < 0 || x + xDiff > 19 || y + yDiff > 19 || z + zDiff > 19)
							{
								result++;
								continue;
							}

							if (grid[x + xDiff, y + yDiff, z + zDiff] == false)
							{
								result++;
							}
						}
					}
				}
			}

			return result.ToString();
		}

		public record Point(int x, int y, int z);
	}
}
