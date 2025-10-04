namespace SplitMate.Shared.Wrappers
{
	public class FailedValidationResult<T> : IResult<T>, IFailedValidationResult
	{
		public FailedValidationResult(int errorCode, List<string>? messages)
		{
			Messages = messages ?? [];
			ErrorCode = errorCode;
			Message = Messages.FirstOrDefault() ?? string.Empty;
		}

		public IReadOnlyList<string> Messages { get; }
		public string Message { get; }
		public bool IsSuccess => false;
		public int? ErrorCode { get; }
		public T? Data => default;
	}

	public class FailedValidationResult : IResult, IFailedValidationResult
	{
		public FailedValidationResult(int errorCode, List<string>? messages)
		{
			ErrorCode = errorCode;
			Messages = messages ?? [];
			Message = Messages.FirstOrDefault() ?? string.Empty;
		}

		public IReadOnlyList<string> Messages { get; }
		public string Message { get; }
		public bool IsSuccess => false;
		public int? ErrorCode { get; }
	}

	public interface IFailedValidationResult
	{
		bool IsSuccess { get; }
		int? ErrorCode { get; }
		IReadOnlyList<string> Messages { get; }
	}
}
