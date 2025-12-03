using Shared;

namespace Year2022.Day25
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			long sum = 0;

			foreach (string line in input.AsLines())
			{
				long number = 0;

				int pos = 0; ;
				foreach (char c in line.Reverse())
				{
					int nbr = c switch
					{
						'-' => -1,
						'=' => -2,
						'0' => 0,
						'1' => 1,
						'2' => 2,
						_ => throw new Exception("error")
					}; ;

					number += nbr * (long)Math.Pow(5, pos);

					pos++;
				}

				sum += number;
			}

			long remainder = sum;
			string result = "";

			while (remainder > 0)
			{
				long pos = remainder % 5L;

				char c = 'x';
				switch (pos)
				{
					case 0:
						c = '0';
						break;
					case 1:
						c = '1';
						break;
					case 2:
						c = '2';
						break;
					case 3:
						c = '=';
						remainder += 2L;
						break;
					case 4:
						c = '-';
						remainder += 1L;
						break;
				}

				remainder = remainder / 5L;
				result = c + result;
			}


			return result;
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			return "stars";
		}
	}
}
