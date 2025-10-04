using MediatR;
using Microsoft.AspNetCore.Mvc;
using SplitMate.Extensions;
using SplitMate.Shared.Features.ShoppingList.Commands;
using SplitMate.Shared.Features.ShoppingList.Queries;

namespace SplitMate.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShoppingListsController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator mediator = mediator;

		[HttpGet]
		public Task<IActionResult> GetShoppingLists()
		{
			return this.ResolveResult(
				resultTask: mediator.Send(new RetrieveAllShoppingListQuery()),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}

		[HttpGet("{shoppingListId:int}")]
		public Task<IActionResult> GetShoppingList(int shoppingListId)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(new RetrieveShoppingListQuery(shoppingListId)),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}

		[HttpPost]
		public Task<IActionResult> AddShoppingList([FromBody] AddNewShoppingListCommand command)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(command),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}

		[HttpPost("{shoppingListId:int}/Import")]
		public Task<IActionResult> ImportShoppingListItems(int shoppingListId, [FromBody] ImportShoppingListItemsCommand command)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(command with { ShoppingListId = shoppingListId }),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}

		[HttpPost("{shoppingListId:int}/Add")]
		public Task<IActionResult> AddShoppingListItem(int shoppingListId, [FromBody] AddShoppingListItemCommand command)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(command with { ShoppingListId = shoppingListId }),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}

		[HttpPatch("{shoppingListId:int}/Change/{shoppingListItemId:int}")]
		public Task<IActionResult> ChangeShoppingListItems(int shoppingListId, int shoppingListItemId, [FromBody] ChangeShoppingListItemCommand command)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(command with { ShoppingListId = shoppingListId, Id = shoppingListItemId }),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}

		[HttpDelete("{shoppingListId:int}/Item/{shoppingListItemId:int}")]
		public Task<IActionResult> DeleteShoppingListItem(int shoppingListId, int shoppingListItemId)
		{
			return this.ResolveResult(
				resultTask: mediator.Send(new DeleteShoppingListItemCommand(shoppingListId, shoppingListItemId)),
				onFailure: (data, error) =>
				{
					return error switch
					{
						_ => BadRequest(data)
					};
				});
		}
	}
}
