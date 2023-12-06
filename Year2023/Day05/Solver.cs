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
		var seedString = seedBlock.ReplaceRemove("seeds: ");
		seeds.AddRange(seedString.TrimSplit(" ").Select(s => long.Parse(s)));

		List<Map> seedToSoil = ParseMap(blocks[1]);
		List<Map> soilToFerilizer = ParseMap(blocks[2]);
		List<Map> fertilizerToWater = ParseMap(blocks[3]);
		List<Map> waterToLight = ParseMap(blocks[4]);
		List<Map> lightToTemperature = ParseMap(blocks[5]);
		List<Map> temperatureToHumidity = ParseMap(blocks[6]);
		List<Map> humidityToLocation = ParseMap(blocks[7]);

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

	private long FindLocation(long start, List<Map> maps)
	{
		foreach(Map map in maps)
		{
			long? dest = map.Move(start);
			if (dest != null)
			{
				return dest.Value;
			}
		}

		return start;
	}

	private static List<Map> ParseMap(string map)
	{
		List<Map> result = new List<Map>();

		IEnumerable<string> lines = map.AsLines().Skip(1);
		foreach (var line in lines)
		{
			(string dest, string source, string length) = line.Split3(" ");
			result.Add(new Map(source, dest, length));
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
			long start = seedNumbers[i].ToLong();
			long length = seedNumbers[i + 1].ToLong();
			seeds.Add(new Range(start, start + length -1));
		}

		List<Map> seedToSoil = ParseMap(blocks[1]);
		List<Map> soilToFerilizer = ParseMap(blocks[2]);
		List<Map> fertilizerToWater = ParseMap(blocks[3]);
		List<Map> waterToLight = ParseMap(blocks[4]);
		List<Map> lightToTemperature = ParseMap(blocks[5]);
		List<Map> temperatureToHumidity = ParseMap(blocks[6]);
		List<Map> humidityToLocation = ParseMap(blocks[7]);
		

		List<Range> soils = FindRanges(seeds, seedToSoil);
		List<Range> fertilizers = FindRanges(soils, soilToFerilizer);
		List<Range> waters = FindRanges(fertilizers, fertilizerToWater);
		List<Range> lights = FindRanges(waters, waterToLight);
		List<Range> temperatures = FindRanges(lights, lightToTemperature);
		List<Range> humiditys = FindRanges(temperatures, temperatureToHumidity);
		List<Range> locations = FindRanges(humiditys, humidityToLocation);
		
		result = locations.Min(l => l.start);

		return result.ToString();
	}

	private List<Range> FindRanges(List<Range> sources, List<Map> mappings)
	{
		List<Range> newRanges = new List<Range>();

		Queue<Range> queue = new Queue<Range>(sources);

		while(queue.Any())
		{
			Range source = queue.Dequeue();
			bool found = false;

			foreach (Map map in mappings)
			{
				if(source.start >= map.sourceStart && source.end < map.sourceEnd)
				{
					newRanges.Add(new Range(source.start - map.sourceStart + map.destStart, source.end - map.sourceStart + map.destStart));
					found = true;
				}
				else if (source.start < map.sourceStart && source.end >= map.sourceStart && source.end < map.sourceEnd)
				{
					queue.Enqueue(new Range(source.start, map.sourceStart - 1));
					newRanges.Add(new Range(map.destStart, map.destStart + source.end - map.sourceStart));
					found = true;
				}
				else if(source.start < map.sourceEnd && source.end >= map.sourceEnd && source.start >= map.sourceStart)
				{
					queue.Enqueue(new Range(map.sourceEnd, source.end));
					newRanges.Add(new Range(map.destStart + source.start - map.sourceStart, map.destEnd - 1));
					found = true;
				}
				else if(source.start < map.sourceStart && source.end >= map.sourceEnd)
				{
					queue.Enqueue(new Range(source.start, map.sourceStart - 1));
					newRanges.Add(new Range(map.destStart, map.destEnd - 1));
					queue.Enqueue(new Range(map.sourceEnd, source.end));
					found = true;
				}
				if (found)
				{
					break;
				}
			}

			if(found == false)
			{
				// Keep the same
				newRanges.Add(source);
			}
		}

		return newRanges;
	}

	public class Range
	{
		public Range(long start, long end)
		{
			this.start = start;
			this.end = end;
			this.length = end - start + 1;
		}
		public long start;
		public long end;
		public long length;
	}

	public class Map
	{
		public Map(string sourceStart, string destStart, string length)
		{
			this.sourceStart = long.Parse(sourceStart);
			this.sourceEnd = long.Parse(sourceStart) + long.Parse(length);
			this.destStart = long.Parse(destStart);
			this.destEnd = long.Parse(destStart) + long.Parse(length);
		}

		public long? Move(long source)
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
