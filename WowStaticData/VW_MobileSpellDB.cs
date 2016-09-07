using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class VW_MobileSpellDB
	{
		private Hashtable m_records;

		public VW_MobileSpellRec GetRecord(int id)
		{
			return (VW_MobileSpellRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<VW_MobileSpellRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					VW_MobileSpellRec vW_MobileSpellRec = (VW_MobileSpellRec)enumerator.get_Current();
					if (!callback.Invoke(vW_MobileSpellRec))
					{
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public bool Load(string path, AssetBundle nonLocalizedBundle, AssetBundle localizedBundle, string locale)
		{
			string text = string.Concat(new string[]
			{
				path,
				locale,
				"/VW_MobileSpell_",
				locale,
				".txt"
			});
			if (this.m_records != null)
			{
				Debug.Log("Already loaded static db " + text);
				return false;
			}
			TextAsset textAsset = localizedBundle.LoadAsset<TextAsset>(text);
			if (textAsset == null)
			{
				Debug.Log("Unable to load static db " + text);
				return false;
			}
			string text2 = textAsset.ToString();
			this.m_records = new Hashtable();
			int num = 0;
			int num2;
			do
			{
				num2 = text2.IndexOf('\n', num);
				if (num2 >= 0)
				{
					string valueLine = text2.Substring(num, num2 - num + 1).Trim();
					VW_MobileSpellRec vW_MobileSpellRec = new VW_MobileSpellRec();
					vW_MobileSpellRec.Deserialize(valueLine);
					this.m_records.Add(vW_MobileSpellRec.ID, vW_MobileSpellRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
