using SplitMate.Shared;

namespace SplitMate.Domain.Entities
{
	public class ShoppingItem
	{
		public int Id { get; set; }
		public decimal Value { get; set; }
		public required string Name { get; set; }
		public ShoppingItemType Type { get; set; }

		public int? UserId { get; private set; }
		public User? User { get; set; }

		public int ShoppingListId { get; private set; }
		public ShoppingList ShoppingList { get; set; } = null!;
	}
}
