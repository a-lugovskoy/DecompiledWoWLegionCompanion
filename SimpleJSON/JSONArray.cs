using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SimpleJSON
{
	public class JSONArray : JSONNode, IEnumerable
	{
		private List<JSONNode> m_List = new List<JSONNode>();

		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.get_Count())
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List.get_Item(aIndex);
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.get_Count())
				{
					this.m_List.Add(value);
				}
				else
				{
					this.m_List.set_Item(aIndex, value);
				}
			}
		}

		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		public override int Count
		{
			get
			{
				return this.m_List.get_Count();
			}
		}

		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				List<JSONNode>.Enumerator enumerator = this.m_List.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						JSONNode current = enumerator.get_Current();
						yield return current;
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
			this.m_List.Add(aItem);
		}

		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.get_Count())
			{
				return null;
			}
			JSONNode result = this.m_List.get_Item(aIndex);
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			List<JSONNode>.Enumerator enumerator = this.m_List.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.get_Current();
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
			string text = "[ ";
			using (List<JSONNode>.Enumerator enumerator = this.m_List.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.get_Current();
					if (text.get_Length() > 2)
					{
						text += ", ";
					}
					text += current.ToString();
				}
			}
			text += " ]";
			return text;
		}

		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			using (List<JSONNode>.Enumerator enumerator = this.m_List.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.get_Current();
					if (text.get_Length() > 3)
					{
						text += ", ";
					}
					text = text + "\n" + aPrefix + "   ";
					text += current.ToString(aPrefix + "   ");
				}
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.get_Count());
			for (int i = 0; i < this.m_List.get_Count(); i++)
			{
				this.m_List.get_Item(i).Serialize(aWriter);
			}
		}
	}
}
