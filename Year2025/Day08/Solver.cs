
using System.Diagnostics;

namespace Year2025.Day08;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point3DWithNetwork> points = ParsePoints(input);
		PriorityQueue<(Point3DWithNetwork, Point3DWithNetwork), long> pairDistances = CalculcatePairDistances(points);

		// HACK: In example we make 10 connections, in full data we make 1000
		int connectionsToMake = points.Count == 20 ? 10 : 1000;

		int freeId = 0;

		for (int i = 0; i < connectionsToMake; i++)
		{
			(Point3DWithNetwork p1, Point3DWithNetwork p2) = pairDistances.Dequeue();

			ConnectPoints(p1, p2, points, ref freeId);
		}

		// Count networks
		IEnumerable<IGrouping<int?, Point3DWithNetwork>> networkPointCount = points
			.Where(p => p.Network.HasValue)
			.GroupBy(p => p.Network);

		var sortedCount = networkPointCount
			.Select(g => g.Count())
			.OrderDescending()
			.ToArray();

		result = sortedCount[0] * sortedCount[1] * sortedCount[2];

		return result.ToString();
	}



	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Point3DWithNetwork> points = ParsePoints(input);
		PriorityQueue<(Point3DWithNetwork, Point3DWithNetwork), long> pairDistances = CalculcatePairDistances(points);

		int freeId = 0;

		while (pairDistances.Count > 0)
		{
			(Point3DWithNetwork p1, Point3DWithNetwork p2) = pairDistances.Dequeue();

			ConnectPoints(p1, p2, points, ref freeId);

			// Check if all points are in the same network
			if (points.Select(p => p.Network).Distinct().Count() == 1)
			{
				result = p1.x * p2.x;
				break;
			}
		}

		return result.ToString();
	}

	private static List<Point3DWithNetwork> ParsePoints(string input)
	{
		List<Point3DWithNetwork> points = new();
		foreach (string line in input.AsLines())
		{
			(string x, string y, string z) = line.Split3(",");
			points.Add(new Point3DWithNetwork(x.ToInt(), y.ToInt(), z.ToInt()));
		}

		return points;
	}

	private static PriorityQueue<(Point3DWithNetwork, Point3DWithNetwork), long> CalculcatePairDistances(List<Point3DWithNetwork> points)
	{
		PriorityQueue<(Point3DWithNetwork, Point3DWithNetwork), long> pairDistances = new();
		foreach (IEnumerable<Point3DWithNetwork> pair in points.DifferentCombinations(2))
		{
			Point3DWithNetwork p1 = pair.First();
			Point3DWithNetwork p2 = pair.Skip(1).First();

			long dx = p1.x - p2.x;
			long dy = p1.y - p2.y;
			long dz = p1.z - p2.z;

			long dist = dx * dx + dy * dy + dz * dz;

			pairDistances.Enqueue((p1, p2), dist);
		}

		return pairDistances;
	}

	private static void ConnectPoints(Point3DWithNetwork p1, Point3DWithNetwork p2, List<Point3DWithNetwork> points, ref int freeNetworkId)
	{
		if (p1.Network == null && p2.Network == null)
		{
			p1.Network = freeNetworkId;
			p2.Network = freeNetworkId;
			freeNetworkId++;
		}
		else if (p1.Network != null && p2.Network != null)
		{
			if (p1.Network == p2.Network)
			{
				return;
			}

			int oldId = p2.Network.Value;
			int newId = p1.Network.Value;

			foreach (Point3DWithNetwork p in points.Where(p => p.Network == oldId))
			{
				p.Network = newId;
			}
		}
		else if (p1.Network != null)
		{
			p2.Network = p1.Network;
		}
		else if (p2.Network != null)
		{
			p1.Network = p2.Network;
		}
		else
		{
			Debug.Fail("Should be unreachable");
		}
	}

	private class Point3DWithNetwork : Point3D
	{
		public Point3DWithNetwork(int x, int y, int z) : base(x, y, z)
		{
		}

		public int? Network { get; set; }
	}
}
