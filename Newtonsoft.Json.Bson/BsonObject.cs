using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	internal class BsonObject : BsonToken, IEnumerable, IEnumerable<BsonProperty>
	{
		private readonly List<BsonProperty> _children = new List<BsonProperty>();

		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}
	}
}
