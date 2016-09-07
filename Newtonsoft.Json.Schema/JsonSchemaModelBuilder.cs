using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaModelBuilder
	{
		private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();

		private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();

		private JsonSchemaNode _node;

		public JsonSchemaModel Build(JsonSchema schema)
		{
			this._nodes = new JsonSchemaNodeCollection();
			this._node = this.AddSchema(null, schema);
			this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			return this.BuildNodeModel(this._node);
		}

		public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
		{
			string id;
			if (existingNode != null)
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				id = JsonSchemaNode.GetId(Enumerable.Union<JsonSchema>(existingNode.Schemas, new JsonSchema[]
				{
					schema
				}));
			}
			else
			{
				id = JsonSchemaNode.GetId(new JsonSchema[]
				{
					schema
				});
			}
			if (this._nodes.Contains(id))
			{
				return this._nodes.get_Item(id);
			}
			JsonSchemaNode jsonSchemaNode = (existingNode == null) ? new JsonSchemaNode(schema) : existingNode.Combine(schema);
			this._nodes.Add(jsonSchemaNode);
			this.AddProperties(schema.Properties, jsonSchemaNode.Properties);
			this.AddProperties(schema.PatternProperties, jsonSchemaNode.PatternProperties);
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.get_Count(); i++)
				{
					this.AddItem(jsonSchemaNode, i, schema.Items.get_Item(i));
				}
			}
			if (schema.AdditionalProperties != null)
			{
				this.AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				jsonSchemaNode = this.AddSchema(jsonSchemaNode, schema.Extends);
			}
			return jsonSchemaNode;
		}

		public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
		{
			if (source != null)
			{
				using (IEnumerator<KeyValuePair<string, JsonSchema>> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, JsonSchema> current = enumerator.get_Current();
						this.AddProperty(target, current.get_Key(), current.get_Value());
					}
				}
			}
		}

		public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
		{
			JsonSchemaNode existingNode;
			target.TryGetValue(propertyName, ref existingNode);
			target.set_Item(propertyName, this.AddSchema(existingNode, schema));
		}

		public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
		{
			JsonSchemaNode existingNode = (parentNode.Items.get_Count() <= index) ? null : parentNode.Items.get_Item(index);
			JsonSchemaNode jsonSchemaNode = this.AddSchema(existingNode, schema);
			if (parentNode.Items.get_Count() <= index)
			{
				parentNode.Items.Add(jsonSchemaNode);
			}
			else
			{
				parentNode.Items.set_Item(index, jsonSchemaNode);
			}
		}

		public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalProperties = this.AddSchema(parentNode.AdditionalProperties, schema);
		}

		private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
		{
			JsonSchemaModel jsonSchemaModel;
			if (this._nodeModels.TryGetValue(node, ref jsonSchemaModel))
			{
				return jsonSchemaModel;
			}
			jsonSchemaModel = JsonSchemaModel.Create(node.Schemas);
			this._nodeModels.set_Item(node, jsonSchemaModel);
			using (Dictionary<string, JsonSchemaNode>.Enumerator enumerator = node.Properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JsonSchemaNode> current = enumerator.get_Current();
					if (jsonSchemaModel.Properties == null)
					{
						jsonSchemaModel.Properties = new Dictionary<string, JsonSchemaModel>();
					}
					jsonSchemaModel.Properties.set_Item(current.get_Key(), this.BuildNodeModel(current.get_Value()));
				}
			}
			using (Dictionary<string, JsonSchemaNode>.Enumerator enumerator2 = node.PatternProperties.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, JsonSchemaNode> current2 = enumerator2.get_Current();
					if (jsonSchemaModel.PatternProperties == null)
					{
						jsonSchemaModel.PatternProperties = new Dictionary<string, JsonSchemaModel>();
					}
					jsonSchemaModel.PatternProperties.set_Item(current2.get_Key(), this.BuildNodeModel(current2.get_Value()));
				}
			}
			for (int i = 0; i < node.Items.get_Count(); i++)
			{
				if (jsonSchemaModel.Items == null)
				{
					jsonSchemaModel.Items = new List<JsonSchemaModel>();
				}
				jsonSchemaModel.Items.Add(this.BuildNodeModel(node.Items.get_Item(i)));
			}
			if (node.AdditionalProperties != null)
			{
				jsonSchemaModel.AdditionalProperties = this.BuildNodeModel(node.AdditionalProperties);
			}
			return jsonSchemaModel;
		}
	}
}
