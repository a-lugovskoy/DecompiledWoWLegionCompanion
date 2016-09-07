using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class SpellDurationDB
	{
		private Hashtable m_records;

		public SpellDurationRec GetRecord(int id)
		{
			return (SpellDurationRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<SpellDurationRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SpellDurationRec spellDurationRec = (SpellDurationRec)enumerator.get_Current();
					if (!callback.Invoke(spellDurationRec))
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
			string text = path + "NonLocalized/SpellDuration.txt";
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
					SpellDurationRec spellDurationRec = new SpellDurationRec();
					spellDurationRec.Deserialize(valueLine);
					this.m_records.Add(spellDurationRec.ID, spellDurationRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
