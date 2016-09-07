using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;

public class PartyBuffsPopup : MonoBehaviour
{
	public Text m_partyBuffsLabel;

	public PartyBuffDisplay m_partyBuffDisplayPrefab;

	public GameObject m_partyBuffRoot;

	private void Awake()
	{
		this.m_partyBuffsLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_partyBuffsLabel.set_text(StaticDB.GetString("PARTY_BUFFS", "Party Buffs [PH]"));
	}

	public void OnEnable()
	{
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_canvasBlurManager.AddBlurRef_Level2Canvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_canvasBlurManager.RemoveBlurRef_Level2Canvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	public void Init(int[] buffIDs)
	{
		PartyBuffDisplay[] componentsInChildren = this.m_partyBuffRoot.GetComponentsInChildren<PartyBuffDisplay>(true);
		PartyBuffDisplay[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			PartyBuffDisplay partyBuffDisplay = array[i];
			Object.DestroyImmediate(partyBuffDisplay.get_gameObject());
		}
		for (int j = 0; j < buffIDs.Length; j++)
		{
			int ability = buffIDs[j];
			PartyBuffDisplay partyBuffDisplay2 = Object.Instantiate<PartyBuffDisplay>(this.m_partyBuffDisplayPrefab);
			partyBuffDisplay2.get_transform().SetParent(this.m_partyBuffRoot.get_transform(), false);
			partyBuffDisplay2.SetAbility(ability);
		}
	}
}
