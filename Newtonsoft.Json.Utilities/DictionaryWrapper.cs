using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	internal class DictionaryWrapper<TKey, TValue> : IEnumerable, IWrappedDictionary, ICollection, IDictionary, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IEnumerator, IDictionaryEnumerator
		{
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			public object Key
			{
				get
				{
					return this.Entry.get_Key();
				}
			}

			public object Value
			{
				get
				{
					return this.Entry.get_Value();
				}
			}

			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> current = this._e.get_Current();
					object arg_30_0 = current.get_Key();
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> current2 = this._e.get_Current();
					return new DictionaryEntry(arg_30_0, current2.get_Value());
				}
			}

			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			public void Reset()
			{
				this._e.Reset();
			}
		}

		private readonly IDictionary _dictionary;

		private readonly IDictionary<TKey, TValue> _genericDictionary;

		private object _syncRoot;

		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.get_IsFixedSize();
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return Enumerable.ToList<TKey>(this._genericDictionary.get_Keys());
				}
				return this._dictionary.get_Keys();
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return Enumerable.ToList<TValue>(this._genericDictionary.get_Values());
				}
				return this._dictionary.get_Values();
			}
		}

		object IDictionary.Item
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.get_Item((TKey)((object)key));
				}
				return this._dictionary.get_Item(key);
			}
			set
			{
				if (this._genericDictionary != null)
				{
					this._genericDictionary.set_Item((TKey)((object)key), (TValue)((object)value));
				}
				else
				{
					this._dictionary.set_Item(key, value);
				}
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.get_IsSynchronized();
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		public ICollection<TKey> Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.get_Keys();
				}
				return Enumerable.ToList<TKey>(Enumerable.Cast<TKey>(this._dictionary.get_Keys()));
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.get_Values();
				}
				return Enumerable.ToList<TValue>(Enumerable.Cast<TValue>(this._dictionary.get_Values()));
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.get_Item(key);
				}
				return (TValue)((object)this._dictionary.get_Item(key));
			}
			set
			{
				if (this._genericDictionary != null)
				{
					this._genericDictionary.set_Item(key, value);
				}
				else
				{
					this._dictionary.set_Item(key, value);
				}
			}
		}

		public int Count
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.get_Count();
				}
				return this._dictionary.get_Count();
			}
		}

		public bool IsReadOnly
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.get_IsReadOnly();
				}
				return this._dictionary.get_IsReadOnly();
			}
		}

		public object UnderlyingDictionary
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary;
				}
				return this._dictionary;
			}
		}

		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void IDictionary.Add(object key, object value)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add((TKey)((object)key), (TValue)((object)value));
			}
			else
			{
				this._dictionary.Add(key, value);
			}
		}

		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._genericDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
			}
			return this._dictionary.GetEnumerator();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
			}
			else
			{
				this._dictionary.CopyTo(array, index);
			}
		}

		public void Add(TKey key, TValue value)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
			}
			else
			{
				this._dictionary.Add(key, value);
			}
		}

		public bool ContainsKey(TKey key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey(key);
			}
			return this._dictionary.Contains(key);
		}

		public bool Remove(TKey key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Remove(key);
			}
			if (this._dictionary.Contains(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.TryGetValue(key, ref value);
			}
			if (!this._dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)((object)this._dictionary.get_Item(key));
			return true;
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(item);
			}
			else
			{
				((IList)this._dictionary).Add(item);
			}
		}

		public void Clear()
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Clear();
			}
			else
			{
				this._dictionary.Clear();
			}
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Contains(item);
			}
			return ((IList)this._dictionary).Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.CopyTo(array, arrayIndex);
			}
			else
			{
				IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.get_Current();
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)dictionaryEntry.get_Key()), (TValue)((object)dictionaryEntry.get_Value()));
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Remove(item);
			}
			if (!this._dictionary.Contains(item.get_Key()))
			{
				return true;
			}
			object obj = this._dictionary.get_Item(item.get_Key());
			if (object.Equals(obj, item.get_Value()))
			{
				this._dictionary.Remove(item.get_Key());
				return true;
			}
			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.GetEnumerator();
			}
			return Enumerable.Select<DictionaryEntry, KeyValuePair<TKey, TValue>>(Enumerable.Cast<DictionaryEntry>(this._dictionary), (DictionaryEntry de) => new KeyValuePair<TKey, TValue>((TKey)((object)de.get_Key()), (TValue)((object)de.get_Value()))).GetEnumerator();
		}

		public void Remove(object key)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Remove((TKey)((object)key));
			}
			else
			{
				this._dictionary.Remove(key);
			}
		}
	}
}
