using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowJamMessages.MobilePlayerJSON;
using WowStatConstants;
using WowStaticData;

public class MissionResultsPanel : MonoBehaviour
{
	public GameObject m_darknessBG;

	public GameObject m_popupView;

	public GameObject missionFollowerSlotGroup;

	public GameObject enemyPortraitsGroup;

	public GameObject treasureChestHorde;

	public GameObject treasureChestAlliance;

	public GameObject missionEncounterPrefab;

	public Text missionNameText;

	public Text missionLocationText;

	public Text missioniLevelText;

	public Image missionTypeImage;

	public GameObject missionFollowerSlotPrefab;

	public Image m_scrollingEnvironment_Back;

	public Image m_scrollingEnvironment_Mid;

	public Image m_scrollingEnvironment_Fore;

	public GameObject m_lootGroupObj;

	public MissionRewardDisplay m_missionRewardDisplayPrefab;

	public GameObject m_mechanicEffectDisplayPrefab;

	public Text missionPercentChanceText;

	public GameObject m_missionChanceSpinner;

	public GameObject m_partyBuffGroup;

	public Text m_partyBuffsText;

	public GameObject m_lootBorderNormal;

	public GameObject m_lootBorderLitUp;

	public GameObject m_missionSuccessMessage;

	public GameObject m_missionFailMessage;

	public GameObject m_missionInProgressMessage;

	public Text m_missionTimeRemainingText;

	public float m_messageTimeToDelayEntrance;

	public float m_messageFadeInTime;

	public bool m_messagePunchScale;

	public float m_messagePunchScaleAmount;

	public float m_messagePunchScaleDuration;

	public float m_lootEffectInitialDelay;

	public float m_lootEffectDelay;

	public Text m_okButtonText;

	public Text m_inProgressText;

	public Text m_successText;

	public Text m_failureText;

	[Header("Bonus Loot")]
	public GameObject m_bonusLootDisplay;

	public Text m_bonusLootChanceText;

	public MissionRewardDisplay m_bonusMissionRewardDisplay;

	[Header("XP Display")]
	public GameObject m_followerExperienceDisplayArea;

	public GameObject m_followerExperienceDisplayPrefab;

	public float m_missionDetailsFadeOutDelay;

	public float m_experienceDisplayInitialEntranceDelay;

	public float m_experienceDisplayEntranceDelay;

	public AutoFadeOut m_missionResultsDisplayCanvasGroupAutoFadeOut;

	private MissionResultType m_currentResultType;

	private FancyEntrance m_fancyEntrance;

	private long m_missionStartedTime;

	private long m_missionDurationInSeconds;

	private int m_garrMissionID;

	private bool m_attemptedAutoComplete;

	private float m_timeUntilFadeOutMissionDetailsDisplay;

	private float m_timeUntilShowFollowerExperienceDisplays;

	private void Awake()
	{
		this.m_attemptedAutoComplete = false;
		this.m_darknessBG.SetActive(false);
		this.m_popupView.SetActive(false);
		if (this.m_partyBuffsText != null)
		{
			this.m_partyBuffsText.set_font(GeneralHelpers.LoadStandardFont());
			this.m_partyBuffsText.set_text(StaticDB.GetString("PARTY_BUFFS", null));
		}
		if (this.m_bonusLootChanceText != null)
		{
			this.m_bonusLootChanceText.set_font(GeneralHelpers.LoadStandardFont());
		}
	}

