using System.Text.RegularExpressions;

namespace Year2022.Day19;

public class Solver : ISolver
{
	private int cache;

	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		int result = 0;

		List<Blueprint> blueprints = new();
		foreach (string line in input.AsLines())
		{
			var matches = Regex.Match(line, "Blueprint (\\d+): Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.");

			Blueprint blueprint = new Blueprint(
				int.Parse(matches.Groups[1].Value),
				new Resources(int.Parse(matches.Groups[2].Value), 0, 0),
				new Resources(int.Parse(matches.Groups[3].Value), 0, 0),
				new Resources(int.Parse(matches.Groups[4].Value), int.Parse(matches.Groups[5].Value), 0),
				new Resources(int.Parse(matches.Groups[6].Value), 0, int.Parse(matches.Groups[7].Value))
				);

			blueprints.Add(blueprint);
		}

		Dictionary<Blueprint, int> qualityLevels = new();

		var startingRobots = new Resources(1, 0, 0);

		foreach (Blueprint blueprint in blueprints)
		{
			cache = int.MinValue;
			int geodes = CalculateCrackedGeodes(startingRobots, 0, 0, new Resources(0, 0, 0), 1, blueprint, 24);
			qualityLevels.Add(blueprint, geodes * blueprint.id);
		}

		result = qualityLevels.Values.Sum();

		return result.ToString();
	}

	public int CalculateCrackedGeodes(
		Resources miningRobots,
		int nbrGeodeRobots,
		int crackedGeodes,
		Resources resources,
		int minutes,
		Blueprint blueprint,
		int max)
	{

		Resources minedResources = resources.Add(miningRobots);
		int newCrackedGeodes = crackedGeodes + nbrGeodeRobots;

		if (minutes == max)
		{
			cache = Math.Max(cache, newCrackedGeodes);
			return newCrackedGeodes;
		}

		if (newCrackedGeodes + nbrGeodeRobots * (max - minutes) + (max - minutes) * (max - minutes - 1) / 2 < cache)
		{
			return int.MinValue;
		}

		if (resources.obsidian >= blueprint.geodeRobotCost.obsidian &&
			resources.clay >= blueprint.geodeRobotCost.clay &&
			resources.ore >= blueprint.geodeRobotCost.ore)
		{
			// Build geode robot, and don't try anything else
			Resources newResources = minedResources.Subtract(blueprint.geodeRobotCost);
			return CalculateCrackedGeodes(miningRobots, nbrGeodeRobots + 1, newCrackedGeodes, newResources, minutes + 1, blueprint, max);
		}


		List<int> paths = new List<int>();

		if (miningRobots.obsidian < blueprint.maxCosts.obsidian &&
			resources.obsidian >= blueprint.obsidianRobotCost.obsidian &&
			resources.clay >= blueprint.obsidianRobotCost.clay &&
			resources.ore >= blueprint.obsidianRobotCost.ore)
		{
			// Build obsidian robot, and don't try anything else
			Resources newResources = minedResources.Subtract(blueprint.obsidianRobotCost);
			Resources newRobots = new Resources(miningRobots.ore, miningRobots.clay, miningRobots.obsidian + 1);

			paths.Add(CalculateCrackedGeodes(newRobots, nbrGeodeRobots, newCrackedGeodes, newResources, minutes + 1, blueprint, max));
		}
		if (miningRobots.clay < blueprint.maxCosts.clay &&
			resources.obsidian >= blueprint.clayRobotCost.obsidian &&
			resources.clay >= blueprint.clayRobotCost.clay &&
			resources.ore >= blueprint.clayRobotCost.ore)
		{
			// Build clay robot
			Resources newResources = minedResources.Subtract(blueprint.clayRobotCost);
			Resources newRobots = new Resources(miningRobots.ore, miningRobots.clay + 1, miningRobots.obsidian);

			paths.Add(CalculateCrackedGeodes(newRobots, nbrGeodeRobots, newCrackedGeodes, newResources, minutes + 1, blueprint, max));
		}
		if (miningRobots.ore < blueprint.maxCosts.ore &&
			resources.obsidian >= blueprint.oreRobotCost.obsidian &&
			resources.clay >= blueprint.oreRobotCost.clay &&
			resources.ore >= blueprint.oreRobotCost.ore)
		{
			// Build ore robot
			Resources newResources = minedResources.Subtract(blueprint.oreRobotCost);
			Resources newRobots = new Resources(miningRobots.ore + 1, miningRobots.clay, miningRobots.obsidian);

			paths.Add(CalculateCrackedGeodes(newRobots, nbrGeodeRobots, newCrackedGeodes, newResources, minutes + 1, blueprint, max));
		}

		paths.Add(CalculateCrackedGeodes(miningRobots, nbrGeodeRobots, newCrackedGeodes, minedResources, minutes + 1, blueprint, max));


		return paths.Max();
	}
	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		int result = 0;

		List<Blueprint> blueprints = new();
		foreach (string line in input.AsLines().Take(3))
		{
			var matches = Regex.Match(line, "Blueprint (\\d+): Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.");

			Blueprint blueprint = new Blueprint(
				int.Parse(matches.Groups[1].Value),
				new Resources(int.Parse(matches.Groups[2].Value), 0, 0),
				new Resources(int.Parse(matches.Groups[3].Value), 0, 0),
				new Resources(int.Parse(matches.Groups[4].Value), int.Parse(matches.Groups[5].Value), 0),
				new Resources(int.Parse(matches.Groups[6].Value), 0, int.Parse(matches.Groups[7].Value))
				);

			blueprints.Add(blueprint);
		}

		Dictionary<Blueprint, int> qualityLevels = new();

		var startingRobots = new Resources(1, 0, 0);

		foreach (Blueprint blueprint in blueprints)
		{
			cache = int.MinValue;
			int geodes = CalculateCrackedGeodes(startingRobots, 0, 0, new Resources(0, 0, 0), 1, blueprint, 32);
			qualityLevels.Add(blueprint, geodes);
		}

		result = qualityLevels.Values.Aggregate((a, b) => a * b);

		return result.ToString();
	}

	public class Blueprint
	{
		public int id;
		public Resources oreRobotCost;
		public Resources clayRobotCost;
		public Resources obsidianRobotCost;
		public Resources geodeRobotCost;
		public Resources maxCosts;

		public Blueprint(int id, Resources oreRobotCost, Resources clayRobotCost, Resources obsidianRobotCost, Resources geodeRobotCost)
		{
			this.id = id;
			this.oreRobotCost = oreRobotCost;
			this.clayRobotCost = clayRobotCost;
			this.obsidianRobotCost = obsidianRobotCost;
			this.geodeRobotCost = geodeRobotCost;

			maxCosts = new Resources(
				new[] { oreRobotCost.ore, clayRobotCost.ore, obsidianRobotCost.ore, geodeRobotCost.ore }.Max(),
				new[] { oreRobotCost.clay, clayRobotCost.clay, obsidianRobotCost.clay, geodeRobotCost.clay }.Max(),
				new[] { oreRobotCost.obsidian, clayRobotCost.obsidian, obsidianRobotCost.obsidian, geodeRobotCost.obsidian }.Max());
		}
	}

	public record Resources(int ore, int clay, int obsidian)
	{
		public Resources Add(Resources other)
		{
			return new Resources(
				ore + other.ore,
				clay + other.clay,
				obsidian + other.obsidian);
		}

		public Resources Subtract(Resources other)
		{
			return new Resources(
				ore - other.ore,
				clay - other.clay,
				obsidian - other.obsidian);
		}
	};

}
