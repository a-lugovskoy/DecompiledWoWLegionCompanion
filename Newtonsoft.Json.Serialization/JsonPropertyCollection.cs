using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
	public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
	{
		private readonly Type _type;

		public JsonPropertyCollection(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			this._type = type;
		}

		protected override string GetKeyForItem(JsonProperty item)
		{
			return item.PropertyName;
		}

		public void AddProperty(JsonProperty property)
		{
			if (base.Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				JsonProperty jsonProperty = base.get_Item(property.PropertyName);
				if (!jsonProperty.Ignored)
				{
					throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						property.PropertyName,
						this._type
					}));
				}
				this.Remove(jsonProperty);
			}
			this.Add(property);
		}

		public JsonProperty GetClosestMatchProperty(string propertyName)
		{
			JsonProperty property = this.GetProperty(propertyName, 4);
			if (property == null)
			{
				property = this.GetProperty(propertyName, 5);
			}
			return property;
		}

		public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
		{
			using (IEnumerator<JsonProperty> enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonProperty current = enumerator.get_Current();
					if (string.Equals(propertyName, current.PropertyName, comparisonType))
					{
						return current;
					}
				}
			}
			return null;
		}
	}
}