	private void OnEnable()
	{
		this.m_missionSuccessMessage.SetActive(false);
		this.m_missionFailMessage.SetActive(false);
		this.m_missionInProgressMessage.SetActive(false);
		if (Main.instance != null)
		{
			Main expr_39 = Main.instance;
			expr_39.MissionSuccessChanceChangedAction = (Action<int>)Delegate.Combine(expr_39.MissionSuccessChanceChangedAction, new Action<int>(this.OnMissionSuccessChanceChanged));
			Main expr_5F = Main.instance;
			expr_5F.FollowerDataChangedAction = (Action)Delegate.Combine(expr_5F.FollowerDataChangedAction, new Action(this.HandleFollowerDataChanged));
		}
		if (AdventureMapPanel.instance != null)
		{
			AdventureMapPanel expr_95 = AdventureMapPanel.instance;
			expr_95.ShowMissionResultAction = (Action<int, int, bool>)Delegate.Combine(expr_95.ShowMissionResultAction, new Action<int, int, bool>(this.ShowMissionResults));
		}
		this.m_okButtonText.set_text(StaticDB.GetString("OK", null));
		this.m_inProgressText.set_text(StaticDB.GetString("IN_PROGRESS", null));
		this.m_successText.set_text(StaticDB.GetString("MISSION_SUCCESS", null));
		this.m_failureText.set_text(StaticDB.GetString("MISSION_FAILED", null));
	}

	private void OnDisable()
	{
		if (Main.instance != null)
		{
			Main expr_15 = Main.instance;
			expr_15.MissionSuccessChanceChangedAction = (Action<int>)Delegate.Remove(expr_15.MissionSuccessChanceChangedAction, new Action<int>(this.OnMissionSuccessChanceChanged));
			Main expr_3B = Main.instance;
			expr_3B.FollowerDataChangedAction = (Action)Delegate.Remove(expr_3B.FollowerDataChangedAction, new Action(this.HandleFollowerDataChanged));
		}
		if (AdventureMapPanel.instance != null)
		{
			AdventureMapPanel expr_71 = AdventureMapPanel.instance;
			expr_71.ShowMissionResultAction = (Action<int, int, bool>)Delegate.Remove(expr_71.ShowMissionResultAction, new Action<int, int, bool>(this.ShowMissionResults));
		}
	}

