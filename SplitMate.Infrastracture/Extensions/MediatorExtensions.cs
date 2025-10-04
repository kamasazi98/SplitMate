using MediatR;
using SplitMate.Shared.Wrappers;

namespace SplitMate.Infrastracture.Extensions
{
	public static class MediatorExtensions
	{
		public static IResult<TResposne> Success<TRequest, TResposne>(this IRequestHandler<TRequest, IResult<TResposne>> _, TResposne response)
			where TRequest : IRequest<IResult<TResposne>>
		{
			return Result<TResposne>.Success(response);
		}
		public static IResult Success<TRequest>(this IRequestHandler<TRequest, IResult> _)
			where TRequest : IRequest<IResult>
		{
			return Result.Success();
		}

		public static IResult<TResposne> Fail<TRequest, TResposne>(this IRequestHandler<TRequest, IResult<TResposne>> _, int errorCode)
			where TRequest : IRequest<IResult<TResposne>>
		{
			return Result<TResposne>.Fail(errorCode);
		}
		public static IResult<TResposne> Fail<TRequest, TResposne>(this IRequestHandler<TRequest, IResult<TResposne>> _, int errorCode, string message)
			where TRequest : IRequest<IResult<TResposne>>
		{
			return Result<TResposne>.Fail(message, errorCode);
		}
		public static IResult<TResposne> Fail<TRequest, TResposne>(this IRequestHandler<TRequest, IResult<TResposne>> _, int errorCode, List<string> messages)
			where TRequest : IRequest<IResult<TResposne>>
		{
			return Result<TResposne>.Fail(messages, errorCode);
		}
		public static IResult Fail<TRequest>(this IRequestHandler<TRequest, IResult> _, int errorCode)
			where TRequest : IRequest<IResult>
		{
			return Result.Fail(errorCode);
		}
		public static IResult Fail<TRequest>(this IRequestHandler<TRequest, IResult> _, int errorCode, string message)
			where TRequest : IRequest<IResult>
		{
			return Result.Fail(message, errorCode);
		}

	}
}
