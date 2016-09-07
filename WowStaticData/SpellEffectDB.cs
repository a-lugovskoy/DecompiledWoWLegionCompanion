using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class SpellEffectDB
	{
		private Hashtable m_records;

		public SpellEffectRec GetRecord(int id)
		{
			return (SpellEffectRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<SpellEffectRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SpellEffectRec spellEffectRec = (SpellEffectRec)enumerator.get_Current();
					if (!callback.Invoke(spellEffectRec))
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

		public void EnumRecordsByParentID(int parentID, Predicate<SpellEffectRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SpellEffectRec spellEffectRec = (SpellEffectRec)enumerator.get_Current();
					if (spellEffectRec.SpellID == parentID && !callback.Invoke(spellEffectRec))
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
			string text = path + "NonLocalized/SpellEffect.txt";
			if (this.m_records != null)
			{
				Debug.Log("Already loaded static db " + text);
				return false;
			}
			TextAsset textAsset = nonLocalizedBundle.LoadAsset<TextAsset>(text);
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
					SpellEffectRec spellEffectRec = new SpellEffectRec();
					spellEffectRec.Deserialize(valueLine);
					this.m_records.Add(spellEffectRec.ID, spellEffectRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
