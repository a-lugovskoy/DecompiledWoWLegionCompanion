using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;

public class EmissaryPopup : MonoBehaviour
{
	public Text m_descriptionText;

	public void FactionUpdate(MobileClientEmissaryFactionUpdate msg)
	{
		this.m_descriptionText.set_text(string.Empty);
		MobileEmissaryFaction[] faction = msg.Faction;
		for (int i = 0; i < faction.Length; i++)
		{
			MobileEmissaryFaction mobileEmissaryFaction = faction[i];
			Text expr_28 = this.m_descriptionText;
			string text = expr_28.get_text();
			expr_28.set_text(string.Concat(new object[]
			{
				text,
				"FactionID:\t",
				mobileEmissaryFaction.FactionID,
				"\t Standing:\t",
				mobileEmissaryFaction.FactionAmount,
				"\n"
			}));
		}
	}
}
