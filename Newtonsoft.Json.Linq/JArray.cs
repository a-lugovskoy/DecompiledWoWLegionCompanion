using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
	public class JArray : JContainer, IEnumerable, IEnumerable<JToken>, ICollection<JToken>, IList<JToken>
	{
		private IList<JToken> _values = new List<JToken>();

		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this.SetItem((int)key, value);
			}
		}

		public JToken this[int index]
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

		public JArray()
		{
		}

		public JArray(JArray other) : base(other)
		{
		}

		public JArray(params object[] content) : this(content)
		{
		}

		public JArray(object content)
		{
			this.Add(content);
		}

		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		internal override bool DeepEquals(JToken node)
		{
			JArray jArray = node as JArray;
			return jArray != null && base.ContentsEqual(jArray);
		}

		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		public static JArray Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JArray from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					reader.TokenType
				}));
			}
			JArray jArray = new JArray();
			jArray.SetLineInfo(reader as IJsonLineInfo);
			jArray.ReadTokenFrom(reader);
			return jArray;
		}

		public static JArray Parse(string json)
		{
			JsonReader reader = new JsonTextReader(new StringReader(json));
			return JArray.Load(reader);
		}

		public static JArray FromObject(object o)
		{
			return JArray.FromObject(o, new JsonSerializer());
		}

		public static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jToken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jToken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					jToken.Type
				}));
			}
			return (JArray)jToken;
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JToken current = enumerator.get_Current();
					current.WriteTo(writer, converters);
				}
			}
			writer.WriteEndArray();
		}

		public int IndexOf(JToken item)
		{
			return base.IndexOfItem(item);
		}

		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item);
		}

		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		public void Add(JToken item)
		{
			this.Add(item);
		}

		public void Clear()
		{
			this.ClearItems();
		}

		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}
	}
}
