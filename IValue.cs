using System;

namespace Knight
{
	/// <summary>
	/// The interface which all values within Knight implement.
	/// </summary> 
	public interface IValue : IEquatable<IValue>
	{
		/// <summary>
		/// Executes this value.
		/// </summary>
		IValue Run();

		/// <summary>
		/// Dumps a debug representation of this class to standard out.
		/// </summary>
		/// <remarks>
		/// A trailing newline should not be printed.
		/// </remarks>
		void Dump();

		/// <summary>
		/// Converts <c>this</c> to its string representation.
		/// </summary>
		/// <remarks>
		/// All conversions within Knight are well-defined and shouldn't throw exceptions.
		/// </summary>
		string ToString();

		/// <summary>
		/// Converts <c>this</c> to its boolean representation.
		/// </summary>
		/// <remarks>
		/// All conversions within Knight are well-defined and shouldn't throw exceptions.
		/// </summary>
		bool ToBool();

		/// <summary>
		/// Converts <c>this</c> to its long representation.
		/// </summary>
		/// <remarks>
		/// All conversions within Knight are well-defined and shouldn't throw exceptions.
		/// </summary>
		long ToLong();

		/// <summary>
		/// Performs the <c>+</c> operation.
		/// </summary>
		IValue Add(IValue rhs);

		/// <summary>
		/// Performs the <c>-</c> operation.
		/// </summary>
		IValue Sub(IValue rhs);

		/// <summary>
		/// Performs the <c>*</c> operation.
		/// </summary>
		IValue Mul(IValue rhs);

		/// <summary>
		/// Performs the <c>/</c> operation.
		/// </summary>
		IValue Div(IValue rhs);

		/// <summary>
		/// Performs the <c>%</c> operation.
		/// </summary>
		IValue Mod(IValue rhs);

		/// <summary>
		/// Performs the <c>^</c> operation.
		/// </summary>
		IValue Pow(IValue rhs);

		// note that equality uses the normal `Equals`.
	}
}
