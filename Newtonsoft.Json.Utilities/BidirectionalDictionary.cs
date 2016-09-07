using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		public BidirectionalDictionary() : this(EqualityComparer<TFirst>.get_Default(), EqualityComparer<TSecond>.get_Default())
		{
		}

		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
		}

		public void Add(TFirst first, TSecond second)
		{
			if (this._firstToSecond.ContainsKey(first) || this._secondToFirst.ContainsKey(second))
			{
				throw new ArgumentException("Duplicate first or second");
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, ref second);
		}

		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, ref first);
		}
	}
}
