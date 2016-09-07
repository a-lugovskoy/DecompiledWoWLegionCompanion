using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class MissionFollowerSlot : MonoBehaviour
{
	public Image m_portraitFrameImage;

	public Image m_portraitImage;

	public Image m_qualityColorImage;

	public Image m_portraitRingImage;

	public Image m_levelBorderImage;

	public Text m_levelText;

	public GameObject m_abilityAreaRootObject;

	public MissionDetailView m_missionDetailView;

	public GameObject m_missionMechanicCounterPrefab;

	public bool m_disableButtonWhenFollowerAssigned;

	private GarrFollowerRec m_garrFollowerRec;

	private JamGarrisonFollower m_follower;

	public GameObject m_heartPanel;

	public GameObject m_heartArea;

	public GameObject m_enemyPortraitsGroup;

	public GameObject m_fullHeartPrefab;

	public GameObject m_emptyHeartPrefab;

	public GameObject m_outlineHeartPrefab;

	private bool isOccupied;

	private int currentGarrFollowerID;

	private void Awake()
	{
		this.ClearFollower();
	}

	private void Start()
	{
		if (this.m_missionDetailView != null)
		{
			MissionDetailView expr_17 = this.m_missionDetailView;
			expr_17.FollowerSlotsChangedAction = (Action)Delegate.Combine(expr_17.FollowerSlotsChangedAction, new Action(this.InitHeartPanel));
		}
	}

	private void OnDestroy()
	{
		if (this.m_missionDetailView != null)
		{
			MissionDetailView expr_17 = this.m_missionDetailView;
			expr_17.FollowerSlotsChangedAction = (Action)Delegate.Remove(expr_17.FollowerSlotsChangedAction, new Action(this.InitHeartPanel));
		}
	}

	private void Update()
	{
	}

	public bool IsOccupied()
	{
		return this.isOccupied;
	}

	public int GetCurrentGarrFollowerID()
	{
		return this.currentGarrFollowerID;
	}

	public void OnTapCombatAllySlot()
	{
		if (this.m_missionDetailView != null && this.m_missionDetailView.GetCombatAllyMissionState() == CombatAllyMissionState.available)
		{
			this.ClearFollower();
		}
	}

	public void OnTapFollowerSlot()
	{
		if (this.m_missionDetailView != null)
		{
			this.ClearFollower();
		}
	}

	public void ClearFollower()
	{
		if (this.currentGarrFollowerID != 0)
		{
			GameObject gameObject = base.get_transform().get_parent().get_parent().get_parent().get_parent().get_gameObject();
			gameObject.BroadcastMessage("RemoveFromParty", this.currentGarrFollowerID, 1);
		}
		this.SetFollower(0);
		if (this.m_disableButtonWhenFollowerAssigned)
		{
			this.m_portraitFrameImage.GetComponent<Image>().set_enabled(true);
		}
	}

	public void SetFollower(int garrFollowerID)
	{
		this.m_follower = null;
		this.m_garrFollowerRec = null;
		if (this.m_abilityAreaRootObject != null)
		{
			RectTransform[] componentsInChildren = this.m_abilityAreaRootObject.GetComponentsInChildren<RectTransform>(true);
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != null && componentsInChildren[i] != this.m_abilityAreaRootObject.get_transform())
					{
						Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
					}
				}
			}
		}
		if (garrFollowerID == 0)
		{
			if (this.m_portraitRingImage != null)
			{
				this.m_portraitRingImage.get_gameObject().SetActive(false);
			}
			if (this.m_heartPanel != null)
			{
				this.m_heartPanel.SetActive(false);
			}
			this.m_levelBorderImage.get_gameObject().SetActive(false);
			this.m_portraitImage.get_gameObject().SetActive(false);
			this.m_qualityColorImage.get_gameObject().SetActive(false);
			this.m_levelBorderImage.set_color(Color.get_white());
			this.m_levelText.get_gameObject().SetActive(false);
			this.isOccupied = false;
			this.m_portraitFrameImage.set_enabled(true);
			if (this.currentGarrFollowerID != garrFollowerID)
			{
				AdventureMapPanel.instance.MissionFollowerSlotChanged(this.currentGarrFollowerID, false);
			}
			bool flag = this.currentGarrFollowerID != 0;
			this.currentGarrFollowerID = 0;
			if (flag && this.m_missionDetailView != null)
			{
				this.m_missionDetailView.UpdateMissionStatus();
			}
			if (this.m_disableButtonWhenFollowerAssigned)
			{
				this.m_portraitFrameImage.GetComponent<Image>().set_enabled(true);
			}
			if (this.m_missionDetailView != null)
			{
				this.m_missionDetailView.NotifyFollowerSlotsChanged();
			}
			return;
		}
		this.m_portraitRingImage.get_gameObject().SetActive(true);
		this.m_levelBorderImage.get_gameObject().SetActive(true);
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(garrFollowerID);
		if (record == null)
		{
			return;
		}
		if (record.GarrFollowerTypeID != 4u)
		{
			return;
		}
		MissionMechanic[] mechanics = base.get_gameObject().get_transform().get_parent().get_parent().get_parent().get_gameObject().GetComponentsInChildren<MissionMechanic>(true);
		if (mechanics == null)
		{
			return;
		}
		JamGarrisonFollower jamGarrisonFollower = PersistentFollowerData.followerDictionary.get_Item(garrFollowerID);
		float num = 0f;
		if (this.m_missionDetailView != null)
		{
			num = MissionDetailView.ComputeFollowerBias(jamGarrisonFollower, jamGarrisonFollower.FollowerLevel, (jamGarrisonFollower.ItemLevelWeapon + jamGarrisonFollower.ItemLevelArmor) / 2, this.m_missionDetailView.GetCurrentMissionID());
		}
		if (num == -1f)
		{
			this.m_levelText.set_color(Color.get_red());
		}
		else if (num < 0f)
		{
			this.m_levelText.set_color(new Color(0.9333f, 0.4392f, 0.2117f));
		}
		else
		{
			this.m_levelText.set_color(Color.get_white());
		}
		if (this.m_abilityAreaRootObject != null && num > -1f)
		{
			for (int j = 0; j < jamGarrisonFollower.AbilityID.Length; j++)
			{
				GarrAbilityRec garrAbilityRec = StaticDB.garrAbilityDB.GetRecord(jamGarrisonFollower.AbilityID[j]);
				if ((garrAbilityRec.Flags & 1u) == 0u)
				{
					StaticDB.garrAbilityEffectDB.EnumRecordsByParentID(garrAbilityRec.ID, delegate(GarrAbilityEffectRec garrAbilityEffectRec)
					{
						if (garrAbilityEffectRec.GarrMechanicTypeID == 0u)
						{
							return true;
						}
						if (garrAbilityEffectRec.AbilityAction != 0u)
						{
							return true;
						}
						GarrMechanicTypeRec record2 = StaticDB.garrMechanicTypeDB.GetRecord((int)garrAbilityEffectRec.GarrMechanicTypeID);
						if (record2 == null)
						{
							return true;
						}
						bool flag3 = false;
						for (int k = 0; k < mechanics.Length; k++)
						{
							if (mechanics[k].m_missionMechanicTypeID == record2.ID)
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							return true;
						}
						GameObject gameObject = Object.Instantiate<GameObject>(this.m_missionMechanicCounterPrefab);
						gameObject.get_transform().SetParent(this.m_abilityAreaRootObject.get_transform(), false);
						MissionMechanicTypeCounter component = gameObject.GetComponent<MissionMechanicTypeCounter>();
						component.usedIcon.get_gameObject().SetActive(false);
						Sprite sprite2 = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, garrAbilityRec.IconFileDataID);
						if (sprite2 != null)
						{
							component.missionMechanicIcon.set_sprite(sprite2);
						}
						component.countersMissionMechanicTypeID = record2.ID;
						return false;
					});
				}
			}
		}
		this.m_levelText.get_gameObject().SetActive(true);
		if (jamGarrisonFollower.FollowerLevel < 110)
		{
			this.m_levelText.set_text(StaticDB.GetString("LEVEL", null) + " " + jamGarrisonFollower.FollowerLevel);
		}
		else
		{
			this.m_levelText.set_text(StaticDB.GetString("ILVL", null) + " " + (jamGarrisonFollower.ItemLevelArmor + jamGarrisonFollower.ItemLevelWeapon) / 2);
		}
		this.m_portraitImage.get_gameObject().SetActive(true);
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.PortraitIcons, (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record.AllianceIconFileDataID : record.HordeIconFileDataID);
		if (sprite != null)
		{
			this.m_portraitImage.set_sprite(sprite);
		}
		Color qualityColor = GeneralHelpers.GetQualityColor(jamGarrisonFollower.Quality);
		this.m_qualityColorImage.set_color(qualityColor);
		this.m_levelBorderImage.set_color(qualityColor);
		this.isOccupied = true;
		bool flag2 = (jamGarrisonFollower.Flags & 8) != 0;
		this.m_qualityColorImage.get_gameObject().SetActive(!flag2);
		this.m_levelBorderImage.get_gameObject().SetActive(!flag2);
		if (this.m_heartPanel != null)
		{
			this.m_heartPanel.SetActive(flag2);
			if (flag2)
			{
				this.m_garrFollowerRec = record;
				this.m_follower = jamGarrisonFollower;
			}
		}
		this.m_portraitFrameImage.set_enabled(!flag2);
		this.currentGarrFollowerID = garrFollowerID;
		if (this.m_disableButtonWhenFollowerAssigned)
		{
			this.m_portraitFrameImage.GetComponent<Image>().set_enabled(false);
		}
		if (this.m_missionDetailView != null)
		{
			this.m_missionDetailView.UpdateMissionStatus();
			this.m_missionDetailView.NotifyFollowerSlotsChanged();
		}
	}

	public void InitHeartPanel()
	{
		if (this.m_heartPanel == null || this.m_follower == null || this.m_garrFollowerRec == null)
		{
			return;
		}
		Image[] componentsInChildren = this.m_heartArea.GetComponentsInChildren<Image>(true);
		Image[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Image image = array[i];
			if (image != null && image.get_gameObject() != this.m_heartArea)
			{
				Object.DestroyImmediate(image.get_gameObject());
			}
		}
		int num = 1;
		if (GeneralHelpers.MissionHasUncounteredDeadly(this.m_enemyPortraitsGroup))
		{
			num = this.m_follower.Durability;
		}
		for (int j = 0; j < this.m_follower.Durability - num; j++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.m_fullHeartPrefab);
			gameObject.get_transform().SetParent(this.m_heartArea.get_transform(), false);
		}
		for (int k = 0; k < num; k++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_outlineHeartPrefab);
			gameObject2.get_transform().SetParent(this.m_heartArea.get_transform(), false);
		}
		for (int l = 0; l < this.m_garrFollowerRec.Vitality - this.m_follower.Durability; l++)
		{
			GameObject gameObject3 = Object.Instantiate<GameObject>(this.m_emptyHeartPrefab);
			gameObject3.get_transform().SetParent(this.m_heartArea.get_transform(), false);
		}
	}

	public void PlayUnslotSound()
	{
		Main.instance.m_UISound.Play_DefaultNavClick();
	}
}
