using Shared;

namespace Year2022.Day06
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			List<char> last4 = new List<char>();

			int result = 0;
			foreach (char c in input)
			{
				if (!char.IsAsciiLetter(c))
				{
					continue;
				}

				result++;

				if (last4.Count == 4)
				{
					last4.RemoveAt(0);
				}

				last4.Add(c);

				if (last4.Distinct().Count() == 4)
				{
					break;
				}
			}

			return result.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			List<char> last4 = new List<char>();

			int result = 0;
			foreach (char c in input)
			{
				if (!char.IsAsciiLetter(c))
				{
					continue;
				}

				result++;

				if (last4.Count == 14)
				{
					last4.RemoveAt(0);
				}

				last4.Add(c);

				if (last4.Distinct().Count() == 14)
				{
					break;
				}
			}

			return result.ToString();
		}
	}
}
