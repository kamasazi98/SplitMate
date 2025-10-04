using SplitMate.Client.Managers;

namespace SplitMate.Client
{
	public static class Bootstrapper
	{
		public static IServiceCollection AddWebClient(this IServiceCollection services)
		{
			ArgumentNullException.ThrowIfNull(services);
			services.AddManagers();

			return services;
		}
		private static IServiceCollection AddManagers(this IServiceCollection services)
		{
			var managers = typeof(IManager);

			var types = managers
				.Assembly
				.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract)
				.Select(t => new
				{
					Service = t.GetInterface($"I{t.Name}"),
					Implementation = t
				})
				.Where(t => t.Service != null);

			foreach (var type in types)
			{
				if (managers.IsAssignableFrom(type.Service))
				{
					services.AddTransient(type.Service, type.Implementation);
				}
			}

			return services;
		}
	}
}
