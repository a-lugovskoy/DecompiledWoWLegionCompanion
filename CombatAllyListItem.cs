using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class CombatAllyListItem : MonoBehaviour
{
	public MissionFollowerSlot m_combatAllySlot;

	public Text m_combatAllyLabel;

	public Text m_assignChampionText;

	public Text m_championName;

	public Text m_combatAllyAbilityText;

	public Text m_combatAbilityName;

	public SpellDisplay m_combatAllySupportSpellDisplay;

	public GameObject m_unassignCombatAllyButton;

	public Text m_unassignCombatAllyButtonLabel;

	private int m_combatAllyMissionID;

	private void Awake()
	{
		this.m_combatAllyLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_assignChampionText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_championName.set_font(GeneralHelpers.LoadStandardFont());
		this.m_combatAllyAbilityText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_combatAbilityName.set_font(GeneralHelpers.LoadStandardFont());
		this.m_unassignCombatAllyButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_combatAllyLabel.set_text(StaticDB.GetString("COMBAT_ALLY", null));
		this.m_assignChampionText.set_text(StaticDB.GetString("ORDER_HALL_ZONE_SUPPORT_DESCRIPTION_2", null));
		this.m_combatAllyAbilityText.set_text(StaticDB.GetString("COMBAT_ALLY_ABILITY", null));
		this.m_unassignCombatAllyButtonLabel.set_text(StaticDB.GetString("UNASSIGN", null));
		this.UpdateVisuals();
	}

	private void OnEnable()
	{
		this.UpdateVisuals();
		Main expr_0B = Main.instance;
		expr_0B.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(expr_0B.GarrisonDataResetFinishedAction, new Action(this.HandleDataResetFinished));
		Main expr_31 = Main.instance;
		expr_31.StartLogOutAction = (Action)Delegate.Combine(expr_31.StartLogOutAction, new Action(this.HandleStartLogout));
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.GarrisonDataResetFinishedAction = (Action)Delegate.Remove(expr_05.GarrisonDataResetFinishedAction, new Action(this.HandleDataResetFinished));
		Main expr_2B = Main.instance;
		expr_2B.StartLogOutAction = (Action)Delegate.Remove(expr_2B.StartLogOutAction, new Action(this.HandleStartLogout));
	}

	private void HandleStartLogout()
	{
		this.ClearCombatAllyDisplay();
	}

	public void HandleDataResetFinished()
	{
		this.UpdateVisuals();
	}

	public void HandleCompleteMissionResult(int garrMissionID, int result, int missionSuccessChance)
	{
		this.UpdateVisuals();
	}

	private void ClearCombatAllyDisplay()
	{
		this.m_combatAllySlot.SetFollower(0);
		this.m_combatAllyLabel.get_gameObject().SetActive(true);
		this.m_assignChampionText.get_gameObject().SetActive(true);
		this.m_championName.get_gameObject().SetActive(false);
		this.m_combatAllyAbilityText.get_gameObject().SetActive(false);
		this.m_combatAbilityName.get_gameObject().SetActive(false);
		this.m_combatAllySupportSpellDisplay.get_gameObject().SetActive(false);
		this.m_unassignCombatAllyButton.SetActive(false);
	}

	public void UpdateVisuals()
	{
		CombatAllyMissionState combatAllyMissionState = CombatAllyMissionState.notAvailable;
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
						this.m_combatAllyMissionID = jamGarrisonMobileMission.MissionRecID;
						if (jamGarrisonMobileMission.MissionState == 1)
						{
							combatAllyMissionState = CombatAllyMissionState.inProgress;
						}
						else
						{
							combatAllyMissionState = CombatAllyMissionState.available;
						}
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
		if (combatAllyMissionState == CombatAllyMissionState.inProgress)
		{
			using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator2 = PersistentFollowerData.followerDictionary.get_Values().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					JamGarrisonFollower current = enumerator2.get_Current();
					if (current.CurrentMissionID == this.m_combatAllyMissionID)
					{
						this.m_combatAllySlot.SetFollower(current.GarrFollowerID);
						this.m_combatAllyLabel.get_gameObject().SetActive(false);
						this.m_assignChampionText.get_gameObject().SetActive(false);
						this.m_championName.get_gameObject().SetActive(true);
						GarrFollowerRec record2 = StaticDB.garrFollowerDB.GetRecord(current.GarrFollowerID);
						CreatureRec record3 = StaticDB.creatureDB.GetRecord((GarrisonStatus.Faction() != PVP_FACTION.ALLIANCE) ? record2.HordeCreatureID : record2.AllianceCreatureID);
						this.m_championName.set_text(record3.Name);
						this.m_championName.set_color(GeneralHelpers.GetQualityColor(current.Quality));
						this.m_combatAllySupportSpellDisplay.get_gameObject().SetActive(true);
						this.m_combatAllySupportSpellDisplay.SetSpell(current.ZoneSupportSpellID);
						this.m_unassignCombatAllyButton.SetActive(true);
						break;
					}
				}
			}
		}
		else
		{
			this.ClearCombatAllyDisplay();
		}
	}

	public void UnassignCombatAlly()
	{
		Main.instance.CompleteMission(this.m_combatAllyMissionID);
	}
}
