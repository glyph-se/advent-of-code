using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;
public static class CycleHelpers
{
	public static (int start, int length, Dictionary<int, TReturn> results) FindCycle<TReturn>(
		Func<TReturn, TReturn> func,
		TReturn initialState,
		Func<TReturn, string> serializer)
	{
		Dictionary<int, TReturn> results = new();
		Dictionary<string, int> seen = new();

		TReturn state = initialState;

		int cycle = 0;
		while (true)
		{
			cycle++;
			state = func(state);

			string key = serializer(state);
			if (seen.ContainsKey(key))
			{
				// We are done
				int prevSeen = seen[key];
				return (prevSeen, cycle-prevSeen, results);
			}

			results.Add(cycle, state);
			seen[key] = cycle;
		}
	}
}
