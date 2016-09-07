using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class GarrFollowerQualityDB
	{
		private Hashtable m_records;

		public GarrFollowerQualityRec GetRecord(int id)
		{
			return (GarrFollowerQualityRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<GarrFollowerQualityRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GarrFollowerQualityRec garrFollowerQualityRec = (GarrFollowerQualityRec)enumerator.get_Current();
					if (!callback.Invoke(garrFollowerQualityRec))
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

		public void EnumRecordsByParentID(int parentID, Predicate<GarrFollowerQualityRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GarrFollowerQualityRec garrFollowerQualityRec = (GarrFollowerQualityRec)enumerator.get_Current();
					if ((ulong)garrFollowerQualityRec.Quality == (ulong)((long)parentID) && !callback.Invoke(garrFollowerQualityRec))
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
			string text = path + "NonLocalized/GarrFollowerQuality.txt";
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
					GarrFollowerQualityRec garrFollowerQualityRec = new GarrFollowerQualityRec();
					garrFollowerQualityRec.Deserialize(valueLine);
					this.m_records.Add(garrFollowerQualityRec.ID, garrFollowerQualityRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
