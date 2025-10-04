
namespace SplitMate.Domain.Entities
{
	public class ShoppingList
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public decimal SumValue { get; set; }
		public DateTime CreateDate { get; set; }
		public bool IsSettled { get; set; }

		public int UserId { get; private set; }
		public User User { get; set; } = null!;

		public ICollection<ShoppingItem> Items { get; set; } = [];
	}
}
