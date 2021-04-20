using System;
using System.Collections.Generic;

namespace Knight
{
	/// <summary>
	/// An identifier within Knight, used to uniquely identify variables.
	/// </summary>
	/// <remarks>
	/// As per the Knight specs, all variables in Knight are global.
	/// </remarks>
    public class Identifier : NonIdempotent, IValue
    {
		/// <summary>
		/// The list of all identifiers and their associated values.
		/// </summary>
		private static IDictionary<string, IValue> ENV = new Dictionary<string, IValue>();

		/// <summary>
		/// The name of this identifier.
		/// </summary>
		private string _name;

		/// <summary>
		/// Create a new identifier with the given <paramref name="name">.
		/// </summary>
		public Identifier(string name) => _name = name;

		/// <summary>
		/// Attempts to parse an <c>Identifier</c> from the given <paramref name="stream"/>, returnning <see langword="null"/> if nothing could be parsed.
		/// </summary>
		/// <returns>
		/// The parsed <c>Identifier</c>, or <see langword="null"/> if the <paramref name="stream"> didn't start with a lower case letter or <c>_</c>.
		/// </returns>
		internal static Identifier Parse(Stream stream) {
			bool isLower(char c) => char.IsLower(c) || c == '_';

			var contents = stream.TakeWhileIfStartsWith(isLower, c => isLower(c) || char.IsDigit(c));
			
			return contents == null ? null : new Identifier(contents);
		}

		/// <inheritdoc/>
		public override void Dump() => Console.Write($"Identifier({_name})");

		/// <summary>
		/// Fetches the value associated with this identifier.
		/// </summary>
    	/// <exception cref="RuntimeException">Thrown if the identifier hasn't been assigned to yet.</exception>
		public override IValue Run() {
			IValue result;

			// A future improvement could be to cache variables and not names. (cf the C++ impl).
			if (ENV.TryGetValue(_name, out result))
				return result;
			
			throw new RuntimeException($"Unknown identifier '{_name}'.");
		}

		/// <summary>
		/// Associates <paramref name="value"/> with this identifier.
		/// </summary>
		public void Assign(IValue value) => ENV[_name] = value;
    }
}
