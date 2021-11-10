using System;

namespace HSA.Homework.Web.Core.Cache
{
	public class CacheEntry<T>
		where T: class, new()
	{
		public T Value { get; set; }
		public long TimeToRecomputeValueInSeconds { get; set; }
		public DateTime Expiry { get; set; }

		public bool ShouldBeRecalculated(int beta = 1)
		{
			//avoid log(0)
			var secondsToAdd = TimeToRecomputeValueInSeconds * beta * Math.Log(new Random().NextDouble()+0.1);
			return DateTime.Compare(
				DateTime.Now.AddSeconds(secondsToAdd),
				Expiry) > 0;
		}
	}
}
