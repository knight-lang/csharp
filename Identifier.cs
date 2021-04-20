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
    public class Variable : NonIdempotent, IValue
    {
		private static Dictionary<string, Variable> ENV = new Dictionary<string, Variable>();

		internal static Variable Parse(Stream stream) {
			bool isLower(char c) => char.IsLower(c) || c == '_';

			var name = stream.TakeWhileIfStartsWith(isLower, c => isLower(c) || char.IsDigit(c));
			
			if (name == null)
				return null;
			
			Variable variable;

			if (!ENV.TryGetValue(name, out variable)) {
				variable = new Variable(name);
				ENV.Add(name, variable);
			}
			
			return variable;
		}

		private string _name;
		private IValue _value;

		/// <summary>
		/// Create a new identifier with the given <paramref name="name">.
		/// </summary>
		public Variable(string name) => _name = name;

		/// <inheritdoc/>
		public override void Dump() => Console.Write($"Variable({_name})");

		/// <summary>
		/// Fetches the value associated with this identifier.
		/// </summary>
    	/// <exception cref="RuntimeException">Thrown if the identifier hasn't been assigned to yet.</exception>
		public override IValue Run() {
			if (_value == null)
				throw new RuntimeException($"Unknown identifier '{_name}'.");

			return _value;
		}

		/// <summary>
		/// Associates <paramref name="value"/> with this identifier.
		/// </summary>
		public void Assign(IValue value) => _value = value;
    }
}
