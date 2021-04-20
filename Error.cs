namespace Knight
{
	/// <summary>
	/// The parent class that all exceptions in Knight stem from.
	/// </summary>
	public abstract class KnightException : System.Exception
	{
		/// <inheritdoc/>
		public KnightException(string message) : base(message) { }

		/// <inheritdoc/>
		public KnightException(string message, System.Exception inner) : base(message, inner) { }
	}

	/// <summary>
	/// An exception occured whilst running Knight code.
	/// </summary>
	public class RuntimeException : KnightException
	{
		/// <inheritdoc/>
		public RuntimeException(string message) : base(message) { }

		/// <inheritdoc/>
		public RuntimeException(string message, System.Exception inner) : base(message, inner) { }
	}

	/// <summary>
	/// An exception occured whilst parsing Knight code.
	/// </summary>
	public class ParseException : KnightException
	{
		/// <inheritdoc/>
		public ParseException(string message) : base(message) { }

		/// <inheritdoc/>
		public ParseException(string message, System.Exception inner) : base(message, inner) { }
	}
}
