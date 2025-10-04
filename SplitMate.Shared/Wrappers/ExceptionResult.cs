namespace SplitMate.Shared.Wrappers
{
	public class ExceptionResult<T> : IResult<T>, IExceptionResult
	{
		public ExceptionResult(int errorCode, List<string>? messages)
		{
			ErrorCode = errorCode;
			Messages = messages ?? [];
			Message = Messages.FirstOrDefault() ?? string.Empty;
		}

		public IReadOnlyList<string> Messages { get; }
		public string Message { get; }
		public bool IsSuccess => false;
		public int? ErrorCode { get; }
		public T? Data => default;
	}

	public class ExceptionResult : IResult, IExceptionResult
	{
		public ExceptionResult(int errorCode, List<string>? messages)
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

	public interface IExceptionResult
	{
		bool IsSuccess { get; }
		int? ErrorCode { get; }
		IReadOnlyList<string> Messages { get; }
	}
}
