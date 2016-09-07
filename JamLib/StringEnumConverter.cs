using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace JamLib
{
	public class StringEnumConverter : Newtonsoft.Json.Converters.StringEnumConverter
	{
		public override bool CanConvert(Type objectType)
		{
			Type type = (!objectType.get_IsGenericType() || objectType.GetGenericTypeDefinition() != typeof(Nullable)) ? objectType : Nullable.GetUnderlyingType(objectType);
			if (type.get_IsEnum())
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(DataContractAttribute), false);
				return customAttributes != null && customAttributes.Length > 0;
			}
			return false;
		}
	}
}
