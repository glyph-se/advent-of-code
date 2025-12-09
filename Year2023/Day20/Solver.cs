using System.Diagnostics;

namespace Year2023.Day20;

public class Solver : ISolver
{
	static Dictionary<string, IModule> modules = null!;

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		ParseModules(input);

		long low = 0;
		long high = 0;

		for (int i = 1; i <= 1000; i++)
		{
			Queue<(string next, string prev, bool state)> queue = new();

			queue.Enqueue(("broadcaster", "button", false));

			while (queue.Any())
			{
				var op = queue.Dequeue();

				if (op.state)
				{
					high++;
				}
				else
				{
					low++;
				}

				IModule pushedModule = modules[op.next];

				List<(string next, bool state)> results = pushedModule.Send(op.state, op.prev);

				results.Select(r => (r.next, op.next, r.state)).ToList().ForEach(r => queue.Enqueue(r));
			}
		}

		result = low * high;

		return result.ToString();
	}

	private static void ParseModules(string input)
	{
		modules = new Dictionary<string, IModule>();

		modules.Add("output", new OutputModule("output"));
		modules.Add("rx", new OutputModule("rx"));

		foreach (string line in input.ParseLines())
		{
			var split = line.TrimSplit(" -> ");
			var name = split[0];
			var dest = split[1].TrimSplit(",").ToList();

			IModule module = null!;
			if (name.StartsWith("%"))
			{
				module = new FlipFlopModule(name.Substring(1), dest);
			}
			if (name.StartsWith("&"))
			{
				module = new ConjuncationModule(name.Substring(1), dest);
			}
			if (name.StartsWith("b"))
			{
				module = new BroadcastModule(dest);
			}
			modules.Add(module!.Name, module);
		}

		foreach (ConjuncationModule cm in modules.Values.OfType<ConjuncationModule>())
		{
			// Find all modules with dest to me
			foreach (IModule m in modules.Values)
			{
				if (m.Dest.Contains(cm.Name))
				{
					cm.Inputs.Add(m.Name, false);
				}
			}
		}
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 1;

		ParseModules(input);

		IModule? goesToRx = modules
			.Values
			.Where(m => m.Dest.FirstOrDefault() == "rx")
			.SingleOrDefault();

		if (goesToRx == null)
		{
			return "Example not valid for part 2";
		}

		HashSet<string> watch = modules
			.Values
			.Where(m => m.Dest.FirstOrDefault() == goesToRx.Name)
			.Select(m => m.Name)
			.ToHashSet();

		for (int i = 1; i <= int.MaxValue; i++)
		{
			Queue<(string next, string prev, bool state)> queue = new();

			queue.Enqueue(("broadcaster", "button", false));

			while (queue.Any())
			{
				var op = queue.Dequeue();

				IModule pushedModule = modules[op.next];

				List<(string next, bool state)> results = pushedModule.Send(op.state, op.prev);

				results
					.Select(r => (r.next, op.next, r.state))
					.ToList()
					.ForEach(r => queue.Enqueue(r));

				IEnumerable<string> found = results
					.Where(r => r.state == false)
					.Select(r => r.next)
					.Intersect(watch);

				// Any of the modules we watch where found in this cycle
				if (found.Any())
				{
					found
						.ToList()
						.ForEach(f => watch.Remove(f));

					result = MathHelpers.lcm(result, i);
				}

				if (watch.Count == 0)
				{
					// Found all
					return result.ToString();
				}
			}
		}

		throw new Exception("Should not happen");
	}

	public interface IModule
	{
		string Name { get; }
		char Prefix { get; }
		List<string> Dest { get; }
		List<(string name, bool state)> Send(bool pulse, string source);
	}

	[DebuggerDisplay("{Prefix}{Name}")]
	public class BroadcastModule : IModule
	{
		public BroadcastModule(List<string> dest)
		{
			Dest = dest;
		}

		public char Prefix => 'b';

		public string Name => "broadcaster";

		public List<string> Dest { get; set; }

		public List<(string name, bool state)> Send(bool pulse, string source)
		{
			List<(string name, bool state)> result = new();
			foreach (string d in Dest)
			{
				result.Add((d, pulse));
			}

			return result;
		}
	}

	[DebuggerDisplay("{Prefix}{Name}")]
	public class FlipFlopModule : IModule
	{
		bool state;
		public FlipFlopModule(string name, List<string> dest)
		{
			Name = name;
			Dest = dest;
			state = false;
		}

		public char Prefix => '%';

		public string Name { get; set; }

		public List<string> Dest { get; set; }

		public List<(string name, bool state)> Send(bool pulse, string source)
		{
			List<(string name, bool state)> result = new();
			if (pulse == true)
			{
				// High pulse = Do nothing
			}
			else
			{
				// Low pulse: Flip state
				state = !state;
				foreach (string d in Dest)
				{
					result.Add((d, state));
				}
			}
			return result;
		}
	}

	[DebuggerDisplay("{Prefix}{Name}")]
	public class ConjuncationModule : IModule
	{
		public ConjuncationModule(string name, List<string> dest)
		{
			Name = name;
			Dest = dest;
			Inputs = new Dictionary<string, bool>();
		}

		public Dictionary<string, bool> Inputs { get; }

		public char Prefix => '&';

		public string Name { get; set; }

		public List<string> Dest { get; set; }

		public List<(string name, bool state)> Send(bool pulse, string source)
		{
			List<(string name, bool state)> result = new();
			Inputs[source] = pulse;

			if (Inputs.Values.All(v => v))
			{
				// All inputs high, send low
				foreach (string d in Dest)
				{

					result.Add((d, false));
				}
			}
			else
			{
				foreach (string d in Dest)
				{
					result.Add((d, true));
				}
			}

			return result;
		}
	}

	[DebuggerDisplay("{Prefix}{Name}")]
	public class OutputModule : IModule
	{
		public OutputModule(string name)
		{
			Name = name;
			Dest = new List<string>();
		}

		public char Prefix => 'o';

		public string Name { get; set; }

		public List<string> Dest { get; set; }

		public List<(string name, bool state)> Send(bool pulse, string source)
		{
			List<(string name, bool state)> result = new();

			return result;
		}
	}
}
