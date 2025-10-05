using Microsoft.Extensions.DependencyInjection;
using SplitMate.Domain.Specifications;
using SplitMate.Infrastracture.Stores;
using System.Reflection;

namespace SplitMate.Infrastracture
{
	public static class Boostrapper
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			ArgumentNullException.ThrowIfNull(services);
			services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			services.AddStores();
			services.AddSpecs();

			return services;
		}
		private static IServiceCollection AddStores(this IServiceCollection services)
		{
			services.AddTransient<IShoppingListStore, ShoppingListStore>();

			return services;
		}
		private static IServiceCollection AddSpecs(this IServiceCollection services)
		{
			services.AddTransient<ShoppingListSpecification>();

			return services;
		}
	}
}
