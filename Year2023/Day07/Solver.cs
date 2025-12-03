using System.Diagnostics;
using Shared;

namespace Year2023.Day07;

public class Solver : ISolver
{
	private const int JOKER = 0;

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Hand> hands = new List<Hand>();

		foreach (string line in input.AsLines())
		{
			var parts = line.TrimSplit(" ");

			int[] cards = parts[0].ToCharArray().Select(c => CardToInt(c)).ToArray();

			int bid = parts[1].ToInt();

			hands.Add(new Hand(cards, bid));
		}

		hands.Sort();

		for (int i = 1; i <= hands.Count; i++)
		{
			var hand = hands[i - 1];
			hand.rank = i;
		}

		result = hands.Sum(h => h.GetWinnings());

		return result.ToString();
	}

	private int CardToInt(char c)
	{
		if (c == 'A')
		{
			return 400;
		}

		if (c == 'K')
		{
			return 300;
		}

		if (c == 'Q')
		{
			return 200;
		}

		if (c == 'J')
		{
			return 100;
		}

		if (c == 'T')
		{
			return 10;
		}

		return c.ToString().ToInt();
	}
	private int CardJToInt(char c)
	{
		if (c == 'A')
		{
			return 400;
		}

		if (c == 'K')
		{
			return 300;
		}

		if (c == 'Q')
		{
			return 200;
		}

		if (c == 'J')
		{
			return JOKER;
		}

		if (c == 'T')
		{
			return 10;
		}

		return c.ToString().ToInt();
	}




	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<HandJ> hands = new List<HandJ>();

		foreach (string line in input.AsLines())
		{
			var parts = line.TrimSplit(" ");

			int[] cards = parts[0].ToCharArray().Select(c => CardJToInt(c)).ToArray();

			int bid = parts[1].ToInt();

			hands.Add(new HandJ(cards, bid));
		}

		hands.Sort();

		for (int i = 1; i <= hands.Count; i++)
		{
			var hand = hands[i - 1];
			hand.rank = i;
		}


		result = hands.Sum(h => h.GetWinnings());

		return result.ToString();
	}

	[DebuggerDisplay("bid={bid} cards={cards[0]}-{cards[1]}-{cards[2]}-{cards[3]}-{cards[4]}")]
	public class Hand : IComparable<Hand>
	{
		public Hand(int[] cards, int bid)
		{
			this.cards = cards.ToArray();
			this.bid = bid;
		}

		public int[] cards;

		public int bid;

		public int rank;

		public long GetWinnings()
		{
			return bid * rank;
		}

		public int CompareTo(Hand? other)
		{
			var thisGroup = cards
				.GroupBy(e => e)
				.OrderByDescending(e => e.Count())
				.ToArray();

			var otherGroup = other!.cards
				.GroupBy(e => e)
				.OrderByDescending(e => e.Count())
				.ToArray();

			if (thisGroup[0].Count() > otherGroup[0].Count())
			{
				return 1;
			}

			if (thisGroup[0].Count() < otherGroup[0].Count())
			{
				return -1;
			}

			if (thisGroup[1].Count() > otherGroup[1].Count())
			{
				return 1;
			}

			if (thisGroup[1].Count() < otherGroup[1].Count())
			{
				return -1;
			}

			for (int i = 0; i < cards.Length; i++)
			{
				var c1 = cards[i];
				var c2 = other.cards[i];

				if (c1 > c2)
				{
					return 1;
				}

				if (c1 < c2)
				{
					return -1;
				}
			}

			return 0;
		}
	}

	[DebuggerDisplay("bid={bid} cards={cards[0]}-{cards[1]}-{cards[2]}-{cards[3]}-{cards[4]}")]
	public class HandJ : IComparable<HandJ>
	{
		public HandJ(int[] cards, int bid)
		{
			this.cards = cards.ToArray();
			this.bid = bid;
			this.jokers = cards.Count(c => c == JOKER);
		}

		public int[] cards;

		public int bid;

		public int rank;

		public int jokers;

		public long GetWinnings()
		{
			return bid * rank;
		}

		public int CompareTo(HandJ? other)
		{
			var thisGroup = cards
				.Where(e => e != JOKER)
				.GroupBy(e => e)
				.Select(e => e.Count())
				.OrderByDescending(e => e)
				.Concat(new[] { 0, 0 }) // Protect from out of bounds below
				.ToArray();

			var otherGroup = other!.cards
				.Where(e => e != JOKER)
				.GroupBy(e => e)
				.Select(e => e.Count())
				.OrderByDescending(e => e)
				.Concat(new[] { 0, 0 }) // Protect from out of bounds below
				.ToArray();

			if (thisGroup[0] + jokers > otherGroup[0] + other.jokers)
			{
				return 1;
			}

			if (thisGroup[0] + jokers < otherGroup[0] + other.jokers)
			{
				return -1;
			}

			if (thisGroup[1] > otherGroup[1])
			{
				return 1;
			}

			if (thisGroup[1] < otherGroup[1])
			{
				return -1;
			}

			for (int i = 0; i < cards.Length; i++)
			{
				var c1 = cards[i];
				var c2 = other.cards[i];

				if (c1 > c2)
				{
					return 1;
				}

				if (c1 < c2)
				{
					return -1;
				}
			}

			return 0;
		}
	}
}
