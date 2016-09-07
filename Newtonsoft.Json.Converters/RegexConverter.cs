using Newtonsoft.Json.Bson;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Converters
{
	public class RegexConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Regex regex = (Regex)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			if (bsonWriter != null)
			{
				this.WriteBson(bsonWriter, regex);
			}
			else
			{
				this.WriteJson(writer, regex);
			}
		}

		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string text = null;
			if (this.HasFlag(regex.get_Options(), 1))
			{
				text += "i";
			}
			if (this.HasFlag(regex.get_Options(), 2))
			{
				text += "m";
			}
			if (this.HasFlag(regex.get_Options(), 16))
			{
				text += "s";
			}
			text += "u";
			if (this.HasFlag(regex.get_Options(), 4))
			{
				text += "x";
			}
			writer.WriteRegex(regex.ToString(), text);
		}

		private void WriteJson(JsonWriter writer, Regex regex)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName("Options");
			writer.WriteValue(regex.get_Options());
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			BsonReader bsonReader = reader as BsonReader;
			if (bsonReader != null)
			{
				return this.ReadBson(bsonReader);
			}
			return this.ReadJson(reader);
		}

		private object ReadBson(BsonReader reader)
		{
			string text = (string)reader.Value;
			int num = text.LastIndexOf("/");
			string text2 = text.Substring(1, num - 1);
			string text3 = text.Substring(num + 1);
			RegexOptions regexOptions = 0;
			string text4 = text3;
			for (int i = 0; i < text4.get_Length(); i++)
			{
				char c = text4.get_Chars(i);
				char c2 = c;
				if (c2 != 'i')
				{
					if (c2 != 'm')
					{
						if (c2 != 's')
						{
							if (c2 == 'x')
							{
								regexOptions |= 4;
							}
						}
						else
						{
							regexOptions |= 16;
						}
					}
					else
					{
						regexOptions |= 2;
					}
				}
				else
				{
					regexOptions |= 1;
				}
			}
			return new Regex(text2, regexOptions);
		}

		private Regex ReadJson(JsonReader reader)
		{
			reader.Read();
			reader.Read();
			string text = (string)reader.Value;
			reader.Read();
			reader.Read();
			int num = Convert.ToInt32(reader.Value, CultureInfo.get_InvariantCulture());
			reader.Read();
			return new Regex(text, num);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Regex);
		}
	}
}
