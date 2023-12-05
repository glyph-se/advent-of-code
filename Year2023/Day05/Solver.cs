using System.Collections.Concurrent;
using System.Linq;
using Shared;
using Shared.Helpers;

namespace Year2023.Day05;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.AsLineBlocks().ToArray();

		List<long> seeds = new List<long>();
		var seedBlock = blocks[0];
		var seedString = seedBlock.Replace("seeds: ", "");
		seeds.AddRange(seedString.TrimSplit(" ").Select(s => long.Parse(s)));

		List<Range> seedToSoil = ParseMap(blocks[1]);
		List<Range> soilToFerilizer = ParseMap(blocks[2]);
		List<Range> fertilizerToWater = ParseMap(blocks[3]);
		List<Range> waterToLight = ParseMap(blocks[4]);
		List<Range> lightToTemperature = ParseMap(blocks[5]);
		List<Range> temperatureToHumidity = ParseMap(blocks[6]);
		List<Range> humidityToLocation = ParseMap(blocks[7]);

		List<long> locations = new List<long>();

		foreach (long seed in seeds)
		{
			long soil = FindLocation(seed, seedToSoil);
			long fertilizer = FindLocation(soil, soilToFerilizer);
			long water = FindLocation(fertilizer, fertilizerToWater);
			long light = FindLocation(water, waterToLight);
			long temperature = FindLocation(light, lightToTemperature);
			long humidity = FindLocation(temperature, temperatureToHumidity);
			long location = FindLocation(humidity, humidityToLocation);
			locations.Add(location);
		}

		result = locations.Min();

		return result.ToString();
	}

	private long FindLocation(long start, List<Range> map)
	{
		foreach(Range range in map)
		{
			long? dest = range.Map(start);
			if (dest != null)
			{
				return dest.Value;
			}
		}

		return start;
	}

	private static List<Range> ParseMap(string map)
	{
		List<Range> result = new List<Range>();

		IEnumerable<string> lines = map.AsLines().Skip(1);
		foreach (var line in lines)
		{
			(string dest, string source, string length) = line.Split3(" ");
			result.Add(new Range(source, dest, length));
		}

		return result;
	}

	/// <summary>
	/// This is very slow, parse the debugger after a while to get the result.
	/// </summary>
	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		var blocks = input.AsLineBlocks().ToArray();

		List<Range> seeds = new List<Range>();
		var seedBlock = blocks[0];
		var seedString = seedBlock.Replace("seeds: ", "");
		string[] seedNumbers = seedString.TrimSplit(" ").ToArray();

		for (int i = 0; i < seedNumbers.Length; i += 2)
		{
			seeds.Add(new Range(seedNumbers[i], long.MinValue.ToString(), seedNumbers[i + 1]));
		}

		List<Range> seedToSoil = ParseMap(blocks[1]);
		List<Range> soilToFerilizer = ParseMap(blocks[2]);
		List<Range> fertilizerToWater = ParseMap(blocks[3]);
		List<Range> waterToLight = ParseMap(blocks[4]);
		List<Range> lightToTemperature = ParseMap(blocks[5]);
		List<Range> temperatureToHumidity = ParseMap(blocks[6]);
		List<Range> humidityToLocation = ParseMap(blocks[7]);

		ConcurrentBag<long> allMin = new ConcurrentBag<long>();

		Parallel.ForEach(seeds, seed =>
			{
				long parMin = long.MaxValue;
				for (long i = seed.sourceStart; i < seed.sourceEnd; i++)
				{
					long soil = FindLocation(i, seedToSoil);
					long fertilizer = FindLocation(soil, soilToFerilizer);
					long water = FindLocation(fertilizer, fertilizerToWater);
					long light = FindLocation(water, waterToLight);
					long temperature = FindLocation(light, lightToTemperature);
					long humidity = FindLocation(temperature, temperatureToHumidity);
					long location = FindLocation(humidity, humidityToLocation);
					parMin = long.Min(parMin, location);
				}
				allMin.Add(parMin);
			});

		result = allMin.Min();

		return result.ToString();
	}

	public class Range
	{
		public Range(string sourceStart, string destStart, string length)
		{
			this.sourceStart = long.Parse(sourceStart);
			this.sourceEnd = long.Parse(sourceStart) + long.Parse(length);
			this.destStart = long.Parse(destStart);
			this.destEnd = long.Parse(destStart) + long.Parse(length);
		}

		public long? Map(long source)
		{
			// Range "58 x 2" is 58 59, NOT 60
			if(source >= sourceStart && source < sourceEnd)
			{
				long diff = source - sourceStart;
				return destStart + diff;
			}

			return null;
		}

		public long sourceStart;
		public long sourceEnd;
		public long destStart;
		public long destEnd;
	}
}
