
namespace Year2025.Day09;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point> points = new();

		foreach (string line in input.AsLines())
		{
			(string x, string y) = line.Split2(",");
			points.Add(new Point(x.ToInt(), y.ToInt()));
		}

		foreach (IEnumerable<Point> pair in points.DifferentCombinations(2))
		{
			long area = Area(pair.First(), pair.Skip(1).First());
			if (area > result)
			{
				result = area;
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point> points = new();

		foreach (string line in input.AsLines())
		{
			(string x, string y) = line.Split2(",");
			points.Add(new Point(x.ToInt(), y.ToInt()));
		}

		foreach (IEnumerable<Point> pair in points.DifferentCombinations(2))
		{
			var p1 = pair.First();
			var p2 = pair.Skip(1).First();

			long x1 = Math.Max(p1.x, p2.x);
			long x2 = Math.Min(p1.x, p2.x);

			long y1 = Math.Max(p1.y, p2.y);
			long y2 = Math.Min(p1.y, p2.y);

			bool isValid = true;

			for (int i = 0; i < points.Count; i++)
			{
				var l1 = points[i];
				var l2 = points[(i + 1) % points.Count];

				if (!(Math.Max(l1.x, l2.x) <= x2 || Math.Min(l1.x, l2.x) >= x1 || Math.Max(l1.y, l2.y) <= y2 || Math.Min(l1.y, l2.y) >= y1))
				{
					isValid = false;
					break;
				}
			}

			if (!isValid)
			{
				continue;
			}

			long area = Area(p1, p2);
			if (area > result)
			{
				result = area;
			}
		}

		return result.ToString();
	}

	private long Area(Point p1, Point p2)
	{
		long x1 = Math.Max(p1.x, p2.x);
		long x2 = Math.Min(p1.x, p2.x);

		long y1 = Math.Max(p1.y, p2.y);
		long y2 = Math.Min(p1.y, p2.y);

		return (x1 - x2 + 1) * (y1 - y2 + 1);
	}
}
