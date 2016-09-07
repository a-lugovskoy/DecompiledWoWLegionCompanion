using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	public class JsonSchemaGenerator
	{
		private class TypeSchema
		{
			public Type Type
			{
				get;
				private set;
			}

			public JsonSchema Schema
			{
				get;
				private set;
			}

			public TypeSchema(Type type, JsonSchema schema)
			{
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				this.Type = type;
				this.Schema = schema;
			}
		}

		private IContractResolver _contractResolver;

		private JsonSchemaResolver _resolver;

		private IList<JsonSchemaGenerator.TypeSchema> _stack = new List<JsonSchemaGenerator.TypeSchema>();

		private JsonSchema _currentSchema;

		public UndefinedSchemaIdHandling UndefinedSchemaIdHandling
		{
			get;
			set;
		}

		public IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
		{
			this._currentSchema = typeSchema.Schema;
			this._stack.Add(typeSchema);
			this._resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		private JsonSchemaGenerator.TypeSchema Pop()
		{
			JsonSchemaGenerator.TypeSchema result = this._stack.get_Item(this._stack.get_Count() - 1);
			this._stack.RemoveAt(this._stack.get_Count() - 1);
			JsonSchemaGenerator.TypeSchema typeSchema = Enumerable.LastOrDefault<JsonSchemaGenerator.TypeSchema>(this._stack);
			if (typeSchema != null)
			{
				this._currentSchema = typeSchema.Schema;
			}
			else
			{
				this._currentSchema = null;
			}
			return result;
		}

		public JsonSchema Generate(Type type)
		{
			return this.Generate(type, new JsonSchemaResolver(), false);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return this.Generate(type, resolver, false);
		}

		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			this._resolver = resolver;
			return this.GenerateInternal(type, rootSchemaNullable ? Required.Default : Required.Always, false);
		}

		private string GetTitle(Type type)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Title))
			{
				return jsonContainerAttribute.Title;
			}
			return null;
		}

		private string GetDescription(Type type)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Description))
			{
				return jsonContainerAttribute.Description;
			}
			DescriptionAttribute attribute = ReflectionUtils.GetAttribute<DescriptionAttribute>(type);
			if (attribute != null)
			{
				return attribute.get_Description();
			}
			return null;
		}

		private string GetTypeId(Type type, bool explicitOnly)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Id))
			{
				return jsonContainerAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			UndefinedSchemaIdHandling undefinedSchemaIdHandling = this.UndefinedSchemaIdHandling;
			if (undefinedSchemaIdHandling == UndefinedSchemaIdHandling.UseTypeName)
			{
				return type.get_FullName();
			}
			if (undefinedSchemaIdHandling != UndefinedSchemaIdHandling.UseAssemblyQualifiedName)
			{
				return null;
			}
			return type.get_AssemblyQualifiedName();
		}

		private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = this.GetTypeId(type, false);
			string typeId2 = this.GetTypeId(type, true);
			if (!string.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = this._resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
					{
						JsonSchema expr_76 = schema;
						JsonSchemaType? type2 = expr_76.Type;
						expr_76.Type = ((!type2.get_HasValue()) ? default(JsonSchemaType?) : new JsonSchemaType?(type2.GetValueOrDefault() | JsonSchemaType.Null));
					}
					if (required)
					{
						bool? required2 = schema.Required;
						if (!required2.GetValueOrDefault() || !required2.get_HasValue())
						{
							schema.Required = new bool?(true);
						}
					}
					return schema;
				}
			}
			if (Enumerable.Any<JsonSchemaGenerator.TypeSchema>(this._stack, (JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
			{
				throw new Exception("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					type
				}));
			}
			JsonContract jsonContract = this.ContractResolver.ResolveContract(type);
			JsonConverter jsonConverter;
			if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
			{
				JsonSchema schema2 = jsonConverter.GetSchema();
				if (schema2 != null)
				{
					return schema2;
				}
			}
			this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
			if (typeId2 != null)
			{
				this.CurrentSchema.Id = typeId2;
			}
			if (required)
			{
				this.CurrentSchema.Required = new bool?(true);
			}
			this.CurrentSchema.Title = this.GetTitle(type);
			this.CurrentSchema.Description = this.GetDescription(type);
			if (jsonConverter != null)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
			}
			else if (jsonContract is JsonDictionaryContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
				Type type3;
				Type type4;
				ReflectionUtils.GetDictionaryKeyValueTypes(type, out type3, out type4);
				if (type3 != null && typeof(IConvertible).IsAssignableFrom(type3))
				{
					this.CurrentSchema.AdditionalProperties = this.GenerateInternal(type4, Required.Default, false);
				}
			}
			else if (jsonContract is JsonArrayContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
				this.CurrentSchema.Id = this.GetTypeId(type, false);
				JsonArrayAttribute jsonArrayAttribute = JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
				bool flag = jsonArrayAttribute == null || jsonArrayAttribute.AllowNullItems;
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
				if (collectionItemType != null)
				{
					this.CurrentSchema.Items = new List<JsonSchema>();
					this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, flag ? Required.Default : Required.Always, false));
				}
			}
			else if (jsonContract is JsonPrimitiveContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
				JsonSchemaType? type5 = this.CurrentSchema.Type;
				if (type5.GetValueOrDefault() == JsonSchemaType.Integer && type5.get_HasValue() && type.get_IsEnum() && !type.IsDefined(typeof(FlagsAttribute), true))
				{
					this.CurrentSchema.Enum = new List<JToken>();
					this.CurrentSchema.Options = new Dictionary<JToken, string>();
					EnumValues<long> namesAndValues = EnumUtils.GetNamesAndValues<long>(type);
					using (IEnumerator<EnumValue<long>> enumerator = namesAndValues.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							EnumValue<long> current = enumerator.get_Current();
							JToken jToken = JToken.FromObject(current.Value);
							this.CurrentSchema.Enum.Add(jToken);
							this.CurrentSchema.Options.Add(jToken, current.Name);
						}
					}
				}
			}
			else if (jsonContract is JsonObjectContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
				this.CurrentSchema.Id = this.GetTypeId(type, false);
				this.GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
			}
			else if (jsonContract is JsonISerializableContract)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
				this.CurrentSchema.Id = this.GetTypeId(type, false);
				this.GenerateISerializableContract(type, (JsonISerializableContract)jsonContract);
			}
			else if (jsonContract is JsonStringContract)
			{
				JsonSchemaType jsonSchemaType = ReflectionUtils.IsNullable(jsonContract.UnderlyingType) ? this.AddNullType(JsonSchemaType.String, valueRequired) : JsonSchemaType.String;
				this.CurrentSchema.Type = new JsonSchemaType?(jsonSchemaType);
			}
			else
			{
				if (!(jsonContract is JsonLinqContract))
				{
					throw new Exception("Unexpected contract type: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						jsonContract
					}));
				}
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
			}
			return this.Pop().Schema;
		}

		private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
		{
			if (valueRequired != Required.Always)
			{
				return type | JsonSchemaType.Null;
			}
			return type;
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private void GenerateObjectSchema(Type type, JsonObjectContract contract)
		{
			this.CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
			using (IEnumerator<JsonProperty> enumerator = contract.Properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonProperty current = enumerator.get_Current();
					if (!current.Ignored)
					{
						NullValueHandling? nullValueHandling = current.NullValueHandling;
						bool flag = (nullValueHandling.GetValueOrDefault() == NullValueHandling.Ignore && nullValueHandling.get_HasValue()) || this.HasFlag(current.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || current.ShouldSerialize != null || current.GetIsSpecified != null;
						JsonSchema jsonSchema = this.GenerateInternal(current.PropertyType, current.Required, !flag);
						if (current.DefaultValue != null)
						{
							jsonSchema.Default = JToken.FromObject(current.DefaultValue);
						}
						this.CurrentSchema.Properties.Add(current.PropertyName, jsonSchema);
					}
				}
			}
			if (type.get_IsSealed())
			{
				this.CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
		{
			this.CurrentSchema.AllowAdditionalProperties = true;
		}

		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			if (!value.get_HasValue())
			{
				return true;
			}
			JsonSchemaType? jsonSchemaType = (!value.get_HasValue()) ? default(JsonSchemaType?) : new JsonSchemaType?(value.GetValueOrDefault() & flag);
			return jsonSchemaType.GetValueOrDefault() == flag && jsonSchemaType.get_HasValue();
		}

		private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			switch (typeCode)
			{
			case 0:
			case 1:
				return jsonSchemaType | JsonSchemaType.String;
			case 2:
				return jsonSchemaType | JsonSchemaType.Null;
			case 3:
				return jsonSchemaType | JsonSchemaType.Boolean;
			case 4:
				return jsonSchemaType | JsonSchemaType.String;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				return jsonSchemaType | JsonSchemaType.Integer;
			case 13:
			case 14:
			case 15:
				return jsonSchemaType | JsonSchemaType.Float;
			case 16:
				return jsonSchemaType | JsonSchemaType.String;
			case 18:
				return jsonSchemaType | JsonSchemaType.String;
			}
			throw new Exception("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				typeCode,
				type
			}));
		}
	}
}
