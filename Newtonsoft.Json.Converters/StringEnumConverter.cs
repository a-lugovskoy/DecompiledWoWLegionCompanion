using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Converters
{
	public class StringEnumConverter : JsonConverter
	{
		private readonly Dictionary<Type, BidirectionalDictionary<string, string>> _enumMemberNamesPerType = new Dictionary<Type, BidirectionalDictionary<string, string>>();

		public bool CamelCaseText
		{
			get;
			set;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			string text = @enum.ToString("G");
			if (char.IsNumber(text.get_Chars(0)) || text.get_Chars(0) == '-')
			{
				writer.WriteValue(value);
			}
			else
			{
				BidirectionalDictionary<string, string> enumNameMap = this.GetEnumNameMap(@enum.GetType());
				string text2;
				enumNameMap.TryGetByFirst(text, out text2);
				text2 = (text2 ?? text);
				if (this.CamelCaseText)
				{
					text2 = StringUtils.ToCamelCase(text2);
				}
				writer.WriteValue(text2);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type type = (!ReflectionUtils.IsNullableType(objectType)) ? objectType : Nullable.GetUnderlyingType(objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullableType(objectType))
				{
					throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						objectType
					}));
				}
				return null;
			}
			else
			{
				if (reader.TokenType == JsonToken.String)
				{
					BidirectionalDictionary<string, string> enumNameMap = this.GetEnumNameMap(type);
					string text;
					enumNameMap.TryGetBySecond(reader.Value.ToString(), out text);
					text = (text ?? reader.Value.ToString());
					return Enum.Parse(type, text, true);
				}
				if (reader.TokenType == JsonToken.Integer)
				{
					return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.get_InvariantCulture(), type);
				}
				throw new Exception("Unexpected token when parsing enum. Expected String or Integer, got {0}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					reader.TokenType
				}));
			}
		}

		private BidirectionalDictionary<string, string> GetEnumNameMap(Type t)
		{
			BidirectionalDictionary<string, string> bidirectionalDictionary;
			if (!this._enumMemberNamesPerType.TryGetValue(t, ref bidirectionalDictionary))
			{
				Dictionary<Type, BidirectionalDictionary<string, string>> enumMemberNamesPerType = this._enumMemberNamesPerType;
				lock (enumMemberNamesPerType)
				{
					if (this._enumMemberNamesPerType.TryGetValue(t, ref bidirectionalDictionary))
					{
						return bidirectionalDictionary;
					}
					bidirectionalDictionary = new BidirectionalDictionary<string, string>(StringComparer.get_OrdinalIgnoreCase(), StringComparer.get_OrdinalIgnoreCase());
					FieldInfo[] fields = t.GetFields();
					for (int i = 0; i < fields.Length; i++)
					{
						FieldInfo fieldInfo = fields[i];
						string name = fieldInfo.get_Name();
						string text = Enumerable.SingleOrDefault<string>(Enumerable.Select<EnumMemberAttribute, string>(Enumerable.Cast<EnumMemberAttribute>(fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), true)), (EnumMemberAttribute a) => a.Value)) ?? fieldInfo.get_Name();
						string text2;
						if (bidirectionalDictionary.TryGetBySecond(text, out text2))
						{
							throw new Exception("Enum name '{0}' already exists on enum '{1}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
							{
								text,
								t.get_Name()
							}));
						}
						bidirectionalDictionary.Add(name, text);
					}
					this._enumMemberNamesPerType.set_Item(t, bidirectionalDictionary);
				}
				return bidirectionalDictionary;
			}
			return bidirectionalDictionary;
		}

		public override bool CanConvert(Type objectType)
		{
			Type type = (!ReflectionUtils.IsNullableType(objectType)) ? objectType : Nullable.GetUnderlyingType(objectType);
			return type.get_IsEnum();
		}
	}
}
