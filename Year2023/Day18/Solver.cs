namespace Year2023.Day18;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point> points = new();

		Point start = new Point(0, 0);
		points.Add(start);

		Point prev = start;

		long border = 0;

		foreach (string line in input.AsLines())
		{
			(string dir, string distString, string hex) = line.Split3(" ");
			int dist = distString.ToInt();

			Point? next = null;

			switch (dir)
			{
				case "R":
					next = new Point(prev.x + dist, prev.y);
					break;
				case "D":
					next = new Point(prev.x, prev.y + dist);
					break;
				case "L":
					next = new Point(prev.x - dist, prev.y);
					break;
				case "U":
					next = new Point(prev.x, prev.y - dist);
					break;
				default:
					throw new Exception("invalid input");
			}

			points.Add(next!);
			border += dist;
			prev = next!;
		}

		result = CalculateLagoon(points, border);

		return result.ToString();
	}

	private static long CalculateLagoon(List<Point> points, long border)
	{
		long result;

		// Pont's need to be counter-clockwise
		points.Reverse();
		Point? prevP = null;
		long inside = 0;
		foreach (Point p in points)
		{
			if (prevP != null)
			{
				// Shoelace formula
				inside += (long)(prevP.x + p.x) * (long)(prevP.y - p.y) / 2;
			}
			prevP = p;
		}

		// Pick's theorem
		result = (border / 2) + inside + 1;
		return result;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point> points = new();

		Point start = new Point(0, 0);
		points.Add(start);

		Point prev = start;

		long border = 0;

		foreach (string line in input.AsLines())
		{
			(string dir, string distString, string hex) = line.Split3(" ");
			hex = hex.Trim(['(', ')', '#']);

			char hexDir = hex.ToCharArray()[^1];
			int hexDist = int.Parse(hex.Substring(0, 5), System.Globalization.NumberStyles.HexNumber);

			Point? next = null;

			switch (hexDir)
			{
				case '0':
					next = new Point(prev.x + hexDist, prev.y);
					break;
				case '1':
					next = new Point(prev.x, prev.y + hexDist);
					break;
				case '2':
					next = new Point(prev.x - hexDist, prev.y);
					break;
				case '3':
					next = new Point(prev.x, prev.y - hexDist);
					break;
				default:
					throw new Exception("invalid input");
			}

			points.Add(next!);
			border += hexDist;
			prev = next!;
		}

		result = CalculateLagoon(points, border);

		return result.ToString();
	}
}
