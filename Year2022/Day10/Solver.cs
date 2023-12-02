using Shared;
using Shared.Helpers;

namespace Year2022.Day10
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			int xValue = 1;
			int sumSignal = 0;
			int cycle = 0;

			var lines = input.AsLines().ToList();

			foreach (string command in lines)
			{
				if (command == "noop")
				{
					cycle++;

					if (cycle % 40 == 20)
					{
						sumSignal += xValue * cycle;
					}
				}
				else
				{
					var split = command.Split(' ');
					int change = int.Parse(split[1]);

					cycle++;

					if (cycle % 40 == 20)
					{
						sumSignal += xValue * cycle;
					}

					cycle++;

					if (cycle % 40 == 20)
					{
						sumSignal += xValue * cycle;
					}

					xValue += change;
				}
			}

			return sumSignal.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();


			string result = string.Empty;
			result += "\n";

			int xValue = 1;
			int cycle = 0;

			var lines = input.AsLines().ToList();

			foreach (string command in lines)
			{
				if (command == "noop")
				{
					cycle++;

					if (Math.Abs((cycle - 1) % 40 - xValue) <= 1)
					{
						result += "#";
					}
					else
					{
						result += ".";
					}

					if (cycle % 40 == 0)
					{
						result += "\n";
					}
				}
				else
				{
					var split = command.Split(' ');
					int change = int.Parse(split[1]);

					cycle++;

					if (Math.Abs((cycle - 1) % 40 - xValue) <= 1)
					{
						result += "#";
					}
					else
					{
						result += ".";
					}

					if (cycle % 40 == 0)
					{
						result += "\n";
					}

					cycle++;

					if (Math.Abs((cycle - 1) % 40 - xValue) <= 1)
					{
						result += "#";
					}
					else
					{
						result += ".";
					}

					if (cycle % 40 == 0)
					{
						result += "\n";
					}

					xValue += change;
				}
			}

			return result.ToString();
		}
	}
}
