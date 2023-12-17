using Shared;
using Shared.Helpers;

namespace Year2023.Day17;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CityBlock(c, x, y));

		var start = grid[0, 0];
		var end = grid[grid.GetUpperBound(0), grid.GetUpperBound(1)];
		result = Dijkstra(grid, start, end, 1, 3);

		return result.ToString();
	}

	public static int Dijkstra(CityBlock[,] grid, CityBlock start, CityBlock end, int minDistance, int maxDistance)
	{
		var startNode = new Node(start, (0, 0));
		PriorityQueue<Node, int> queue = new();
		HashSet<Node> visited = new();
		Dictionary<Node, int> distances = new();

		queue.Enqueue(startNode, 0);
		distances[startNode] = 0;

		while (queue.Count > 0)
		{
			Node current = queue.Dequeue();

			// Check end condition
			if (current.cb == end)
			{
				return distances[current];
			}

			if (visited.Contains(current))
			{
				continue;
			}
			visited.Add(current);

			foreach (var dir in GridHelpers.UpDowns())
			{
				int distIncrease = 0;
				if(dir == current.blockedDir || dir == current.blockedDir.TurnReverse())
				{
					continue;
				}

				// Add all possible distances
				for(int distance = 1; distance<=maxDistance; distance++)
				{
					int newX = current.cb.x + dir.dx * distance;
					int newY = current.cb.y + dir.dy * distance;

					if(grid.IsInside((newX, newY)))
					{
						var newCityBlock = grid[newX, newY];
						distIncrease += newCityBlock.heat;

						if(distance < minDistance)
						{
							// Need to go longer
							continue;
						}

						int newCost = distances[current] + distIncrease;
						Node newNode = new Node(newCityBlock, dir);

						if (distances.ContainsKey(newNode) && distances[newNode] <= newCost)
						{
							continue;
						}

						distances[newNode] = newCost;
						queue.Enqueue(newNode, newCost);
					}
				}
			}
		}

		throw new Exception("No way to reach end");
	}



	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var grid = input.AsGridMatrix((c, x, y) => new CityBlock(c, x, y));

		var start = grid[0, 0];
		var end = grid[grid.GetUpperBound(0), grid.GetUpperBound(1)];
		result = Dijkstra(grid, start, end, 4, 10);

		return result.ToString();
	}

	public class Node
	{
		public CityBlock cb;
		public (int x, int y) blockedDir;

		public Node(CityBlock cb, (int x, int y) blockedDir)
		{
			this.cb = cb;
			this.blockedDir = blockedDir;
		}

		public override bool Equals(object? obj)
		{
			return obj is Node node &&
						 EqualityComparer<CityBlock>.Default.Equals(cb, node.cb) &&
						 blockedDir.Equals(node.blockedDir);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(cb, blockedDir);
		}
	}

	public class CityBlock : CharPoint
	{
		public CityBlock(char c, int x, int y)
		{
			this.c = c;
			this.x = x;
			this.y = y;
			this.heat = c.ToString().ToInt();
		}

		public int heat;
	}
}
