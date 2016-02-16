using System;
using System.Text.RegularExpressions;

namespace BerlinClock.Classes
{
	/// <summary>
	/// Format a TimeStamp object to Berlin Clock output
	/// <remarks>Available formats: S - seconds row, H1 - hours first row, H2 - hours second row, 
	/// M1 - minutes first row, M2 - minutes second row</remarks>
	/// </summary>
	public class BerlinClockFormatter : IFormatProvider, ICustomFormatter
	{
		private const char YellowLamp = 'Y';
		private const char OffLamp = 'O';
		private const char RedLamp = 'R';

		private static readonly Regex HoursLampReplacementPattern = new Regex(			// expression used to replace 3rd, 6th and 9th
			string.Empty.PadLeft(3, YellowLamp),										// yellow lamp to red one in the first minutes row
			RegexOptions.Compiled);

		public object GetFormat(Type formatType)
		{
			return formatType == typeof(ICustomFormatter)
				? this
				: null;
		}

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (!(arg is TimeSpan))
			{
				throw new ArgumentException("Invalid formatting argument. TimeSpan expected", "arg");
			}

			var time = (TimeSpan)arg;

			switch ((format ?? "").ToLower())
			{
				case "s":
					return ToSecondsFormat(time);
				case "h1":
					return ToHoursFirstRowFormat(time);
				case "h2":
					return ToHoursSecondRowFormat(time);
				case "m1":
					return ToMinutesFirstRowFormat(time);
				case "m2":
					return ToMinutesSecondRowFormat(time);
				case "":
					return ToDefaultFormat(time);
				default:
					throw new FormatException("Not supported Berlin Clock format");
			}
		}

		private static string ToMinutesSecondRowFormat(TimeSpan time)
		{
			// string.PadLeft or PadRight fills the string with a given number of character occurrences

			return string.Empty.PadLeft(time.Minutes % 5, YellowLamp).PadRight(4, OffLamp);
		}

		private static string ToMinutesFirstRowFormat(TimeSpan time)
		{
			return HoursLampReplacementPattern.Replace(
				string.Empty.PadLeft(time.Minutes / 5, YellowLamp).PadRight(11, OffLamp),		// input
				string.Empty.PadLeft(2, YellowLamp) + RedLamp);									// replace with
		}

		private static string ToHoursSecondRowFormat(TimeSpan time)
		{
			return string.Empty.PadLeft((int)time.TotalHours % 5, RedLamp).PadRight(4, OffLamp);
		}

		private static string ToHoursFirstRowFormat(TimeSpan time)
		{
			return string.Empty.PadLeft((int)time.TotalHours / 5, RedLamp).PadRight(4, OffLamp);
		}

		private static string ToSecondsFormat(TimeSpan time)
		{
			return ((time.Seconds % 2) == 0 ? YellowLamp : OffLamp).ToString();
		}

		private static string ToDefaultFormat(TimeSpan time)
		{
			return string.Format(new BerlinClockFormatter(), "{0:S}\r\n{1:H1}\r\n{2:H2}\r\n{3:M1}\r\n{4:M2}",
				time,
				time,
				time,
				time,
				time);
		}
	}
}