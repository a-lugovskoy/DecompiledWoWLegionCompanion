using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Linq
{
	public abstract class JContainer : JToken, IEnumerable, IEnumerable<JToken>, ICollection<JToken>, IList<JToken>, IList, ICollection
	{
		private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
		{
			public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();

			public bool Equals(JToken x, JToken y)
			{
				return object.ReferenceEquals(x, y);
			}

			public int GetHashCode(JToken obj)
			{
				if (obj == null)
				{
					return 0;
				}
				return obj.GetHashCode();
			}
		}

		private object _syncRoot;

		private bool _busy;

		JToken IList<JToken>.Item
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		object IList.Item
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, this.EnsureValue(value));
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
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

		protected abstract IList<JToken> ChildrenTokens
		{
			get;
		}

		public override bool HasValues
		{
			get
			{
				return this.ChildrenTokens.get_Count() > 0;
			}
		}

		public override JToken First
		{
			get
			{
				return Enumerable.FirstOrDefault<JToken>(this.ChildrenTokens);
			}
		}

		public override JToken Last
		{
			get
			{
				return Enumerable.LastOrDefault<JToken>(this.ChildrenTokens);
			}
		}

		public int Count
		{
			get
			{
				return this.ChildrenTokens.get_Count();
			}
		}

		internal JContainer()
		{
		}

		internal JContainer(JContainer other)
		{
			ValidationUtils.ArgumentNotNull(other, "c");
			using (IEnumerator<JToken> enumerator = other.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JToken current = enumerator.get_Current();
					this.Add(current);
				}
			}
		}

		int IList<JToken>.IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		void IList<JToken>.Insert(int index, JToken item)
		{
			this.InsertItem(index, item);
		}

		void IList<JToken>.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		void ICollection<JToken>.Add(JToken item)
		{
			this.Add(item);
		}

		void ICollection<JToken>.Clear()
		{
			this.ClearItems();
		}

		bool ICollection<JToken>.Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		bool ICollection<JToken>.Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		int IList.Add(object value)
		{
			this.Add(this.EnsureValue(value));
			return this.Count - 1;
		}

		void IList.Clear()
		{
			this.ClearItems();
		}

		bool IList.Contains(object value)
		{
			return this.ContainsItem(this.EnsureValue(value));
		}

		int IList.IndexOf(object value)
		{
			return this.IndexOfItem(this.EnsureValue(value));
		}

		void IList.Insert(int index, object value)
		{
			this.InsertItem(index, this.EnsureValue(value));
		}

		void IList.Remove(object value)
		{
			this.RemoveItem(this.EnsureValue(value));
		}

		void IList.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyItemsTo(array, index);
		}

		internal void CheckReentrancy()
		{
			if (this._busy)
			{
				throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					base.GetType()
				}));
			}
		}

		internal bool ContentsEqual(JContainer container)
		{
			JToken jToken = this.First;
			JToken jToken2 = container.First;
			if (jToken == jToken2)
			{
				return true;
			}
			while (jToken != null || jToken2 != null)
			{
				if (jToken == null || jToken2 == null || !jToken.DeepEquals(jToken2))
				{
					return false;
				}
				jToken = ((jToken == this.Last) ? null : jToken.Next);
				jToken2 = ((jToken2 == container.Last) ? null : jToken2.Next);
			}
			return true;
		}

		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenTokens);
		}

		public override IEnumerable<T> Values<T>()
		{
			return this.ChildrenTokens.Convert<JToken, T>();
		}

		[DebuggerHidden]
		public IEnumerable<JToken> Descendants()
		{
			IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator();
			while (enumerator.MoveNext())
			{
				JToken current = enumerator.get_Current();
				yield return current;
			}
			yield break;
		}

		internal bool IsMultiContent(object content)
		{
			return content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);
		}

		internal JToken EnsureParentToken(JToken item)
		{
			if (item == null)
			{
				return new JValue(null);
			}
			if (item.Parent != null)
			{
				item = item.CloneToken();
			}
			else
			{
				JContainer jContainer = this;
				while (jContainer.Parent != null)
				{
					jContainer = jContainer.Parent;
				}
				if (item == jContainer)
				{
					item = item.CloneToken();
				}
			}
			return item;
		}

		internal int IndexOfItem(JToken item)
		{
			return this.ChildrenTokens.IndexOf(item, JContainer.JTokenReferenceEqualityComparer.Instance);
		}

		internal virtual void InsertItem(int index, JToken item)
		{
			if (index > this.ChildrenTokens.get_Count())
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item);
			JToken jToken = (index != 0) ? this.ChildrenTokens.get_Item(index - 1) : null;
			JToken jToken2 = (index != this.ChildrenTokens.get_Count()) ? this.ChildrenTokens.get_Item(index) : null;
			this.ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jToken;
			if (jToken != null)
			{
				jToken.Next = item;
			}
			item.Next = jToken2;
			if (jToken2 != null)
			{
				jToken2.Previous = item;
			}
			this.ChildrenTokens.Insert(index, item);
		}

		internal virtual void RemoveItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= this.ChildrenTokens.get_Count())
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			this.CheckReentrancy();
			JToken jToken = this.ChildrenTokens.get_Item(index);
			JToken jToken2 = (index != 0) ? this.ChildrenTokens.get_Item(index - 1) : null;
			JToken jToken3 = (index != this.ChildrenTokens.get_Count() - 1) ? this.ChildrenTokens.get_Item(index + 1) : null;
			if (jToken2 != null)
			{
				jToken2.Next = jToken3;
			}
			if (jToken3 != null)
			{
				jToken3.Previous = jToken2;
			}
			jToken.Parent = null;
			jToken.Previous = null;
			jToken.Next = null;
			this.ChildrenTokens.RemoveAt(index);
		}

		internal virtual bool RemoveItem(JToken item)
		{
			int num = this.IndexOfItem(item);
			if (num >= 0)
			{
				this.RemoveItemAt(num);
				return true;
			}
			return false;
		}

		internal virtual JToken GetItem(int index)
		{
			return this.ChildrenTokens.get_Item(index);
		}

		internal virtual void SetItem(int index, JToken item)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= this.ChildrenTokens.get_Count())
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			JToken jToken = this.ChildrenTokens.get_Item(index);
			if (JContainer.IsTokenUnchanged(jToken, item))
			{
				return;
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item);
			this.ValidateToken(item, jToken);
			JToken jToken2 = (index != 0) ? this.ChildrenTokens.get_Item(index - 1) : null;
			JToken jToken3 = (index != this.ChildrenTokens.get_Count() - 1) ? this.ChildrenTokens.get_Item(index + 1) : null;
			item.Parent = this;
			item.Previous = jToken2;
			if (jToken2 != null)
			{
				jToken2.Next = item;
			}
			item.Next = jToken3;
			if (jToken3 != null)
			{
				jToken3.Previous = item;
			}
			this.ChildrenTokens.set_Item(index, item);
			jToken.Parent = null;
			jToken.Previous = null;
			jToken.Next = null;
		}

		internal virtual void ClearItems()
		{
			this.CheckReentrancy();
			using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JToken current = enumerator.get_Current();
					current.Parent = null;
					current.Previous = null;
					current.Next = null;
				}
			}
			this.ChildrenTokens.Clear();
		}

		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing == null || existing.Parent != this)
			{
				return;
			}
			int index = this.IndexOfItem(existing);
			this.SetItem(index, replacement);
		}

		internal virtual bool ContainsItem(JToken item)
		{
			return this.IndexOfItem(item) != -1;
		}

		internal virtual void CopyItemsTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.get_Length())
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (this.Count > array.get_Length() - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JToken current = enumerator.get_Current();
					array.SetValue(current, arrayIndex + num);
					num++;
				}
			}
		}

		internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
		{
			JValue jValue = currentValue as JValue;
			return jValue != null && ((jValue.Type == JTokenType.Null && newValue == null) || jValue.Equals(newValue));
		}

		internal virtual void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					o.GetType(),
					base.GetType()
				}));
			}
		}

		public virtual void Add(object content)
		{
			this.AddInternal(this.ChildrenTokens.get_Count(), content);
		}

		public void AddFirst(object content)
		{
			this.AddInternal(0, content);
		}

		internal void AddInternal(int index, object content)
		{
			if (this.IsMultiContent(content))
			{
				IEnumerable enumerable = (IEnumerable)content;
				int num = index;
				IEnumerator enumerator = enumerable.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.get_Current();
						this.AddInternal(num, current);
						num++;
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
			else
			{
				JToken item = this.CreateFromContent(content);
				this.InsertItem(index, item);
			}
		}

		internal JToken CreateFromContent(object content)
		{
			if (content is JToken)
			{
				return (JToken)content;
			}
			return new JValue(content);
		}

		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		public void ReplaceAll(object content)
		{
			this.ClearItems();
			this.Add(content);
		}

		public void RemoveAll()
		{
			this.ClearItems();
		}

		internal void ReadTokenFrom(JsonReader r)
		{
			int depth = r.Depth;
			if (!r.Read())
			{
				throw new Exception("Error reading {0} from JsonReader.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					base.GetType().get_Name()
				}));
			}
			this.ReadContentFrom(r);
			int depth2 = r.Depth;
			if (depth2 > depth)
			{
				throw new Exception("Unexpected end of content while loading {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					base.GetType().get_Name()
				}));
			}
		}

		internal void ReadContentFrom(JsonReader r)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo lineInfo = r as IJsonLineInfo;
			JContainer jContainer = this;
			while (true)
			{
				if (jContainer is JProperty && ((JProperty)jContainer).Value != null)
				{
					if (jContainer == this)
					{
						break;
					}
					jContainer = jContainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.None:
					goto IL_242;
				case JsonToken.StartObject:
				{
					JObject jObject = new JObject();
					jObject.SetLineInfo(lineInfo);
					jContainer.Add(jObject);
					jContainer = jObject;
					goto IL_242;
				}
				case JsonToken.StartArray:
				{
					JArray jArray = new JArray();
					jArray.SetLineInfo(lineInfo);
					jContainer.Add(jArray);
					jContainer = jArray;
					goto IL_242;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jConstructor = new JConstructor(r.Value.ToString());
					jConstructor.SetLineInfo(jConstructor);
					jContainer.Add(jConstructor);
					jContainer = jConstructor;
					goto IL_242;
				}
				case JsonToken.PropertyName:
				{
					string name = r.Value.ToString();
					JProperty jProperty = new JProperty(name);
					jProperty.SetLineInfo(lineInfo);
					JObject jObject2 = (JObject)jContainer;
					JProperty jProperty2 = jObject2.Property(name);
					if (jProperty2 == null)
					{
						jContainer.Add(jProperty);
					}
					else
					{
						jProperty2.Replace(jProperty);
					}
					jContainer = jProperty;
					goto IL_242;
				}
				case JsonToken.Comment:
				{
					JValue jValue = JValue.CreateComment(r.Value.ToString());
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					goto IL_242;
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jValue = new JValue(r.Value);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					goto IL_242;
				}
				case JsonToken.Null:
				{
					JValue jValue = new JValue(null, JTokenType.Null);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					goto IL_242;
				}
				case JsonToken.Undefined:
				{
					JValue jValue = new JValue(null, JTokenType.Undefined);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					goto IL_242;
				}
				case JsonToken.EndObject:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					goto IL_242;
				case JsonToken.EndArray:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					goto IL_242;
				case JsonToken.EndConstructor:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					goto IL_242;
				}
				goto Block_4;
				IL_242:
				if (!r.Read())
				{
					return;
				}
			}
			return;
			Block_4:
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				r.TokenType
			}));
		}

		internal int ContentsHashCode()
		{
			int num = 0;
			using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JToken current = enumerator.get_Current();
					num ^= current.GetDeepHashCode();
				}
			}
			return num;
		}

		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is JToken)
			{
				return (JToken)value;
			}
			throw new ArgumentException("Argument is not a JToken.");
		}
	}
}
