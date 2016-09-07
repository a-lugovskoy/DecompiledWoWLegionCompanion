using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SimpleJSON
{
	public class JSONClass : JSONNode, IEnumerable
	{
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict.get_Item(aKey);
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict.set_Item(aKey, value);
				}
				else
				{
					this.m_Dict.Add(aKey, value);
				}
			}
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.get_Count())
				{
					return null;
				}
				return Enumerable.ElementAt<KeyValuePair<string, JSONNode>>(this.m_Dict, aIndex).get_Value();
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.get_Count())
				{
					return;
				}
				string key = Enumerable.ElementAt<KeyValuePair<string, JSONNode>>(this.m_Dict, aIndex).get_Key();
				this.m_Dict.set_Item(key, value);
			}
		}

		public override int Count
		{
			get
			{
				return this.m_Dict.get_Count();
			}
		}

		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				Dictionary<string, JSONNode>.Enumerator enumerator = this.m_Dict.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, JSONNode> current = enumerator.get_Current();
						yield return current.get_Value();
					}
				}
				finally
				{
				}
				yield break;
			}
		}

		public override void Add(string aKey, JSONNode aItem)
		{
			if (!string.IsNullOrEmpty(aKey))
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict.set_Item(aKey, aItem);
				}
				else
				{
					this.m_Dict.Add(aKey, aItem);
				}
			}
			else
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
			}
		}

		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode result = this.m_Dict.get_Item(aKey);
			this.m_Dict.Remove(aKey);
			return result;
		}

		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.get_Count())
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = Enumerable.ElementAt<KeyValuePair<string, JSONNode>>(this.m_Dict, aIndex);
			this.m_Dict.Remove(keyValuePair.get_Key());
			return keyValuePair.get_Value();
		}

		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = Enumerable.First<KeyValuePair<string, JSONNode>>(Enumerable.Where<KeyValuePair<string, JSONNode>>(this.m_Dict, (KeyValuePair<string, JSONNode> k) => k.get_Value() == aNode));
				this.m_Dict.Remove(keyValuePair.get_Key());
				result = aNode;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			Dictionary<string, JSONNode>.Enumerator enumerator = this.m_Dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.get_Current();
					yield return current;
				}
			}
			finally
			{
			}
			yield break;
		}

		public override string ToString()
		{
			string text = "{";
			using (Dictionary<string, JSONNode>.Enumerator enumerator = this.m_Dict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.get_Current();
					if (text.get_Length() > 2)
					{
						text += ", ";
					}
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\"",
						JSONNode.Escape(current.get_Key()),
						"\":",
						current.get_Value().ToString()
					});
				}
			}
			text += "}";
			return text;
		}

		public override string ToString(string aPrefix)
		{
			string text = "{ ";
			using (Dictionary<string, JSONNode>.Enumerator enumerator = this.m_Dict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.get_Current();
					if (text.get_Length() > 3)
					{
						text += ", ";
					}
					text = text + "\n" + aPrefix + "   ";
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\"",
						JSONNode.Escape(current.get_Key()),
						"\" : ",
						current.get_Value().ToString(aPrefix + "   ")
					});
				}
			}
			text = text + "\n" + aPrefix + "}";
			return text;
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.get_Count());
			using (Dictionary<string, JSONNode>.KeyCollection.Enumerator enumerator = this.m_Dict.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					aWriter.Write(current);
					this.m_Dict.get_Item(current).Serialize(aWriter);
				}
			}
		}
	}
}
