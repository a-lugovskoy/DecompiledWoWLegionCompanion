using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	internal class ListWrapper<T> : CollectionWrapper<T>, IEnumerable, IList, ICollection, IWrappedList, IList<T>, ICollection<T>, IEnumerable<T>
	{
		private readonly IList<T> _genericList;

		public T this[int index]
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.get_Item(index);
				}
				return (T)((object)this.get_Item(index));
			}
			set
			{
				if (this._genericList != null)
				{
					this._genericList.set_Item(index, value);
				}
				else
				{
					this.set_Item(index, value);
				}
			}
		}

		public override int Count
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.get_Count();
				}
				return base.Count;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.get_IsReadOnly();
				}
				return base.IsReadOnly;
			}
		}

		public object UnderlyingList
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList;
				}
				return this.UnderlyingCollection;
			}
		}

		public ListWrapper(IList list) : base(list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is IList<T>)
			{
				this._genericList = (IList<T>)list;
			}
		}

		public ListWrapper(IList<T> list) : base(list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericList = list;
		}

		public int IndexOf(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.IndexOf(item);
			}
			return this.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			if (this._genericList != null)
			{
				this._genericList.Insert(index, item);
			}
			else
			{
				this.Insert(index, item);
			}
		}

		public void RemoveAt(int index)
		{
			if (this._genericList != null)
			{
				this._genericList.RemoveAt(index);
			}
			else
			{
				this.RemoveAt(index);
			}
		}

		public override void Add(T item)
		{
			if (this._genericList != null)
			{
				this._genericList.Add(item);
			}
			else
			{
				base.Add(item);
			}
		}

		public override void Clear()
		{
			if (this._genericList != null)
			{
				this._genericList.Clear();
			}
			else
			{
				base.Clear();
			}
		}

		public override bool Contains(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.Contains(item);
			}
			return base.Contains(item);
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericList != null)
			{
				this._genericList.CopyTo(array, arrayIndex);
			}
			else
			{
				base.CopyTo(array, arrayIndex);
			}
		}

		public override bool Remove(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.Remove(item);
			}
			bool flag = base.Contains(item);
			if (flag)
			{
				base.Remove(item);
			}
			return flag;
		}

		public override IEnumerator<T> GetEnumerator()
		{
			if (this._genericList != null)
			{
				return this._genericList.GetEnumerator();
			}
			return base.GetEnumerator();
		}
	}
}
