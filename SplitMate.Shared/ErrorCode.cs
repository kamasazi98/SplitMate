namespace SplitMate.Shared
{
	public static class ErrorCode
	{
		public const int VALIDATION_FAIL = 400;
		public const int UNAUTHORIZED = 401;
		public const int NOT_FOUND = 404;
		public const int CONFLICT = 409;
		public const int INTERNAL_ERROR = 500;

		public const int SHOPPING_LIST_ITEM_CANNOT_PROCESS_ENTITY = 501_422;
		public const int SHOPPING_LIST_ITEM_NOT_FOUND = 502_404;
	}
}
