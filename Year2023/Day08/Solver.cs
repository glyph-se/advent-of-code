using Shared;

namespace Year2023.Day08;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.AsLineBlocks();

		var dirs = blocks[0].ToCharArray();

		Dictionary<string, (string, string)> graph = new Dictionary<string, (string, string)>();

		foreach (string line in blocks[1].AsLines())
		{
			(string source, string dests) = line.Split2("=");
			(string leftDest, string rightDest) = dests.Split2(",");
			leftDest = leftDest.ReplaceRemove("(");
			rightDest = rightDest.ReplaceRemove(")");

			graph.Add(source, (leftDest, rightDest));
		}

		string current = "AAA";

		if (!graph.ContainsKey(current))
		{
			return "Example not valid for part 1";
		}

		for (int i = 0; true; i++)
		{
			var dir = dirs[i % dirs.Length];
			var nav = graph[current];

			if (dir == 'L')
			{
				current = nav.Item1;
			}
			if (dir == 'R')
			{
				current = nav.Item2;
			}

			if (current == "ZZZ")
			{
				result = i + 1;
				break;
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.AsLineBlocks();

		var dirs = blocks[0].ToCharArray();

		Dictionary<string, (string, string)> graph = new Dictionary<string, (string, string)>();

		foreach (string line in blocks[1].AsLines())
		{
			(string source, string dests) = line.Split2("=");
			(string leftDest, string rightDest) = dests.Split2(",");
			leftDest = leftDest.ReplaceRemove("(");
			rightDest = rightDest.ReplaceRemove(")");

			graph.Add(source, (leftDest, rightDest));
		}

		List<string> starts = graph.Keys.Where(k => k.EndsWith("A")).ToList();

		List<long> results = new List<long>();

		foreach (string start in starts)
		{
			string current = start;
			for (long i = 0; true; i++)
			{
				var dir = dirs[i % dirs.Length];
				var nav = graph[current];

				if (dir == 'L')
				{
					current = nav.Item1;
				}
				if (dir == 'R')
				{
					current = nav.Item2;
				}

				if (current.EndsWith("Z"))
				{
					results.Add(i + 1);
					break;
				}
			}
		}

		result = results.Aggregate((a, b) => MathHelpers.lcm(a, b));

		return result.ToString();
	}
}
