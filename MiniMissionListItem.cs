using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class MiniMissionListItem : MonoBehaviour
{
	public Text m_missionName;

	public Image m_missionTypeIcon;

	public Image m_rareMissionHighlight;

	public Image m_missionTypeBG;

	public Image m_missionLocation;

	public Image m_statusDarkener;

	public Text m_missionLevel;

	public Text m_missionTime;

	public Text m_rareMissionLabel;

	public Text m_statusText;

	public MissionRewardDisplay m_missionRewardDisplayPrefab;

	public GameObject m_previewLootGroup;

	public GameObject m_previewMechanicEffectPrefab;

	public GameObject m_previewMechanicsGroup;

	private JamGarrisonMobileMission m_mission;

	private void Awake()
	{
		this.m_missionName.set_font(GeneralHelpers.LoadFancyFont());
		this.m_missionLevel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_missionTime.set_font(GeneralHelpers.LoadStandardFont());
		this.m_rareMissionLabel.set_font(GeneralHelpers.LoadFancyFont());
		this.m_statusText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_rareMissionLabel.set_text(StaticDB.GetString("RARE", "Rare!"));
	}

	public void SetMission(JamGarrisonMobileMission mission)
	{
		this.m_statusDarkener.get_gameObject().SetActive(false);
		this.m_statusText.get_gameObject().SetActive(false);
		this.m_mission = mission;
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(mission.MissionRecID);
		if (record == null)
		{
			return;
		}
		if (this.m_missionTypeIcon != null)
		{
			GarrMissionTypeRec record2 = StaticDB.garrMissionTypeDB.GetRecord((int)record.GarrMissionTypeID);
			this.m_missionTypeIcon.set_sprite(TextureAtlas.instance.GetAtlasSprite((int)record2.UiTextureAtlasMemberID));
		}
		bool flag = false;
		if (mission.MissionState == 1)
		{
			flag = true;
			this.m_statusDarkener.get_gameObject().SetActive(true);
			this.m_statusDarkener.set_color(new Color(0f, 0f, 0f, 0.3529412f));
			this.m_statusText.get_gameObject().SetActive(true);
			this.m_missionTime.get_gameObject().SetActive(false);
		}
		this.m_previewMechanicsGroup.SetActive(!flag);
		Duration duration = new Duration(record.MissionDuration);
		string text;
		if (duration.DurationValue >= 28800)
		{
			text = "<color=#ff8600ff>" + duration.DurationString + "</color>";
		}
		else
		{
			text = "<color=#BEBEBEFF>" + duration.DurationString + "</color>";
		}
		this.m_missionTime.set_text("(" + text + ")");
		this.m_missionName.set_text(record.Name);
		if (this.m_missionLevel != null)
		{
			if (record.TargetLevel < 110)
			{
				this.m_missionLevel.set_text(string.Empty + record.TargetLevel);
			}
			else
			{
				this.m_missionLevel.set_text(string.Concat(new object[]
				{
					string.Empty,
					record.TargetLevel,
					"\n(",
					record.TargetItemLevel,
					")"
				}));
			}
		}
		bool flag2 = (record.Flags & 1u) != 0u;
		this.m_rareMissionLabel.get_gameObject().SetActive(flag2);
		this.m_rareMissionHighlight.get_gameObject().SetActive(flag2);
		if (flag2)
		{
			this.m_missionTypeBG.set_color(new Color(0f, 0f, 1f, 0.24f));
		}
		else
		{
			this.m_missionTypeBG.set_color(new Color(0f, 0f, 0f, 0.478f));
		}
		this.m_missionLocation.set_enabled(false);
		UiTextureKitRec record3 = StaticDB.uiTextureKitDB.GetRecord((int)record.UiTextureKitID);
		if (record3 != null)
		{
			int uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID(record3.KitPrefix + "-List");
			if (uITextureAtlasMemberID > 0)
			{
				Sprite atlasSprite = TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID);
				if (atlasSprite != null)
				{
					this.m_missionLocation.set_enabled(true);
					this.m_missionLocation.set_sprite(atlasSprite);
				}
			}
		}
		this.UpdateMechanicPreview(flag, mission);
		MissionRewardDisplay[] componentsInChildren = this.m_previewLootGroup.GetComponentsInChildren<MissionRewardDisplay>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
			}
		}
		MissionRewardDisplay.InitMissionRewards(this.m_missionRewardDisplayPrefab.get_gameObject(), this.m_previewLootGroup.get_transform(), mission.Reward);
	}

	public void UpdateMechanicPreview(bool missionInProgress, JamGarrisonMobileMission mission)
	{
		if (this.m_previewMechanicsGroup != null)
		{
			AbilityDisplay[] componentsInChildren = this.m_previewMechanicsGroup.GetComponentsInChildren<AbilityDisplay>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
				}
			}
		}
		if (!missionInProgress)
		{
			for (int j = 0; j < mission.Encounter.Length; j++)
			{
				int id = (mission.Encounter[j].MechanicID.Length <= 0) ? 0 : mission.Encounter[j].MechanicID[0];
				GarrMechanicRec record = StaticDB.garrMechanicDB.GetRecord(id);
				if (record != null && record.GarrAbilityID != 0)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_previewMechanicEffectPrefab);
					gameObject.get_transform().SetParent(this.m_previewMechanicsGroup.get_transform(), false);
					AbilityDisplay component = gameObject.GetComponent<AbilityDisplay>();
					component.SetAbility(record.GarrAbilityID, false, false, null);
					FollowerCanCounterMechanic canCounterStatus = GeneralHelpers.HasFollowerWhoCanCounter((int)record.GarrMechanicTypeID);
					component.SetCanCounterStatus(canCounterStatus);
				}
			}
		}
	}

	public void OnTap()
	{
		this.PlayClickSound();
		AllPopups.instance.HideAllPopups();
		if (this.m_mission.MissionState == 1)
		{
			if (AdventureMapPanel.instance.ShowMissionResultAction != null)
			{
				AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_mission.MissionRecID, 0, false);
			}
			return;
		}
		if (this.m_mission.MissionState == 0)
		{
			AdventureMapPanel.instance.SelectMissionFromList(this.m_mission.MissionRecID);
			return;
		}
	}

	private void Update()
	{
		if (this.m_mission.MissionState == 1)
		{
			long num = GarrisonStatus.CurrentTime() - this.m_mission.StartTime;
			long num2 = this.m_mission.MissionDuration - num;
			num2 = ((num2 <= 0L) ? 0L : num2);
			Duration duration = new Duration((int)num2);
			if (num2 > 0L)
			{
				this.m_statusText.set_text(duration.DurationString + " <color=#ff0000ff>(" + StaticDB.GetString("IN_PROGRESS", null) + ")</color>");
			}
			else
			{
				this.m_statusText.set_text("<color=#00ff00ff>(" + StaticDB.GetString("TAP_TO_COMPLETE", null) + ")</color>");
			}
		}
	}

	public void PlayClickSound()
	{
		Main.instance.m_UISound.Play_ButtonBlackClick();
	}

	public int GetMissionID()
	{
		return (this.m_mission != null) ? this.m_mission.MissionRecID : 0;
	}
}
