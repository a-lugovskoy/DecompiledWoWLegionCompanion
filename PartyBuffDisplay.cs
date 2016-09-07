using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;
using WowStaticData;

public class PartyBuffDisplay : MonoBehaviour
{
	public Image m_abilityIcon;

	public Text m_abilityName;

	public Text m_abilityDescription;

	public void SetAbility(int garrAbilityID)
	{
		GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(garrAbilityID);
		if (record == null)
		{
			Debug.LogWarning("Invalid garrAbilityID " + garrAbilityID);
			return;
		}
		this.m_abilityName.set_text(record.Name);
		this.m_abilityDescription.set_text(WowTextParser.parser.Parse(record.Description, 0));
		this.m_abilityDescription.set_supportRichText(WowTextParser.parser.IsRichText());
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, record.IconFileDataID);
		if (sprite != null)
		{
			this.m_abilityIcon.set_sprite(sprite);
		}
	}
}
