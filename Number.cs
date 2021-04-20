using System;

namespace Knight
{
	/// <summary>
	/// The number class within Knight
	/// </summmary>
	/// <remarks>
	/// Note that Knight only uses integers. As such, the <c>Number</c> class is simply a wrapper around <c>long</c>
	/// </remarks>
	public class Number : Literal<long>, IComparable<IValue>
	{
		/// <inheritdoc/>
		public Number(long data) : base(data) {}

		/// <summary>
		/// Attempts to parse a <c>Number</c> from the given <paramref name="stream"/>, returnning <see langword="null"/> if nothing could be parsed.
		/// </summary>
		/// <param name="stream">
		/// The stream from which to parse.
		/// </param>
		/// <returns>
		/// The parsed <c>Number</c>, or <see langword="null"/> if the <paramref name="stream"> didn't start with a digit (ie <c>0</c> through <c>9</c>).
		/// </returns>
		internal static Number Parse(Stream stream) {
			var contents = stream.TakeWhileIfStartsWith(char.IsDigit);
			
			return contents == null ? null : new Number(long.Parse(contents));
		}

		/// <inheritdoc />
		public override void Dump() => Console.Write($"Number({this})");

		/// <summary>
		/// Returns whether <c>this</c> is nonzero.
		/// </summary>
		public override bool ToBool() => _data != 0;

		/// <summary>
		/// Simply returns the data associated with <c>this</c>.
		/// </summary>
		public override long ToLong() => _data;

		/// <summary>
		/// Compares the data associated with <c>this</c> wiht the <c>long</c> representation of <paramref name="other"/>.
		/// </sumary>
		public int CompareTo(IValue other) => _data.CompareTo(other.ToLong());		

		/// <summary>
		/// Returns <c>this</c> added with the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Add(IValue rhs) => new Number(_data + rhs.ToLong());

		/// <summary>
		/// Returns <c>this</c> subtracted by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Sub(IValue rhs) => new Number(_data - rhs.ToLong());

		/// <summary>
		/// Returns <c>this</c> multiplied by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Mul(IValue rhs) => new Number(_data * rhs.ToLong());

		/// <summary>
		/// Returns <c>this</c> divided by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		/// <exception cref="RuntimeException">Thrown if <paramref name="rhs"/> is zero. </exception>
		public override IValue Div(IValue rhs) {
			var rlong = rhs.ToLong();
			
			if (rlong == 0)
				throw new RuntimeException("Cannot divide by zero!");

			return new Number(_data / rlong);
		}
		
		/// <summary>
		/// Returns <c>this</c> modulod by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		/// <exception cref="RuntimeException">Thrown if <paramref name="rhs"/> is zero. </exception>
		public override IValue Mod(IValue rhs) {
			var rlong = rhs.ToLong();
			
			if (rlong == 0)
				throw new RuntimeException("Cannot modulo by zero!");

			return new Number(_data % rlong);
		}
		
		/// <summary>
		/// Returns <c>this</c> exponentiated by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Pow(IValue rhs) => new Number((long) Math.Pow(_data, rhs.ToLong()));
	}
}
