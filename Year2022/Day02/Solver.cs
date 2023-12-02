using Shared;
using Shared.Helpers;

namespace Year2022.Day02
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			var games = input.AsLines();

			int score = 0;

			foreach (string game in games)
			{
				char other = game.ToCharArray()[0];
				char me = game.ToCharArray()[2];

				if (me == 'Y') // paper
					score += 2;
				if (me == 'X') // rock
					score += 1;
				if (me == 'Z') // scissors
					score += 3;

				if (other == 'A')  // rock
				{
					if (me == 'Y')
						score += 6;
					if (me == 'X')
						score += 3;
				}
				if (other == 'B')  // paper
				{
					if (me == 'Z')
						score += 6;
					if (me == 'Y')
						score += 3;
				}
				if (other == 'C')  // scissors
				{
					if (me == 'X')
						score += 6;
					if (me == 'Z')
						score += 3;
				}
			}

			return score.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			var games = input.AsLines();

			int score = 0;

			foreach (string game in games)
			{
				char other = game.ToCharArray()[0];
				char result = game.ToCharArray()[2];

				if (result == 'Y') // Y means draw
					score += 3;
				if (result == 'X') // X means lose
					score += 0;
				if (result == 'Z') // Z means win
					score += 6;


				if (other == 'A')  // rock
				{
					if (result == 'Y') // same = rock 
						score += 1;
					if (result == 'X') // lose = scissor
						score += 3;
					if (result == 'Z') // win = papper
						score += 2;
				}
				if (other == 'B')  // paper
				{
					if (result == 'Y') // same = paper 
						score += 2;
					if (result == 'X') // lose = rock
						score += 1;
					if (result == 'Z') // win = scissor
						score += 3;
				}
				if (other == 'C')  // scissor
				{
					if (result == 'Y') // same = scissor 
						score += 3;
					if (result == 'X') // lose = papper
						score += 2;
					if (result == 'Z') // win = rock
						score += 1;
				}
			}

			return score.ToString();
		}
	}
}
