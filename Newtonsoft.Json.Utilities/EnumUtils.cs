using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	internal static class EnumUtils
	{
		public static T Parse<T>(string enumMemberName) where T : struct
		{
			return EnumUtils.Parse<T>(enumMemberName, false);
		}

		public static T Parse<T>(string enumMemberName, bool ignoreCase) where T : struct
		{
			ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
			return (T)((object)Enum.Parse(typeof(T), enumMemberName, ignoreCase));
		}

		public static bool TryParse<T>(string enumMemberName, bool ignoreCase, out T value) where T : struct
		{
			ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
			return MiscellaneousUtils.TryAction<T>(() => EnumUtils.Parse<T>(enumMemberName, ignoreCase), out value);
		}

		public static IList<T> GetFlagsValues<T>(T value) where T : struct
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsDefined(typeof(FlagsAttribute), false))
			{
				throw new Exception("Enum type {0} is not a set of flags.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					typeFromHandle
				}));
			}
			Type underlyingType = Enum.GetUnderlyingType(value.GetType());
			ulong num = Convert.ToUInt64(value, CultureInfo.get_InvariantCulture());
			EnumValues<ulong> namesAndValues = EnumUtils.GetNamesAndValues<T>();
			IList<T> list = new List<T>();
			using (IEnumerator<EnumValue<ulong>> enumerator = namesAndValues.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EnumValue<ulong> current = enumerator.get_Current();
					if ((num & current.Value) == current.Value && current.Value != 0uL)
					{
						list.Add((T)((object)Convert.ChangeType(current.Value, underlyingType, CultureInfo.get_CurrentCulture())));
					}
				}
			}
			if (list.get_Count() == 0 && Enumerable.SingleOrDefault<EnumValue<ulong>>(namesAndValues, (EnumValue<ulong> v) => v.Value == 0uL) != null)
			{
				list.Add(default(T));
			}
			return list;
		}

		public static EnumValues<ulong> GetNamesAndValues<T>() where T : struct
		{
			return EnumUtils.GetNamesAndValues<ulong>(typeof(T));
		}

		public static EnumValues<TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>() where TEnum : struct where TUnderlyingType : struct
		{
			return EnumUtils.GetNamesAndValues<TUnderlyingType>(typeof(TEnum));
		}

		public static EnumValues<TUnderlyingType> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			ValidationUtils.ArgumentTypeIsEnum(enumType, "enumType");
			IList<object> values = EnumUtils.GetValues(enumType);
			IList<string> names = EnumUtils.GetNames(enumType);
			EnumValues<TUnderlyingType> enumValues = new EnumValues<TUnderlyingType>();
			for (int i = 0; i < values.get_Count(); i++)
			{
				try
				{
					enumValues.Add(new EnumValue<TUnderlyingType>(names.get_Item(i), (TUnderlyingType)((object)Convert.ChangeType(values.get_Item(i), typeof(TUnderlyingType), CultureInfo.get_CurrentCulture()))));
				}
				catch (OverflowException ex)
				{
					throw new Exception(string.Format(CultureInfo.get_InvariantCulture(), "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", new object[]
					{
						Enum.GetUnderlyingType(enumType),
						typeof(TUnderlyingType),
						Convert.ToUInt64(values.get_Item(i), CultureInfo.get_InvariantCulture())
					}), ex);
				}
			}
			return enumValues;
		}

		public static IList<T> GetValues<T>()
		{
			return Enumerable.ToList<T>(Enumerable.Cast<T>(EnumUtils.GetValues(typeof(T))));
		}

		public static IList<object> GetValues(Type enumType)
		{
			if (!enumType.get_IsEnum())
			{
				throw new ArgumentException("Type '" + enumType.get_Name() + "' is not an enum.");
			}
			List<object> list = new List<object>();
			IEnumerable<FieldInfo> enumerable = Enumerable.Where<FieldInfo>(enumType.GetFields(), (FieldInfo field) => field.get_IsLiteral());
			using (IEnumerator<FieldInfo> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FieldInfo current = enumerator.get_Current();
					object value = current.GetValue(enumType);
					list.Add(value);
				}
			}
			return list;
		}

		public static IList<string> GetNames<T>()
		{
			return EnumUtils.GetNames(typeof(T));
		}

		public static IList<string> GetNames(Type enumType)
		{
			if (!enumType.get_IsEnum())
			{
				throw new ArgumentException("Type '" + enumType.get_Name() + "' is not an enum.");
			}
			List<string> list = new List<string>();
			IEnumerable<FieldInfo> enumerable = Enumerable.Where<FieldInfo>(enumType.GetFields(), (FieldInfo field) => field.get_IsLiteral());
			using (IEnumerator<FieldInfo> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FieldInfo current = enumerator.get_Current();
					list.Add(current.get_Name());
				}
			}
			return list;
		}

		public static TEnumType GetMaximumValue<TEnumType>(Type enumType) where TEnumType : IConvertible, IComparable<TEnumType>
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (!typeof(TEnumType).IsAssignableFrom(underlyingType))
			{
				throw new ArgumentException(string.Format(CultureInfo.get_InvariantCulture(), "TEnumType is not assignable from the enum's underlying type of {0}.", new object[]
				{
					underlyingType.get_Name()
				}));
			}
			ulong num = 0uL;
			IList<object> values = EnumUtils.GetValues(enumType);
			if (enumType.IsDefined(typeof(FlagsAttribute), false))
			{
				using (IEnumerator<object> enumerator = values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TEnumType tEnumType = (TEnumType)((object)enumerator.get_Current());
						num |= tEnumType.ToUInt64(CultureInfo.get_InvariantCulture());
					}
				}
			}
			else
			{
				using (IEnumerator<object> enumerator2 = values.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TEnumType tEnumType2 = (TEnumType)((object)enumerator2.get_Current());
						ulong num2 = tEnumType2.ToUInt64(CultureInfo.get_InvariantCulture());
						if (num.CompareTo(num2) == -1)
						{
							num = num2;
						}
					}
				}
			}
			return (TEnumType)((object)Convert.ChangeType(num, typeof(TEnumType), CultureInfo.get_InvariantCulture()));
		}
	}
}
