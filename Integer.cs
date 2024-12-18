using System;

namespace Knight
{
	/// <summary>
	/// The number class within Knight
	/// </summary>
	/// <remarks>
	/// Note that Knight only uses integers. As such, the <c>Integer</c> class is simply a wrapper around <c>long</c>
	/// </remarks>
	public class Integer : Literal<long>, IComparable<IValue>
	{
		internal static Integer? Parse(Stream stream)
		{
			var contents = stream.TakeWhileIfStartsWith(char.IsDigit);

			return contents == null ? null : new Integer(long.Parse(contents));
		}

		/// <inheritdoc/>
		public Integer(long data) : base(data) {}

		/// <inheritdoc />
		public override void Dump() => Console.Write(this);

		/// <summary>
		/// Returns whether <c>this</c> is nonzero.
		/// </summary>
		public override bool ToBool() => _data != 0;

		/// <summary>
		/// Simply returns the data associated with <c>this</c>.
		/// </summary>
		public override long ToLong() => _data;

		public override IValue[] ToList()
		{
			if (_data == 0)
			{
				return new IValue[]{ this };
			}

			var digits = new IValue[(int) Math.Log10(Math.Abs(_data)) + 1];

			long num = _data;
			for (int idx = digits.Length; num != 0; num /= 10)
			{
				digits[--idx] = new Integer(num % 10);
			}

			return digits;
		}

		/// <summary>
		/// Compares the data associated with <c>this</c> wiht the <c>long</c> representation of <paramref name="other"/>.
		/// </sumary>
		public int CompareTo(IValue? other) => _data.CompareTo(other?.ToLong());

		/// <summary>
		/// Returns <c>this</c> added with the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Add(IValue rhs) => new Integer(_data + rhs.ToLong());

		/// <summary>
		/// Returns <c>this</c> subtracted by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Sub(IValue rhs) => new Integer(_data - rhs.ToLong());

		/// <summary>
		/// Returns <c>this</c> multiplied by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Mul(IValue rhs) => new Integer(_data * rhs.ToLong());

		/// <summary>
		/// Returns <c>this</c> divided by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		/// <exception cref="RuntimeException">Thrown if <paramref name="rhs"/> is zero. </exception>
		public override IValue Div(IValue rhs)
		{
			var rlong = rhs.ToLong();

			if (rlong == 0)
			{
				throw new RuntimeException("Cannot divide by zero!");
			}

			return new Integer(_data / rlong);
		}

		/// <summary>
		/// Returns <c>this</c> modulod by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		/// <exception cref="RuntimeException">Thrown if <paramref name="rhs"/> is zero. </exception>
		public override IValue Mod(IValue rhs)
		{
			var rlong = rhs.ToLong();

			if (rlong == 0)
			{
				throw new RuntimeException("Cannot modulo by zero!");
			}

			return new Integer(_data % rlong);
		}

		/// <summary>
		/// Returns <c>this</c> exponentiated by the <c>long</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Pow(IValue rhs) => new Integer((long) Math.Pow(_data, rhs.ToLong()));
	}
}
