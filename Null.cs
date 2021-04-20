using System;

namespace Knight
{
	/// <summary>
	/// A helper type that is used as the type parameter for <c>Null</c>.
	/// </summary>
	/// <remarks>
	/// This isn't meant to be used outside of Knight, and is only <c>public</c> because it's needed for <c>Literal</c>'s constructor.
	/// </remarks>
	public struct _unit : IEquatable<_unit> {
		public bool Equals(_unit rhs) => true;
	}

	/// <summary>
	/// The class that represents null values within Knight.
	/// </summary>
	public class Null : Literal<_unit>
	{
		internal static Null Parse(Stream stream) {
			if (!stream.StartsWith('N'))
				return null;

			stream.StripKeyword();

			return new Null();
		}

		/// <summary>
		/// Creates a new <c>Null</c>.
		/// </summary>
		public Null() : base(new _unit()) {}

		/// <inheritdoc/>
		public override void Dump() => Console.Write("Null()");

		/// <summary>
		/// Simply returns <c>"null"</c>.
		/// </summary>
		public override string ToString() => "null";

		/// <summary>
		/// Simply returns <c>false</c>.
		/// </summary>
		public override bool ToBool() => false;

		/// <summary>
		/// Simply returns <c>0</c>.
		/// </summary>
		public override long ToLong() => 0;
	}
}
