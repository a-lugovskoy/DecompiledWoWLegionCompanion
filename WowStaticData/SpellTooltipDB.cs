using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class SpellTooltipDB
	{
		private Hashtable m_records;

		public SpellTooltipRec GetRecord(int id)
		{
			return (SpellTooltipRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<SpellTooltipRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SpellTooltipRec spellTooltipRec = (SpellTooltipRec)enumerator.get_Current();
					if (!callback.Invoke(spellTooltipRec))
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
				"/SpellTooltip_",
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
					SpellTooltipRec spellTooltipRec = new SpellTooltipRec();
					spellTooltipRec.Deserialize(valueLine);
					this.m_records.Add(spellTooltipRec.ID, spellTooltipRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
