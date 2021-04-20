namespace Knight
{
	/// <summary>
	/// A class that represents types that may return a different result each time they are <c>Run</c>.
	/// </summary>
	public abstract class NonIdempotent : IValue
	{
		/// <inheritdoc/>
		public abstract IValue Run();

		/// <inheritdoc/>
		public abstract void Dump();

		/// <summary>
		/// Converts the result of <c>Run</c>ning <c>this</c> class to a string.
		/// </summary>
		public override string ToString() => Run().ToString();

		/// <summary>
		/// Converts the result of <c>Run</c>ning <c>this</c> class to a long.
		/// </summary>
		public long ToLong() => Run().ToLong();

		/// <summary>
		/// Converts the result of <c>Run</c>ning <c>this</c> class to a bool.
		/// </summary>
		public bool ToBool() => Run().ToBool();

		/// <summary>
		/// Checks to see if the result of <c>Run</c>ning <c>this</c> is equal to the result of <c>Run</c>ing <paramref name="obj"/>.
		/// </summary>
		public bool Equals(IValue obj) => false; 

		/// <summary>
		/// Takes the result of <c>Run</c>ning <c>this</c> and adds to it the result of <c>Run</c>ing <paramref name="rhs"/>.
		/// </summary>
		public IValue Add(IValue rhs) => Run().Add(rhs.Run());

		/// <summary>
		/// Takes the result of <c>Run</c>ning <c>this</c> and subtracts from it the result of <c>Run</c>ing <paramref name="rhs"/>.
		/// </summary>
		public IValue Sub(IValue rhs) => Run().Sub(rhs.Run());

		/// <summary>
		/// Takes the result of <c>Run</c>ning <c>this</c> and multiplies it by the result of <c>Run</c>ing <paramref name="rhs"/>.
		/// </summary>
		public IValue Mul(IValue rhs) => Run().Mul(rhs.Run());

		/// <summary>
		/// Takes the result of <c>Run</c>ning <c>this</c> and divides it by the result of <c>Run</c>ing <paramref name="rhs"/>.
		/// </summary>
		public IValue Div(IValue rhs) => Run().Div(rhs.Run());

		/// <summary>
		/// Takes the result of <c>Run</c>ning <c>this</c> and modulos it by the result of <c>Run</c>ing <paramref name="rhs"/>.
		/// </summary>
		public IValue Mod(IValue rhs) => Run().Mod(rhs.Run());

		/// <summary>
		/// Takes the result of <c>Run</c>ning <c>this</c> and exponentiates it by the result of <c>Run</c>ing <paramref name="rhs"/>.
		/// </summary>
		public IValue Pow(IValue rhs) => Run().Pow(rhs.Run());
	}
}
