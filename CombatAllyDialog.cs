using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class CombatAllyDialog : MonoBehaviour
{
	public FollowerInventoryListItem m_combatAllyChampionListItemPrefab;

	public GameObject m_combatAllyListContent;

	public Text m_combatAllyCost;

	public Image m_combatAllyCostResourceIcon;

	public Text m_titleText;

	public MissionPanelSlider m_missionPanelSlider;

	public void Start()
	{
		this.m_combatAllyCost.set_font(GeneralHelpers.LoadStandardFont());
		this.m_titleText.set_text(StaticDB.GetString("COMBAT_ALLY", null));
	}

	public void OnEnable()
	{
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
		if (this.m_missionPanelSlider.m_sliderPanel.IsShowing() || this.m_missionPanelSlider.m_sliderPanel.IsBusyMoving())
		{
			base.get_gameObject().SetActive(false);
			return;
		}
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	private void CreateCombatAllyItems(int combatAllyMissionID, int combatAllyMissionCost)
	{
		using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator = PersistentFollowerData.followerDictionary.get_Values().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonFollower current = enumerator.get_Current();
				FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(current);
				if (current.ZoneSupportSpellID > 0 && (followerStatus == FollowerStatus.available || followerStatus == FollowerStatus.onMission))
				{
					FollowerInventoryListItem followerInventoryListItem = Object.Instantiate<FollowerInventoryListItem>(this.m_combatAllyChampionListItemPrefab);
					followerInventoryListItem.get_transform().SetParent(this.m_combatAllyListContent.get_transform(), false);
					followerInventoryListItem.SetCombatAllyChampion(current, combatAllyMissionID, combatAllyMissionCost);
				}
			}
		}
	}

	public void Init()
	{
		FollowerInventoryListItem[] componentsInChildren = this.m_combatAllyListContent.GetComponentsInChildren<FollowerInventoryListItem>(true);
		FollowerInventoryListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			FollowerInventoryListItem followerInventoryListItem = array[i];
			Object.DestroyImmediate(followerInventoryListItem.get_gameObject());
		}
		int num = 0;
		IEnumerator enumerator = PersistentMissionData.missionDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)enumerator.get_Current();
				GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(jamGarrisonMobileMission.MissionRecID);
				if (record != null)
				{
					if ((record.Flags & 16u) != 0u)
					{
						this.CreateCombatAllyItems(jamGarrisonMobileMission.MissionRecID, (int)record.MissionCost);
						num = (int)record.MissionCost;
						break;
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
		if (num <= GarrisonStatus.Resources())
		{
			this.m_combatAllyCost.set_text(string.Concat(new object[]
			{
				StaticDB.GetString("COST2", "Cost:"),
				" <color=#ffffffff>",
				num,
				"</color>"
			}));
		}
		else
		{
			this.m_combatAllyCost.set_text(string.Concat(new object[]
			{
				StaticDB.GetString("COST2", "Cost:"),
				" <color=#ff0000ff>",
				num,
				"</color>"
			}));
		}
		Sprite sprite = GeneralHelpers.LoadCurrencyIcon(1220);
		if (sprite != null)
		{
			this.m_combatAllyCostResourceIcon.set_sprite(sprite);
		}
	}
}
