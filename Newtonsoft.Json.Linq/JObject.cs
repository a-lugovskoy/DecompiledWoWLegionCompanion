using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
	public class JObject : JContainer, IEnumerable, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, ICustomTypeDescriptor, INotifyPropertyChanged, IEnumerable<KeyValuePair<string, JToken>>
	{
		public class JPropertKeyedCollection : KeyedCollection<string, JToken>
		{
			public IDictionary<string, JToken> Dictionary
			{
				get
				{
					return base.get_Dictionary();
				}
			}

			public JPropertKeyedCollection(IEqualityComparer<string> comparer) : base(comparer)
			{
			}

			protected override string GetKeyForItem(JToken item)
			{
				return ((JProperty)item).Name;
			}

			protected override void InsertItem(int index, JToken item)
			{
				if (this.Dictionary == null)
				{
					base.InsertItem(index, item);
				}
				else
				{
					string keyForItem = this.GetKeyForItem(item);
					this.Dictionary.set_Item(keyForItem, item);
					base.get_Items().Insert(index, item);
				}
			}
		}

		private JObject.JPropertKeyedCollection _properties = new JObject.JPropertKeyedCollection(StringComparer.get_Ordinal());

		public event PropertyChangedEventHandler PropertyChanged;

		ICollection<string> IDictionary<string, JToken>.Keys
		{
			get
			{
				return this._properties.Dictionary.get_Keys();
			}
		}

		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			get
			{
				return this._properties.Dictionary.get_Values();
			}
		}

		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
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
				return this._properties;
			}
		}

		public override JTokenType Type
		{
			get
			{
				return JTokenType.Object;
			}
		}

		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this[text];
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this[text] = value;
			}
		}

		public JToken this[string propertyName]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				JProperty jProperty = this.Property(propertyName);
				return (jProperty == null) ? null : jProperty.Value;
			}
			set
			{
				JProperty jProperty = this.Property(propertyName);
				if (jProperty != null)
				{
					jProperty.Value = value;
				}
				else
				{
					this.Add(new JProperty(propertyName, value));
					this.OnPropertyChanged(propertyName);
				}
			}
		}

		public JObject()
		{
		}

		public JObject(JObject other) : base(other)
		{
		}

		public JObject(params object[] content) : this(content)
		{
		}

		public JObject(object content)
		{
			this.Add(content);
		}

		bool IDictionary<string, JToken>.ContainsKey(string key)
		{
			return this._properties.Dictionary != null && this._properties.Dictionary.ContainsKey(key);
		}

		void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
		{
			this.Add(new JProperty(item.get_Key(), item.get_Value()));
		}

		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			base.RemoveAll();
		}

		bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
		{
			JProperty jProperty = this.Property(item.get_Key());
			return jProperty != null && jProperty.Value == item.get_Value();
		}

		void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JProperty jProperty = (JProperty)enumerator.get_Current();
					array[arrayIndex + num] = new KeyValuePair<string, JToken>(jProperty.Name, jProperty.Value);
					num++;
				}
			}
		}

		bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
		{
			if (!this.Contains(item))
			{
				return false;
			}
			this.Remove(item.get_Key());
			return true;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return this.GetProperties(null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
			using (IEnumerator<KeyValuePair<string, JToken>> enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JToken> current = enumerator.get_Current();
					propertyDescriptorCollection.Add(new JPropertyDescriptor(current.get_Key(), JObject.GetTokenPropertyType(current.get_Value())));
				}
			}
			return propertyDescriptorCollection;
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return new TypeConverter();
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return null;
		}

		internal override bool DeepEquals(JToken node)
		{
			JObject jObject = node as JObject;
			return jObject != null && base.ContentsEqual(jObject);
		}

		internal override void InsertItem(int index, JToken item)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			base.InsertItem(index, item);
		}

		internal override void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					o.GetType(),
					base.GetType()
				}));
			}
			JProperty jProperty = (JProperty)o;
			if (existing != null)
			{
				JProperty jProperty2 = (JProperty)existing;
				if (jProperty.Name == jProperty2.Name)
				{
					return;
				}
			}
			if (this._properties.Dictionary != null && this._properties.Dictionary.TryGetValue(jProperty.Name, ref existing))
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					jProperty.Name,
					base.GetType()
				}));
			}
		}

		internal void InternalPropertyChanged(JProperty childProperty)
		{
			this.OnPropertyChanged(childProperty.Name);
		}

		internal void InternalPropertyChanging(JProperty childProperty)
		{
		}

		internal override JToken CloneToken()
		{
			return new JObject(this);
		}

		public IEnumerable<JProperty> Properties()
		{
			return Enumerable.Cast<JProperty>(this.ChildrenTokens);
		}

		public JProperty Property(string name)
		{
			if (this._properties.Dictionary == null)
			{
				return null;
			}
			if (name == null)
			{
				return null;
			}
			JToken jToken;
			this._properties.Dictionary.TryGetValue(name, ref jToken);
			return (JProperty)jToken;
		}

		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(Enumerable.Select<JProperty, JToken>(this.Properties(), (JProperty p) => p.Value));
		}

		public static JObject Load(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JObject from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					reader.TokenType
				}));
			}
			JObject jObject = new JObject();
			jObject.SetLineInfo(reader as IJsonLineInfo);
			jObject.ReadTokenFrom(reader);
			return jObject;
		}

		public static JObject Parse(string json)
		{
			JsonReader reader = new JsonTextReader(new StringReader(json));
			return JObject.Load(reader);
		}

		public static JObject FromObject(object o)
		{
			return JObject.FromObject(o, new JsonSerializer());
		}

		public static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jToken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jToken != null && jToken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					jToken.Type
				}));
			}
			return (JObject)jToken;
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JProperty jProperty = (JProperty)enumerator.get_Current();
					jProperty.WriteTo(writer, converters);
				}
			}
			writer.WriteEndObject();
		}

		public void Add(string propertyName, JToken value)
		{
			this.Add(new JProperty(propertyName, value));
		}

		public bool Remove(string propertyName)
		{
			JProperty jProperty = this.Property(propertyName);
			if (jProperty == null)
			{
				return false;
			}
			jProperty.Remove();
			return true;
		}

		public bool TryGetValue(string propertyName, out JToken value)
		{
			JProperty jProperty = this.Property(propertyName);
			if (jProperty == null)
			{
				value = null;
				return false;
			}
			value = jProperty.Value;
			return true;
		}

		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		[DebuggerHidden]
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator();
			while (enumerator.MoveNext())
			{
				JProperty jProperty = (JProperty)enumerator.get_Current();
				yield return new KeyValuePair<string, JToken>(jProperty.Name, jProperty.Value);
			}
			yield break;
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private static Type GetTokenPropertyType(JToken token)
		{
			if (token is JValue)
			{
				JValue jValue = (JValue)token;
				return (jValue.Value == null) ? typeof(object) : jValue.Value.GetType();
			}
			return token.GetType();
		}
	}
}
