using System.Collections.Generic;
using System.Diagnostics;
using Shared;
using Shared.Helpers;

namespace Year2023.Day20;

public class Solver : ISolver
{
	static List<IModule> modules;

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		modules = new List<IModule>();

		modules.Add(new OutputModule("output"));
		modules.Add(new OutputModule("rx"));

		foreach (string line in input.AsLines())
		{
			var split = line.TrimSplit(" -> ");
			var name = split[0];
			var dest = split[1].TrimSplit(",").ToList();

			if (name.StartsWith("%"))
			{
				modules.Add(new FlipFlopModule(name.Substring(1), dest));
			}
			if (name.StartsWith("&"))
			{
				modules.Add(new ConjuncationModule(name.Substring(1), dest));
			}
			if (name.StartsWith("b"))
			{
				modules.Add(new BroadcastModule(dest));
			}
		}

		foreach(ConjuncationModule cm in modules.OfType<ConjuncationModule>())
		{
			// Find all modules with dest to me
			foreach(IModule m in modules)
			{
				if (m.Dest.Contains(cm.Name))
				{
					cm.Inputs.Add(m.Name, false);
				}
			}
		}

		long low = 0;
		long high = 0;

		for(int i = 1; i <= 1000; i++)
		{
			Queue<(string next, string prev, bool state)> queue = new();

			queue.Enqueue(("broadcaster", "button", false));

			while (queue.Any())
			{
				var op = queue.Dequeue();

				IModule pushedModule = modules.Where(m => m.Name == op.next).Single();

				if (op.state)
				{
					high++;
				}
				else
				{
					low++;
				}

				List<(string next, bool state)> results = pushedModule.Send(op.state, op.prev);

				results.Select(r => (r.next, op.next, r.state)).ToList().ForEach(r => queue.Enqueue(r));
			}
		}

		result = low * high;

		return result.ToString();
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		return result.ToString();
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
				IModule m = modules.Single(m => m.Name == d);

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
					IModule m = modules.Single(m => m.Name == d);

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
					IModule m = modules.Single(m => m.Name == d);

					result.Add((d, false));
				}
			}
			else
			{
				foreach (string d in Dest)
				{
					IModule m = modules.Single(m => m.Name == d);

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
