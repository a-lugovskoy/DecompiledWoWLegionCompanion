using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
	public static class LinqExtensions
	{
		public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T j) => j.Ancestors()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T j) => j.Descendants()).AsJEnumerable();
		}

		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<JObject, JProperty>(source, (JObject d) => d.Properties()).AsJEnumerable<JProperty>();
		}

		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
		{
			return source.Values(key).AsJEnumerable();
		}

		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
		{
			return source.Values(key);
		}

		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		public static U Value<U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "source");
			JToken jToken = value as JToken;
			if (jToken == null)
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return jToken.Convert<JToken, U>();
		}

		[DebuggerHidden]
		internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
		{
			bool flag = false;
			ValidationUtils.ArgumentNotNull(source, "source");
			IEnumerator<T> enumerator = source.GetEnumerator();
			while (enumerator.MoveNext())
			{
				JToken jToken = enumerator.get_Current();
				if (key == null)
				{
					if (jToken is JValue)
					{
						yield return ((JValue)jToken).Convert<JValue, U>();
					}
					IEnumerator<JToken> enumerator2 = jToken.Children().GetEnumerator();
					try
					{
						uint num;
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							JToken current = enumerator2.get_Current();
							U u = current.Convert<JToken, U>();
							flag = true;
							return;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					continue;
				}
				JToken jToken2 = jToken[key];
				if (jToken2 != null)
				{
					yield return jToken2.Convert<JToken, U>();
				}
			}
			yield break;
		}

		public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T c) => c.Children()).Convert<JToken, U>();
		}

		[DebuggerHidden]
		internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			IEnumerator<T> enumerator = source.GetEnumerator();
			while (enumerator.MoveNext())
			{
				JToken token = enumerator.get_Current();
				yield return token.Convert<JToken, U>();
			}
			yield break;
		}

		internal static U Convert<T, U>(this T token) where T : JToken
		{
			if (token == null)
			{
				return default(U);
			}
			if (token is U && typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
			{
				return (U)((object)token);
			}
			JValue jValue = token as JValue;
			if (jValue == null)
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					token.GetType(),
					typeof(T)
				}));
			}
			if (jValue.Value is U)
			{
				return (U)((object)jValue.Value);
			}
			Type type = typeof(U);
			if (ReflectionUtils.IsNullableType(type))
			{
				if (jValue.Value == null)
				{
					return default(U);
				}
				type = Nullable.GetUnderlyingType(type);
			}
			return (U)((object)System.Convert.ChangeType(jValue.Value, type, CultureInfo.get_InvariantCulture()));
		}

		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			if (source is IJEnumerable<T>)
			{
				return (IJEnumerable<T>)source;
			}
			return new JEnumerable<T>(source);
		}
	}
}
