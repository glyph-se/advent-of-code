using Shared;
using Year2022.Common;

namespace Year2022.Day15
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			List<Measurement> measurements = ParseInput(input);

			HashSet<Point> beacons = measurements
				.Select(m => m.beacon)
				.ToHashSet();

			int result = 0;

			int searchXStart = 0, searchXEnd = 0, searchY = 0;
			switch (measurements.Count)
			{
				case 14:
					searchXEnd = 25;
					searchXStart = -4;
					searchY = 10;
					break;
				case 32:
					searchXEnd = 10_000_000;
					searchXStart = -1_000_000;
					searchY = 2_000_000;
					break;
			}

			for (int x = searchXStart; x < searchXEnd; x++)
			{
				Point pTest = new Point(x, searchY);

				// Check if I am a beacon
				if (beacons.Contains(pTest))
				{
					continue;
				}

				foreach (var p in measurements)
				{
					// Check if any sensor covers this point within its beacon distance
					if (Distance(pTest, p.sensor) <= p.distance)
					{
						result++;
						break;
					}
				}
			}


			return result.ToString();
		}

		private static int Distance(Point p1, Point p2)
		{
			return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			List<Measurement> measurements = ParseInput(input);

			long result = 0;

			int searchSpace = 0;
			switch (measurements.Count)
			{
				case 14:
					searchSpace = 20;
					break;
				case 32:
					searchSpace = 4_000_000;
					break;
			}

			foreach (Measurement m in measurements)
			{
				int range = m.distance + 1;
				for (int r = 0; r <= range; r++)
				{
					(int, int)[] dirs = { (1, 1), (1, -1), (-1, 1), (-1, -1) };

					foreach ((int signX, int signY) in dirs)
					{
						Point pTest = new(
							m.sensor.x + r * signX,
							m.sensor.y + (range - r) * signY);

						if (pTest.x > searchSpace || pTest.x < 0 || pTest.y > searchSpace || pTest.y < 0)
						{
							continue;
						}

						if (CanHaveBeacon(measurements, pTest))
						{
							return (pTest.x * 4_000_000L + pTest.y).ToString();
						}
					}
				}
			}

			return result.ToString();
		}

		private static List<Measurement> ParseInput(string input)
		{
			List<Measurement> points = new();

			foreach (string line in input.AsLines())
			{
				var split = line.Split(':');

				var sensor = split[0].Replace("Sensor at ", "").Split(", ");

				Point p1 = new(
					int.Parse(sensor[0].Substring(2)),
					int.Parse(sensor[1].Substring(2)));


				var beacon = split[1].Replace(" closest beacon is at ", "").Split(", ");

				Point p2 = new(
					int.Parse(beacon[0].Substring(2)),
					int.Parse(beacon[1].Substring(2)));

				points.Add(new Measurement(p1, p2));
			}

			return points;
		}

		public bool CanHaveBeacon(IEnumerable<Measurement> m, Point pTest)
		{
			foreach (var p in m)
			{
				// Check distance to each sensor
				if (Distance(pTest, p.sensor) <= p.distance)
				{
					return false;
				}
			}

			return true;
		}

		public class Measurement
		{
			public Measurement(Point sensor, Point beacon)
			{
				this.sensor = sensor;
				this.beacon = beacon;
				distance = Distance(sensor, beacon);
			}

			public Point sensor;
			public Point beacon;
			public int distance;
		}

		public record Point(int x, int y);
	}
}
