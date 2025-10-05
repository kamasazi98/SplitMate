namespace SplitMate.Domain
{
	public class ProblemException : Exception
	{
		public ProblemException(int errorCode)
			=> ErrorCode = errorCode;

		public ProblemException(int errorCode, string errorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public int ErrorCode { get; }
		public string? ErrorMessage { get; }
	}
}
