using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;
public static class CycleHelpers
{
	public static (int cycleStart, int cycleLength, Dictionary<int, TReturn>) FindCycle<TReturn>(Func<TReturn> func, Func<TReturn, string> serializer)
	{
		Dictionary<int, TReturn> results = new();

		//TODO

		return (0, 0, results);
	}
}
