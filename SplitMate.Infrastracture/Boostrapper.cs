using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SplitMate.Infrastracture
{
	public static class Boostrapper
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			ArgumentNullException.ThrowIfNull(services);
			services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			return services;
		}
	}
}
