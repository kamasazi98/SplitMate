using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SplitMate.Domain;
using SplitMate.Domain.Entities;
using SplitMate.Domain.Specifications;
using SplitMate.Infrastracture.Data;
using SplitMate.Shared;

namespace SplitMate.Infrastracture.Stores
{
	internal class ShoppingListStore(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider) : IShoppingListStore
	{
		private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
		private readonly IServiceProvider serviceProvider = serviceProvider;

		public async Task<ShoppingListSpecification> Retrieve(int id, CancellationToken cancellationToken)
		{
			var shoppingList = await applicationDbContext.ShoppingLists
				.Include(x => x.User)
				.Include(x => x.Items)
				.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
				?? throw new ProblemException(ErrorCode.NOT_FOUND, $"ShoppingLists not found by id {id}");

			return Build(shoppingList);
		}
		public ShoppingListSpecification CreateNew()
			=> serviceProvider.GetRequiredService<ShoppingListSpecification>();
		private ShoppingListSpecification Build(ShoppingList shoppingList)
			=> serviceProvider.GetRequiredService<ShoppingListSpecification>().Initialize(shoppingList);

	}

	public interface IShoppingListStore
	{
		ShoppingListSpecification CreateNew();
		Task<ShoppingListSpecification> Retrieve(int id, CancellationToken cancellationToken);
	}
}
