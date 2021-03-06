using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	internal class BsonArray : BsonToken, IEnumerable, IEnumerable<BsonToken>
	{
		private readonly List<BsonToken> _children = new List<BsonToken>();

		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}
	}
}
