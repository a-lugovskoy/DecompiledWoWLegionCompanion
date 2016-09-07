using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class CharShipmentDB
	{
		private Hashtable m_records;

		public CharShipmentRec GetRecord(int id)
		{
			return (CharShipmentRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<CharShipmentRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CharShipmentRec charShipmentRec = (CharShipmentRec)enumerator.get_Current();
					if (!callback.Invoke(charShipmentRec))
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

		public void EnumRecordsByParentID(int parentID, Predicate<CharShipmentRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CharShipmentRec charShipmentRec = (CharShipmentRec)enumerator.get_Current();
					if ((ulong)charShipmentRec.ContainerID == (ulong)((long)parentID) && !callback.Invoke(charShipmentRec))
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
			string text = path + "NonLocalized/CharShipment.txt";
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
					CharShipmentRec charShipmentRec = new CharShipmentRec();
					charShipmentRec.Deserialize(valueLine);
					this.m_records.Add(charShipmentRec.ID, charShipmentRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
