using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SplitMate.Shared.Extensions
{
	public static class HttpClientExtensions
	{
		private static readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);

		public static async Task<ApiResult> ToApiResult(this Task<HttpResponseMessage> httpResponseMessageTask)
		{
			ArgumentNullException.ThrowIfNull(httpResponseMessageTask);
			var message = await httpResponseMessageTask.ConfigureAwait(false);
			if (message.IsSuccessStatusCode)
				return new ApiResult(message.StatusCode);

			FailedResponse? failedResponse = null;
			var failedResponseRaw = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
			try
			{
				using var stream = await message.Content.ReadAsStreamAsync().ConfigureAwait(false);
				failedResponse = await JsonSerializer.DeserializeAsync<FailedResponse>(stream, jsonSerializerOptions).ConfigureAwait(false);
			}
			catch { }
			if (failedResponse == null)
				failedResponse = new FailedResponse((int)message.StatusCode, []);

			return new ApiResult(message.StatusCode, failedResponseRaw, failedResponse);
		}
		public static async Task<ApiResult<T>> ToApiResult<T>(this Task<HttpResponseMessage> httpResponseMessageTask)
		{
			ArgumentNullException.ThrowIfNull(httpResponseMessageTask);
			var message = await httpResponseMessageTask.ConfigureAwait(false);
			return await ToApiResultAsync<T>(message);
		}
		public static async Task<ApiResult<T>> ToApiResultAsync<T>(this HttpResponseMessage? httpResponseMessage)
		{
			ArgumentNullException.ThrowIfNull(httpResponseMessage);
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				using var stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
				var successResult = JsonSerializer.Deserialize<T>(stream, jsonSerializerOptions);
				return new ApiResult<T>(httpResponseMessage.StatusCode, successResult);
			}

			FailedResponse? failedResponse = null;
			var failedResponseRaw = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
			try
			{
				using var stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
				failedResponse = await JsonSerializer.DeserializeAsync<FailedResponse>(stream, jsonSerializerOptions).ConfigureAwait(false);
			}
			catch { }

			if (failedResponse == null)
				failedResponse = new FailedResponse((int)httpResponseMessage.StatusCode, []);

			return new ApiResult<T>(httpResponseMessage.StatusCode, failedResponseRaw, failedResponse);
		}

		public static Task<HttpResponseMessage> PostFileAsync(this HttpClient client, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, FileContent file, string requestFilesParameterName = "file")
		{
			using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
			var multiForm = new MultipartFormDataContent
			{
				{ new StreamContent(new MemoryStream(file.File)), requestFilesParameterName, file.FileName }
			};
			request.Content = multiForm;
			return client.SendAsync(request);
		}
		public static Task<HttpResponseMessage> PostFilesAsync(this HttpClient client, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, List<FileContent> files, string requestFilesParameterName = "files")
		{
			using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
			var multiForm = new MultipartFormDataContent();
			files.ForEach(x => multiForm.Add(new StreamContent(new MemoryStream(x.File)), requestFilesParameterName, x.FileName));
			request.Content = multiForm;
			return client.SendAsync(request);
		}
		public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T value)
		{
			var jsonContent = JsonSerializer.Serialize(value);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri)
			{
				Content = content
			};
			return httpClient.SendAsync(httpRequestMessage);
		}
	}

	public record FileContent(byte[] File, string FileName);

	public class ApiResult
	{
		public ApiResult(HttpStatusCode statusCode)
		{
			IsSuccess = true;
			StatusCode = statusCode;
		}
		public ApiResult(HttpStatusCode statusCode, string? failedResponseRaw, FailedResponse? failedResponse)
		{
			IsSuccess = false;
			StatusCode = statusCode;
			FailedResponseRaw = failedResponseRaw;
			FailedResponse = failedResponse;
		}

		public HttpStatusCode StatusCode { get; }
		[MemberNotNullWhen(false, [nameof(FailedResponse), nameof(FailedResponseRaw)])]
		public virtual bool IsSuccess { get; }
		public virtual string? FailedResponseRaw { get; }
		public virtual FailedResponse? FailedResponse { get; }
	}

	public class ApiResult<T> : ApiResult
	{
		public ApiResult(HttpStatusCode statusCode, T? response) : base(statusCode)
		{
			Response = response;
		}
		public ApiResult(HttpStatusCode statusCode, string? failedResponseRaw, FailedResponse? failedResponse)
			: base(statusCode, failedResponseRaw, failedResponse)
		{ }

		[MemberNotNullWhen(false, [nameof(FailedResponse), nameof(FailedResponseRaw)])]
		[MemberNotNullWhen(true, nameof(Response))]
		public override bool IsSuccess => base.IsSuccess;
		public override string? FailedResponseRaw => base.FailedResponseRaw;
		public override FailedResponse? FailedResponse => base.FailedResponse;

		public T? Response { get; }
	}

	public record FailedResponse(int? ErrorCode, IReadOnlyList<string>? Messages)
	{
		public IReadOnlyList<string> Messages { get; init; } = Messages ?? [];
		public string? Message => Messages?.FirstOrDefault();
	}
}
