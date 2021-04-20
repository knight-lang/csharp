using System;

namespace Knight
{
	/// <summary>
	/// A wrapper class around <c>bool</c> that implements <c>IValue</c>.
	/// </summary>
	public class Boolean : Literal<bool>, IComparable<IValue>
	{
		/// <inheritdoc/>
		public Boolean(bool data) : base(data) {}

		/// <summary>
		/// Attempts to parse a <c>Boolean</c> from the given <paramref name="stream"/>, returnning <see langword="null"/> if nothing could be parsed.
		/// </summary>
		/// <returns>
		/// The parsed <c>Boolean</c>, or <see langword="null"/> if the <paramref name="stream"> didn't start with <c>T</c> or <c>F</c>.
		/// </returns>
		public static Boolean Parse(Stream stream) {
			if (!stream.StartsWith('T', 'F'))
				return null;

			var ret = new Boolean(stream.Take() == 'T');
			stream.StripKeyword();

			return ret;
		}
		
		/// <inheritdoc/>
		public override void Dump() => Console.Write($"Boolean({this})");

		/// <summary>
		/// Returns <c>"true"</c> or <c>"false"</c>.
		/// </summary>
		public override string ToString() => _data ? "true" : "false";
		
		/// <summary>
		/// Returns whether this class is true.
		/// </summary>
		public override bool ToBool() => _data;

		/// <summary>
		/// Returns <c>1</c> or <c>0</c>.
		/// </summary>
		public override long ToLong() => _data ? 1 : 0;

		/// <summary>
		/// Compares <c>this</c> to <paramref name="obj"/>.
		/// </summary>
		/// <returns>
		/// Returns <c>-1</c> if <c>this</c> is false and <paramref name="obj"/> is truthy, <c>1</c> if <c>this</c> is 
		/// true and <paramref name="obj"/> is falsey, and <c>0</c> otherwise.
		/// </returns>
		public int CompareTo(IValue obj) => _data.CompareTo(obj.ToBool());
	}
}
