namespace Year2023.Day04;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		foreach (string line in input.AsLines())
		{
			var (cardPart, winningNumberPart, myNumberPart) = line.Split3(new[] { ":", "|" });
			int card = cardPart.ReplaceRemove("Card ").ToInt();
			var winningNumbers = winningNumberPart.TrimSplit(" ");
			var myNumbers = myNumberPart.TrimSplit(" ");

			int count = winningNumbers.Intersect(myNumbers).Count();

			if (count > 0)
			{
				int score = (int)Math.Pow(2, count - 1);
				result += score;
			}

		}

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		Dictionary<int, int> instances = new Dictionary<int, int>();

		for (int i = 0; i < 300; i++)
		{
			instances.Add(i, 0);
		}

		foreach (string line in input.AsLines())
		{
			var (cardPart, winningNumberPart, myNumberPart) = line.Split3(new[] { ":", "|" });
			int card = cardPart.ReplaceRemove("Card ").ToInt();
			var winningNumbers = winningNumberPart.TrimSplit(" ");
			var myNumbers = myNumberPart.TrimSplit(" ");

			instances[card]++;

			int count = winningNumbers.Intersect(myNumbers).Count();

			for (int i = 1; i <= count; i++)
			{
				instances[card + i] += instances[card];
			}
		}

		result = instances.Values.Sum();

		return result.ToString();
	}
}
