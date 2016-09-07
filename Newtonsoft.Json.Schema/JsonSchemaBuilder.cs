using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaBuilder
	{
		private JsonReader _reader;

		private readonly IList<JsonSchema> _stack;

		private readonly JsonSchemaResolver _resolver;

		private JsonSchema _currentSchema;

		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		public JsonSchemaBuilder(JsonSchemaResolver resolver)
		{
			this._stack = new List<JsonSchema>();
			this._resolver = resolver;
		}

		private void Push(JsonSchema value)
		{
			this._currentSchema = value;
			this._stack.Add(value);
			this._resolver.LoadedSchemas.Add(value);
		}

		private JsonSchema Pop()
		{
			JsonSchema currentSchema = this._currentSchema;
			this._stack.RemoveAt(this._stack.get_Count() - 1);
			this._currentSchema = Enumerable.LastOrDefault<JsonSchema>(this._stack);
			return currentSchema;
		}

		internal JsonSchema Parse(JsonReader reader)
		{
			this._reader = reader;
			if (reader.TokenType == JsonToken.None)
			{
				this._reader.Read();
			}
			return this.BuildSchema();
		}

		private JsonSchema BuildSchema()
		{
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected StartObject while parsing schema object, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					this._reader.TokenType
				}));
			}
			this._reader.Read();
			if (this._reader.TokenType == JsonToken.EndObject)
			{
				this.Push(new JsonSchema());
				return this.Pop();
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.get_InvariantCulture());
			this._reader.Read();
			if (!(text == "$ref"))
			{
				this.Push(new JsonSchema());
				this.ProcessSchemaProperty(text);
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
				{
					text = Convert.ToString(this._reader.Value, CultureInfo.get_InvariantCulture());
					this._reader.Read();
					this.ProcessSchemaProperty(text);
				}
				return this.Pop();
			}
			string text2 = (string)this._reader.Value;
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				if (this._reader.TokenType == JsonToken.StartObject)
				{
					throw new Exception("Found StartObject within the schema reference with the Id '{0}'".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						text2
					}));
				}
			}
			JsonSchema schema = this._resolver.GetSchema(text2);
			if (schema == null)
			{
				throw new Exception("Could not resolve schema reference for Id '{0}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					text2
				}));
			}
			return schema;
		}

		private void ProcessSchemaProperty(string propertyName)
		{
			if (propertyName != null)
			{
				if (JsonSchemaBuilder.<>f__switch$map2 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(29);
					dictionary.Add("type", 0);
					dictionary.Add("id", 1);
					dictionary.Add("title", 2);
					dictionary.Add("description", 3);
					dictionary.Add("properties", 4);
					dictionary.Add("items", 5);
					dictionary.Add("additionalProperties", 6);
					dictionary.Add("patternProperties", 7);
					dictionary.Add("required", 8);
					dictionary.Add("requires", 9);
					dictionary.Add("identity", 10);
					dictionary.Add("minimum", 11);
					dictionary.Add("maximum", 12);
					dictionary.Add("exclusiveMinimum", 13);
					dictionary.Add("exclusiveMaximum", 14);
					dictionary.Add("maxLength", 15);
					dictionary.Add("minLength", 16);
					dictionary.Add("maxItems", 17);
					dictionary.Add("minItems", 18);
					dictionary.Add("divisibleBy", 19);
					dictionary.Add("disallow", 20);
					dictionary.Add("default", 21);
					dictionary.Add("hidden", 22);
					dictionary.Add("readonly", 23);
					dictionary.Add("format", 24);
					dictionary.Add("pattern", 25);
					dictionary.Add("options", 26);
					dictionary.Add("enum", 27);
					dictionary.Add("extends", 28);
					JsonSchemaBuilder.<>f__switch$map2 = dictionary;
				}
				int num;
				if (JsonSchemaBuilder.<>f__switch$map2.TryGetValue(propertyName, ref num))
				{
					switch (num)
					{
					case 0:
						this.CurrentSchema.Type = this.ProcessType();
						return;
					case 1:
						this.CurrentSchema.Id = (string)this._reader.Value;
						return;
					case 2:
						this.CurrentSchema.Title = (string)this._reader.Value;
						return;
					case 3:
						this.CurrentSchema.Description = (string)this._reader.Value;
						return;
					case 4:
						this.ProcessProperties();
						return;
					case 5:
						this.ProcessItems();
						return;
					case 6:
						this.ProcessAdditionalProperties();
						return;
					case 7:
						this.ProcessPatternProperties();
						return;
					case 8:
						this.CurrentSchema.Required = new bool?((bool)this._reader.Value);
						return;
					case 9:
						this.CurrentSchema.Requires = (string)this._reader.Value;
						return;
					case 10:
						this.ProcessIdentity();
						return;
					case 11:
						this.CurrentSchema.Minimum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 12:
						this.CurrentSchema.Maximum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 13:
						this.CurrentSchema.ExclusiveMinimum = new bool?((bool)this._reader.Value);
						return;
					case 14:
						this.CurrentSchema.ExclusiveMaximum = new bool?((bool)this._reader.Value);
						return;
					case 15:
						this.CurrentSchema.MaximumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 16:
						this.CurrentSchema.MinimumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 17:
						this.CurrentSchema.MaximumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 18:
						this.CurrentSchema.MinimumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 19:
						this.CurrentSchema.DivisibleBy = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.get_InvariantCulture()));
						return;
					case 20:
						this.CurrentSchema.Disallow = this.ProcessType();
						return;
					case 21:
						this.ProcessDefault();
						return;
					case 22:
						this.CurrentSchema.Hidden = new bool?((bool)this._reader.Value);
						return;
					case 23:
						this.CurrentSchema.ReadOnly = new bool?((bool)this._reader.Value);
						return;
					case 24:
						this.CurrentSchema.Format = (string)this._reader.Value;
						return;
					case 25:
						this.CurrentSchema.Pattern = (string)this._reader.Value;
						return;
					case 26:
						this.ProcessOptions();
						return;
					case 27:
						this.ProcessEnum();
						return;
					case 28:
						this.ProcessExtends();
						return;
					}
				}
			}
			this._reader.Skip();
		}

		private void ProcessExtends()
		{
			this.CurrentSchema.Extends = this.BuildSchema();
		}

		private void ProcessEnum()
		{
			if (this._reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception("Expected StartArray token while parsing enum values, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					this._reader.TokenType
				}));
			}
			this.CurrentSchema.Enum = new List<JToken>();
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
			{
				JToken jToken = JToken.ReadFrom(this._reader);
				this.CurrentSchema.Enum.Add(jToken);
			}
		}

		private void ProcessOptions()
		{
			this.CurrentSchema.Options = new Dictionary<JToken, string>(new JTokenEqualityComparer());
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType != JsonToken.StartArray)
			{
				throw new Exception("Expected array token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					this._reader.TokenType
				}));
			}
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
			{
				if (this._reader.TokenType != JsonToken.StartObject)
				{
					throw new Exception("Expect object token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						this._reader.TokenType
					}));
				}
				string text = null;
				JToken jToken = null;
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
				{
					string text2 = Convert.ToString(this._reader.Value, CultureInfo.get_InvariantCulture());
					this._reader.Read();
					string text3 = text2;
					if (text3 != null)
					{
						if (JsonSchemaBuilder.<>f__switch$map3 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
							dictionary.Add("value", 0);
							dictionary.Add("label", 1);
							JsonSchemaBuilder.<>f__switch$map3 = dictionary;
						}
						int num;
						if (JsonSchemaBuilder.<>f__switch$map3.TryGetValue(text3, ref num))
						{
							if (num == 0)
							{
								jToken = JToken.ReadFrom(this._reader);
								continue;
							}
							if (num == 1)
							{
								text = (string)this._reader.Value;
								continue;
							}
						}
					}
					throw new Exception("Unexpected property in JSON schema option: {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						text2
					}));
				}
				if (jToken == null)
				{
					throw new Exception("No value specified for JSON schema option.");
				}
				if (this.CurrentSchema.Options.ContainsKey(jToken))
				{
					throw new Exception("Duplicate value in JSON schema option collection: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						jToken
					}));
				}
				this.CurrentSchema.Options.Add(jToken, text);
			}
		}

		private void ProcessDefault()
		{
			this.CurrentSchema.Default = JToken.ReadFrom(this._reader);
		}

		private void ProcessIdentity()
		{
			this.CurrentSchema.Identity = new List<string>();
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType != JsonToken.StartArray)
			{
				if (tokenType != JsonToken.String)
				{
					throw new Exception("Expected array or JSON property name string token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						this._reader.TokenType
					}));
				}
				this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
			}
			else
			{
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
				{
					if (this._reader.TokenType != JsonToken.String)
					{
						throw new Exception("Exception JSON property name string token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							this._reader.TokenType
						}));
					}
					this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
				}
			}
		}

		private void ProcessAdditionalProperties()
		{
			if (this._reader.TokenType == JsonToken.Boolean)
			{
				this.CurrentSchema.AllowAdditionalProperties = (bool)this._reader.Value;
			}
			else
			{
				this.CurrentSchema.AdditionalProperties = this.BuildSchema();
			}
		}

		private void ProcessPatternProperties()
		{
			Dictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected start object token.");
			}
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				string text = Convert.ToString(this._reader.Value, CultureInfo.get_InvariantCulture());
				this._reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						text
					}));
				}
				dictionary.Add(text, this.BuildSchema());
			}
			this.CurrentSchema.PatternProperties = dictionary;
		}

		private void ProcessItems()
		{
			this.CurrentSchema.Items = new List<JsonSchema>();
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType != JsonToken.StartObject)
			{
				if (tokenType != JsonToken.StartArray)
				{
					throw new Exception("Expected array or JSON schema object token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						this._reader.TokenType
					}));
				}
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
				{
					this.CurrentSchema.Items.Add(this.BuildSchema());
				}
			}
			else
			{
				this.CurrentSchema.Items.Add(this.BuildSchema());
			}
		}

		private void ProcessProperties()
		{
			IDictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected StartObject token while parsing schema properties, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					this._reader.TokenType
				}));
			}
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				string text = Convert.ToString(this._reader.Value, CultureInfo.get_InvariantCulture());
				this._reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						text
					}));
				}
				dictionary.Add(text, this.BuildSchema());
			}
			this.CurrentSchema.Properties = dictionary;
		}

		private JsonSchemaType? ProcessType()
		{
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType == JsonToken.StartArray)
			{
				JsonSchemaType? result = new JsonSchemaType?(JsonSchemaType.None);
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
				{
					if (this._reader.TokenType != JsonToken.String)
					{
						throw new Exception("Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							this._reader.TokenType
						}));
					}
					result = ((!result.get_HasValue()) ? default(JsonSchemaType?) : new JsonSchemaType?(result.GetValueOrDefault() | JsonSchemaBuilder.MapType(this._reader.Value.ToString())));
				}
				return result;
			}
			if (tokenType != JsonToken.String)
			{
				throw new Exception("Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					this._reader.TokenType
				}));
			}
			return new JsonSchemaType?(JsonSchemaBuilder.MapType(this._reader.Value.ToString()));
		}

		internal static JsonSchemaType MapType(string type)
		{
			JsonSchemaType result;
			if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, ref result))
			{
				throw new Exception("Invalid JSON schema type: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					type
				}));
			}
			return result;
		}

		internal static string MapType(JsonSchemaType type)
		{
			return Enumerable.Single<KeyValuePair<string, JsonSchemaType>>(JsonSchemaConstants.JsonSchemaTypeMapping, (KeyValuePair<string, JsonSchemaType> kv) => kv.get_Value() == type).get_Key();
		}
	}
}
