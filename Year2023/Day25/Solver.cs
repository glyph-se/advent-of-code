namespace Year2023.Day25;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;


		Dictionary<string, List<string>> components = new();

		foreach (var line in input.ParseLines())
		{
			(string name, string connectionString) = line.Split2([":"]);

			var connections = connectionString.TrimSplit([" "]);

			foreach (var connection in connections)
			{
				if (!components.ContainsKey(name))
				{
					components[name] = new();
				}

				if (!components.ContainsKey(connection))
				{
					components[connection] = new();
				}

				components[name].Add(connection);
				components[connection].Add(name);
			}
		}

		Random random = new Random();

		int cutSize;
		List<List<string>> subgraphs;

		do
		{
			Dictionary<string, List<string>> copy = components.ToDictionary(kvp => kvp.Key, kvp => new List<string>(kvp.Value));
			(cutSize, subgraphs) = KargersAlgorithm(copy, random);
		}
		while (cutSize != 3);

		result = subgraphs.First().Count() * subgraphs.Last().Count();


		return result.ToString();
	}

	/// <remarks>From https://medium.com/@dev.elect.iitd/kargers-algorithm-d8067eb1b790</remarks>
	(int size, List<List<string>>) KargersAlgorithm(Dictionary<string, List<string>> graph, Random r)
	{
		// This is not part of we algorithm, but we need to store the resulting graph
		List<List<string>> subgraphs = new List<List<string>>();

		foreach (var node in graph.Keys)
		{
			subgraphs.Add(new List<string>() { node });
		}

		while (graph.Count > 2)
		{
			// Choose a random edge
			var v = graph.Keys.ElementAt(r.Next(graph.Count));
			var w = graph[v].ElementAt(r.Next(graph[v].Count));

			// Contract the graph by removing things.
			foreach (var node in graph[w])
			{
				if (node != v)
				{
					graph[v].Add(node);
				}
				graph[node].Remove(w);

				if (node != v)
				{
					graph[node].Add(v);
				}
			}

			// Delete the absorbed node
			graph.Remove(w);

			List<string> subV = subgraphs.Where(s => s.Contains(v)).Single();
			List<string> subW = subgraphs.Where(s => s.Contains(w)).Single();

			subgraphs.Remove(subW);
			subV.AddRange(subW);
		}

		// We have 2 elements in the graph now, they will have the same number of edges / cuts between them.
		int minCut = graph.First().Value.Count();

		return (minCut, subgraphs);
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		return "stars";
	}
}
