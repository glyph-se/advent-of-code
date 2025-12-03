using System.Text.RegularExpressions;
using Shared;

namespace Year2023.Day15;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var step = input.TrimSplit(",");

		foreach(var x in step)
		{
			result += Hash(x);
		}

		return result.ToString();
	}

	private static int Hash(string x)
	{
		int hash = 0;
		var chars = x.ToCharArray();

		foreach (char c in chars)
		{
			hash += (int)c;
			hash *= 17;
			hash = hash % 256;
		}

		return hash;
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		int result = 0;

		var step = input.TrimSplit(",");

		Dictionary<int, List<Lens>> boxes = new();

		for(int i  = 0; i <= 255; i++)
		{
			boxes.Add(i, new List<Lens>());
		}

		foreach (var x in step)
		{
			var r = Regex.Match(x, "(\\w+)([-=])(\\d+)?");

			string label = r.Groups[1].Value;
			string op = r.Groups[2].Value;
			string length = r.Groups[3].Value;

			int boxNbr = Hash(label);
			var lenses = boxes[boxNbr];

			if (op == "-")
			{
				lenses.RemoveAll(l => l.label == label);
			}

			if (op == "=")
			{
				var lens = lenses.SingleOrDefault(l => l.label == label);
				if (lens != default)
				{
					lens.length = length.ToInt();
				}
				else
				{
					lenses.Add(new Lens(label, length.ToInt()));
				}
			}
		}

		foreach(var box in boxes)
		{
			foreach(var lens in box.Value)
			{
				result += (box.Key + 1) * (box.Value.IndexOf(lens) + 1) * lens.length;
			}
		}

		return result.ToString();
	}

	/// <remarks>
	/// This class is needed in order to update the length of a lens. The built-int tuple is a value type and not a reference type.
	/// </remarks>
	public class Lens(string label, int length)
	{
		public string label = label;
		public int length = length;
	}
}
