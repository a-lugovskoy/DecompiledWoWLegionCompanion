using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	internal static class JsonSchemaConstants
	{
		public const string TypePropertyName = "type";

		public const string PropertiesPropertyName = "properties";

		public const string ItemsPropertyName = "items";

		public const string RequiredPropertyName = "required";

		public const string PatternPropertiesPropertyName = "patternProperties";

		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		public const string RequiresPropertyName = "requires";

		public const string IdentityPropertyName = "identity";

		public const string MinimumPropertyName = "minimum";

		public const string MaximumPropertyName = "maximum";

		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		public const string MinimumItemsPropertyName = "minItems";

		public const string MaximumItemsPropertyName = "maxItems";

		public const string PatternPropertyName = "pattern";

		public const string MaximumLengthPropertyName = "maxLength";

		public const string MinimumLengthPropertyName = "minLength";

		public const string EnumPropertyName = "enum";

		public const string OptionsPropertyName = "options";

		public const string ReadOnlyPropertyName = "readonly";

		public const string TitlePropertyName = "title";

		public const string DescriptionPropertyName = "description";

		public const string FormatPropertyName = "format";

		public const string DefaultPropertyName = "default";

		public const string TransientPropertyName = "transient";

		public const string DivisibleByPropertyName = "divisibleBy";

		public const string HiddenPropertyName = "hidden";

		public const string DisallowPropertyName = "disallow";

		public const string ExtendsPropertyName = "extends";

		public const string IdPropertyName = "id";

		public const string OptionValuePropertyName = "value";

		public const string OptionLabelPropertyName = "label";

		public const string ReferencePropertyName = "$ref";

		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping;

		static JsonSchemaConstants()
		{
			// Note: this type is marked as 'beforefieldinit'.
			Dictionary<string, JsonSchemaType> dictionary = new Dictionary<string, JsonSchemaType>();
			dictionary.Add("string", JsonSchemaType.String);
			dictionary.Add("object", JsonSchemaType.Object);
			dictionary.Add("integer", JsonSchemaType.Integer);
			dictionary.Add("number", JsonSchemaType.Float);
			dictionary.Add("null", JsonSchemaType.Null);
			dictionary.Add("boolean", JsonSchemaType.Boolean);
			dictionary.Add("array", JsonSchemaType.Array);
			dictionary.Add("any", JsonSchemaType.Any);
			JsonSchemaConstants.JsonSchemaTypeMapping = dictionary;
		}
	}
}
