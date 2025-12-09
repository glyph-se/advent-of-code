using System.Collections.Concurrent;

namespace Year2025.Day07;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.ParseGridMatrix((c, x, y) => new CharPoint(c, x, y));

		CharPoint start = grid.AsList().Where(g => g.c == 'S').Single();

		HashSet<Point> beams = new();
		beams.Add(new Point(start.x, start.y));

		while (beams.First().y < grid.LengthY())
		{
			HashSet<Point> newBeams = new();

			foreach (var beam in beams)
			{
				if (grid[beam.x, beam.y + 1].c == '.')
				{
					beam.y++;
					newBeams.Add(beam);
				}
				else if (grid[beam.x, beam.y + 1].c == '^')
				{
					beam.y++;
					newBeams.Add(new Point(beam.x - 1, beam.y));
					newBeams.Add(new Point(beam.x + 1, beam.y));
					result++;
				}
			}

			beams = newBeams;
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.ParseGridMatrix((c, x, y) => new CharPoint(c, x, y));

		CharPoint start = grid.AsList().Where(g => g.c == 'S').Single();

		// ConcurrentDictionary just to get the "AddOrUpdate" method
		ConcurrentDictionary<Point, long> beams = new();
		beams.TryAdd(new Point(start.x, start.y), 1L);

		while (beams.First().Key.y < grid.LengthY())
		{
			ConcurrentDictionary<Point, long> newBeams = new();

			foreach (KeyValuePair<Point, long> beam in beams)
			{
				if (grid[beam.Key.x, beam.Key.y + 1].c == '.')
				{
					beam.Key.y++;
					newBeams.AddOrUpdate(beam.Key, p => beam.Value, (p, v) => v + beam.Value);
				}
				else if (grid[beam.Key.x, beam.Key.y + 1].c == '^')
				{
					beam.Key.y++;
					newBeams.AddOrUpdate(new Point(beam.Key.x - 1, beam.Key.y), p => beam.Value, (p, v) => v + beam.Value);
					newBeams.AddOrUpdate(new Point(beam.Key.x + 1, beam.Key.y), p => beam.Value, (p, v) => v + beam.Value);
				}
			}

			beams = newBeams;
		}

		result = beams.Values.Sum();

		return result.ToString();
	}
}
