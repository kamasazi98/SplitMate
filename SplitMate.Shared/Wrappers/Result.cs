using System.Diagnostics.CodeAnalysis;

namespace SplitMate.Shared.Wrappers
{
	public class Result : IResult
	{
		protected Result(bool succeeded, List<string>? messages, int? errorCode)
		{
			Messages = messages ?? [];
			IsSuccess = succeeded;
			ErrorCode = errorCode;
			Message = Messages.FirstOrDefault() ?? string.Empty;
		}

		public IReadOnlyList<string> Messages { get; }
		public string Message { get; }
		public bool IsSuccess { get; }
		public int? ErrorCode { get; }

		public static IResult Fail(int errorCode) => new Result(false, null, errorCode);
		public static IResult Fail(string message, int errorCode) => new Result(false, [message], errorCode);
		public static IResult Fail(List<string> messages, int errorCode) => new Result(false, messages, errorCode);

		public static IResult Success() => new Result(true, null, null);
		public static IResult Success(string message) => new Result(true, [message], null);
	}

	public class Result<T> : Result, IResult<T>
	{
		public T? Data { get; }

		protected Result(bool succeeded, List<string>? messages, int? errorCode, T? data)
			: base(succeeded, messages, errorCode)
		{
			Data = data;
		}

		public new static IResult<T> Fail(int errorCode) => new Result<T>(false, null, errorCode, default);
		public new static IResult<T> Fail(string message, int errorCode) => new Result<T>(false, [message], errorCode, default);
		public new static IResult<T> Fail(List<string> messages, int errorCode) => new Result<T>(false, messages, errorCode, default);

		public new static IResult<T> Success() => new Result<T>(true, null, null, default);
		public new static IResult<T> Success(string message) => new Result<T>(true, [message], null, default);

		public static IResult<T> Success(T data) => new Result<T>(true, null, null, data);
		public static IResult<T> Success(T data, string message) => new Result<T>(true, [message], null, data);
		public static IResult<T> Success(T data, List<string> messages) => new Result<T>(true, messages, null, data);
	}

	public interface IResult
	{
		IReadOnlyList<string> Messages { get; }
		string Message { get; }
		bool IsSuccess { get; }
		int? ErrorCode { get; }
	}

	public interface IResult<out T> : IResult
	{
		[MemberNotNullWhen(true, [nameof(Data)])]
		new bool IsSuccess { get; }
		T? Data { get; }
	}
}
