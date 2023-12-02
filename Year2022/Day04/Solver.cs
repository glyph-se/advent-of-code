using Shared;
using Shared.Helpers;

namespace Year2022.Day04
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			int score = 0;

			IList<string> pairs = input.AsLines();

			foreach (string pair in pairs)
			{
				var split = pair.Split(",");
				string pair1 = split[0];
				string pair2 = split[1];
				var e1 = new Elf(pair1);
				var e2 = new Elf(pair2);

				if (e1.start >= e2.start && e1.end <= e2.end)
				{
					score++;
				}
				else if (e2.start >= e1.start && e2.end <= e1.end)
				{
					score++;
				}
			}

			return score.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			int score = 0;

			IList<string> pairs = input.AsLines();

			foreach (string pair in pairs)
			{
				var split = pair.Split(",");
				string pair1 = split[0];
				string pair2 = split[1];
				var e1 = new Elf(pair1);
				var e2 = new Elf(pair2);

				if (e1.start >= e2.start && e1.start <= e2.end)
				{
					score++;
				}
				else if (e2.start >= e1.start && e2.start <= e1.end)
				{
					score++;
				}
			}

			return score.ToString();
		}

		private class Elf
		{
			public int start;
			public int end;

			public Elf(string assignment)
			{
				var split = assignment.Split("-");
				start = int.Parse(split[0]);
				end = int.Parse(split[1]);
			}
		}
	}
}
