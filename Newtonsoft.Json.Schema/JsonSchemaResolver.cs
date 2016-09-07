using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	public class JsonSchemaResolver
	{
		public IList<JsonSchema> LoadedSchemas
		{
			get;
			protected set;
		}

		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		public virtual JsonSchema GetSchema(string id)
		{
			return Enumerable.SingleOrDefault<JsonSchema>(this.LoadedSchemas, (JsonSchema s) => s.Id == id);
		}
	}
}
