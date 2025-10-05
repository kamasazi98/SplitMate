using SplitMate.Domain.Entities;
using SplitMate.Shared;

namespace SplitMate.Domain.Specifications
{
	public partial class ShoppingListSpecification
	{
		public record CreateNewCommand(string? Name, User CreatedBy)
		{
			public void Validate()
			{
				if (CreatedBy == null)
					throw new ProblemException(ErrorCode.NOT_FOUND, "User not found");
			}
		}
		public record AddItemCommand(decimal Value, string Name, ShoppingItemType Type, User? User)
		{
			public void Validate()
				=> ValidateItem(Value, Name, Type, User);
		}
		public record ChangeItemCommand(int ItemId, decimal Value, string Name, ShoppingItemType Type, User? User)
		{
			public void Validate(ShoppingList entity)
			{
				ValidateItem(Value, Name, Type, User);
				if (!entity.Items.Any(x => x.Id == ItemId))
					throw new ProblemException(ErrorCode.SHOPPING_LIST_ITEM_NOT_FOUND, $"Item not found [{ItemId}] in Shopping list [{entity.Id}].");
			}
		}
		public record DeleteItemCommand(int ItemId)
		{
			public void Validate(ShoppingList entity)
			{
				if (!entity.Items.Any(x => x.Id == ItemId))
					throw new ProblemException(ErrorCode.SHOPPING_LIST_ITEM_NOT_FOUND, $"Item not found [{ItemId}] in Shopping list [{entity.Id}].");
			}
		}

		private static void ValidateItem(decimal Value, string Name, ShoppingItemType Type, User? User)
		{
			if (Value <= decimal.Zero)
				throw new ProblemException(ErrorCode.SHOPPING_LIST_ITEM_CANNOT_PROCESS_ENTITY, $"Value cannot be less than 0");
			if (string.IsNullOrEmpty(Name))
				throw new ProblemException(ErrorCode.SHOPPING_LIST_ITEM_CANNOT_PROCESS_ENTITY, $"Name cannot be empty");
			if (Type == ShoppingItemType.OnePerson && User == null)
				throw new ProblemException(ErrorCode.SHOPPING_LIST_ITEM_CANNOT_PROCESS_ENTITY, $"User cannot be null when {nameof(Type)} is {Type}");
		}
	}
}
