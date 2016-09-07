using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaModel
	{
		public bool Required
		{
			get;
			set;
		}

		public JsonSchemaType Type
		{
			get;
			set;
		}

		public int? MinimumLength
		{
			get;
			set;
		}

		public int? MaximumLength
		{
			get;
			set;
		}

		public double? DivisibleBy
		{
			get;
			set;
		}

		public double? Minimum
		{
			get;
			set;
		}

		public double? Maximum
		{
			get;
			set;
		}

		public bool ExclusiveMinimum
		{
			get;
			set;
		}

		public bool ExclusiveMaximum
		{
			get;
			set;
		}

		public int? MinimumItems
		{
			get;
			set;
		}

		public int? MaximumItems
		{
			get;
			set;
		}

		public IList<string> Patterns
		{
			get;
			set;
		}

		public IList<JsonSchemaModel> Items
		{
			get;
			set;
		}

		public IDictionary<string, JsonSchemaModel> Properties
		{
			get;
			set;
		}

		public IDictionary<string, JsonSchemaModel> PatternProperties
		{
			get;
			set;
		}

		public JsonSchemaModel AdditionalProperties
		{
			get;
			set;
		}

		public bool AllowAdditionalProperties
		{
			get;
			set;
		}

		public IList<JToken> Enum
		{
			get;
			set;
		}

		public JsonSchemaType Disallow
		{
			get;
			set;
		}

		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.Required = false;
		}

		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			using (IEnumerator<JsonSchema> enumerator = schemata.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchema current = enumerator.get_Current();
					JsonSchemaModel.Combine(jsonSchemaModel, current);
				}
			}
			return jsonSchemaModel;
		}

		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			bool arg_2F_1;
			if (!model.Required)
			{
				bool? required = schema.Required;
				arg_2F_1 = (required.get_HasValue() && required.get_Value());
			}
			else
			{
				arg_2F_1 = true;
			}
			model.Required = arg_2F_1;
			JsonSchemaType arg_5C_0 = model.Type;
			JsonSchemaType? type = schema.Type;
			model.Type = (arg_5C_0 & ((!type.get_HasValue()) ? JsonSchemaType.Any : type.get_Value()));
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			bool arg_104_1;
			if (!model.ExclusiveMinimum)
			{
				bool? exclusiveMinimum = schema.ExclusiveMinimum;
				arg_104_1 = (exclusiveMinimum.get_HasValue() && exclusiveMinimum.get_Value());
			}
			else
			{
				arg_104_1 = true;
			}
			model.ExclusiveMinimum = arg_104_1;
			bool arg_138_1;
			if (!model.ExclusiveMaximum)
			{
				bool? exclusiveMaximum = schema.ExclusiveMaximum;
				arg_138_1 = (exclusiveMaximum.get_HasValue() && exclusiveMaximum.get_Value());
			}
			else
			{
				arg_138_1 = true;
			}
			model.ExclusiveMaximum = arg_138_1;
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.AllowAdditionalProperties = (model.AllowAdditionalProperties && schema.AllowAdditionalProperties);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, new JTokenEqualityComparer());
			}
			JsonSchemaType arg_1E5_0 = model.Disallow;
			JsonSchemaType? disallow = schema.Disallow;
			model.Disallow = (arg_1E5_0 | ((!disallow.get_HasValue()) ? JsonSchemaType.None : disallow.get_Value()));
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
