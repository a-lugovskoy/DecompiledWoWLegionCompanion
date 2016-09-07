using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;
using WowStaticData;

public class SpellDisplay : MonoBehaviour
{
	public Image m_spellIcon;

	public Text m_iconError;

	public Text m_spellName;

	private int m_spellID;

	public void SetSpell(int spellID)
	{
		this.m_spellID = spellID;
		VW_MobileSpellRec record = StaticDB.vw_mobileSpellDB.GetRecord(this.m_spellID);
		if (record == null)
		{
			this.m_spellName.set_text("Err Spell ID " + this.m_spellID);
			Debug.LogWarning("Invalid spellID " + this.m_spellID);
			return;
		}
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, record.SpellIconFileDataID);
		if (sprite != null)
		{
			this.m_spellIcon.set_sprite(sprite);
			this.m_iconError.get_gameObject().SetActive(false);
		}
		else
		{
			Debug.LogWarning("Invalid or missing icon: " + record.SpellIconFileDataID);
			this.m_iconError.get_gameObject().SetActive(true);
			this.m_iconError.set_text("Missing Icon " + record.SpellIconFileDataID);
		}
		this.m_spellName.set_text(record.Name);
	}

	public void ShowTooltip()
	{
		Main.instance.allPopups.ShowSpellInfoPopup(this.m_spellID);
	}
}
