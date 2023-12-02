using System.Text.Json;
using Shared;
using Shared.Helpers;

namespace Year2022.Day13
{
	public class Solver : ISolver
	{
		public async Task<string> PartOne(string input)
		{
			await Task.Yield();

			int result = 0;
			int index = 1;

			foreach (string packets in input.AsLineBlocks())
			{
				var lines = packets.AsLines();
				if (IsRightOrder(lines[0], lines[1]))
				{
					result += index;
				}
				index++;
			}

			return result.ToString();
		}

		public async Task<string> PartTwo(string input)
		{
			await Task.Yield();

			int result = 0;

			string divider1 = "\n[[2]]\n";
			string divider2 = "\n[[6]]\n";

			input += divider1 + divider2;

			List<JsonElement> packets = new();

			foreach (string packet in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
			{
				packets.Add(JsonSerializer.Deserialize<JsonElement>(packet));
			}

			packets.Sort(new PackageComparer());

			int index1 = packets
				.Select(p => p.GetRawText())
				.ToList()
				.IndexOf(divider1.Trim()) + 1;

			int index2 = packets
				.Select(p => p.GetRawText())
				.ToList()
				.IndexOf(divider2.Trim()) + 1;

			result = index1 * index2;

			return result.ToString();
		}


		public bool IsRightOrder(string left, string right)
		{
			JsonElement leftData = JsonSerializer.Deserialize<JsonElement>(left);
			JsonElement rightData = JsonSerializer.Deserialize<JsonElement>(right);

			PackageComparer comp = new PackageComparer();

			return comp.Compare(leftData, rightData) < 0;
		}
	}

	internal class PackageComparer : IComparer<JsonElement>
	{
		public int Compare(JsonElement x, JsonElement y)
		{
			if (x.ValueKind == JsonValueKind.Number && y.ValueKind == JsonValueKind.Number)
			{
				return x.GetInt32() - y.GetInt32();
			}
			else if (x.ValueKind == JsonValueKind.Array && y.ValueKind == JsonValueKind.Array)
			{
				for (int i = 0; i < x.GetArrayLength() && i < y.GetArrayLength(); i++)
				{
					int res = Compare(x[i], y[i]);
					if (res != 0)
					{
						return res;
					}
				}

				if (x.GetArrayLength() < y.GetArrayLength())
				{
					return -1;
				}

				if (x.GetArrayLength() > y.GetArrayLength())
				{
					return 1;
				}

				return 0;
			}
			else
			{
				if (x.ValueKind == JsonValueKind.Number)
				{
					return Compare(JsonSerializer.Deserialize<JsonElement>("[" + x.GetRawText() + "]"), y);
				}
				if (y.ValueKind == JsonValueKind.Number)
				{
					return Compare(x, JsonSerializer.Deserialize<JsonElement>("[" + y.GetRawText() + "]"));
				}
			}

			return 0;
		}
	}
}
