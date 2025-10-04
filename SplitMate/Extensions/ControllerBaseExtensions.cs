using Microsoft.AspNetCore.Mvc;
using SplitMate.Shared.Extensions;
using SplitMate.Shared.Wrappers;
using IResult = SplitMate.Shared.Wrappers.IResult;


namespace SplitMate.Extensions
{
	internal static class ControllerBaseExtensions
	{
		public static async Task<IActionResult> ResolveResult<T>(this ControllerBase controller, Task<IResult<T>> resultTask, Func<T?, ActionResult>? onSuccess = null, Func<FailedResponse, int?, ActionResult>? onFailure = null)
		{
			ArgumentNullException.ThrowIfNull(controller);

			var result = await resultTask;

			if (result is null)
				return controller.StatusCode(StatusCodes.Status501NotImplemented);
			if (result.IsSuccess)
			{
				if (onSuccess is null)
					return controller.Ok(result.Data);
				return onSuccess.Invoke(result.Data);
			}
			if (result is IFailedValidationResult failedResult)
				return controller.BadRequest(new FailedResponse(failedResult.ErrorCode, failedResult.Messages));
			if (result is IExceptionResult exceptionResult)
				return controller.StatusCode(StatusCodes.Status500InternalServerError, new FailedResponse(exceptionResult.ErrorCode, exceptionResult.Messages));
			if (onFailure != null)
				return onFailure.Invoke(new FailedResponse(result.ErrorCode, result.Messages), result.ErrorCode);

			return controller.BadRequest(new FailedResponse(result.ErrorCode, result.Messages));
		}
		public static async Task<IActionResult> ResolveResult(this ControllerBase controller, Task<IResult> resultTask, Func<ActionResult>? onSuccess = null, Func<FailedResponse, int?, ActionResult>? onFailure = null)
		{
			ArgumentNullException.ThrowIfNull(controller);

			var result = await resultTask;

			if (result is null)
				return controller.StatusCode(StatusCodes.Status501NotImplemented);
			if (result.IsSuccess)
			{
				if (onSuccess is null)
					return controller.NoContent();
				return onSuccess.Invoke();
			}
			if (result is IFailedValidationResult failedResult)
				return controller.BadRequest(new FailedResponse(failedResult.ErrorCode, failedResult.Messages));
			if (result is IExceptionResult exceptionResult)
				return controller.StatusCode(StatusCodes.Status500InternalServerError, new FailedResponse(exceptionResult.ErrorCode, exceptionResult.Messages));
			if (onFailure != null)
				return onFailure.Invoke(new FailedResponse(result.ErrorCode, result.Messages), result.ErrorCode);

			return controller.BadRequest(new FailedResponse(result.ErrorCode, result.Messages));
		}

		public static ActionResult PaymentRequired(this ControllerBase controller, FailedResponse response) => controller.StatusCode(StatusCodes.Status402PaymentRequired, response);
		public static ActionResult Forbidden(this ControllerBase controller, FailedResponse response) => controller.StatusCode(StatusCodes.Status403Forbidden, response);
		public static ActionResult ServiceUnavailable(this ControllerBase controller, FailedResponse response) => controller.StatusCode(StatusCodes.Status503ServiceUnavailable, response);
		public static ActionResult Gone(this ControllerBase controller, FailedResponse response) => controller.StatusCode(StatusCodes.Status410Gone, response);
		public static ActionResult InternalServerError(this ControllerBase controller, FailedResponse response) => controller.StatusCode(StatusCodes.Status500InternalServerError, response);
	}
}
