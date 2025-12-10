using System.Collections.Concurrent;

namespace Year2025.Day10;

public class Solver : ISolver
{
	public async Task<string> PartOne(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Machine> machines = ParseMachines(input);

		ConcurrentBag<long> presses = new();

		Parallel.ForEach(machines, machine =>
		{
			presses.Add(PressButtonsToLight(machine));
		});

		result = presses.Sum();

		return result.ToString();
	}


	private static long PressButtonsToLight(Machine machine)
	{
		for (int numberOfPresses = 0; numberOfPresses <= machine.lights.Count; numberOfPresses++)
		{
			foreach (IEnumerable<HashSet<int>> pressedButtons in machine.buttons.DifferentCombinations(numberOfPresses))
			{
				// All lights start as off
				List<bool> lightStatus = machine.lights.Select(l => false).ToList();

				foreach (HashSet<int> button in pressedButtons)
				{
					foreach (int light in button)
					{
						lightStatus[light] = !lightStatus[light];
					}
				}

				if (lightStatus.SequenceEqual(machine.lights))
				{
					return numberOfPresses;
				}
			}
		}

		throw new Exception("unreachable code");
	}

	public async Task<string> PartTwo(string input)
	{
		await Task.Yield();

		long result = 0;

		List<Machine> machines = ParseMachines(input);

		ConcurrentBag<long> presses = new();

		Parallel.ForEach(machines, machine =>
		{
			presses.Add(PressButtonsToJoltage(machine));
		});

		result = presses.Sum();

		return result.ToString();
	}

	private static long PressButtonsToJoltage(Machine machine)
	{
		int minPresses = machine.joltage.Sum();
		Dfs(machine.buttons, machine.joltage, 0, ref minPresses);
		return minPresses;
	}

	private static void Dfs(List<HashSet<int>> buttonsAvailable, List<int> joltageStatus, int pressesPerformed, ref int minPresses)
	{
		if (joltageStatus.Any(j => j < 0))
		{
			return;
		}

		int minPresses1 = minPresses;
		if (joltageStatus.Any(j => j >= minPresses1 - pressesPerformed))
		{
			return;
		}

		if (joltageStatus.All(j => j == 0))
		{
			minPresses = Math.Min(minPresses, pressesPerformed);
			return;
		}

		for (int j1 = 0; j1 < joltageStatus.Count; j1++)
		{
			for (int j2 = 0; j2 < joltageStatus.Count; j2++)
			{
				if (j1 == j2)
				{
					continue;
				}

				if (joltageStatus[j1] > joltageStatus[j2])
				{
					var useful = buttonsAvailable
						.Where(b => b.Contains(j1) && !b.Contains(j2));

					if (!useful.Any())
					{
						return;
					}
					if (useful.Count() == 1)
					{
						Dfs(buttonsAvailable, PressButton(joltageStatus, useful.First()), pressesPerformed + 1, ref minPresses);
						return;
					}
				}
			}
		}

		for (int i = 0; i < buttonsAvailable.Count; i++)
		{
			Dfs(buttonsAvailable[i..], PressButton(joltageStatus, buttonsAvailable[i]), pressesPerformed + 1, ref minPresses);
		}
	}

	private static List<int> PressButton(List<int> oldJoltage, HashSet<int> button)
	{
		List<int> newJoltage = oldJoltage.ToList();
		for (int i = 0; i < newJoltage.Count; i++)
		{
			if (button.Contains(i))
			{
				newJoltage[i]--;
			}
		}
		return newJoltage;
	}

	private static List<Machine> ParseMachines(string input)
	{
		List<Machine> machines = new();
		foreach (string line in input.ParseLines())
		{
			var parts = line.Split(" ");

			IEnumerable<bool> lights = parts.First()[1..^1]
				.Select(c => c == '#');

			IEnumerable<IEnumerable<int>> buttons = parts
				.Skip(1)
				.SkipLast(1)
				.Select(b => b[1..^1].Split(",").Select(s => s.ToInt()));

			IEnumerable<int> joltage = parts.Last()[1..^1]
				.Split(",")
				.Select(c => c.ToInt());

			machines.Add(new Machine(lights.ToList(), buttons.Select(l => new HashSet<int>(l)).ToList(), joltage.ToList()));
		}

		return machines;
	}

	private class Machine
	{
		public Machine(List<bool> lights, List<HashSet<int>> buttons, List<int> joltage)
		{
			this.lights = lights;
			this.buttons = buttons.OrderByDescending(b => b.Count).ToList(); // Optimization for part 2 - press the longest buttons first
			this.joltage = joltage;
		}

		public List<bool> lights;

		public List<HashSet<int>> buttons;

		public List<int> joltage;
	}
}
