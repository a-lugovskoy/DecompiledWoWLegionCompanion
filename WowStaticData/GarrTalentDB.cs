using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class GarrTalentDB
	{
		private Hashtable m_records;

		public GarrTalentRec GetRecord(int id)
		{
			return (GarrTalentRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<GarrTalentRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GarrTalentRec garrTalentRec = (GarrTalentRec)enumerator.get_Current();
					if (!callback.Invoke(garrTalentRec))
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

		public void EnumRecordsByParentID(int parentID, Predicate<GarrTalentRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GarrTalentRec garrTalentRec = (GarrTalentRec)enumerator.get_Current();
					if ((ulong)garrTalentRec.GarrTalentTreeID == (ulong)((long)parentID) && !callback.Invoke(garrTalentRec))
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
				"/GarrTalent_",
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
					GarrTalentRec garrTalentRec = new GarrTalentRec();
					garrTalentRec.Deserialize(valueLine);
					this.m_records.Add(garrTalentRec.ID, garrTalentRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
