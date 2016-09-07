using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json
{
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		private class SchemaScope
		{
			private readonly JTokenType _tokenType;

			private readonly IList<JsonSchemaModel> _schemas;

			private readonly Dictionary<string, bool> _requiredProperties;

			public string CurrentPropertyName
			{
				get;
				set;
			}

			public int ArrayItemCount
			{
				get;
				set;
			}

			public IList<JsonSchemaModel> Schemas
			{
				get
				{
					return this._schemas;
				}
			}

			public Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return this._requiredProperties;
				}
			}

			public JTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
			}

			public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
			{
				this._tokenType = tokenType;
				this._schemas = schemas;
				this._requiredProperties = Enumerable.ToDictionary<string, string, bool>(Enumerable.Distinct<string>(Enumerable.SelectMany<JsonSchemaModel, string>(schemas, new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties))), (string p) => p, (string p) => false);
			}

			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				if (schema == null || schema.Properties == null)
				{
					return Enumerable.Empty<string>();
				}
				return Enumerable.Select<KeyValuePair<string, JsonSchemaModel>, string>(Enumerable.Where<KeyValuePair<string, JsonSchemaModel>>(schema.Properties, (KeyValuePair<string, JsonSchemaModel> p) => p.get_Value().Required), (KeyValuePair<string, JsonSchemaModel> p) => p.get_Key());
			}
		}

		private readonly JsonReader _reader;

		private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

		private JsonSchema _schema;

		private JsonSchemaModel _model;

		private JsonValidatingReader.SchemaScope _currentScope;

		public event ValidationEventHandler ValidationEventHandler;

		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				return (jsonLineInfo == null) ? 0 : jsonLineInfo.LineNumber;
			}
		}

		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				return (jsonLineInfo == null) ? 0 : jsonLineInfo.LinePosition;
			}
		}

		public override object Value
		{
			get
			{
				return this._reader.Value;
			}
		}

		public override int Depth
		{
			get
			{
				return this._reader.Depth;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this._reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		public override JsonToken TokenType
		{
			get
			{
				return this._reader.TokenType;
			}
		}

		public override Type ValueType
		{
			get
			{
				return this._reader.ValueType;
			}
		}

		private IEnumerable<JsonSchemaModel> CurrentSchemas
		{
			get
			{
				return this._currentScope.Schemas;
			}
		}

		private IEnumerable<JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				if (this._currentScope == null)
				{
					return new List<JsonSchemaModel>(new JsonSchemaModel[]
					{
						this._model
					});
				}
				if (this._currentScope.Schemas == null || this._currentScope.Schemas.get_Count() == 0)
				{
					return Enumerable.Empty<JsonSchemaModel>();
				}
				switch (this._currentScope.TokenType)
				{
				case JTokenType.None:
					return this._currentScope.Schemas;
				case JTokenType.Object:
				{
					if (this._currentScope.CurrentPropertyName == null)
					{
						throw new Exception("CurrentPropertyName has not been set on scope.");
					}
					IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
					using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							JsonSchemaModel current = enumerator.get_Current();
							JsonSchemaModel jsonSchemaModel;
							if (current.Properties != null && current.Properties.TryGetValue(this._currentScope.CurrentPropertyName, ref jsonSchemaModel))
							{
								list.Add(jsonSchemaModel);
							}
							if (current.PatternProperties != null)
							{
								using (IEnumerator<KeyValuePair<string, JsonSchemaModel>> enumerator2 = current.PatternProperties.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										KeyValuePair<string, JsonSchemaModel> current2 = enumerator2.get_Current();
										if (Regex.IsMatch(this._currentScope.CurrentPropertyName, current2.get_Key()))
										{
											list.Add(current2.get_Value());
										}
									}
								}
							}
							if (list.get_Count() == 0 && current.AllowAdditionalProperties && current.AdditionalProperties != null)
							{
								list.Add(current.AdditionalProperties);
							}
						}
					}
					return list;
				}
				case JTokenType.Array:
				{
					IList<JsonSchemaModel> list2 = new List<JsonSchemaModel>();
					using (IEnumerator<JsonSchemaModel> enumerator3 = this.CurrentSchemas.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							JsonSchemaModel current3 = enumerator3.get_Current();
							if (!CollectionUtils.IsNullOrEmpty<JsonSchemaModel>(current3.Items))
							{
								if (current3.Items.get_Count() == 1)
								{
									list2.Add(current3.Items.get_Item(0));
								}
								if (current3.Items.get_Count() > this._currentScope.ArrayItemCount - 1)
								{
									list2.Add(current3.Items.get_Item(this._currentScope.ArrayItemCount - 1));
								}
							}
							if (current3.AllowAdditionalProperties && current3.AdditionalProperties != null)
							{
								list2.Add(current3.AdditionalProperties);
							}
						}
					}
					return list2;
				}
				case JTokenType.Constructor:
					return Enumerable.Empty<JsonSchemaModel>();
				default:
					throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						this._currentScope.TokenType
					}));
				}
			}
		}

		public JsonSchema Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				if (this.TokenType != JsonToken.None)
				{
					throw new Exception("Cannot change schema while validating JSON.");
				}
				this._schema = value;
				this._model = null;
			}
		}

		public JsonReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		public JsonValidatingReader(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new Stack<JsonValidatingReader.SchemaScope>();
		}

		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		private void Push(JsonValidatingReader.SchemaScope scope)
		{
			this._stack.Push(scope);
			this._currentScope = scope;
		}

		private JsonValidatingReader.SchemaScope Pop()
		{
			JsonValidatingReader.SchemaScope result = this._stack.Pop();
			this._currentScope = ((this._stack.get_Count() == 0) ? null : this._stack.Peek());
			return result;
		}

		private void RaiseError(string message, JsonSchemaModel schema)
		{
			string message2 = (!((IJsonLineInfo)this).HasLineInfo()) ? message : (message + " Line {0}, position {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				((IJsonLineInfo)this).LineNumber,
				((IJsonLineInfo)this).LinePosition
			}));
			this.OnValidationEvent(new JsonSchemaException(message2, null, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
		}

		private void OnValidationEvent(JsonSchemaException exception)
		{
			ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler != null)
			{
				validationEventHandler(this, new ValidationEventArgs(exception));
				return;
			}
			throw exception;
		}

		private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			JToken jToken = new JValue(this._reader.Value);
			if (schema.Enum != null)
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.get_InvariantCulture());
				jToken.WriteTo(new JsonTextWriter(stringWriter), new JsonConverter[0]);
				if (!schema.Enum.ContainsValue(jToken, new JTokenEqualityComparer()))
				{
					this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						stringWriter.ToString()
					}), schema);
				}
			}
			JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
			if (currentNodeSchemaType.get_HasValue() && JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.get_Value()))
			{
				this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					currentNodeSchemaType
				}), schema);
			}
		}

		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				return new JsonSchemaType?(JsonSchemaType.Object);
			case JsonToken.StartArray:
				return new JsonSchemaType?(JsonSchemaType.Array);
			case JsonToken.Integer:
				return new JsonSchemaType?(JsonSchemaType.Integer);
			case JsonToken.Float:
				return new JsonSchemaType?(JsonSchemaType.Float);
			case JsonToken.String:
				return new JsonSchemaType?(JsonSchemaType.String);
			case JsonToken.Boolean:
				return new JsonSchemaType?(JsonSchemaType.Boolean);
			case JsonToken.Null:
				return new JsonSchemaType?(JsonSchemaType.Null);
			}
			return default(JsonSchemaType?);
		}

		public override byte[] ReadAsBytes()
		{
			byte[] result = this._reader.ReadAsBytes();
			this.ValidateCurrentToken();
			return result;
		}

		public override decimal? ReadAsDecimal()
		{
			decimal? result = this._reader.ReadAsDecimal();
			this.ValidateCurrentToken();
			return result;
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = this._reader.ReadAsDateTimeOffset();
			this.ValidateCurrentToken();
			return result;
		}

		public override bool Read()
		{
			if (!this._reader.Read())
			{
				return false;
			}
			if (this._reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			this.ValidateCurrentToken();
			return true;
		}

		private void ValidateCurrentToken()
		{
			if (this._model == null)
			{
				JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
				this._model = jsonSchemaModelBuilder.Build(this._schema);
			}
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> schemas = Enumerable.ToList<JsonSchemaModel>(Enumerable.Where<JsonSchemaModel>(this.CurrentMemberSchemas, new Func<JsonSchemaModel, bool>(this.ValidateObject)));
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, schemas));
				return;
			}
			case JsonToken.StartArray:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> schemas2 = Enumerable.ToList<JsonSchemaModel>(Enumerable.Where<JsonSchemaModel>(this.CurrentMemberSchemas, new Func<JsonSchemaModel, bool>(this.ValidateArray)));
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, schemas2));
				return;
			}
			case JsonToken.StartConstructor:
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
				return;
			case JsonToken.PropertyName:
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel current = enumerator.get_Current();
						this.ValidatePropertyName(current);
					}
				}
				return;
			case JsonToken.Raw:
				return;
			case JsonToken.Integer:
				this.ProcessValue();
				using (IEnumerator<JsonSchemaModel> enumerator2 = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						JsonSchemaModel current2 = enumerator2.get_Current();
						this.ValidateInteger(current2);
					}
				}
				return;
			case JsonToken.Float:
				this.ProcessValue();
				using (IEnumerator<JsonSchemaModel> enumerator3 = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						JsonSchemaModel current3 = enumerator3.get_Current();
						this.ValidateFloat(current3);
					}
				}
				return;
			case JsonToken.String:
				this.ProcessValue();
				using (IEnumerator<JsonSchemaModel> enumerator4 = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						JsonSchemaModel current4 = enumerator4.get_Current();
						this.ValidateString(current4);
					}
				}
				return;
			case JsonToken.Boolean:
				this.ProcessValue();
				using (IEnumerator<JsonSchemaModel> enumerator5 = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						JsonSchemaModel current5 = enumerator5.get_Current();
						this.ValidateBoolean(current5);
					}
				}
				return;
			case JsonToken.Null:
				this.ProcessValue();
				using (IEnumerator<JsonSchemaModel> enumerator6 = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						JsonSchemaModel current6 = enumerator6.get_Current();
						this.ValidateNull(current6);
					}
				}
				return;
			case JsonToken.Undefined:
				return;
			case JsonToken.EndObject:
				using (IEnumerator<JsonSchemaModel> enumerator7 = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						JsonSchemaModel current7 = enumerator7.get_Current();
						this.ValidateEndObject(current7);
					}
				}
				this.Pop();
				return;
			case JsonToken.EndArray:
				using (IEnumerator<JsonSchemaModel> enumerator8 = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator8.MoveNext())
					{
						JsonSchemaModel current8 = enumerator8.get_Current();
						this.ValidateEndArray(current8);
					}
				}
				this.Pop();
				return;
			case JsonToken.EndConstructor:
				this.Pop();
				return;
			case JsonToken.Date:
				return;
			}
			throw new ArgumentOutOfRangeException();
		}

		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				List<string> list = Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, bool>, string>(Enumerable.Where<KeyValuePair<string, bool>>(requiredProperties, (KeyValuePair<string, bool> kv) => !kv.get_Value()), (KeyValuePair<string, bool> kv) => kv.get_Key()));
				if (list.get_Count() > 0)
				{
					this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						string.Join(", ", list.ToArray())
					}), schema);
				}
			}
		}

		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			int arrayItemCount = this._currentScope.ArrayItemCount;
			if (schema.MaximumItems.get_HasValue())
			{
				int? maximumItems = schema.MaximumItems;
				if (maximumItems.get_HasValue() && arrayItemCount > maximumItems.get_Value())
				{
					this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						arrayItemCount,
						schema.MaximumItems
					}), schema);
				}
			}
			if (schema.MinimumItems.get_HasValue())
			{
				int? minimumItems = schema.MinimumItems;
				if (minimumItems.get_HasValue() && arrayItemCount < minimumItems.get_Value())
				{
					this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						arrayItemCount,
						schema.MinimumItems
					}), schema);
				}
			}
		}

		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Null))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
		}

		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Boolean))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
		}

		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			string text = this._reader.Value.ToString();
			if (schema.MaximumLength.get_HasValue())
			{
				int? maximumLength = schema.MaximumLength;
				if (maximumLength.get_HasValue() && text.get_Length() > maximumLength.get_Value())
				{
					this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						text,
						schema.MaximumLength
					}), schema);
				}
			}
			if (schema.MinimumLength.get_HasValue())
			{
				int? minimumLength = schema.MinimumLength;
				if (minimumLength.get_HasValue() && text.get_Length() < minimumLength.get_Value())
				{
					this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						text,
						schema.MinimumLength
					}), schema);
				}
			}
			if (schema.Patterns != null)
			{
				using (IEnumerator<string> enumerator = schema.Patterns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						if (!Regex.IsMatch(text, current))
						{
							this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
							{
								text,
								current
							}), schema);
						}
					}
				}
			}
		}

		private void ValidateInteger(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			long num = Convert.ToInt64(this._reader.Value, CultureInfo.get_InvariantCulture());
			if (schema.Maximum.get_HasValue())
			{
				double? maximum = schema.Maximum;
				if (maximum.get_HasValue() && (double)num > maximum.get_Value())
				{
					this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						num,
						schema.Maximum
					}), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					double arg_B9_0 = (double)num;
					double? maximum2 = schema.Maximum;
					if (arg_B9_0 == maximum2.GetValueOrDefault() && maximum2.get_HasValue())
					{
						this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							num,
							schema.Maximum
						}), schema);
					}
				}
			}
			if (schema.Minimum.get_HasValue())
			{
				double? minimum = schema.Minimum;
				if (minimum.get_HasValue() && (double)num < minimum.get_Value())
				{
					this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						num,
						schema.Minimum
					}), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					double arg_187_0 = (double)num;
					double? minimum2 = schema.Minimum;
					if (arg_187_0 == minimum2.GetValueOrDefault() && minimum2.get_HasValue())
					{
						this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							num,
							schema.Minimum
						}), schema);
					}
				}
			}
			if (schema.DivisibleBy.get_HasValue() && !JsonValidatingReader.IsZero((double)num % schema.DivisibleBy.get_Value()))
			{
				this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					JsonConvert.ToString(num),
					schema.DivisibleBy
				}), schema);
			}
		}

		private void ProcessValue()
		{
			if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
			{
				this._currentScope.ArrayItemCount++;
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel current = enumerator.get_Current();
						if (current != null && current.Items != null && current.Items.get_Count() > 1 && this._currentScope.ArrayItemCount >= current.Items.get_Count())
						{
							this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
							{
								this._currentScope.ArrayItemCount
							}), current);
						}
					}
				}
			}
		}

		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			double num = Convert.ToDouble(this._reader.Value, CultureInfo.get_InvariantCulture());
			if (schema.Maximum.get_HasValue())
			{
				double? maximum = schema.Maximum;
				if (maximum.get_HasValue() && num > maximum.get_Value())
				{
					this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						JsonConvert.ToString(num),
						schema.Maximum
					}), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					double arg_B7_0 = num;
					double? maximum2 = schema.Maximum;
					if (arg_B7_0 == maximum2.GetValueOrDefault() && maximum2.get_HasValue())
					{
						this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							JsonConvert.ToString(num),
							schema.Maximum
						}), schema);
					}
				}
			}
			if (schema.Minimum.get_HasValue())
			{
				double? minimum = schema.Minimum;
				if (minimum.get_HasValue() && num < minimum.get_Value())
				{
					this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						JsonConvert.ToString(num),
						schema.Minimum
					}), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					double arg_183_0 = num;
					double? minimum2 = schema.Minimum;
					if (arg_183_0 == minimum2.GetValueOrDefault() && minimum2.get_HasValue())
					{
						this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							JsonConvert.ToString(num),
							schema.Minimum
						}), schema);
					}
				}
			}
			if (schema.DivisibleBy.get_HasValue() && !JsonValidatingReader.IsZero(num % schema.DivisibleBy.get_Value()))
			{
				this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					JsonConvert.ToString(num),
					schema.DivisibleBy
				}), schema);
			}
		}

		private static bool IsZero(double value)
		{
			double num = 2.2204460492503131E-16;
			return Math.Abs(value) < 10.0 * num;
		}

		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.get_InvariantCulture());
			if (this._currentScope.RequiredProperties.ContainsKey(text))
			{
				this._currentScope.RequiredProperties.set_Item(text, true);
			}
			if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, text))
			{
				this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					text
				}), schema);
			}
			this._currentScope.CurrentPropertyName = text;
		}

		private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
		{
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				using (IEnumerator<string> enumerator = schema.PatternProperties.get_Keys().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						if (Regex.IsMatch(propertyName, current))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		private bool ValidateArray(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Array);
		}

		private bool ValidateObject(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Object);
		}

		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (!JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
			{
				this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					currentSchema.Type,
					currentType
				}), currentSchema);
				return false;
			}
			return true;
		}
	}
}
