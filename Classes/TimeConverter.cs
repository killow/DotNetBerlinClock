using System;
using System.Linq;

namespace BerlinClock.Classes
{
    public class TimeConverter : ITimeConverter
    {
        public string ConvertTime(string aTime)
        {
			// split time string to integer components (hours, minutes and seconds)
			int[] aTimeComponents = aTime.Split(':').Select(x => Convert.ToInt32(x)).ToArray();

	        var timeToConvert = new TimeSpan(aTimeComponents[0], aTimeComponents[1], aTimeComponents[2]);

	        return string.Format(new BerlinClockFormatter(), "{0}", timeToConvert);
        }
    }
}
