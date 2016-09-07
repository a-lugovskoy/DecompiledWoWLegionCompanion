using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;
using WowStaticData;

public class EquipmentDialog : MonoBehaviour
{
	public AbilityDisplay m_abilityDisplay;

	public Text m_titleText;

	public Text m_abilityNameText;

	public Text m_abilityDescription;

	public Text m_noEquipmentMessage;

	public FollowerInventoryListItem m_equipmentListItemPrefab;

	public GameObject m_equipmentListContent;

	private void Awake()
	{
		this.m_titleText.set_font(GeneralHelpers.LoadFancyFont());
		this.m_titleText.set_text(StaticDB.GetString("EQUIPMENT", null));
		this.m_noEquipmentMessage.set_font(GeneralHelpers.LoadStandardFont());
		this.m_noEquipmentMessage.set_text(StaticDB.GetString("NO_EQUIPMENT2", "You do not have any Champion Equipment to equip."));
	}

	public void OnEnable()
	{
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	public void SetAbility(int garrAbilityID, FollowerDetailView followerDetailView)
	{
		GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(garrAbilityID);
		if (record == null)
		{
			Debug.LogWarning("Invalid garrAbilityID " + garrAbilityID);
			return;
		}
		this.m_abilityNameText.set_text(record.Name);
		this.m_abilityDescription.set_text(WowTextParser.parser.Parse(record.Description, 0));
		this.m_abilityDescription.set_supportRichText(WowTextParser.parser.IsRichText());
		this.m_abilityDisplay.SetAbility(garrAbilityID, true, true, null);
		FollowerInventoryListItem[] componentsInChildren = this.m_equipmentListContent.GetComponentsInChildren<FollowerInventoryListItem>(true);
		FollowerInventoryListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			FollowerInventoryListItem followerInventoryListItem = array[i];
			Object.DestroyImmediate(followerInventoryListItem.get_gameObject());
		}
		bool active = true;
		IEnumerator enumerator = PersistentEquipmentData.equipmentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MobileFollowerEquipment mobileFollowerEquipment = (MobileFollowerEquipment)enumerator.get_Current();
				GarrAbilityRec record2 = StaticDB.garrAbilityDB.GetRecord(mobileFollowerEquipment.GarrAbilityID);
				if (record2 != null)
				{
					if ((record2.Flags & 64u) == 0u)
					{
						FollowerInventoryListItem followerInventoryListItem2 = Object.Instantiate<FollowerInventoryListItem>(this.m_equipmentListItemPrefab);
						followerInventoryListItem2.get_transform().SetParent(this.m_equipmentListContent.get_transform(), false);
						followerInventoryListItem2.SetEquipment(mobileFollowerEquipment, followerDetailView, garrAbilityID);
						active = false;
					}
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
		this.m_noEquipmentMessage.get_gameObject().SetActive(active);
	}
}
