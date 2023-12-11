using Shared;
using Shared.Helpers;
using static Year2023.Day10.Solver;

namespace Year2023.Day11;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		result = CalculateDistance(input, 2);

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		result = CalculateDistance(input, 1000000);

		return result.ToString();
	}

	public long CalculateDistance(string input, int expansion)
	{
		long result = 0;

		var universe = input.AsGridMatrix((c, x, y) => new CharPoint(c, x, y));
		universe = universe.ExtendGridMatrixWithPoint(1, (c, x, y) => new CharPoint(c, x, y));

		var universeList = universe.AsList();

		// Expansion
		Dictionary<int, int> emptyCol = new Dictionary<int, int>();
		Dictionary<int, int> emptyRow = new Dictionary<int, int>();

		for (int i = 0; i < universe.GetLength(0); i++)
		{
			bool allEmpty = universeList.Where(c => c.x == i).All(c => c.c == '.');

			if (allEmpty)
			{
				emptyCol[i] = expansion;
			}
		}

		for (int j = 0; j < universe.GetLength(0); j++)
		{
			bool allEmpty = universeList.Where(c => c.y == j).All(c => c.c == '.');

			if (allEmpty)
			{
				emptyRow[j] = expansion;
			}
		}

		var galaxies = new List<CharPoint>();

		galaxies.AddRange(universeList.Where(cp => cp.c == '#'));

		foreach (var g1 in galaxies)
		{
			foreach (var g2 in galaxies)
			{
				long distance = 0;

				long smallX = Math.Min(g1.x, g2.x);
				long bigX = Math.Max(g1.x, g2.x);
				long smallY = Math.Min(g1.y, g2.y);
				long bigY = Math.Max(g1.y, g2.y);

				for (long x = smallX; x < bigX; x++)
				{
					distance++;
					if (emptyCol.ContainsKey((int)x))
					{
						distance += emptyCol[(int)x] - 1;
					}
				}

				for (long y = smallY; y < bigY; y++)
				{
					distance++;
					if (emptyRow.ContainsKey((int)y))
					{
						distance += emptyRow[(int)y] - 1;
					}
				}

				result += distance;
			}
		}

		result = result / 2;

		return result;
	}
}
