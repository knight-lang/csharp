using System;

namespace Knight
{
	/// <summary>
	/// The class that represents values that do not change after being <c>Run</c>.
	/// </summary>
	/// <typeparam name="T">
	/// The type that this class is wrapping. For example, <c>bool</c>, <c>string</c>, etc.
	/// </typeparam>
	public abstract class Literal<T> : IValue
	where T: IEquatable<T>
	{
		/// <summary>
		/// The data associated with this class.
		/// </summary>
		protected readonly T _data;

		/// <summary>
		/// Creates a new <c>Literal</c> with the given <paramref name="data"/>.
		/// </summary>
		public Literal(T data) => _data = data;

		/// <summary>
		/// Snmply returns <c>this</c>.
		/// </summary>
		/// <remarks>
		/// <c>Run</c>ning a <c>Literal</c> always returns <c>this</c> as <c>Literal</c>s, by definition, do not change when ran.
		/// </remarks>
		public IValue Run() => this;

		/// <inheritdoc/>
		public abstract void Dump();

		/// <inheritdoc/>
		public override string ToString() => _data.ToString();

		/// <inheritdoc/>
		public abstract bool ToBool();

		/// <inheritdoc/>
		public abstract long ToLong();

		/// <summary>
		/// Returns whether <c>this</c> is equivalent to <paramref name="obj"/>.
		/// </summary>
		/// <remarks>
		/// Two <c>this</c> is considered equivalent to <paramref name="obj"/> if they are both of the same type and their <c>_data</c>s are equal.
		/// </remarks>
		public bool Equals(IValue obj) => GetType() == obj.GetType() && _data.Equals(((Literal<T>) obj)._data);

		/// <inheritdoc/>
		public virtual IValue Add(IValue rhs) => throw new NotImplementedException($"Add isn't implemented for {GetType()}");

		/// <inheritdoc/>
		public virtual IValue Sub(IValue rhs) => throw new NotImplementedException($"Sub isn't implemented for {GetType()}");

		/// <inheritdoc/>
		public virtual IValue Mul(IValue rhs) => throw new NotImplementedException($"Mul isn't implemented for {GetType()}");

		/// <inheritdoc/>
		public virtual IValue Div(IValue rhs) => throw new NotImplementedException($"Div isn't implemented for {GetType()}");

		/// <inheritdoc/>
		public virtual IValue Mod(IValue rhs) => throw new NotImplementedException($"Mod isn't implemented for {GetType()}");

		/// <inheritdoc/>
		public virtual IValue Pow(IValue rhs) => throw new NotImplementedException($"Pow isn't implemented for {GetType()}");
	}
}
