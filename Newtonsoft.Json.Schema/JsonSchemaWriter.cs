using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaWriter
	{
		private readonly JsonWriter _writer;

		private readonly JsonSchemaResolver _resolver;

		public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
			this._resolver = resolver;
		}

		private void ReferenceOrWriteSchema(JsonSchema schema)
		{
			if (schema.Id != null && this._resolver.GetSchema(schema.Id) != null)
			{
				this._writer.WriteStartObject();
				this._writer.WritePropertyName("$ref");
				this._writer.WriteValue(schema.Id);
				this._writer.WriteEndObject();
			}
			else
			{
				this.WriteSchema(schema);
			}
		}

		public void WriteSchema(JsonSchema schema)
		{
			ValidationUtils.ArgumentNotNull(schema, "schema");
			if (!this._resolver.LoadedSchemas.Contains(schema))
			{
				this._resolver.LoadedSchemas.Add(schema);
			}
			this._writer.WriteStartObject();
			this.WritePropertyIfNotNull(this._writer, "id", schema.Id);
			this.WritePropertyIfNotNull(this._writer, "title", schema.Title);
			this.WritePropertyIfNotNull(this._writer, "description", schema.Description);
			this.WritePropertyIfNotNull(this._writer, "required", schema.Required);
			this.WritePropertyIfNotNull(this._writer, "readonly", schema.ReadOnly);
			this.WritePropertyIfNotNull(this._writer, "hidden", schema.Hidden);
			this.WritePropertyIfNotNull(this._writer, "transient", schema.Transient);
			if (schema.Type.get_HasValue())
			{
				this.WriteType("type", this._writer, schema.Type.get_Value());
			}
			if (!schema.AllowAdditionalProperties)
			{
				this._writer.WritePropertyName("additionalProperties");
				this._writer.WriteValue(schema.AllowAdditionalProperties);
			}
			else if (schema.AdditionalProperties != null)
			{
				this._writer.WritePropertyName("additionalProperties");
				this.ReferenceOrWriteSchema(schema.AdditionalProperties);
			}
			this.WriteSchemaDictionaryIfNotNull(this._writer, "properties", schema.Properties);
			this.WriteSchemaDictionaryIfNotNull(this._writer, "patternProperties", schema.PatternProperties);
			this.WriteItems(schema);
			this.WritePropertyIfNotNull(this._writer, "minimum", schema.Minimum);
			this.WritePropertyIfNotNull(this._writer, "maximum", schema.Maximum);
			this.WritePropertyIfNotNull(this._writer, "exclusiveMinimum", schema.ExclusiveMinimum);
			this.WritePropertyIfNotNull(this._writer, "exclusiveMaximum", schema.ExclusiveMaximum);
			this.WritePropertyIfNotNull(this._writer, "minLength", schema.MinimumLength);
			this.WritePropertyIfNotNull(this._writer, "maxLength", schema.MaximumLength);
			this.WritePropertyIfNotNull(this._writer, "minItems", schema.MinimumItems);
			this.WritePropertyIfNotNull(this._writer, "maxItems", schema.MaximumItems);
			this.WritePropertyIfNotNull(this._writer, "divisibleBy", schema.DivisibleBy);
			this.WritePropertyIfNotNull(this._writer, "format", schema.Format);
			this.WritePropertyIfNotNull(this._writer, "pattern", schema.Pattern);
			if (schema.Enum != null)
			{
				this._writer.WritePropertyName("enum");
				this._writer.WriteStartArray();
				using (IEnumerator<JToken> enumerator = schema.Enum.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JToken current = enumerator.get_Current();
						current.WriteTo(this._writer, new JsonConverter[0]);
					}
				}
				this._writer.WriteEndArray();
			}
			if (schema.Default != null)
			{
				this._writer.WritePropertyName("default");
				schema.Default.WriteTo(this._writer, new JsonConverter[0]);
			}
			if (schema.Options != null)
			{
				this._writer.WritePropertyName("options");
				this._writer.WriteStartArray();
				using (IEnumerator<KeyValuePair<JToken, string>> enumerator2 = schema.Options.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<JToken, string> current2 = enumerator2.get_Current();
						this._writer.WriteStartObject();
						this._writer.WritePropertyName("value");
						current2.get_Key().WriteTo(this._writer, new JsonConverter[0]);
						if (current2.get_Value() != null)
						{
							this._writer.WritePropertyName("label");
							this._writer.WriteValue(current2.get_Value());
						}
						this._writer.WriteEndObject();
					}
				}
				this._writer.WriteEndArray();
			}
			if (schema.Disallow.get_HasValue())
			{
				this.WriteType("disallow", this._writer, schema.Disallow.get_Value());
			}
			if (schema.Extends != null)
			{
				this._writer.WritePropertyName("extends");
				this.ReferenceOrWriteSchema(schema.Extends);
			}
			this._writer.WriteEndObject();
		}

		private void WriteSchemaDictionaryIfNotNull(JsonWriter writer, string propertyName, IDictionary<string, JsonSchema> properties)
		{
			if (properties != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteStartObject();
				using (IEnumerator<KeyValuePair<string, JsonSchema>> enumerator = properties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, JsonSchema> current = enumerator.get_Current();
						writer.WritePropertyName(current.get_Key());
						this.ReferenceOrWriteSchema(current.get_Value());
					}
				}
				writer.WriteEndObject();
			}
		}

		private void WriteItems(JsonSchema schema)
		{
			if (CollectionUtils.IsNullOrEmpty<JsonSchema>(schema.Items))
			{
				return;
			}
			this._writer.WritePropertyName("items");
			if (schema.Items.get_Count() == 1)
			{
				this.ReferenceOrWriteSchema(schema.Items.get_Item(0));
				return;
			}
			this._writer.WriteStartArray();
			using (IEnumerator<JsonSchema> enumerator = schema.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchema current = enumerator.get_Current();
					this.ReferenceOrWriteSchema(current);
				}
			}
			this._writer.WriteEndArray();
		}

		private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
		{
			IList<JsonSchemaType> list2;
			if (Enum.IsDefined(typeof(JsonSchemaType), type))
			{
				List<JsonSchemaType> list = new List<JsonSchemaType>();
				list.Add(type);
				list2 = list;
			}
			else
			{
				list2 = Enumerable.ToList<JsonSchemaType>(Enumerable.Where<JsonSchemaType>(EnumUtils.GetFlagsValues<JsonSchemaType>(type), (JsonSchemaType v) => v != JsonSchemaType.None));
			}
			if (list2.get_Count() == 0)
			{
				return;
			}
			writer.WritePropertyName(propertyName);
			if (list2.get_Count() == 1)
			{
				writer.WriteValue(JsonSchemaBuilder.MapType(list2.get_Item(0)));
				return;
			}
			writer.WriteStartArray();
			using (IEnumerator<JsonSchemaType> enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaType current = enumerator.get_Current();
					writer.WriteValue(JsonSchemaBuilder.MapType(current));
				}
			}
			writer.WriteEndArray();
		}

		private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
		{
			if (value != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(value);
			}
		}
	}
}
