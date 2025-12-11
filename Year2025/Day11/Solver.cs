namespace Year2025.Day11;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;
		Dictionary<string, List<string>> graph = ParseGraph(input);



		result = FindNumberOfPaths(graph, "you", "out");

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		Dictionary<string, List<string>> graph = ParseGraph(input);

		// This is an actual fix needed. NOT A HACK
		// Add the "out" node to the graph
		if (!graph.ContainsKey("out"))
		{
			graph.Add("out", new List<string>());
		}

		result += FindNumberOfPaths(graph, "svr", "dac") * FindNumberOfPaths(graph, "dac", "fft") * FindNumberOfPaths(graph, "fft", "out");
		result += FindNumberOfPaths(graph, "svr", "fft") * FindNumberOfPaths(graph, "fft", "dac") * FindNumberOfPaths(graph, "dac", "out");

		return result.ToString();
	}



	private static Dictionary<string, List<string>> ParseGraph(string input)
	{
		Dictionary<string, List<string>> graph = new();

		foreach (string line in input.ParseLines())
		{
			(string start, string ends) = line.Split2(":");

			graph.Add(start, ends.TrimSplit(" ").ToList());
		}

		// Hack for example inputs
		if (!graph.ContainsKey("you"))
		{
			graph.Add("you", new List<string>());
		}
		if (!graph.ContainsKey("svr"))
		{
			graph.Add("svr", new List<string>());
		}
		if (!graph.ContainsKey("dac"))
		{
			graph.Add("dac", new List<string>());
		}
		if (!graph.ContainsKey("fft"))
		{
			graph.Add("fft", new List<string>());
		}

		return graph;
	}
	private static long FindNumberOfPaths(Dictionary<string, List<string>> graph, string start, string end)
	{
		Dictionary<string, long> numberOfPaths = new();

		long DFS(string node)
		{
			if (numberOfPaths.ContainsKey(node))
			{
				return numberOfPaths[node];
			}

			if (node == end)
			{
				return 1;
			}

			long pathCount = 0;

			foreach (var edge in graph[node])
			{
				pathCount += DFS(edge);
			}

			numberOfPaths[node] = pathCount;

			return pathCount;
		}

		return DFS(start);
	}
}