	private void UpdateMissionRemainingTimeDisplay()
	{
		if (!this.m_missionInProgressMessage.get_activeSelf())
		{
			return;
		}
		long num = GarrisonStatus.CurrentTime() - this.m_missionStartedTime;
		long num2 = this.m_missionDurationInSeconds - num;
		bool flag = num2 < 0L && this.m_popupView.get_gameObject().get_activeSelf();
		num2 = ((num2 <= 0L) ? 0L : num2);
		Duration duration = new Duration((int)num2);
		this.m_missionTimeRemainingText.set_text(duration.DurationString);
		if (flag && !this.m_attemptedAutoComplete)
		{
			if (AdventureMapPanel.instance.ShowMissionResultAction != null)
			{
				AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_garrMissionID, 1, false);
			}
			Main.instance.CompleteMission(this.m_garrMissionID);
			this.m_attemptedAutoComplete = true;
		}
	}

	private void Update()
	{
		this.UpdateMissionRemainingTimeDisplay();
		if (!this.m_followerExperienceDisplayArea.get_activeSelf() && (this.m_currentResultType == MissionResultType.success || this.m_currentResultType == MissionResultType.failure))
		{
			this.m_timeUntilFadeOutMissionDetailsDisplay -= Time.get_deltaTime();
			if (this.m_timeUntilFadeOutMissionDetailsDisplay < 0f)
			{
				this.m_missionResultsDisplayCanvasGroupAutoFadeOut.EnableFadeOut();
			}
			this.m_timeUntilShowFollowerExperienceDisplays -= Time.get_deltaTime();
			if (this.m_timeUntilShowFollowerExperienceDisplays < 0f)
			{
				this.m_followerExperienceDisplayArea.SetActive(true);
			}
		}
	}

	public void UpdateMissionStatus(int garrMissionID)
	{
		MissionMechanic[] componentsInChildren = this.enemyPortraitsGroup.GetComponentsInChildren<MissionMechanic>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetCountered(false, false, true);
		}
		AbilityDisplay[] componentsInChildren2 = this.enemyPortraitsGroup.GetComponentsInChildren<AbilityDisplay>(true);
		if (componentsInChildren2 == null)
		{
			return;
		}
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].SetCountered(false, true);
		}
		MissionMechanicTypeCounter[] componentsInChildren3 = base.get_gameObject().GetComponentsInChildren<MissionMechanicTypeCounter>(true);
		if (componentsInChildren3 == null)
		{
			return;
		}
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].usedIcon.get_gameObject().SetActive(false);
			for (int l = 0; l < componentsInChildren.Length; l++)
			{
				if (componentsInChildren3[k].countersMissionMechanicTypeID == componentsInChildren[l].m_missionMechanicTypeID && !componentsInChildren[l].IsCountered())
				{
					componentsInChildren[l].SetCountered(true, false, false);
					componentsInChildren2[l].SetCountered(true, false);
					break;
				}
			}
		}
		MissionFollowerSlot[] componentsInChildren4 = base.get_gameObject().GetComponentsInChildren<MissionFollowerSlot>(true);
		List<JamGarrisonFollower> list = new List<JamGarrisonFollower>();
		for (int m = 0; m < componentsInChildren4.Length; m++)
		{
			int currentGarrFollowerID = componentsInChildren4[m].GetCurrentGarrFollowerID();
			if (PersistentFollowerData.followerDictionary.ContainsKey(currentGarrFollowerID))
			{
				JamGarrisonFollower jamGarrisonFollower = PersistentFollowerData.followerDictionary.get_Item(currentGarrFollowerID);
				list.Add(jamGarrisonFollower);
			}
		}
		int chance = -1000;
		if (MissionDataCache.missionDataDictionary.ContainsKey(this.m_garrMissionID))
		{
			chance = (int)MissionDataCache.missionDataDictionary.get_Item(this.m_garrMissionID);
		}
		else
		{
			MobilePlayerEvaluateMission mobilePlayerEvaluateMission = new MobilePlayerEvaluateMission();
			mobilePlayerEvaluateMission.GarrMissionID = garrMissionID;
			mobilePlayerEvaluateMission.GarrFollowerID = new int[list.get_Count()];
			int num = 0;
			using (List<JamGarrisonFollower>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JamGarrisonFollower current = enumerator.get_Current();
					mobilePlayerEvaluateMission.GarrFollowerID[num++] = current.GarrFollowerID;
				}
			}
			Login.instance.SendToMobileServer(mobilePlayerEvaluateMission);
		}
		this.OnMissionSuccessChanceChanged(chance);
	}

	private void OnMissionSuccessChanceChanged(int chance)
	{
		if (this.m_garrMissionID == 0)
		{
			return;
		}
		if (!base.get_gameObject().get_activeSelf())
		{
			return;
		}
		this.m_bonusLootDisplay.SetActive(false);
		if (chance <= -1000)
		{
			this.missionPercentChanceText.set_text("%");
			this.m_missionChanceSpinner.SetActive(true);
		}
		else
		{
			this.missionPercentChanceText.set_text(chance + "%");
			this.m_missionChanceSpinner.SetActive(false);
		}
		this.m_lootBorderNormal.SetActive(chance < 100);
		this.m_lootBorderLitUp.SetActive(chance >= 100);
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(this.m_garrMissionID);
		if (record == null)
		{
			Debug.LogError("Invalid Mission ID:" + this.m_garrMissionID);
			return;
		}
		if (StaticDB.rewardPackDB.GetRecord(record.OvermaxRewardPackID) == null)
		{
			return;
		}
		if (record.OvermaxRewardPackID > 0)
		{
			this.m_bonusLootDisplay.SetActive(true);
			this.m_bonusLootChanceText.set_text(string.Concat(new object[]
			{
				"<color=#ffff00ff>",
				StaticDB.GetString("BONUS_ROLL", null),
				" </color>\n<color=#ff8600ff>",
				Math.Max(0, chance - 100),
				"%</color>"
			}));
		}
	}

	public void ShowMissionResults(int garrMissionID, int missionResultType, bool awardOvermax)
	{
		this.m_missionResultsDisplayCanvasGroupAutoFadeOut.Reset();
		this.m_currentResultType = (MissionResultType)missionResultType;
		this.m_followerExperienceDisplayArea.SetActive(false);
		this.m_attemptedAutoComplete = false;
		this.m_garrMissionID = garrMissionID;
		this.m_darknessBG.SetActive(true);
		this.m_popupView.SetActive(true);
		this.m_bonusLootDisplay.SetActive(false);
		if (this.missionFollowerSlotGroup != null)
		{
			MissionFollowerSlot[] componentsInChildren = this.missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i] != this.missionFollowerSlotGroup.get_transform())
				{
					Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
				}
			}
		}
		MissionEncounter[] componentsInChildren2 = this.enemyPortraitsGroup.GetComponentsInChildren<MissionEncounter>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j] != null && componentsInChildren2[j] != this.enemyPortraitsGroup.get_transform())
			{
				Object.DestroyImmediate(componentsInChildren2[j].get_gameObject());
			}
		}
		if (this.treasureChestHorde != null && this.treasureChestAlliance != null)
		{
			if (GarrisonStatus.Faction() == PVP_FACTION.HORDE)
			{
				this.treasureChestHorde.SetActive(true);
				this.treasureChestAlliance.SetActive(false);
			}
			else
			{
				this.treasureChestHorde.SetActive(false);
				this.treasureChestAlliance.SetActive(true);
			}
		}
		JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(garrMissionID);
		this.m_missionStartedTime = jamGarrisonMobileMission.StartTime;
		this.m_missionDurationInSeconds = jamGarrisonMobileMission.MissionDuration;
		for (int k = 0; k < jamGarrisonMobileMission.Encounter.Length; k++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.missionEncounterPrefab);
			gameObject.get_transform().SetParent(this.enemyPortraitsGroup.get_transform(), false);
			MissionEncounter component = gameObject.GetComponent<MissionEncounter>();
			int garrMechanicID = (jamGarrisonMobileMission.Encounter[k].MechanicID.Length <= 0) ? 0 : jamGarrisonMobileMission.Encounter[k].MechanicID[0];
			component.SetEncounter(jamGarrisonMobileMission.Encounter[k].EncounterID, garrMechanicID);
		}
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		this.missionNameText.set_text(record.Name);
		this.missionLocationText.set_text(record.Location);
		this.missioniLevelText.set_text(StaticDB.GetString("ITEM_LEVEL_ABBREVIATION", null) + " " + record.TargetItemLevel);
		GarrMissionTypeRec record2 = StaticDB.garrMissionTypeDB.GetRecord((int)record.GarrMissionTypeID);
		this.missionTypeImage.set_overrideSprite(TextureAtlas.instance.GetAtlasSprite((int)record2.UiTextureAtlasMemberID));
		if (this.missionFollowerSlotGroup != null)
		{
			int num = 0;
			while ((long)num < (long)((ulong)record.MaxFollowers))
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.missionFollowerSlotPrefab);
				gameObject2.get_transform().SetParent(this.missionFollowerSlotGroup.get_transform(), false);
				MissionFollowerSlot component2 = gameObject2.GetComponent<MissionFollowerSlot>();
				component2.m_enemyPortraitsGroup = this.enemyPortraitsGroup;
				num++;
			}
		}
		if (record.UiTextureKitID > 0u)
		{
			UiTextureKitRec record3 = StaticDB.uiTextureKitDB.GetRecord((int)record.UiTextureKitID);
			this.m_scrollingEnvironment_Back.set_enabled(false);
			this.m_scrollingEnvironment_Mid.set_enabled(false);
			this.m_scrollingEnvironment_Fore.set_enabled(false);
			int uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("_" + record3.KitPrefix + "-Back");
			if (uITextureAtlasMemberID > 0)
			{
				Sprite atlasSprite = TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID);
				if (atlasSprite != null)
				{
					this.m_scrollingEnvironment_Back.set_enabled(true);
					this.m_scrollingEnvironment_Back.set_sprite(atlasSprite);
				}
			}
			int uITextureAtlasMemberID2 = TextureAtlas.GetUITextureAtlasMemberID("_" + record3.KitPrefix + "-Mid");
			if (uITextureAtlasMemberID2 > 0)
			{
				Sprite atlasSprite2 = TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID2);
				if (atlasSprite2 != null)
				{
					this.m_scrollingEnvironment_Mid.set_enabled(true);
					this.m_scrollingEnvironment_Mid.set_sprite(atlasSprite2);
				}
			}
			int uITextureAtlasMemberID3 = TextureAtlas.GetUITextureAtlasMemberID("_" + record3.KitPrefix + "-Fore");
			if (uITextureAtlasMemberID3 > 0)
			{
				Sprite atlasSprite3 = TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID3);
				if (atlasSprite3 != null)
				{
					this.m_scrollingEnvironment_Fore.set_enabled(true);
					this.m_scrollingEnvironment_Fore.set_sprite(atlasSprite3);
				}
			}
		}
		else
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"DATA ERROR: Mission UITextureKit Not Set for mission ID:",
				record.ID,
				" - ",
				record.Name
			}));
			Debug.LogWarning("This means the scrolling background images will show the wrong location");
		}
		if (this.m_lootGroupObj == null || this.m_missionRewardDisplayPrefab == null)
		{
			return;
		}
		MissionRewardDisplay[] componentsInChildren3 = this.m_lootGroupObj.GetComponentsInChildren<MissionRewardDisplay>(true);
		for (int l = 0; l < componentsInChildren3.Length; l++)
		{
			if (componentsInChildren3[l] != null)
			{
				Object.DestroyImmediate(componentsInChildren3[l].get_gameObject());
			}
		}
		MissionFollowerSlot[] componentsInChildren4 = this.missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
		int num2 = 0;
		using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator = PersistentFollowerData.followerDictionary.get_Values().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonFollower current = enumerator.get_Current();
				if (current.CurrentMissionID == garrMissionID)
				{
					componentsInChildren4[num2++].SetFollower(current.GarrFollowerID);
					if (missionResultType == 1)
					{
						PersistentFollowerData.CachePreMissionFollower(current);
					}
				}
			}
		}
		this.UpdateMissionStatus(garrMissionID);
		MissionFollowerSlot[] array = componentsInChildren4;
		for (int m = 0; m < array.Length; m++)
		{
			MissionFollowerSlot missionFollowerSlot = array[m];
			missionFollowerSlot.InitHeartPanel();
		}
		MissionRewardDisplay.InitMissionRewards(this.m_missionRewardDisplayPrefab.get_gameObject(), this.m_lootGroupObj.get_transform(), jamGarrisonMobileMission.Reward);
		if (record.OvermaxRewardPackID > 0 && jamGarrisonMobileMission.OvermaxReward.Length > 0)
		{
			this.m_bonusLootDisplay.SetActive(true);
			JamGarrisonMissionReward jamGarrisonMissionReward = jamGarrisonMobileMission.OvermaxReward[0];
			if (jamGarrisonMissionReward.ItemID > 0)
			{
				this.m_bonusMissionRewardDisplay.InitReward(MissionRewardDisplay.RewardType.item, jamGarrisonMissionReward.ItemID, (int)jamGarrisonMissionReward.ItemQuantity, 0, jamGarrisonMissionReward.ItemFileDataID);
			}
			else if (jamGarrisonMissionReward.FollowerXP > 0u)
			{
				this.m_bonusMissionRewardDisplay.InitReward(MissionRewardDisplay.RewardType.followerXP, 0, (int)jamGarrisonMissionReward.FollowerXP, 0, 0);
			}
			else if (jamGarrisonMissionReward.CurrencyQuantity > 0u)
			{
				if (jamGarrisonMissionReward.CurrencyType == 0)
				{
					this.m_bonusMissionRewardDisplay.InitReward(MissionRewardDisplay.RewardType.gold, 0, (int)(jamGarrisonMissionReward.CurrencyQuantity / 10000u), 0, 0);
				}
				else
				{
					CurrencyTypesRec record4 = StaticDB.currencyTypesDB.GetRecord(jamGarrisonMissionReward.CurrencyType);
					int rewardQuantity = (int)((ulong)jamGarrisonMissionReward.CurrencyQuantity / (ulong)(((record4.Flags & 8u) == 0u) ? 1L : 100L));
					this.m_bonusMissionRewardDisplay.InitReward(MissionRewardDisplay.RewardType.currency, jamGarrisonMissionReward.CurrencyType, rewardQuantity, 0, 0);
				}
			}
		}
		this.m_timeUntilFadeOutMissionDetailsDisplay = this.m_missionDetailsFadeOutDelay;
		this.m_timeUntilShowFollowerExperienceDisplays = this.m_experienceDisplayInitialEntranceDelay;
		if (missionResultType == 2)
		{
			this.InitFollowerExperienceDisplays();
			Main.instance.m_UISound.Play_MissionSuccess();
			this.m_missionInProgressMessage.SetActive(false);
			this.m_missionSuccessMessage.SetActive(true);
			this.m_missionFailMessage.SetActive(false);
			if (this.m_fancyEntrance != null)
			{
				Object.DestroyImmediate(this.m_fancyEntrance);
				iTween.Stop(this.m_missionSuccessMessage);
				this.m_missionSuccessMessage.get_transform().set_localScale(Vector3.get_one());
				iTween.Stop(this.m_missionFailMessage);
				this.m_missionFailMessage.get_transform().set_localScale(Vector3.get_one());
			}
			this.m_missionSuccessMessage.SetActive(false);
			this.m_fancyEntrance = this.m_missionSuccessMessage.AddComponent<FancyEntrance>();
			this.m_fancyEntrance.m_fadeInCanvasGroup = this.m_missionSuccessMessage.GetComponent<CanvasGroup>();
			this.m_fancyEntrance.m_fadeInTime = this.m_messageFadeInTime;
			this.m_fancyEntrance.m_punchScale = this.m_messagePunchScale;
			this.m_fancyEntrance.m_punchScaleAmount = this.m_messagePunchScaleAmount;
			this.m_fancyEntrance.m_punchScaleDuration = this.m_messagePunchScaleDuration;
			this.m_fancyEntrance.m_timeToDelayEntrance = this.m_messageTimeToDelayEntrance;
			this.m_missionSuccessMessage.SetActive(true);
			MissionRewardDisplay[] componentsInChildren5 = this.m_lootGroupObj.GetComponentsInChildren<MissionRewardDisplay>(true);
			for (int n = 0; n < componentsInChildren5.Length; n++)
			{
				componentsInChildren5[n].ShowResultSuccess(this.m_lootEffectInitialDelay + this.m_lootEffectDelay * (float)n);
			}
			if (awardOvermax)
			{
				this.m_bonusMissionRewardDisplay.ShowResultSuccess(this.m_lootEffectInitialDelay + this.m_lootEffectDelay * (float)componentsInChildren5.Length);
			}
			else
			{
				this.m_bonusMissionRewardDisplay.ShowResultFail(this.m_lootEffectInitialDelay + this.m_lootEffectDelay * (float)componentsInChildren5.Length);
			}
		}
		if (missionResultType == 3)
		{
			this.InitFollowerExperienceDisplays();
			Main.instance.m_UISound.Play_MissionFailure();
			this.m_missionInProgressMessage.SetActive(false);
			this.m_missionSuccessMessage.SetActive(false);
			this.m_missionFailMessage.SetActive(true);
			if (this.m_fancyEntrance != null)
			{
				Object.DestroyImmediate(this.m_fancyEntrance);
				iTween.Stop(this.m_missionSuccessMessage);
				this.m_missionSuccessMessage.get_transform().set_localScale(Vector3.get_one());
				iTween.Stop(this.m_missionFailMessage);
				this.m_missionFailMessage.get_transform().set_localScale(Vector3.get_one());
			}
			this.m_missionFailMessage.SetActive(false);
			this.m_fancyEntrance = this.m_missionFailMessage.AddComponent<FancyEntrance>();
			this.m_fancyEntrance.m_fadeInCanvasGroup = this.m_missionFailMessage.GetComponent<CanvasGroup>();
			this.m_fancyEntrance.m_fadeInTime = this.m_messageFadeInTime;
			this.m_fancyEntrance.m_punchScale = this.m_messagePunchScale;
			this.m_fancyEntrance.m_punchScaleAmount = this.m_messagePunchScaleAmount;
			this.m_fancyEntrance.m_punchScaleDuration = this.m_messagePunchScaleDuration;
			this.m_fancyEntrance.m_timeToDelayEntrance = this.m_messageTimeToDelayEntrance;
			this.m_missionFailMessage.SetActive(true);
			MissionRewardDisplay[] componentsInChildren6 = this.m_lootGroupObj.GetComponentsInChildren<MissionRewardDisplay>(true);
			for (int num3 = 0; num3 < componentsInChildren6.Length; num3++)
			{
				componentsInChildren6[num3].ShowResultFail(this.m_lootEffectInitialDelay + this.m_lootEffectDelay * (float)num3);
			}
			this.m_bonusMissionRewardDisplay.ShowResultFail(this.m_lootEffectInitialDelay + this.m_lootEffectDelay * (float)componentsInChildren6.Length);
		}
		if (missionResultType == 0)
		{
			this.m_missionInProgressMessage.SetActive(true);
			this.m_missionSuccessMessage.SetActive(false);
			this.m_missionFailMessage.SetActive(false);
			this.m_bonusMissionRewardDisplay.ClearResults();
		}
		if (missionResultType == 1)
		{
			this.m_missionInProgressMessage.SetActive(false);
			this.m_missionSuccessMessage.SetActive(false);
			this.m_missionFailMessage.SetActive(false);
			FollowerExperienceDisplay[] componentsInChildren7 = this.m_followerExperienceDisplayArea.GetComponentsInChildren<FollowerExperienceDisplay>(true);
			FollowerExperienceDisplay[] array2 = componentsInChildren7;
			for (int num4 = 0; num4 < array2.Length; num4++)
			{
				FollowerExperienceDisplay followerExperienceDisplay = array2[num4];
				Object.DestroyImmediate(followerExperienceDisplay.get_gameObject());
			}
		}
		if (this.m_partyBuffGroup != null)
		{
			AbilityDisplay[] componentsInChildren8 = this.m_partyBuffGroup.GetComponentsInChildren<AbilityDisplay>(true);
			AbilityDisplay[] array3 = componentsInChildren8;
			for (int num5 = 0; num5 < array3.Length; num5++)
			{
				AbilityDisplay abilityDisplay = array3[num5];
				Object.DestroyImmediate(abilityDisplay.get_gameObject());
			}
		}
		int num6 = 0;
		using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator2 = PersistentFollowerData.followerDictionary.get_Values().GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				JamGarrisonFollower current2 = enumerator2.get_Current();
				if (current2.CurrentMissionID == garrMissionID)
				{
					int[] buffsForCurrentMission = GeneralHelpers.GetBuffsForCurrentMission(current2.GarrFollowerID, garrMissionID, this.missionFollowerSlotGroup);
					num6 += buffsForCurrentMission.Length;
					int[] array4 = buffsForCurrentMission;
					for (int num7 = 0; num7 < array4.Length; num7++)
					{
						int garrAbilityID = array4[num7];
						GameObject gameObject3 = Object.Instantiate<GameObject>(this.m_mechanicEffectDisplayPrefab);
						gameObject3.get_transform().SetParent(this.m_partyBuffGroup.get_transform(), false);
						AbilityDisplay component3 = gameObject3.GetComponent<AbilityDisplay>();
						component3.SetAbility(garrAbilityID, false, false, null);
					}
				}
			}
		}
		this.m_partyBuffGroup.SetActive(num6 > 0);
	}

	public void OnPartyBuffSectionTapped()
	{
		List<int> list = new List<int>();
		MissionFollowerSlot[] componentsInChildren = this.missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
		MissionFollowerSlot[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			MissionFollowerSlot missionFollowerSlot = array[i];
			int currentGarrFollowerID = missionFollowerSlot.GetCurrentGarrFollowerID();
			if (currentGarrFollowerID != 0)
			{
				int[] buffsForCurrentMission = GeneralHelpers.GetBuffsForCurrentMission(currentGarrFollowerID, this.m_garrMissionID, this.missionFollowerSlotGroup);
				int[] array2 = buffsForCurrentMission;
				for (int j = 0; j < array2.Length; j++)
				{
					int num = array2[j];
					list.Add(num);
				}
			}
		}
		AllPopups.instance.ShowPartyBuffsPopup(list.ToArray());
	}

	private void InitFollowerExperienceDisplays()
	{
		int num = 0;
		using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator = PersistentFollowerData.preMissionFollowerDictionary.get_Values().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonFollower current = enumerator.get_Current();
				if (current.CurrentMissionID == this.m_garrMissionID)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_followerExperienceDisplayPrefab);
					FollowerExperienceDisplay component = gameObject.GetComponent<FollowerExperienceDisplay>();
					FancyEntrance component2 = gameObject.GetComponent<FancyEntrance>();
					float num2 = (float)num * this.m_experienceDisplayEntranceDelay;
					component2.m_timeToDelayEntrance = num2;
					component2.Activate();
					component.SetFollower(current, current, num2);
					component.get_transform().SetParent(this.m_followerExperienceDisplayArea.get_transform(), false);
					num++;
				}
			}
		}
	}

	public void HandleFollowerDataChanged()
	{
		if (!this.m_popupView.get_activeSelf())
		{
			return;
		}
		FollowerExperienceDisplay[] componentsInChildren = this.m_followerExperienceDisplayArea.GetComponentsInChildren<FollowerExperienceDisplay>(true);
		int num = 0;
		FollowerExperienceDisplay[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			FollowerExperienceDisplay followerExperienceDisplay = array[i];
			JamGarrisonFollower jamGarrisonFollower = null;
			if (PersistentFollowerData.preMissionFollowerDictionary.ContainsKey(followerExperienceDisplay.GetFollowerID()))
			{
				jamGarrisonFollower = PersistentFollowerData.preMissionFollowerDictionary.get_Item(followerExperienceDisplay.GetFollowerID());
			}
			JamGarrisonFollower newFollower = null;
			if (PersistentFollowerData.followerDictionary.ContainsKey(followerExperienceDisplay.GetFollowerID()))
			{
				newFollower = PersistentFollowerData.followerDictionary.get_Item(followerExperienceDisplay.GetFollowerID());
			}
			if (jamGarrisonFollower != null)
			{
				float initialEffectDelay = (float)num * this.m_experienceDisplayEntranceDelay;
				followerExperienceDisplay.SetFollower(jamGarrisonFollower, newFollower, initialEffectDelay);
				num++;
			}
		}
	}

	public void HideMissionResults()
	{
		this.m_darknessBG.SetActive(false);
		this.m_popupView.SetActive(false);
	}

	public void CheatCompleteMission()
	{
		Main.instance.ExpediteMissionCheat(this.m_garrMissionID);
		this.HideMissionResults();
	}
}
