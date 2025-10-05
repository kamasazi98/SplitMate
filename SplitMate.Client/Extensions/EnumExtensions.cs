using SplitMate.Shared;

namespace SplitMate.Client.Extensions
{
	public static class EnumExtensions
	{
		public static string TranslateEnum(this ShoppingItemType value)
		{
			var result = value switch
			{
				ShoppingItemType.AllPeople => "Wszyscy",
				ShoppingItemType.OnePerson => "Jedna grupa",
				_ => throw new NotImplementedException()
			};
			return result;
		}
	}
}
