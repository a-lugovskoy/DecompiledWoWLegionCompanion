using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	internal class JsonSerializerInternalReader : JsonSerializerInternalBase
	{
		internal enum PropertyPresence
		{
			None = 0,
			Null = 1,
			Value = 2
		}

		private JsonSerializerProxy _internalSerializer;

		private JsonFormatterConverter _formatterConverter;

		public JsonSerializerInternalReader(JsonSerializer serializer) : base(serializer)
		{
		}

		public void Populate(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(target, "target");
			Type type = target.GetType();
			JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(type);
			if (reader.TokenType == JsonToken.None)
			{
				reader.Read();
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				if (!(jsonContract is JsonArrayContract))
				{
					throw new JsonSerializationException("Cannot populate JSON array onto type '{0}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						type
					}));
				}
				this.PopulateList(CollectionUtils.CreateCollectionWrapper(target), reader, null, (JsonArrayContract)jsonContract);
			}
			else
			{
				if (reader.TokenType != JsonToken.StartObject)
				{
					throw new JsonSerializationException("Unexpected initial token '{0}' when populating object. Expected JSON object or array.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						reader.TokenType
					}));
				}
				this.CheckedRead(reader);
				string id = null;
				if (reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", 4))
				{
					this.CheckedRead(reader);
					id = ((reader.Value == null) ? null : reader.Value.ToString());
					this.CheckedRead(reader);
				}
				if (jsonContract is JsonDictionaryContract)
				{
					this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(target), reader, (JsonDictionaryContract)jsonContract, id);
				}
				else
				{
					if (!(jsonContract is JsonObjectContract))
					{
						throw new JsonSerializationException("Cannot populate JSON object onto type '{0}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							type
						}));
					}
					this.PopulateObject(target, reader, (JsonObjectContract)jsonContract, id);
				}
			}
		}

		private JsonContract GetContractSafe(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return base.Serializer.ContractResolver.ResolveContract(type);
		}

		private JsonContract GetContractSafe(Type type, object value)
		{
			if (value == null)
			{
				return this.GetContractSafe(type);
			}
			return base.Serializer.ContractResolver.ResolveContract(value.GetType());
		}

		public object Deserialize(JsonReader reader, Type objectType)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.TokenType == JsonToken.None && !this.ReadForType(reader, objectType, null))
			{
				return null;
			}
			return this.CreateValueNonProperty(reader, objectType, this.GetContractSafe(objectType));
		}

		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		private JsonFormatterConverter GetFormatterConverter()
		{
			if (this._formatterConverter == null)
			{
				this._formatterConverter = new JsonFormatterConverter(this.GetInternalSerializer());
			}
			return this._formatterConverter;
		}

		private JToken CreateJToken(JsonReader reader, JsonContract contract)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (contract != null && contract.UnderlyingType == typeof(JRaw))
			{
				return JRaw.Create(reader);
			}
			JToken token;
			using (JTokenWriter jTokenWriter = new JTokenWriter())
			{
				jTokenWriter.WriteToken(reader);
				token = jTokenWriter.Token;
			}
			return token;
		}

		private JToken CreateJObject(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			JToken token;
			using (JTokenWriter jTokenWriter = new JTokenWriter())
			{
				jTokenWriter.WriteStartObject();
				if (reader.TokenType == JsonToken.PropertyName)
				{
					jTokenWriter.WriteToken(reader, reader.Depth - 1);
				}
				else
				{
					jTokenWriter.WriteEndObject();
				}
				token = jTokenWriter.Token;
			}
			return token;
		}

		private object CreateValueProperty(JsonReader reader, JsonProperty property, object target, bool gottenCurrentValue, object currentValue)
		{
			JsonContract contractSafe = this.GetContractSafe(property.PropertyType, currentValue);
			Type propertyType = property.PropertyType;
			JsonConverter converter = this.GetConverter(contractSafe, property.MemberConverter);
			if (converter != null && converter.CanRead)
			{
				if (!gottenCurrentValue && target != null && property.Readable)
				{
					currentValue = property.ValueProvider.GetValue(target);
				}
				return converter.ReadJson(reader, propertyType, currentValue, this.GetInternalSerializer());
			}
			return this.CreateValueInternal(reader, propertyType, contractSafe, property, currentValue);
		}

		private object CreateValueNonProperty(JsonReader reader, Type objectType, JsonContract contract)
		{
			JsonConverter converter = this.GetConverter(contract, null);
			if (converter != null && converter.CanRead)
			{
				return converter.ReadJson(reader, objectType, null, this.GetInternalSerializer());
			}
			return this.CreateValueInternal(reader, objectType, contract, null, null);
		}

		private object CreateValueInternal(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue)
		{
			if (contract is JsonLinqContract)
			{
				return this.CreateJToken(reader, contract);
			}
			while (true)
			{
				switch (reader.TokenType)
				{
				case JsonToken.StartObject:
					goto IL_6C;
				case JsonToken.StartArray:
					goto IL_7A;
				case JsonToken.StartConstructor:
				case JsonToken.EndConstructor:
					goto IL_F8;
				case JsonToken.Comment:
					if (!reader.Read())
					{
						goto Block_8;
					}
					continue;
				case JsonToken.Raw:
					goto IL_12F;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
					goto IL_89;
				case JsonToken.String:
					goto IL_9C;
				case JsonToken.Null:
				case JsonToken.Undefined:
					goto IL_106;
				}
				break;
			}
			goto IL_145;
			IL_6C:
			return this.CreateObject(reader, objectType, contract, member, existingValue);
			IL_7A:
			return this.CreateList(reader, objectType, contract, member, existingValue, null);
			IL_89:
			return this.EnsureType(reader.Value, CultureInfo.get_InvariantCulture(), objectType);
			IL_9C:
			if (string.IsNullOrEmpty((string)reader.Value) && objectType != null && ReflectionUtils.IsNullableType(objectType))
			{
				return null;
			}
			if (objectType == typeof(byte[]))
			{
				return Convert.FromBase64String((string)reader.Value);
			}
			return this.EnsureType(reader.Value, CultureInfo.get_InvariantCulture(), objectType);
			IL_F8:
			return reader.Value.ToString();
			IL_106:
			if (objectType == typeof(DBNull))
			{
				return DBNull.Value;
			}
			return this.EnsureType(reader.Value, CultureInfo.get_InvariantCulture(), objectType);
			IL_12F:
			return new JRaw((string)reader.Value);
			IL_145:
			throw new JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
			Block_8:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter)
		{
			JsonConverter result = null;
			if (memberConverter != null)
			{
				result = memberConverter;
			}
			else if (contract != null)
			{
				JsonConverter matchingConverter;
				if (contract.Converter != null)
				{
					result = contract.Converter;
				}
				else if ((matchingConverter = base.Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
				{
					result = matchingConverter;
				}
				else if (contract.InternalConverter != null)
				{
					result = contract.InternalConverter;
				}
			}
			return result;
		}

		private object CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue)
		{
			this.CheckedRead(reader);
			string text = null;
			if (reader.TokenType == JsonToken.PropertyName)
			{
				string text3;
				string text4;
				Type type;
				while (true)
				{
					string text2 = reader.Value.ToString();
					bool flag;
					if (string.Equals(text2, "$ref", 4))
					{
						this.CheckedRead(reader);
						if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null)
						{
							break;
						}
						text3 = ((reader.Value == null) ? null : reader.Value.ToString());
						this.CheckedRead(reader);
						if (text3 != null)
						{
							goto Block_5;
						}
						flag = true;
					}
					else if (string.Equals(text2, "$type", 4))
					{
						this.CheckedRead(reader);
						text4 = reader.Value.ToString();
						this.CheckedRead(reader);
						TypeNameHandling? typeNameHandling = (member == null) ? default(TypeNameHandling?) : member.TypeNameHandling;
						if (((!typeNameHandling.get_HasValue()) ? base.Serializer.TypeNameHandling : typeNameHandling.get_Value()) != TypeNameHandling.None)
						{
							string text5;
							string text6;
							ReflectionUtils.SplitFullyQualifiedTypeName(text4, out text5, out text6);
							try
							{
								type = base.Serializer.Binder.BindToType(text6, text5);
							}
							catch (Exception innerException)
							{
								throw new JsonSerializationException("Error resolving type specified in JSON '{0}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									text4
								}), innerException);
							}
							if (type == null)
							{
								goto Block_12;
							}
							if (objectType != null && !objectType.IsAssignableFrom(type))
							{
								goto Block_14;
							}
							objectType = type;
							contract = this.GetContractSafe(type);
						}
						flag = true;
					}
					else if (string.Equals(text2, "$id", 4))
					{
						this.CheckedRead(reader);
						text = ((reader.Value == null) ? null : reader.Value.ToString());
						this.CheckedRead(reader);
						flag = true;
					}
					else
					{
						if (string.Equals(text2, "$values", 4))
						{
							goto Block_17;
						}
						flag = false;
					}
					if (!flag || reader.TokenType != JsonToken.PropertyName)
					{
						goto IL_2B1;
					}
				}
				throw new JsonSerializationException("JSON reference {0} property must have a string or null value.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					"$ref"
				}));
				Block_5:
				if (reader.TokenType == JsonToken.PropertyName)
				{
					throw new JsonSerializationException("Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						"$ref"
					}));
				}
				return base.Serializer.ReferenceResolver.ResolveReference(this, text3);
				Block_12:
				throw new JsonSerializationException("Type specified in JSON '{0}' was not resolved.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					text4
				}));
				Block_14:
				throw new JsonSerializationException("Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					type.get_AssemblyQualifiedName(),
					objectType.get_AssemblyQualifiedName()
				}));
				Block_17:
				this.CheckedRead(reader);
				object result = this.CreateList(reader, objectType, contract, member, existingValue, text);
				this.CheckedRead(reader);
				return result;
			}
			IL_2B1:
			if (!this.HasDefinedType(objectType))
			{
				return this.CreateJObject(reader);
			}
			if (contract == null)
			{
				throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					objectType
				}));
			}
			JsonDictionaryContract jsonDictionaryContract = contract as JsonDictionaryContract;
			if (jsonDictionaryContract != null)
			{
				if (existingValue == null)
				{
					return this.CreateAndPopulateDictionary(reader, jsonDictionaryContract, text);
				}
				return this.PopulateDictionary(jsonDictionaryContract.CreateWrapper(existingValue), reader, jsonDictionaryContract, text);
			}
			else
			{
				JsonObjectContract jsonObjectContract = contract as JsonObjectContract;
				if (jsonObjectContract != null)
				{
					if (existingValue == null)
					{
						return this.CreateAndPopulateObject(reader, jsonObjectContract, text);
					}
					return this.PopulateObject(existingValue, reader, jsonObjectContract, text);
				}
				else
				{
					JsonPrimitiveContract jsonPrimitiveContract = contract as JsonPrimitiveContract;
					if (jsonPrimitiveContract != null && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", 4))
					{
						this.CheckedRead(reader);
						object result2 = this.CreateValueInternal(reader, objectType, jsonPrimitiveContract, member, existingValue);
						this.CheckedRead(reader);
						return result2;
					}
					JsonISerializableContract jsonISerializableContract = contract as JsonISerializableContract;
					if (jsonISerializableContract != null)
					{
						return this.CreateISerializable(reader, jsonISerializableContract, text);
					}
					throw new JsonSerializationException("Cannot deserialize JSON object into type '{0}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						objectType
					}));
				}
			}
		}

		private JsonArrayContract EnsureArrayContract(Type objectType, JsonContract contract)
		{
			if (contract == null)
			{
				throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					objectType
				}));
			}
			JsonArrayContract jsonArrayContract = contract as JsonArrayContract;
			if (jsonArrayContract == null)
			{
				throw new JsonSerializationException("Cannot deserialize JSON array into type '{0}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					objectType
				}));
			}
			return jsonArrayContract;
		}

		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new JsonSerializationException("Unexpected end when deserializing object.");
			}
		}

		private object CreateList(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue, string reference)
		{
			object result;
			if (this.HasDefinedType(objectType))
			{
				JsonArrayContract jsonArrayContract = this.EnsureArrayContract(objectType, contract);
				if (existingValue == null || objectType == typeof(BitArray))
				{
					result = this.CreateAndPopulateList(reader, reference, jsonArrayContract);
				}
				else
				{
					result = this.PopulateList(jsonArrayContract.CreateWrapper(existingValue), reader, reference, jsonArrayContract);
				}
			}
			else
			{
				result = this.CreateJToken(reader, contract);
			}
			return result;
		}

		private bool HasDefinedType(Type type)
		{
			return type != null && type != typeof(object) && !typeof(JToken).IsAssignableFrom(type);
		}

		private object EnsureType(object value, CultureInfo culture, Type targetType)
		{
			if (targetType == null)
			{
				return value;
			}
			Type objectType = ReflectionUtils.GetObjectType(value);
			if (objectType != targetType)
			{
				try
				{
					return ConvertUtils.ConvertOrCast(value, culture, targetType);
				}
				catch (Exception innerException)
				{
					throw new JsonSerializationException("Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						this.FormatValueForPrint(value),
						targetType
					}), innerException);
				}
				return value;
			}
			return value;
		}

		private string FormatValueForPrint(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (value is string)
			{
				return "\"" + value + "\"";
			}
			return value.ToString();
		}

		private void SetPropertyValue(JsonProperty property, JsonReader reader, object target)
		{
			if (property.Ignored)
			{
				reader.Skip();
				return;
			}
			object obj = null;
			bool flag = false;
			bool gottenCurrentValue = false;
			ObjectCreationHandling valueOrDefault = property.ObjectCreationHandling.GetValueOrDefault(base.Serializer.ObjectCreationHandling);
			if ((valueOrDefault == ObjectCreationHandling.Auto || valueOrDefault == ObjectCreationHandling.Reuse) && (reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject) && property.Readable)
			{
				obj = property.ValueProvider.GetValue(target);
				gottenCurrentValue = true;
				flag = (obj != null && !property.PropertyType.get_IsArray() && !ReflectionUtils.InheritsGenericDefinition(property.PropertyType, typeof(ReadOnlyCollection)) && !property.PropertyType.get_IsValueType());
			}
			if (!property.Writable && !flag)
			{
				reader.Skip();
				return;
			}
			if (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == NullValueHandling.Ignore && reader.TokenType == JsonToken.Null)
			{
				reader.Skip();
				return;
			}
			if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) && JsonReader.IsPrimitiveToken(reader.TokenType) && MiscellaneousUtils.ValueEquals(reader.Value, property.DefaultValue))
			{
				reader.Skip();
				return;
			}
			object currentValue = (!flag) ? null : obj;
			object obj2 = this.CreateValueProperty(reader, property, target, gottenCurrentValue, currentValue);
			if ((!flag || obj2 != obj) && this.ShouldSetPropertyValue(property, obj2))
			{
				property.ValueProvider.SetValue(target, obj2);
				if (property.SetIsSpecified != null)
				{
					property.SetIsSpecified.Invoke(target, true);
				}
			}
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool ShouldSetPropertyValue(JsonProperty property, object value)
		{
			return (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) != NullValueHandling.Ignore || value != null) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(value, property.DefaultValue)) && property.Writable;
		}

		private object CreateAndPopulateDictionary(JsonReader reader, JsonDictionaryContract contract, string id)
		{
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || base.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object dictionary = contract.DefaultCreator.Invoke();
				IWrappedDictionary wrappedDictionary = contract.CreateWrapper(dictionary);
				this.PopulateDictionary(wrappedDictionary, reader, contract, id);
				return wrappedDictionary.UnderlyingDictionary;
			}
			throw new JsonSerializationException("Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				contract.UnderlyingType
			}));
		}

		private object PopulateDictionary(IWrappedDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, string id)
		{
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, dictionary.UnderlyingDictionary);
			}
			contract.InvokeOnDeserializing(dictionary.UnderlyingDictionary, base.Serializer.Context);
			int depth = reader.Depth;
			JsonToken tokenType;
			while (true)
			{
				tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						break;
					}
				}
				else
				{
					object obj = reader.Value;
					try
					{
						try
						{
							obj = this.EnsureType(obj, CultureInfo.get_InvariantCulture(), contract.DictionaryKeyType);
						}
						catch (Exception innerException)
						{
							throw new JsonSerializationException("Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
							{
								reader.Value,
								contract.DictionaryKeyType
							}), innerException);
						}
						if (!this.ReadForType(reader, contract.DictionaryValueType, null))
						{
							throw new JsonSerializationException("Unexpected end when deserializing object.");
						}
						dictionary.set_Item(obj, this.CreateValueNonProperty(reader, contract.DictionaryValueType, this.GetContractSafe(contract.DictionaryValueType)));
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(dictionary, contract, obj, ex))
						{
							throw;
						}
						this.HandleError(reader, depth);
					}
				}
				if (!reader.Read())
				{
					goto Block_6;
				}
			}
			if (tokenType != JsonToken.EndObject)
			{
				throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			}
			contract.InvokeOnDeserialized(dictionary.UnderlyingDictionary, base.Serializer.Context);
			return dictionary.UnderlyingDictionary;
			Block_6:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		private object CreateAndPopulateList(JsonReader reader, string reference, JsonArrayContract contract)
		{
			return CollectionUtils.CreateAndPopulateList(contract.CreatedType, delegate(IList l, bool isTemporaryListReference)
			{
				if (reference != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot preserve reference to array or readonly list: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						contract.UnderlyingType
					}));
				}
				if (contract.OnSerializing != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot call OnSerializing on an array or readonly list: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						contract.UnderlyingType
					}));
				}
				if (contract.OnError != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot call OnError on an array or readonly list: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						contract.UnderlyingType
					}));
				}
				if (!contract.IsMultidimensionalArray)
				{
					this.PopulateList(contract.CreateWrapper(l), reader, reference, contract);
				}
				else
				{
					this.PopulateMultidimensionalArray(l, reader, reference, contract);
				}
			});
		}

		private bool ReadForTypeArrayHack(JsonReader reader, Type t)
		{
			bool result;
			try
			{
				result = this.ReadForType(reader, t, null);
			}
			catch (JsonReaderException)
			{
				if (reader.TokenType != JsonToken.EndArray)
				{
					throw;
				}
				result = true;
			}
			return result;
		}

		private object PopulateList(IWrappedCollection wrappedList, JsonReader reader, string reference, JsonArrayContract contract)
		{
			object underlyingCollection = wrappedList.UnderlyingCollection;
			if (wrappedList.get_IsFixedSize())
			{
				reader.Skip();
				return wrappedList.UnderlyingCollection;
			}
			if (reference != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, reference, underlyingCollection);
			}
			contract.InvokeOnDeserializing(underlyingCollection, base.Serializer.Context);
			int depth = reader.Depth;
			while (this.ReadForTypeArrayHack(reader, contract.CollectionItemType))
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						contract.InvokeOnDeserialized(underlyingCollection, base.Serializer.Context);
						return wrappedList.UnderlyingCollection;
					}
					try
					{
						object obj = this.CreateValueNonProperty(reader, contract.CollectionItemType, this.GetContractSafe(contract.CollectionItemType));
						wrappedList.Add(obj);
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(underlyingCollection, contract, wrappedList.get_Count(), ex))
						{
							throw;
						}
						this.HandleError(reader, depth);
					}
				}
			}
			throw new JsonSerializationException("Unexpected end when deserializing array.");
		}

		private object PopulateMultidimensionalArray(IList list, JsonReader reader, string reference, JsonArrayContract contract)
		{
			int arrayRank = contract.UnderlyingType.GetArrayRank();
			if (reference != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, reference, list);
			}
			contract.InvokeOnDeserializing(list, base.Serializer.Context);
			Stack<IList> stack = new Stack<IList>();
			stack.Push(list);
			IList list2 = list;
			bool flag = false;
			do
			{
				int depth = reader.Depth;
				if (stack.get_Count() == arrayRank)
				{
					if (!this.ReadForTypeArrayHack(reader, contract.CollectionItemType))
					{
						break;
					}
					JsonToken tokenType = reader.TokenType;
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType != JsonToken.EndArray)
						{
							try
							{
								object obj = this.CreateValueNonProperty(reader, contract.CollectionItemType, this.GetContractSafe(contract.CollectionItemType));
								list2.Add(obj);
							}
							catch (Exception ex)
							{
								if (!base.IsErrorHandled(list, contract, list2.get_Count(), ex))
								{
									throw;
								}
								this.HandleError(reader, depth);
							}
						}
						else
						{
							stack.Pop();
							list2 = stack.Peek();
						}
					}
				}
				else
				{
					if (!reader.Read())
					{
						break;
					}
					JsonToken tokenType = reader.TokenType;
					switch (tokenType)
					{
					case JsonToken.StartArray:
					{
						IList list3 = new List<object>();
						list2.Add(list3);
						stack.Push(list3);
						list2 = list3;
						goto IL_1B8;
					}
					case JsonToken.StartConstructor:
					case JsonToken.PropertyName:
						IL_144:
						if (tokenType != JsonToken.EndArray)
						{
							goto Block_8;
						}
						stack.Pop();
						if (stack.get_Count() > 0)
						{
							list2 = stack.Peek();
						}
						else
						{
							flag = true;
						}
						goto IL_1B8;
					case JsonToken.Comment:
						goto IL_1B8;
					}
					goto IL_144;
					IL_1B8:;
				}
			}
			while (!flag);
			goto IL_1C8;
			Block_8:
			throw new JsonSerializationException("Unexpected token when deserializing multidimensional array: " + reader.TokenType);
			IL_1C8:
			if (!flag)
			{
				throw new JsonSerializationException("Unexpected end when deserializing array." + reader.TokenType);
			}
			contract.InvokeOnDeserialized(list, base.Serializer.Context);
			return list;
		}

		private object CreateISerializable(JsonReader reader, JsonISerializableContract contract, string id)
		{
			Type underlyingType = contract.UnderlyingType;
			SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, this.GetFormatterConverter());
			bool flag = false;
			string text;
			while (true)
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType != JsonToken.EndObject)
						{
							break;
						}
						flag = true;
					}
				}
				else
				{
					text = reader.Value.ToString();
					if (!reader.Read())
					{
						goto Block_4;
					}
					serializationInfo.AddValue(text, JToken.ReadFrom(reader));
				}
				if (flag || !reader.Read())
				{
					goto IL_C1;
				}
			}
			throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			Block_4:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				text
			}));
			IL_C1:
			if (contract.ISerializableCreator == null)
			{
				throw new JsonSerializationException("ISerializable type '{0}' does not have a valid constructor. To correctly implement ISerializable a constructor that takes SerializationInfo and StreamingContext parameters should be present.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					underlyingType
				}));
			}
			object obj = contract.ISerializableCreator(new object[]
			{
				serializationInfo,
				base.Serializer.Context
			});
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, obj);
			}
			contract.InvokeOnDeserializing(obj, base.Serializer.Context);
			contract.InvokeOnDeserialized(obj, base.Serializer.Context);
			return obj;
		}

		private object CreateAndPopulateObject(JsonReader reader, JsonObjectContract contract, string id)
		{
			object obj = null;
			if (contract.UnderlyingType.get_IsInterface() || contract.UnderlyingType.get_IsAbstract())
			{
				throw new JsonSerializationException("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantated.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					contract.UnderlyingType
				}));
			}
			if (contract.OverrideConstructor != null)
			{
				if (contract.OverrideConstructor.GetParameters().Length > 0)
				{
					return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.OverrideConstructor, id);
				}
				obj = contract.OverrideConstructor.Invoke(null);
			}
			else if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || base.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				obj = contract.DefaultCreator.Invoke();
			}
			else if (contract.ParametrizedConstructor != null)
			{
				return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.ParametrizedConstructor, id);
			}
			if (obj == null)
			{
				throw new JsonSerializationException("Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					contract.UnderlyingType
				}));
			}
			this.PopulateObject(obj, reader, contract, id);
			return obj;
		}

		private object CreateObjectFromNonDefaultConstructor(JsonReader reader, JsonObjectContract contract, ConstructorInfo constructorInfo, string id)
		{
			ValidationUtils.ArgumentNotNull(constructorInfo, "constructorInfo");
			Type underlyingType = contract.UnderlyingType;
			IDictionary<JsonProperty, object> dictionary = this.ResolvePropertyAndConstructorValues(contract, reader, underlyingType);
			IDictionary<ParameterInfo, object> dictionary2 = Enumerable.ToDictionary<ParameterInfo, ParameterInfo, object>(constructorInfo.GetParameters(), (ParameterInfo p) => p, (ParameterInfo p) => null);
			IDictionary<JsonProperty, object> dictionary3 = new Dictionary<JsonProperty, object>();
			using (IEnumerator<KeyValuePair<JsonProperty, object>> enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<JsonProperty, object> current = enumerator.get_Current();
					ParameterInfo key = dictionary2.ForgivingCaseSensitiveFind((KeyValuePair<ParameterInfo, object> kv) => kv.get_Key().get_Name(), current.get_Key().UnderlyingName).get_Key();
					if (key != null)
					{
						dictionary2.set_Item(key, current.get_Value());
					}
					else
					{
						dictionary3.Add(current);
					}
				}
			}
			object obj = constructorInfo.Invoke(Enumerable.ToArray<object>(dictionary2.get_Values()));
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, obj);
			}
			contract.InvokeOnDeserializing(obj, base.Serializer.Context);
			using (IEnumerator<KeyValuePair<JsonProperty, object>> enumerator2 = dictionary3.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<JsonProperty, object> current2 = enumerator2.get_Current();
					JsonProperty key2 = current2.get_Key();
					object value = current2.get_Value();
					if (this.ShouldSetPropertyValue(current2.get_Key(), current2.get_Value()))
					{
						key2.ValueProvider.SetValue(obj, value);
					}
					else if (!key2.Writable && value != null)
					{
						JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(key2.PropertyType);
						if (jsonContract is JsonArrayContract)
						{
							JsonArrayContract jsonArrayContract = jsonContract as JsonArrayContract;
							object value2 = key2.ValueProvider.GetValue(obj);
							if (value2 != null)
							{
								IWrappedCollection wrappedCollection = jsonArrayContract.CreateWrapper(value2);
								IWrappedCollection wrappedCollection2 = jsonArrayContract.CreateWrapper(value);
								IEnumerator enumerator3 = wrappedCollection2.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										object current3 = enumerator3.get_Current();
										wrappedCollection.Add(current3);
									}
								}
								finally
								{
									IDisposable disposable = enumerator3 as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
							}
						}
						else if (jsonContract is JsonDictionaryContract)
						{
							JsonDictionaryContract jsonDictionaryContract = jsonContract as JsonDictionaryContract;
							object value3 = key2.ValueProvider.GetValue(obj);
							if (value3 != null)
							{
								IWrappedDictionary wrappedDictionary = jsonDictionaryContract.CreateWrapper(value3);
								IWrappedDictionary wrappedDictionary2 = jsonDictionaryContract.CreateWrapper(value);
								IEnumerator enumerator4 = wrappedDictionary2.GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator4.get_Current();
										wrappedDictionary.Add(dictionaryEntry.get_Key(), dictionaryEntry.get_Value());
									}
								}
								finally
								{
									IDisposable disposable2 = enumerator4 as IDisposable;
									if (disposable2 != null)
									{
										disposable2.Dispose();
									}
								}
							}
						}
					}
				}
			}
			contract.InvokeOnDeserialized(obj, base.Serializer.Context);
			return obj;
		}

		private IDictionary<JsonProperty, object> ResolvePropertyAndConstructorValues(JsonObjectContract contract, JsonReader reader, Type objectType)
		{
			IDictionary<JsonProperty, object> dictionary = new Dictionary<JsonProperty, object>();
			bool flag = false;
			string text;
			while (true)
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType != JsonToken.EndObject)
						{
							break;
						}
						flag = true;
					}
				}
				else
				{
					text = reader.Value.ToString();
					JsonProperty jsonProperty = contract.ConstructorParameters.GetClosestMatchProperty(text) ?? contract.Properties.GetClosestMatchProperty(text);
					if (jsonProperty != null)
					{
						if (!this.ReadForType(reader, jsonProperty.PropertyType, jsonProperty.Converter))
						{
							goto Block_6;
						}
						if (!jsonProperty.Ignored)
						{
							dictionary.set_Item(jsonProperty, this.CreateValueProperty(reader, jsonProperty, null, true, null));
						}
						else
						{
							reader.Skip();
						}
					}
					else
					{
						if (!reader.Read())
						{
							goto Block_8;
						}
						if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
						{
							goto Block_9;
						}
						reader.Skip();
					}
				}
				if (flag || !reader.Read())
				{
					return dictionary;
				}
			}
			throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			Block_6:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				text
			}));
			Block_8:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				text
			}));
			Block_9:
			throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				text,
				objectType.get_Name()
			}));
		}

		private bool ReadForType(JsonReader reader, Type t, JsonConverter propertyConverter)
		{
			bool flag = this.GetConverter(this.GetContractSafe(t), propertyConverter) != null;
			if (flag)
			{
				return reader.Read();
			}
			if (t == typeof(byte[]))
			{
				reader.ReadAsBytes();
				return true;
			}
			if (t == typeof(decimal) || t == typeof(decimal?))
			{
				reader.ReadAsDecimal();
				return true;
			}
			if (t == typeof(DateTimeOffset) || t == typeof(DateTimeOffset?))
			{
				reader.ReadAsDateTimeOffset();
				return true;
			}
			while (reader.Read())
			{
				if (reader.TokenType != JsonToken.Comment)
				{
					return true;
				}
			}
			return false;
		}

		private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, string id)
		{
			contract.InvokeOnDeserializing(newObject, base.Serializer.Context);
			Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary = Enumerable.ToDictionary<JsonProperty, JsonProperty, JsonSerializerInternalReader.PropertyPresence>(contract.Properties, (JsonProperty m) => m, (JsonProperty m) => JsonSerializerInternalReader.PropertyPresence.None);
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, newObject);
			}
			int depth = reader.Depth;
			JsonToken tokenType;
			while (true)
			{
				tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						break;
					}
				}
				else
				{
					string text = reader.Value.ToString();
					try
					{
						JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
						if (closestMatchProperty == null)
						{
							if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
							{
								throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									text,
									contract.UnderlyingType.get_Name()
								}));
							}
							reader.Skip();
						}
						else
						{
							if (!this.ReadForType(reader, closestMatchProperty.PropertyType, closestMatchProperty.Converter))
							{
								throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									text
								}));
							}
							this.SetPropertyPresence(reader, closestMatchProperty, dictionary);
							this.SetPropertyValue(closestMatchProperty, reader, newObject);
						}
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(newObject, contract, text, ex))
						{
							throw;
						}
						this.HandleError(reader, depth);
					}
				}
				if (!reader.Read())
				{
					goto Block_9;
				}
			}
			if (tokenType != JsonToken.EndObject)
			{
				throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			}
			using (Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<JsonProperty, JsonSerializerInternalReader.PropertyPresence> current = enumerator.get_Current();
					JsonProperty key = current.get_Key();
					JsonSerializerInternalReader.PropertyPresence value = current.get_Value();
					JsonSerializerInternalReader.PropertyPresence propertyPresence = value;
					if (propertyPresence != JsonSerializerInternalReader.PropertyPresence.None)
					{
						if (propertyPresence == JsonSerializerInternalReader.PropertyPresence.Null)
						{
							if (key.Required == Required.Always)
							{
								throw new JsonSerializationException("Required property '{0}' expects a value but got null.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									key.PropertyName
								}));
							}
						}
					}
					else
					{
						if (key.Required == Required.AllowNull || key.Required == Required.Always)
						{
							throw new JsonSerializationException("Required property '{0}' not found in JSON.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
							{
								key.PropertyName
							}));
						}
						if (this.HasFlag(key.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), DefaultValueHandling.Populate) && key.Writable)
						{
							key.ValueProvider.SetValue(newObject, this.EnsureType(key.DefaultValue, CultureInfo.get_InvariantCulture(), key.PropertyType));
						}
					}
				}
			}
			contract.InvokeOnDeserialized(newObject, base.Serializer.Context);
			return newObject;
			Block_9:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		private void SetPropertyPresence(JsonReader reader, JsonProperty property, Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> requiredProperties)
		{
			if (property != null)
			{
				requiredProperties.set_Item(property, (reader.TokenType != JsonToken.Null && reader.TokenType != JsonToken.Undefined) ? JsonSerializerInternalReader.PropertyPresence.Value : JsonSerializerInternalReader.PropertyPresence.Null);
			}
		}

		private void HandleError(JsonReader reader, int initialDepth)
		{
			base.ClearErrorContext();
			reader.Skip();
			while (reader.Depth > initialDepth + 1)
			{
				reader.Read();
			}
		}
	}
}
