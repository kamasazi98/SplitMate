namespace SplitMate.Client.Extensions
{
	public static class HttpClientFactoryExtensions
	{
		public static HttpClient MainApiClient(this IHttpClientFactory factory) => factory.CreateClient("MainApi");
	}
}
