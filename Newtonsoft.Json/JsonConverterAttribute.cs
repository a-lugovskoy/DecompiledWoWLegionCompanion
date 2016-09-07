using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json
{
	[AttributeUsage]
	public sealed class JsonConverterAttribute : Attribute
	{
		private readonly Type _converterType;

		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		internal static JsonConverter CreateJsonConverterInstance(Type converterType)
		{
			JsonConverter result;
			try
			{
				result = (JsonConverter)Activator.CreateInstance(converterType);
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					converterType
				}), ex);
			}
			return result;
		}
	}
}
