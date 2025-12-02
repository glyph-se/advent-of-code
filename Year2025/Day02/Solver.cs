using System.Text.RegularExpressions;
using Shared;
using Shared.Helpers;

namespace Year2025.Day02;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		Regex regex = new Regex(@"^(\d+)\1$", RegexOptions.Compiled);

		foreach (string group in input.Split(','))
		{
			(string low, string high) = group.Split2("-");

			long lowNum = low.ToLong();
			long highNum = high.ToLong();

			for(long i= lowNum; i <= highNum; i++)
			{
				if(regex.IsMatch(i.ToString()))
				{
					result += i;
				}
			}
		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		Regex regex = new Regex(@"^(\d+)\1+$", RegexOptions.Compiled);

		foreach (string group in input.Split(','))
		{
			(string low, string high) = group.Split2("-");

			long lowNum = low.ToLong();
			long highNum = high.ToLong();

			for (long i = lowNum; i <= highNum; i++)
			{
				if (regex.IsMatch(i.ToString()))
				{
					result += i;
				}
			}
		}

		return result.ToString();
	}
}
