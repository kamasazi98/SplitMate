using SplitMate.Domain.Entities;

namespace SplitMate.Domain.Specifications
{
	public partial class ShoppingListSpecification
	{
		public ShoppingList Entity { get; private set; } = new();
		public ShoppingListSpecification Initialize(ShoppingList shoppingList)
		{
			Entity = shoppingList;
			return this;
		}
		public void CreateNew(CreateNewCommand command)
		{
			command.Validate();
			Entity.User = command.CreatedBy;
			Entity.Name = command.Name;
			Entity.CreateDate = DateTime.Now;
			Entity.IsSettled = false;
		}
		public ShoppingItem AddItem(AddItemCommand command)
		{
			command.Validate();

			ShoppingItem shoppingItem = new()
			{
				Value = command.Value,
				Name = command.Name,
				Type = command.Type,
				User = command.User,
				ShoppingList = Entity
			};

			Entity.Items.Add(shoppingItem);
			return shoppingItem;
		}
		public void ChangeItem(ChangeItemCommand command)
		{
			command.Validate(Entity);

			var item = Entity.Items.First(x => x.Id == command.ItemId);

			item.Value = command.Value;
			item.Name = command.Name;
			item.Type = command.Type;
			item.User = command.User;
		}
		public ShoppingItem DeleteItem(DeleteItemCommand command)
		{
			command.Validate(Entity);
			var item = Entity.Items.First(y => y.Id == command.ItemId);
			Entity.Items.Remove(item);
			return item;
		}
	}
}
