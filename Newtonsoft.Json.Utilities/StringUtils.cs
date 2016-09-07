using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	internal static class StringUtils
	{
		private delegate void ActionLine(TextWriter textWriter, string line);

		public const string CarriageReturnLineFeed = "\r\n";

		public const string Empty = "";

		public const char CarriageReturn = '\r';

		public const char LineFeed = '\n';

		public const char Tab = '\t';

		public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		public static bool ContainsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			for (int i = 0; i < s.get_Length(); i++)
			{
				if (char.IsWhiteSpace(s.get_Chars(i)))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.get_Length() == 0)
			{
				return false;
			}
			for (int i = 0; i < s.get_Length(); i++)
			{
				if (!char.IsWhiteSpace(s.get_Chars(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static string EnsureEndsWith(string target, string value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (target.get_Length() >= value.get_Length())
			{
				if (string.Compare(target, target.get_Length() - value.get_Length(), value, 0, value.get_Length(), 5) == 0)
				{
					return target;
				}
				string text = target.TrimEnd(null);
				if (string.Compare(text, text.get_Length() - value.get_Length(), value, 0, value.get_Length(), 5) == 0)
				{
					return target;
				}
			}
			return target + value;
		}

		public static bool IsNullOrEmptyOrWhiteSpace(string s)
		{
			return string.IsNullOrEmpty(s) || StringUtils.IsWhiteSpace(s);
		}

		public static void IfNotNullOrEmpty(string value, Action<string> action)
		{
			StringUtils.IfNotNullOrEmpty(value, action, null);
		}

		private static void IfNotNullOrEmpty(string value, Action<string> trueAction, Action<string> falseAction)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (trueAction != null)
				{
					trueAction.Invoke(value);
				}
			}
			else if (falseAction != null)
			{
				falseAction.Invoke(value);
			}
		}

		public static string Indent(string s, int indentation)
		{
			return StringUtils.Indent(s, indentation, ' ');
		}

		public static string Indent(string s, int indentation, char indentChar)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (indentation <= 0)
			{
				throw new ArgumentException("Must be greater than zero.", "indentation");
			}
			StringReader textReader = new StringReader(s);
			StringWriter stringWriter = new StringWriter(CultureInfo.get_InvariantCulture());
			StringUtils.ActionTextReaderLine(textReader, stringWriter, delegate(TextWriter tw, string line)
			{
				tw.Write(new string(indentChar, indentation));
				tw.Write(line);
			});
			return stringWriter.ToString();
		}

		private static void ActionTextReaderLine(TextReader textReader, TextWriter textWriter, StringUtils.ActionLine lineAction)
		{
			bool flag = true;
			string line;
			while ((line = textReader.ReadLine()) != null)
			{
				if (!flag)
				{
					textWriter.WriteLine();
				}
				else
				{
					flag = false;
				}
				lineAction(textWriter, line);
			}
		}

		public static string NumberLines(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			StringReader textReader = new StringReader(s);
			StringWriter stringWriter = new StringWriter(CultureInfo.get_InvariantCulture());
			int lineNumber = 1;
			StringUtils.ActionTextReaderLine(textReader, stringWriter, delegate(TextWriter tw, string line)
			{
				tw.Write(lineNumber.ToString(CultureInfo.get_InvariantCulture()).PadLeft(4));
				tw.Write(". ");
				tw.Write(line);
				lineNumber++;
			});
			return stringWriter.ToString();
		}

		public static string NullEmptyString(string s)
		{
			return (!string.IsNullOrEmpty(s)) ? s : null;
		}

		public static string ReplaceNewLines(string s, string replacement)
		{
			StringReader stringReader = new StringReader(s);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(replacement);
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		public static string Truncate(string s, int maximumLength)
		{
			return StringUtils.Truncate(s, maximumLength, "...");
		}

		public static string Truncate(string s, int maximumLength, string suffix)
		{
			if (suffix == null)
			{
				throw new ArgumentNullException("suffix");
			}
			if (maximumLength <= 0)
			{
				throw new ArgumentException("Maximum length must be greater than zero.", "maximumLength");
			}
			int num = maximumLength - suffix.get_Length();
			if (num <= 0)
			{
				throw new ArgumentException("Length of suffix string is greater or equal to maximumLength");
			}
			if (s != null && s.get_Length() > maximumLength)
			{
				string text = s.Substring(0, num);
				text = text.Trim();
				return text + suffix;
			}
			return s;
		}

		public static StringWriter CreateStringWriter(int capacity)
		{
			StringBuilder stringBuilder = new StringBuilder(capacity);
			return new StringWriter(stringBuilder, CultureInfo.get_InvariantCulture());
		}

		public static int? GetLength(string value)
		{
			if (value == null)
			{
				return default(int?);
			}
			return new int?(value.get_Length());
		}

		public static string ToCharAsUnicode(char c)
		{
			char c2 = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			char c3 = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			char c4 = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			char c5 = MathUtils.IntToHex((int)(c & '\u000f'));
			return new string(new char[]
			{
				'\\',
				'u',
				c2,
				c3,
				c4,
				c5
			});
		}

		public static void WriteCharAsUnicode(TextWriter writer, char c)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			char c2 = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			char c3 = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			char c4 = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			char c5 = MathUtils.IntToHex((int)(c & '\u000f'));
			writer.Write('\\');
			writer.Write('u');
			writer.Write(c2);
			writer.Write(c3);
			writer.Write(c4);
			writer.Write(c5);
		}

		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			TSource[] array = Enumerable.ToArray<TSource>(Enumerable.Where<TSource>(source, (TSource s) => string.Compare(valueSelector.Invoke(s), testValue, 5) == 0));
			int num = array.Length;
			if (num <= 1)
			{
				return (num != 1) ? default(TSource) : array[0];
			}
			IEnumerable<TSource> enumerable = Enumerable.Where<TSource>(source, (TSource s) => string.Compare(valueSelector.Invoke(s), testValue, 4) == 0);
			return Enumerable.SingleOrDefault<TSource>(enumerable);
		}

		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			if (!char.IsUpper(s.get_Chars(0)))
			{
				return s;
			}
			string text = char.ToLower(s.get_Chars(0), CultureInfo.get_InvariantCulture()).ToString(CultureInfo.get_InvariantCulture());
			if (s.get_Length() > 1)
			{
				text += s.Substring(1);
			}
			return text;
		}
	}
}
