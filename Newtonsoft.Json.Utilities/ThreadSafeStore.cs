using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	internal class ThreadSafeStore<TKey, TValue>
	{
		private readonly object _lock = new object();

		private Dictionary<TKey, TValue> _store;

		private readonly Func<TKey, TValue> _creator;

		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			this._creator = creator;
		}

		public TValue Get(TKey key)
		{
			if (this._store == null)
			{
				return this.AddValue(key);
			}
			TValue result;
			if (!this._store.TryGetValue(key, ref result))
			{
				return this.AddValue(key);
			}
			return result;
		}

		private TValue AddValue(TKey key)
		{
			TValue tValue = this._creator.Invoke(key);
			object @lock = this._lock;
			TValue result;
			lock (@lock)
			{
				if (this._store == null)
				{
					this._store = new Dictionary<TKey, TValue>();
					this._store.set_Item(key, tValue);
				}
				else
				{
					TValue tValue2;
					if (this._store.TryGetValue(key, ref tValue2))
					{
						result = tValue2;
						return result;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._store);
					dictionary.set_Item(key, tValue);
					this._store = dictionary;
				}
				result = tValue;
			}
			return result;
		}
	}
}
