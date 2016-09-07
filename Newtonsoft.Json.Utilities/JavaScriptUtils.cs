using System;
using System.IO;

namespace Newtonsoft.Json.Utilities
{
	internal static class JavaScriptUtils
	{
		public static void WriteEscapedJavaScriptString(TextWriter writer, string value, char delimiter, bool appendDelimiters)
		{
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
			if (value != null)
			{
				int num = 0;
				int num2 = 0;
				char[] array = null;
				for (int i = 0; i < value.get_Length(); i++)
				{
					char c = value.get_Chars(i);
					char c2 = c;
					string text;
					switch (c2)
					{
					case '\b':
						text = "\\b";
						goto IL_14F;
					case '\t':
						text = "\\t";
						goto IL_14F;
					case '\n':
						text = "\\n";
						goto IL_14F;
					case '\v':
						IL_4E:
						if (c2 == '\u2028')
						{
							text = "\\u2028";
							goto IL_14F;
						}
						if (c2 == '\u2029')
						{
							text = "\\u2029";
							goto IL_14F;
						}
						if (c2 == '"')
						{
							text = ((delimiter != '"') ? null : "\\\"");
							goto IL_14F;
						}
						if (c2 == '\'')
						{
							text = ((delimiter != '\'') ? null : "\\'");
							goto IL_14F;
						}
						if (c2 == '\\')
						{
							text = "\\\\";
							goto IL_14F;
						}
						if (c2 != '\u0085')
						{
							text = ((c > '\u001f') ? null : StringUtils.ToCharAsUnicode(c));
							goto IL_14F;
						}
						text = "\\u0085";
						goto IL_14F;
					case '\f':
						text = "\\f";
						goto IL_14F;
					case '\r':
						text = "\\r";
						goto IL_14F;
					}
					goto IL_4E;
					IL_14F:
					if (text != null)
					{
						if (array == null)
						{
							array = value.ToCharArray();
						}
						if (num2 > 0)
						{
							writer.Write(array, num, num2);
							num2 = 0;
						}
						writer.Write(text);
						num = i + 1;
					}
					else
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					if (num == 0)
					{
						writer.Write(value);
					}
					else
					{
						writer.Write(array, num, num2);
					}
				}
			}
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
		}

		public static string ToEscapedJavaScriptString(string value)
		{
			return JavaScriptUtils.ToEscapedJavaScriptString(value, '"', true);
		}

		public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
		{
			int? length = StringUtils.GetLength(value);
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter((!length.get_HasValue()) ? 16 : length.get_Value()))
			{
				JavaScriptUtils.WriteEscapedJavaScriptString(stringWriter, value, delimiter, appendDelimiters);
				result = stringWriter.ToString();
			}
			return result;
		}
	}
}
