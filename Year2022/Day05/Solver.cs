using Shared;
using Shared.Helpers;

namespace Year2022.Day05
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			var all = input.AsLineBlocks();

			var crates = all[0].Split('\n')
				.Transpose()
				.Select(cs => cs.Where(char.IsAsciiLetter))
				.Where(cs => cs.Any())
				.Select(cs => new List<char>(cs))
				.ToList();

			var allOps = all[1].AsLines();

			crates.Insert(0, new List<char>());

			foreach (string ops in allOps)
			{
				string[] op = ops.Split(' ');

				int count = int.Parse(op[1]);
				int from = int.Parse(op[3]);
				int to = int.Parse(op[5]);

				for (int i = 0; i < count; i++)
				{
					char toMove = crates[from][0];
					crates[from].RemoveAt(0);
					crates[to].Insert(0, toMove);
				}

			}

			string result = "";
			foreach (var c in crates)
			{
				result += c.FirstOrDefault();
			}

			// Skip first null - byte
			result = result.Substring(1);

			return result.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			var all = input.AsLineBlocks();

			var crates = all[0].Split('\n')
				.Transpose()
				.Select(cs => cs.Where(char.IsAsciiLetter))
				.Where(cs => cs.Any())
				.Select(cs => new List<char>(cs))
				.ToList();

			var allOps = all[1].AsLines();

			crates.Insert(0, new List<char>());

			foreach (string ops in allOps)
			{
				string[] op = ops.Split(' ');

				int count = int.Parse(op[1]);
				int from = int.Parse(op[3]);
				int to = int.Parse(op[5]);

				List<char> toMoveAll = new List<char>();

				for (int i = 0; i < count; i++)
				{
					char toMove = crates[from][0];
					crates[from].RemoveAt(0);
					toMoveAll.Add(toMove);
				}

				crates[to].InsertRange(0, toMoveAll);

			}

			string result = "";
			foreach (var c in crates)
			{
				result += c.FirstOrDefault();
			}

			// Skip first null - byte
			result = result.Substring(1);

			return result.ToString();
		}
	}
}
