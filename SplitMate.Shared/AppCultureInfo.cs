using System.Globalization;

namespace SplitMate.Shared
{
	public static class AppCultureInfo
	{
		public static CultureInfo Create(CultureInfo baseCultureInfo)
		{
			var cultureInfo = (CultureInfo)baseCultureInfo.Clone();

			cultureInfo.DateTimeFormat.DateSeparator = "-";
			cultureInfo.DateTimeFormat.FullDateTimePattern = "d MMMM yyyy HH:mm:ss";
			cultureInfo.DateTimeFormat.LongDatePattern = "d MMMM yyyy";
			cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			cultureInfo.DateTimeFormat.MonthDayPattern = "d MMMM";

			cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
			cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm";
			cultureInfo.DateTimeFormat.TimeSeparator = ":";
			cultureInfo.DateTimeFormat.YearMonthPattern = "MMMM yyyy";

			cultureInfo.NumberFormat.NumberGroupSeparator = " ";
			cultureInfo.NumberFormat.CurrencyGroupSeparator = " ";

			return cultureInfo;
		}

		public const string PolishLanguage = "pl-PL";
	}
}
