using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class CharShipmentContainerDB
	{
		private Hashtable m_records;

		public CharShipmentContainerRec GetRecord(int id)
		{
			return (CharShipmentContainerRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<CharShipmentContainerRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CharShipmentContainerRec charShipmentContainerRec = (CharShipmentContainerRec)enumerator.get_Current();
					if (!callback.Invoke(charShipmentContainerRec))
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
				"/CharShipmentContainer_",
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
					CharShipmentContainerRec charShipmentContainerRec = new CharShipmentContainerRec();
					charShipmentContainerRec.Deserialize(valueLine);
					this.m_records.Add(charShipmentContainerRec.ID, charShipmentContainerRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
