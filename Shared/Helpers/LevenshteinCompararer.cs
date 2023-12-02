namespace Shared.Helpers
{
	/// <summary>
	/// Taken from https://stackoverflow.com/a/17055215
	/// </summary>
	class LevenshteinComparer : IEqualityComparer<string>
	{
		public int MaxDistance { get; set; }
		private Levenshtein _Levenshtein = new Levenshtein();

		public LevenshteinComparer() : this(50) { }

		public LevenshteinComparer(int maxDistance)
		{
			MaxDistance = maxDistance;
		}

#pragma warning disable CS8767
		public bool Equals(string x, string y)
		{
			int distance = _Levenshtein.iLD(x, y);
			return distance <= MaxDistance;
		}
#pragma warning restore CS8767

		public int GetHashCode(string obj)
		{
			return 0;
		}
	}
}
