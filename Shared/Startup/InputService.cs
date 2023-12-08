namespace Shared.Startup;

public static class InputService
{
	public static async Task<string> GetInput(InputType inputType, int year, int day)
	{
		string inputs;

		switch (inputType)
		{
			case InputType.Example1:
				inputs = await ReadOrInputFileAsync(InputConstants.Example1InputPath(year, day));
				break;
			case InputType.Example2:
				inputs = await ReadOrInputFileAsync(InputConstants.Example2InputPath(year, day));
				break;
			case InputType.Example3:
				inputs = await ReadOrInputFileAsync(InputConstants.Example3InputPath(year, day));
				break;
			case InputType.Full:
				inputs = await ReadOrDownloadFileAsync(year, day);
				break;
			case InputType.Custom:
				inputs = ReadFromConsole();
				break;
			default:
				throw new Exception($"Invalid {nameof(InputType)}");
		}

		Console.WriteLine();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine($"Returning {inputs?.Length} characters");
		Console.ResetColor();
		Console.WriteLine();

		return inputs!;
	}

	private static string ReadFromConsole()
	{
		Console.WriteLine("Type input (end with Ctrl+Z):");
		Console.Write("> ");
		IList<string> inputs = new List<string>();
		string? line;

		while ((line = Console.ReadLine()) != null)
		{
			inputs.Add(line);
		}

		if (inputs.Count == 0)
		{
			throw new Exception("No input found");
		}

		return string.Join('\n', inputs);
	}

	private async static Task<string> ReadOrDownloadFileAsync(int year, int day)
	{

		string filePath = InputConstants.FullInputPath(year, day);

		Console.WriteLine();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine($"Using file {filePath}");
		Console.ResetColor();

		if (!File.Exists(filePath))
		{
			Uri uri = new Uri($"https://adventofcode.com/{year}/day/{day}/input");

			Console.WriteLine($"File not found, downloading {uri}...");

			using (var handler = new HttpClientHandler())
			using (var client = new HttpClient(handler))
			{
				string auth = File.ReadAllText(InputConstants.BaseDirectory + ".sessionauth");
				client.DefaultRequestHeaders.Add("Cookie", "session=" + auth);
				byte[] data = await client.GetByteArrayAsync(uri);

				await File.WriteAllBytesAsync(filePath, data);
			}

		}

		string fileContents = await ReadFileAsync(filePath);

		return fileContents;
	}

	private async static Task<string> ReadOrInputFileAsync(string filePath)
	{
		Console.WriteLine();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine($"Using file {filePath}");
		Console.ResetColor();

		if (!File.Exists(filePath))
		{
			Console.WriteLine($"File not found, please enter data...");

			string? data = ReadFromConsole();

			await File.WriteAllTextAsync(filePath, data);
		}

		string fileContents = await ReadFileAsync(filePath);

		return fileContents;
	}

		public static async Task<string> ReadFileAsync(string filePath)
		{
			string fileContents = await File.ReadAllTextAsync(filePath, System.Text.Encoding.UTF8);

			fileContents = fileContents.Replace("\r\n", "\n");
			return fileContents;
		}
}