using System;

namespace Knight
{
	public struct _list : IEquatable<_list>
    {
        internal IValue[] _eles;

        internal _list(IValue[] eles) => _eles = eles;

		public bool Equals(_list other)
        {
            if (_eles.Length != other._eles.Length)
            {
                return false;
            }

            for (int i = 0; i < _eles.Length; ++i)
            {
                if (!_eles[i].Equals(other._eles[i]))
                {
                    return false;
                }
            }

            return true;
        }
	}

	public class List : Literal<_list>, IComparable<IValue>
    {
        private IValue[] _eles
        {
            get => _data._eles;
        }

		internal static List? Parse(Stream stream)
        {
            return stream.TakeIfStartsWith('@') == null ? null : new List();
        }

		public List(params IValue[] eles) : base(new _list(eles)) {}

        public override bool ToBool() => _eles.Length != 0;
        public override long ToLong() => _eles.Length;

		public override IValue[] ToList() => _eles;

        public override void Dump()
        {
            Console.Write('[');

            bool first = true;
            foreach (var item in _eles)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.Write(", ");
                }
                item.Dump();
            }

            Console.Write(']');
        }

		public int CompareTo(IValue? other)
        {
            // TODO
            return 1;
        }

        public override IValue Add(IValue rhs)
        {
            var other = rhs.ToList();
            var combined = new IValue[_eles.Length + other.Length];

            Array.Copy(_eles, combined, _eles.Length);
            Array.Copy(other, 0, combined, _eles.Length, other.Length);

            return new List(combined);
        }

        public override IValue Mul(IValue rhs)
        {
            var amount = rhs.ToLong();
            var result = new IValue[_eles.Length * amount];

            // This technically goes backwards, but it still works
            while (amount-- != 0)
            {
                Array.Copy(_eles, 0, result, amount * _eles.Length, _eles.Length);
            }

            return new List(result);
        }
    }
}
