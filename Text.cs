using System;

namespace Knight
{
	/// <summary>
	/// The string (ie "text") class within Knight.
	/// </summmary>
	/// <remarks>
	/// While we could have named this class <c>String</c> to keep in line with the Knight specs, it would have clashed with 
	/// C#'s <c>System.String</c>. As such, we go with this replacement instead.
	/// </remarks>
	public class Text : Literal<string>, IComparable<IValue>
	{
		internal static Text Parse(Stream stream) {
			if (!stream.StartsWith('\'', '\"'))
				return null;
			
			char quote = stream.Take();
			var start = stream.Source;

			string data = stream.TakeWhile(c => c != quote);

			if (stream.IsEmpty())
				throw new RuntimeException($"unterminated string, starting at {start}");

			stream.Take(); // remove trailling quote.

			return new Text(data ?? "");
		}

		/// <inheritdoc/>
		public Text(string data) : base(data) {}

		/// <inheritdoc/>
		public override void Dump() => Console.Write($"String({_data})");

		/// <summary>
		/// Returns whether <c>this</c> is empty.
		/// </summary>
		public override bool ToBool() => _data != "";

		/// <summary>
		/// Converts <c>this</c> to a <c>long</c>, according to the Knight specs.
		/// </summary>
		/// <remarks>
		/// Roughly, you strip all leading whitespace, and then read an optional <c>+</c> or <c>-</c>, and then take as many
		/// digit chars as possible, returning <c>0</c> if there are no valid leading digits.
		/// </summary>
		public override long ToLong() {
			long ret = 0;
			var str = _data.TrimStart();

			if (str == "")
				return 0;

			var isNegative = str[0] == '-';

			if (str[0] == '-' || str[0] == '+')
				str = str.Substring(1);

			for (; str != "" && char.IsDigit(str[0]); str = str.Substring(1))
				ret = ret * 10 + (str[0] - '0');

			if (isNegative)
				ret *= -1;

			return ret;
		}

		/// <summary>
		/// Returns <c>this</c> concatenated with the <c>string</c> representation of <paramref name="rhs"/>.
		/// </summary>
		public override IValue Add(IValue rhs) => new Text(_data + rhs);

		/// <summary>
		/// Returns <c>this</c> replicated <paramref name="rhs"/> times (When converted to a <c>long</c>).
		/// </summary>
		/// <exception cref="RuntimeException">Thrown if <paramref name="rhs"/> is negative. </exception>
		public override IValue Mul(IValue rhs) {
			var amnt = rhs.ToLong();

			if (amnt < 0)
				throw new RuntimeException($"Cannot repeat by a negative amount '{amnt}'.");

			var s = "";

			for (long i = 0; i < amnt; ++i)
				s += _data;

			return new Text(s);
		}

		/// <summary>
		/// Compares <c>this</c> with <paramref name="obj"/> according to the Knight specs. (ie ASCII string comparison.)
		/// </summary>
		public int CompareTo(IValue obj) => string.Compare(_data, obj.ToString(), StringComparison.Ordinal);
	}
}
