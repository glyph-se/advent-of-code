namespace Year2023.Day19;

public class Solver : ISolver
{
	List<Workflow> workflows = null!;

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.ParseLineBlocks();
		List<Part> parts = new List<Part>();
		workflows = new List<Workflow>();

		foreach (string workFlowLine in blocks[0].ParseLines())
		{
			var split = workFlowLine.TrimSplit(["{", "}", ","]);

			string name = split[0];
			string dest = split[^1];
			List<(char cat, char op, int value, string dest)> checks = new();

			for (int i = 1; i < split.Length - 1; i++)
			{
				string check = split[i];
				var end = check.Substring(2).Split(":");

				checks.Add((check[0], check[1], end[0].ToInt(), end[1]));
			}

			Workflow w = new Workflow(name, checks, dest);
			workflows.Add(w);
		}

		foreach (string partLine in blocks[1].ParseLines())
		{
			string cleaned = partLine.Trim(['{', '}']);
			var ratings = cleaned.TrimSplit(",")
				.Select(s => s.Substring(2).ToInt())
				.ToArray();

			Part p = new Part(ratings[0], ratings[1], ratings[2], ratings[3]);
			parts.Add(p);
		}

		foreach (Part part in parts)
		{
			Workflow nextWorkflow = workflows.Where(w => w.name == "in").Single();

			while (true)
			{
				string nextName = nextWorkflow.Sort(part);

				if (nextName == "R")
				{
					break;
				}

				if (nextName == "A")
				{
					result += part.Sum();
					break;
				}

				nextWorkflow = workflows.Where(w => w.name == nextName).Single();
			}
		}


		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.ParseLineBlocks();
		workflows = new List<Workflow>();

		foreach (string workFlowLine in blocks[0].ParseLines())
		{
			var split = workFlowLine.TrimSplit(["{", "}", ","]);

			string name = split[0];
			string dest = split[^1];
			List<(char cat, char op, int value, string dest)> checks = new();

			for (int i = 1; i < split.Length - 1; i++)
			{
				string check = split[i];
				var end = check.Substring(2).Split(":");

				checks.Add((check[0], check[1], end[0].ToInt(), end[1]));
			}

			Workflow w = new Workflow(name, checks, dest);
			workflows.Add(w);
		}

		result = Calc(new PartRange((1, 4000), (1, 4000), (1, 4000), (1, 4000)), "in");

		return result.ToString();
	}

	public long Calc(PartRange p, string workflowName)
	{
		Workflow w = workflows.Where(w => w.name == workflowName).Single();

		long result = 0;
		foreach (var check in w.checks)
		{
			PartRange newP = p.Copy();

			if (check.cat == 'x' && check.op == '>' && p.x.high > check.value)
			{
				newP.x.low = Math.Max(p.x.low, check.value + 1);
				result += Calc2(check.dest, newP);
				p.x.high = Math.Min(p.x.high, check.value);
			}
			if (check.cat == 'x' && check.op == '<' && p.x.low < check.value)
			{
				newP.x.high = Math.Min(p.x.high, check.value - 1);
				result += Calc2(check.dest, newP);
				p.x.low = Math.Max(p.x.low, check.value);
			}
			if (check.cat == 'm' && check.op == '>' && p.m.high > check.value)
			{
				newP.m.low = Math.Max(p.m.low, check.value + 1);
				result += Calc2(check.dest, newP);
				p.m.high = Math.Min(p.m.high, check.value);
			}
			if (check.cat == 'm' && check.op == '<' && p.m.low < check.value)
			{
				newP.m.high = Math.Min(p.m.high, check.value - 1);
				result += Calc2(check.dest, newP);
				p.m.low = Math.Max(p.m.low, check.value);
			}
			if (check.cat == 'a' && check.op == '>' && p.a.high > check.value)
			{
				newP.a.low = Math.Max(p.a.low, check.value + 1);
				result += Calc2(check.dest, newP);
				p.a.high = Math.Min(p.a.high, check.value);
			}
			if (check.cat == 'a' && check.op == '<' && p.a.low < check.value)
			{
				newP.a.high = Math.Min(p.a.high, check.value - 1);
				result += Calc2(check.dest, newP);
				p.a.low = Math.Max(p.a.low, check.value);
			}
			if (check.cat == 's' && check.op == '>' && p.s.high > check.value)
			{
				newP.s.low = Math.Max(p.s.low, check.value + 1);
				result += Calc2(check.dest, newP);
				p.s.high = Math.Min(p.s.high, check.value);
			}
			if (check.cat == 's' && check.op == '<' && p.s.low < check.value)
			{
				newP.s.high = Math.Min(p.s.high, check.value - 1);
				result += Calc2(check.dest, newP);
				p.s.low = Math.Max(p.s.low, check.value);
			}
		}

		result += Calc2(w.dest, p);

		return result;
	}

	private long Calc2(string dest, PartRange newP)
	{
		if (dest == "R")
		{
			return 0;
		}

		if (dest == "A")
		{
			return newP.Sum();
		}

		return Calc(newP, dest);
	}

	public class Workflow
	{

		public string name;
		public List<(char cat, char op, int value, string dest)> checks;
		public string dest;

		public Workflow(string name, List<(char cat, char op, int value, string dest)> checks, string dest)
		{
			this.name = name;
			this.checks = checks;
			this.dest = dest;
		}

		public string Sort(Part p)
		{
			foreach (var check in checks)
			{
				if (check.cat == 'x' && check.op == '>' && p.x > check.value)
				{
					return check.dest;
				}
				if (check.cat == 'x' && check.op == '<' && p.x < check.value)
				{
					return check.dest;
				}
				if (check.cat == 'm' && check.op == '>' && p.m > check.value)
				{
					return check.dest;
				}
				if (check.cat == 'm' && check.op == '<' && p.m < check.value)
				{
					return check.dest;
				}
				if (check.cat == 'a' && check.op == '>' && p.a > check.value)
				{
					return check.dest;
				}
				if (check.cat == 'a' && check.op == '<' && p.a < check.value)
				{
					return check.dest;
				}
				if (check.cat == 's' && check.op == '>' && p.s > check.value)
				{
					return check.dest;
				}
				if (check.cat == 's' && check.op == '<' && p.s < check.value)
				{
					return check.dest;
				}
			}

			return dest;
		}
	}

	public class Part
	{

		public int x;
		public int m;
		public int a;
		public int s;

		public Part(int x, int m, int a, int s)
		{
			this.x = x;
			this.m = m;
			this.a = a;
			this.s = s;
		}

		public int Sum()
		{
			return this.x + this.m + this.a + this.s;
		}
	}

	public class PartRange
	{
		public (int low, int high) x;
		public (int low, int high) m;
		public (int low, int high) a;
		public (int low, int high) s;

		public PartRange(
			(int low, int high) x,
			(int low, int high) m,
			(int low, int high) a,
			(int low, int high) s)
		{
			this.x = x;
			this.m = m;
			this.a = a;
			this.s = s;
		}

		public long Sum()
		{
			return (long)(x.high - x.low + 1) * (long)(m.high - m.low + 1) * (long)(a.high - a.low + 1) * (long)(s.high - s.low + 1);
		}

		public PartRange Copy()
		{
			return new PartRange(x, m, a, s);
		}
	}
}
