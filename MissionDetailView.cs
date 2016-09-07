using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WowJamMessages;
using WowJamMessages.MobilePlayerJSON;
using WowStatConstants;
using WowStaticData;

public class MissionDetailView : MonoBehaviour
{
	[Header("Main Mission Display")]
	public Text missionNameText;

	public Text missionLocationText;

	public Image missionTypeImage;

	public Text missioniLevelText;

	public Text missionTypeText;

	public Text missionDescriptionText;

	public GameObject missionFollowerSlotGroup;

	public GameObject enemyPortraitsGroup;

	public GameObject missionFollowerSlotPrefab;

	public GameObject missionEncounterPrefab;

	public GameObject missionMechanicPrefab;

	public MissionMechanic missionEnvironmentMechanic;

	public GameObject treasureChestHorde;

	public GameObject treasureChestAlliance;

	public Text missionPercentChanceLabel;

	public Text missionPercentChanceText;

	public GameObject m_missionChanceSpinner;

	public Text missionCostText;

	public Image m_scrollingEnvironment_Back;

	public Image m_scrollingEnvironment_Mid;

	public Image m_scrollingEnvironment_Fore;

	public GameObject m_lootGroupObj;

	public MissionRewardDisplay m_missionRewardDisplayPrefab;

	public GameObject m_mechanicEffectDisplayPrefab;

	public Text m_resourcesDescText;

	public Image m_startMissionButton;

	public Text m_startMissionButtonText;

	public Text m_costDescText;

	public GameObject m_partyBuffGroup;

	public GameObject m_partyDebuffGroup;

	public GameObject m_lootBorderNormal;

	public GameObject m_lootBorderLitUp;

	public Image m_resourceIcon_MissionCost;

	[Header("Combat Ally")]
	public bool m_isCombatAlly;

	public GameObject m_combatAllyAvailableStuff;

	public GameObject m_assignCombatAllyStuff;

	public GameObject m_unassignCombatAllyStuff;

	public GameObject m_combatAllyNotAvailableStuff;

	public Text m_combatAllyThisIsWhatYouGetText;

	public SpellDisplay m_combatAllySupportSpellDisplay;

	[Header("Bonus Loot")]
	public GameObject m_bonusLootDisplay;

	public Text m_bonusLootChanceText;

	public MissionRewardDisplay m_bonusMissionRewardDisplay;

	[Header("Mission Preview")]
	public GameObject m_previewView;

	public Text m_previewMissionNameText;

	public Text m_previewMissionLocationText;

	public Image m_previewMissionTypeImage;

	public Text m_previewMissioniLevelText;

	public Text m_previewMissionTimeText;

	public GameObject m_previewMechanicsGroup;

	public GameObject m_previewMechanicEffectPrefab;

	public GameObject m_previewLootGroup;

	public Transform m_previewSlideUpHintEffectRoot;

	[Header("Misc")]
	public CanvasGroup m_topLevelDetailViewCanvasGroup;

	public MissionPanelSlider m_missionPanelSlider;

	public Shader m_grayscaleShader;

	public bool m_usedForMissionList;

	private int m_currentGarrMissionID;

	private static string m_timeText;

	private static string m_typeText;

	private static string m_iLevelText;

	public Text m_partyBuffsText;

	public Text m_partyDebuffsText;

	private CombatAllyMissionState m_combatAllyMissionState;

	private bool m_isOverMaxChampionSoftCap;

	private bool m_needMoreResources;

	private bool m_needAtLeastOneChampion;

	private int m_percentChance;

	public Action FollowerSlotsChangedAction;

	public void Awake()
	{
		this.missionNameText.set_font(GeneralHelpers.LoadStandardFont());
		if (this.missionLocationText != null)
		{
			this.missionLocationText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.missioniLevelText != null)
		{
			this.missioniLevelText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.missionTypeText != null)
		{
			this.missionTypeText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.missionDescriptionText != null)
		{
			this.missionDescriptionText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.missionPercentChanceLabel != null)
		{
			this.missionPercentChanceLabel.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.missionPercentChanceText != null)
		{
			this.missionPercentChanceText.set_font(GeneralHelpers.LoadStandardFont());
			this.missionPercentChanceLabel.set_text(StaticDB.GetString("CHANCE", "Chance"));
		}
		this.missionCostText.set_font(GeneralHelpers.LoadStandardFont());
		if (this.m_resourcesDescText != null)
		{
			this.m_resourcesDescText.set_font(GeneralHelpers.LoadStandardFont());
		}
		this.m_startMissionButtonText.set_font(GeneralHelpers.LoadStandardFont());
		if (this.m_costDescText != null)
		{
			this.m_costDescText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_combatAllyThisIsWhatYouGetText != null)
		{
			this.m_combatAllyThisIsWhatYouGetText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_bonusLootChanceText != null)
		{
			this.m_bonusLootChanceText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_previewMissionNameText != null)
		{
			this.m_previewMissionNameText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_previewMissionLocationText != null)
		{
			this.m_previewMissionLocationText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_previewMissioniLevelText != null)
		{
			this.m_previewMissioniLevelText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_previewMissionTimeText != null)
		{
			this.m_previewMissionTimeText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_partyBuffsText != null)
		{
			this.m_partyBuffsText.set_font(GeneralHelpers.LoadStandardFont());
		}
		if (this.m_partyDebuffsText != null)
		{
			this.m_partyDebuffsText.set_font(GeneralHelpers.LoadStandardFont());
		}
		this.m_percentChance = 0;
		if (AssetBundleManager.instance.IsInitialized())
		{
			this.OnAssetBundleManagerInitialized();
		}
		else
		{
			AssetBundleManager expr_280 = AssetBundleManager.instance;
			expr_280.InitializedAction = (Action)Delegate.Combine(expr_280.InitializedAction, new Action(this.OnAssetBundleManagerInitialized));
		}
		if (this.m_previewView != null)
		{
			this.m_previewView.SetActive(true);
		}
		Material material = new Material(this.m_grayscaleShader);
		this.m_startMissionButton.set_material(material);
		this.m_startMissionButton.get_material().SetFloat("_GrayscaleAmount", 0f);
	}

	private void Start()
	{
		if (this.m_partyBuffsText != null)
		{
			this.m_partyBuffsText.set_text(StaticDB.GetString("PARTY_BUFFS", null));
		}
		if (this.m_partyDebuffsText != null)
		{
			this.m_partyDebuffsText.set_text(StaticDB.GetString("PARTY_DEBUFFS", null));
		}
	}

	public void OnAssetBundleManagerInitialized()
	{
		this.m_currentGarrMissionID = 0;
		if (MissionDetailView.m_timeText == null)
		{
			MissionDetailView.m_timeText = StaticDB.GetString("TIME", null);
		}
		if (MissionDetailView.m_typeText == null)
		{
			MissionDetailView.m_typeText = StaticDB.GetString("TYPE", null);
		}
		this.m_startMissionButtonText.set_text(StaticDB.GetString("START_MISSION", null));
		if (this.m_costDescText != null)
		{
			this.m_costDescText.set_text(StaticDB.GetString("COST", null));
		}
		MissionDetailView.m_iLevelText = StaticDB.GetString("ITEM_LEVEL_ABBREVIATION", null);
	}

	private void OnEnable()
	{
		if (!this.m_isCombatAlly)
		{
			if (this.m_usedForMissionList)
			{
				AdventureMapPanel expr_1B = AdventureMapPanel.instance;
				expr_1B.MissionSelectedFromListAction = (Action<int>)Delegate.Combine(expr_1B.MissionSelectedFromListAction, new Action<int>(this.HandleMissionSelected));
			}
			else
			{
				AdventureMapPanel expr_46 = AdventureMapPanel.instance;
				expr_46.MissionSelectedFromMapAction = (Action<int>)Delegate.Combine(expr_46.MissionSelectedFromMapAction, new Action<int>(this.HandleMissionSelected));
			}
			if (Main.instance != null)
			{
				Main expr_7C = Main.instance;
				expr_7C.MissionSuccessChanceChangedAction = (Action<int>)Delegate.Combine(expr_7C.MissionSuccessChanceChangedAction, new Action<int>(this.OnMissionSuccessChanceChanged));
			}
		}
		Main expr_A2 = Main.instance;
		expr_A2.StartLogOutAction = (Action)Delegate.Combine(expr_A2.StartLogOutAction, new Action(this.HandleStartLogoutAction));
	}

	private void OnDisable()
	{
		if (!this.m_isCombatAlly)
		{
			if (this.m_usedForMissionList)
			{
				AdventureMapPanel expr_1B = AdventureMapPanel.instance;
				expr_1B.MissionSelectedFromListAction = (Action<int>)Delegate.Remove(expr_1B.MissionSelectedFromListAction, new Action<int>(this.HandleMissionSelected));
			}
			else
			{
				AdventureMapPanel expr_46 = AdventureMapPanel.instance;
				expr_46.MissionSelectedFromMapAction = (Action<int>)Delegate.Remove(expr_46.MissionSelectedFromMapAction, new Action<int>(this.HandleMissionSelected));
			}
			if (Main.instance != null)
			{
				Main expr_7C = Main.instance;
				expr_7C.MissionSuccessChanceChangedAction = (Action<int>)Delegate.Remove(expr_7C.MissionSuccessChanceChangedAction, new Action<int>(this.OnMissionSuccessChanceChanged));
			}
		}
		Main expr_A2 = Main.instance;
		expr_A2.StartLogOutAction = (Action)Delegate.Remove(expr_A2.StartLogOutAction, new Action(this.HandleStartLogoutAction));
	}

	private void SetupInputForPreviewSlider(Button button)
	{
		if (this.m_missionPanelSlider == null)
		{
			return;
		}
		EventTrigger eventTrigger = button.get_gameObject().AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = 13;
		entry.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.m_missionPanelSlider.m_sliderPanel.OnBeginDrag(eventData);
		});
		entry.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.m_missionPanelSlider.StopTheBounce();
		});
		entry.callback.AddListener(delegate(BaseEventData eventData)
		{
			button.set_enabled(false);
		});
		eventTrigger.get_triggers().Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = 5;
		entry2.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.m_missionPanelSlider.m_sliderPanel.OnDrag(eventData);
		});
		eventTrigger.get_triggers().Add(entry2);
		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = 14;
		entry3.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.m_missionPanelSlider.m_sliderPanel.MissionPanelSlider_HandleAutopositioning_Bottom();
		});
		entry3.callback.AddListener(delegate(BaseEventData eventData)
		{
			button.set_enabled(true);
		});
		eventTrigger.get_triggers().Add(entry3);
	}

	public void HandleStartLogoutAction()
	{
		AdventureMapPanel.instance.SelectMissionFromList(0);
		if (this.missionFollowerSlotGroup != null)
		{
			MissionFollowerSlot[] componentsInChildren = this.missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
			}
		}
	}

	private int GetTrueMissionCost(GarrMissionRec garrMissionRec)
	{
		int num = (int)garrMissionRec.MissionCost;
		if (this.enemyPortraitsGroup != null)
		{
			float actionFlat = 0f;
			MissionMechanic[] componentsInChildren = this.enemyPortraitsGroup.GetComponentsInChildren<MissionMechanic>(true);
			MissionMechanic[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				MissionMechanic missionMechanic = array[i];
				if (!missionMechanic.IsCountered())
				{
					if (missionMechanic.AbilityID() != 0)
					{
						StaticDB.garrAbilityEffectDB.EnumRecordsByParentID(missionMechanic.AbilityID(), delegate(GarrAbilityEffectRec garrAbilityEffectRec)
						{
							if (garrAbilityEffectRec.AbilityAction == 39u)
							{
								actionFlat -= garrAbilityEffectRec.ActionValueFlat;
							}
							return true;
						});
					}
				}
			}
			num += (int)((float)num * actionFlat);
		}
		return num;
	}

	private int GetTrueMissionDuration(GarrMissionRec garrMissionRec, List<int> followerBuffAbilityIDs)
	{
		int num = garrMissionRec.MissionDuration;
		float actionFlat = 0f;
		if (this.enemyPortraitsGroup != null)
		{
			MissionMechanic[] componentsInChildren = this.enemyPortraitsGroup.GetComponentsInChildren<MissionMechanic>(true);
			MissionMechanic[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				MissionMechanic missionMechanic = array[i];
				if (!missionMechanic.IsCountered())
				{
					if (missionMechanic.AbilityID() != 0)
					{
						StaticDB.garrAbilityEffectDB.EnumRecordsByParentID(missionMechanic.AbilityID(), delegate(GarrAbilityEffectRec garrAbilityEffectRec)
						{
							if (garrAbilityEffectRec.AbilityAction == 17u)
							{
								actionFlat += 1f - garrAbilityEffectRec.ActionValueFlat;
							}
							return true;
						});
					}
				}
			}
		}
		using (List<int>.Enumerator enumerator = followerBuffAbilityIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.get_Current();
				StaticDB.garrAbilityEffectDB.EnumRecordsByParentID(current, delegate(GarrAbilityEffectRec garrAbilityEffectRec)
				{
					if (garrAbilityEffectRec.AbilityAction == 17u)
					{
						actionFlat += 1f - garrAbilityEffectRec.ActionValueFlat;
					}
					return true;
				});
			}
		}
		num -= (int)((float)num * actionFlat);
		return num;
	}

	public void HandleMissionSelected(int garrMissionID)
	{
		if (garrMissionID <= 0)
		{
			return;
		}
		this.m_currentGarrMissionID = garrMissionID;
		if (this.missionFollowerSlotGroup != null)
		{
			RectTransform[] componentsInChildren = this.missionFollowerSlotGroup.GetComponentsInChildren<RectTransform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i] != this.missionFollowerSlotGroup.get_transform())
				{
					Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
				}
			}
		}
		if (this.enemyPortraitsGroup != null)
		{
			RectTransform[] componentsInChildren2 = this.enemyPortraitsGroup.GetComponentsInChildren<RectTransform>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren2[j] != null && componentsInChildren2[j] != this.enemyPortraitsGroup.get_transform())
				{
					Object.DestroyImmediate(componentsInChildren2[j].get_gameObject());
				}
			}
		}
		if (this.m_previewMechanicsGroup != null)
		{
			AbilityDisplay[] componentsInChildren3 = this.m_previewMechanicsGroup.GetComponentsInChildren<AbilityDisplay>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				if (componentsInChildren3[k] != null)
				{
					Object.DestroyImmediate(componentsInChildren3[k].get_gameObject());
				}
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
		if (this.enemyPortraitsGroup != null)
		{
			for (int l = 0; l < jamGarrisonMobileMission.Encounter.Length; l++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.missionEncounterPrefab);
				gameObject.get_transform().SetParent(this.enemyPortraitsGroup.get_transform(), false);
				MissionEncounter component = gameObject.GetComponent<MissionEncounter>();
				int num = (jamGarrisonMobileMission.Encounter[l].MechanicID.Length <= 0) ? 0 : jamGarrisonMobileMission.Encounter[l].MechanicID[0];
				component.SetEncounter(jamGarrisonMobileMission.Encounter[l].EncounterID, num);
				if (this.m_previewMechanicsGroup != null)
				{
					GarrMechanicRec record = StaticDB.garrMechanicDB.GetRecord(num);
					if (record != null && record.GarrAbilityID != 0)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_previewMechanicEffectPrefab);
						gameObject2.get_transform().SetParent(this.m_previewMechanicsGroup.get_transform(), false);
						AbilityDisplay component2 = gameObject2.GetComponent<AbilityDisplay>();
						component2.SetAbility(record.GarrAbilityID, false, false, null);
						this.SetupInputForPreviewSlider(component2.m_mainButton);
						FollowerCanCounterMechanic canCounterStatus = GeneralHelpers.HasFollowerWhoCanCounter((int)record.GarrMechanicTypeID);
						component2.SetCanCounterStatus(canCounterStatus);
					}
				}
			}
		}
		GarrMissionRec record2 = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		this.missionNameText.set_text(record2.Name);
		if (this.m_previewMissionNameText != null)
		{
			this.m_previewMissionNameText.set_text(record2.Name);
		}
		if (this.m_previewMissionLocationText != null)
		{
			this.m_previewMissionLocationText.set_text(record2.Location);
		}
		if (this.missionDescriptionText != null)
		{
			this.missionDescriptionText.set_text(record2.Description);
		}
		if (this.missioniLevelText != null)
		{
			if (record2.TargetLevel < 110)
			{
				this.missioniLevelText.set_text(string.Empty + record2.TargetLevel);
			}
			else
			{
				this.missioniLevelText.set_text(string.Concat(new object[]
				{
					string.Empty,
					record2.TargetLevel,
					"\n(",
					record2.TargetItemLevel,
					")"
				}));
			}
		}
		if (this.m_previewMissioniLevelText != null)
		{
			this.m_previewMissioniLevelText.set_text(MissionDetailView.m_iLevelText + " " + record2.TargetItemLevel);
		}
		if (this.missionTypeImage != null)
		{
			GarrMissionTypeRec record3 = StaticDB.garrMissionTypeDB.GetRecord((int)record2.GarrMissionTypeID);
			this.missionTypeImage.set_overrideSprite(TextureAtlas.instance.GetAtlasSprite((int)record3.UiTextureAtlasMemberID));
			if (this.m_previewMissionTypeImage != null)
			{
				this.m_previewMissionTypeImage.set_overrideSprite(TextureAtlas.instance.GetAtlasSprite((int)record3.UiTextureAtlasMemberID));
			}
		}
		if (this.missionEnvironmentMechanic != null)
		{
			this.missionEnvironmentMechanic.SetMechanicType((int)record2.EnvGarrMechanicTypeID, 0, true);
			if (record2.EnvGarrMechanicTypeID != 0u)
			{
				GarrMechanicRec record4 = StaticDB.garrMechanicDB.GetRecord((int)record2.EnvGarrMechanicTypeID);
				if (record4 != null && record4.GarrAbilityID != 0)
				{
					GameObject gameObject3 = Object.Instantiate<GameObject>(this.m_previewMechanicEffectPrefab);
					gameObject3.get_transform().SetParent(this.m_previewMechanicsGroup.get_transform(), false);
					AbilityDisplay component3 = gameObject3.GetComponent<AbilityDisplay>();
					component3.SetAbility(record4.GarrAbilityID, false, false, null);
					FollowerCanCounterMechanic canCounterStatus2 = GeneralHelpers.HasFollowerWhoCanCounter((int)record4.GarrMechanicTypeID);
					component3.SetCanCounterStatus(canCounterStatus2);
				}
			}
		}
		if (this.missionTypeText != null)
		{
			GarrMechanicTypeRec record5 = StaticDB.garrMechanicTypeDB.GetRecord((int)record2.EnvGarrMechanicTypeID);
			if (record5 != null)
			{
				this.missionTypeText.get_gameObject().SetActive(true);
				this.missionTypeText.set_text(string.Concat(new string[]
				{
					"<color=#ffff00ff>",
					MissionDetailView.m_typeText,
					": </color><color=#ffffffff>",
					record5.Name,
					"</color>"
				}));
			}
			else
			{
				this.missionTypeText.get_gameObject().SetActive(false);
			}
		}
		Sprite sprite = GeneralHelpers.LoadCurrencyIcon(1220);
		if (sprite != null)
		{
			this.m_resourceIcon_MissionCost.set_sprite(sprite);
		}
		if (this.missionFollowerSlotGroup != null)
		{
			int num2 = 0;
			while ((long)num2 < (long)((ulong)record2.MaxFollowers))
			{
				GameObject gameObject4 = Object.Instantiate<GameObject>(this.missionFollowerSlotPrefab);
				gameObject4.get_transform().SetParent(this.missionFollowerSlotGroup.get_transform(), false);
				MissionFollowerSlot component4 = gameObject4.GetComponent<MissionFollowerSlot>();
				component4.m_missionDetailView = this;
				component4.m_enemyPortraitsGroup = this.enemyPortraitsGroup;
				num2++;
			}
		}
		if (!this.m_isCombatAlly && record2.UiTextureKitID > 0u)
		{
			UiTextureKitRec record6 = StaticDB.uiTextureKitDB.GetRecord((int)record2.UiTextureKitID);
			this.m_scrollingEnvironment_Back.set_enabled(false);
			this.m_scrollingEnvironment_Mid.set_enabled(false);
			this.m_scrollingEnvironment_Fore.set_enabled(false);
			int uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("_" + record6.KitPrefix + "-Back");
			if (uITextureAtlasMemberID > 0)
			{
				Sprite atlasSprite = TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID);
				if (atlasSprite != null)
				{
					this.m_scrollingEnvironment_Back.set_enabled(true);
					this.m_scrollingEnvironment_Back.set_sprite(atlasSprite);
				}
			}
			int uITextureAtlasMemberID2 = TextureAtlas.GetUITextureAtlasMemberID("_" + record6.KitPrefix + "-Mid");
			if (uITextureAtlasMemberID2 > 0)
			{
				Sprite atlasSprite2 = TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID2);
				if (atlasSprite2 != null)
				{
					this.m_scrollingEnvironment_Mid.set_enabled(true);
					this.m_scrollingEnvironment_Mid.set_sprite(atlasSprite2);
				}
			}
			int uITextureAtlasMemberID3 = TextureAtlas.GetUITextureAtlasMemberID("_" + record6.KitPrefix + "-Fore");
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
		else if ((record2.Flags & 16u) == 0u)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"DATA ERROR: Mission UITextureKit Not Set for mission ID:",
				record2.ID,
				" - ",
				record2.Name
			}));
			Debug.LogWarning("This means the scrolling background images will show the wrong location");
		}
		this.UpdateMissionStatus();
		if (this.m_lootGroupObj == null || this.m_missionRewardDisplayPrefab == null)
		{
			return;
		}
		MissionRewardDisplay[] componentsInChildren4 = this.m_lootGroupObj.GetComponentsInChildren<MissionRewardDisplay>(true);
		for (int m = 0; m < componentsInChildren4.Length; m++)
		{
			if (componentsInChildren4[m] != null)
			{
				Object.DestroyImmediate(componentsInChildren4[m].get_gameObject());
			}
		}
		if (this.m_previewLootGroup != null)
		{
			MissionRewardDisplay[] componentsInChildren5 = this.m_previewLootGroup.GetComponentsInChildren<MissionRewardDisplay>(true);
			for (int n = 0; n < componentsInChildren5.Length; n++)
			{
				if (componentsInChildren5[n] != null)
				{
					Object.DestroyImmediate(componentsInChildren5[n].get_gameObject());
				}
			}
		}
		if (!PersistentMissionData.missionDictionary.ContainsKey(this.m_currentGarrMissionID))
		{
			return;
		}
		MissionRewardDisplay.InitMissionRewards(this.m_missionRewardDisplayPrefab.get_gameObject(), this.m_lootGroupObj.get_transform(), jamGarrisonMobileMission.Reward);
		if (this.m_previewLootGroup != null)
		{
			MissionRewardDisplay.InitMissionRewards(this.m_missionRewardDisplayPrefab.get_gameObject(), this.m_previewLootGroup.get_transform(), jamGarrisonMobileMission.Reward);
		}
	}

	public void UpdateMissionStatus()
	{
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(this.m_currentGarrMissionID);
		if (record == null)
		{
			return;
		}
		if ((record.Flags & 16u) == 0u)
		{
			if (this.enemyPortraitsGroup != null)
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
							componentsInChildren[l].SetCountered(true, false, true);
							componentsInChildren2[l].SetCountered(true, true);
							break;
						}
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
			MobilePlayerEvaluateMission mobilePlayerEvaluateMission = new MobilePlayerEvaluateMission();
			mobilePlayerEvaluateMission.GarrMissionID = this.m_currentGarrMissionID;
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
			if (this.missionPercentChanceText != null)
			{
				this.missionPercentChanceText.set_text("%");
				this.m_missionChanceSpinner.SetActive(true);
			}
			if (this.m_partyBuffGroup != null)
			{
				AbilityDisplay[] componentsInChildren5 = this.m_partyBuffGroup.GetComponentsInChildren<AbilityDisplay>(true);
				AbilityDisplay[] array = componentsInChildren5;
				for (int n = 0; n < array.Length; n++)
				{
					AbilityDisplay abilityDisplay = array[n];
					Object.DestroyImmediate(abilityDisplay.get_gameObject());
				}
			}
			if (this.m_partyDebuffGroup != null)
			{
				AbilityDisplay[] componentsInChildren6 = this.m_partyDebuffGroup.GetComponentsInChildren<AbilityDisplay>(true);
				AbilityDisplay[] array2 = componentsInChildren6;
				for (int num2 = 0; num2 < array2.Length; num2++)
				{
					AbilityDisplay abilityDisplay2 = array2[num2];
					Object.DestroyImmediate(abilityDisplay2.get_gameObject());
				}
			}
			List<int> list2 = new List<int>();
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			MissionFollowerSlot[] componentsInChildren7 = this.missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
			MissionFollowerSlot[] array3 = componentsInChildren7;
			for (int num6 = 0; num6 < array3.Length; num6++)
			{
				MissionFollowerSlot missionFollowerSlot = array3[num6];
				int currentGarrFollowerID2 = missionFollowerSlot.GetCurrentGarrFollowerID();
				if (currentGarrFollowerID2 != 0)
				{
					int[] buffsForCurrentMission = GeneralHelpers.GetBuffsForCurrentMission(currentGarrFollowerID2, this.m_currentGarrMissionID, this.missionFollowerSlotGroup);
					num3 += buffsForCurrentMission.Length;
					int[] array4 = buffsForCurrentMission;
					for (int num7 = 0; num7 < array4.Length; num7++)
					{
						int num8 = array4[num7];
						list2.Add(num8);
						GameObject gameObject = Object.Instantiate<GameObject>(this.m_mechanicEffectDisplayPrefab);
						gameObject.get_transform().SetParent(this.m_partyBuffGroup.get_transform(), false);
						AbilityDisplay component = gameObject.GetComponent<AbilityDisplay>();
						component.SetAbility(num8, false, false, null);
					}
					JamGarrisonFollower jamGarrisonFollower2 = PersistentFollowerData.followerDictionary.get_Item(currentGarrFollowerID2);
					if ((jamGarrisonFollower2.Flags & 8) == 0)
					{
						num5++;
					}
				}
			}
			if (this.m_partyBuffGroup != null)
			{
				this.m_partyBuffGroup.SetActive(num3 > 0);
			}
			if (this.m_partyDebuffGroup != null)
			{
				this.m_partyDebuffGroup.SetActive(num4 > 0);
			}
			int trueMissionCost = this.GetTrueMissionCost(record);
			this.missionCostText.set_text(GarrisonStatus.Resources().ToString("N0") + " / " + trueMissionCost.ToString("N0"));
			int numActiveChampions = GeneralHelpers.GetNumActiveChampions();
			int maxActiveChampions = GeneralHelpers.GetMaxActiveChampions();
			this.m_isOverMaxChampionSoftCap = false;
			this.m_needMoreResources = false;
			this.m_needAtLeastOneChampion = false;
			if (numActiveChampions > maxActiveChampions)
			{
				this.m_isOverMaxChampionSoftCap = true;
			}
			if (GarrisonStatus.Resources() < trueMissionCost)
			{
				this.m_needMoreResources = true;
			}
			if (num5 < 1)
			{
				this.m_needAtLeastOneChampion = true;
			}
			if (this.m_needMoreResources)
			{
				this.missionCostText.set_color(Color.get_red());
			}
			else
			{
				this.missionCostText.set_color(Color.get_white());
			}
			bool flag = !this.m_isOverMaxChampionSoftCap && !this.m_needMoreResources && !this.m_needAtLeastOneChampion;
			if (flag)
			{
				this.m_startMissionButton.get_material().SetFloat("_GrayscaleAmount", 0f);
				this.m_startMissionButtonText.set_color(new Color(1f, 0.8588f, 0f, 1f));
				Shadow component2 = this.m_startMissionButtonText.GetComponent<Shadow>();
				component2.set_enabled(true);
			}
			else
			{
				this.m_startMissionButton.get_material().SetFloat("_GrayscaleAmount", 1f);
				this.m_startMissionButtonText.set_color(Color.get_gray());
				Shadow component3 = this.m_startMissionButtonText.GetComponent<Shadow>();
				component3.set_enabled(false);
			}
			int trueMissionDuration = this.GetTrueMissionDuration(record, list2);
			Duration duration = new Duration(trueMissionDuration);
			if (this.missionLocationText != null)
			{
				this.missionLocationText.set_text(string.Concat(new object[]
				{
					StaticDB.GetString("XP", "XP:"),
					" ",
					record.BaseFollowerXP,
					" (<color=#ff8600ff>",
					duration.DurationString,
					"</color>)"
				}));
			}
			if (this.m_previewMissionTimeText != null)
			{
				this.m_previewMissionTimeText.set_text(string.Concat(new string[]
				{
					"<color=#ffff00ff>",
					MissionDetailView.m_timeText,
					": </color><color=#ff8600ff>",
					duration.DurationString,
					"</color>"
				}));
			}
			return;
		}
		MissionFollowerSlot componentInChildren = base.get_gameObject().GetComponentInChildren<MissionFollowerSlot>(true);
		if (componentInChildren == null)
		{
			return;
		}
		if (componentInChildren.IsOccupied())
		{
			if (this.missionDescriptionText != null)
			{
				this.missionDescriptionText.get_gameObject().SetActive(false);
			}
			this.m_combatAllyThisIsWhatYouGetText.get_gameObject().SetActive(true);
			this.m_combatAllySupportSpellDisplay.get_gameObject().SetActive(true);
			int currentGarrFollowerID3 = componentInChildren.GetCurrentGarrFollowerID();
			int zoneSupportSpellID = PersistentFollowerData.followerDictionary.get_Item(currentGarrFollowerID3).ZoneSupportSpellID;
			this.m_combatAllySupportSpellDisplay.SetSpell(zoneSupportSpellID);
		}
		else
		{
			if (this.missionDescriptionText != null)
			{
				this.missionDescriptionText.get_gameObject().SetActive(true);
			}
			this.m_combatAllyThisIsWhatYouGetText.get_gameObject().SetActive(false);
			this.m_combatAllySupportSpellDisplay.get_gameObject().SetActive(false);
		}
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
				int[] buffsForCurrentMission = GeneralHelpers.GetBuffsForCurrentMission(currentGarrFollowerID, this.m_currentGarrMissionID, this.missionFollowerSlotGroup);
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

	private void OnMissionSuccessChanceChanged(int newChance)
	{
		if (this.m_currentGarrMissionID == 0)
		{
			return;
		}
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(this.m_currentGarrMissionID);
		if (record == null)
		{
			Debug.LogError("Invalid Mission ID:" + this.m_currentGarrMissionID);
			return;
		}
		if ((record.Flags & 16u) != 0u)
		{
			return;
		}
		this.m_bonusLootDisplay.SetActive(false);
		this.m_missionChanceSpinner.SetActive(false);
		this.missionPercentChanceText.set_text(newChance + "%");
		this.m_lootBorderNormal.SetActive(newChance < 100);
		this.m_lootBorderLitUp.SetActive(newChance >= 100);
		if (newChance < 0)
		{
			this.missionPercentChanceText.set_color(Color.get_red());
			this.missionPercentChanceLabel.set_color(Color.get_red());
		}
		else
		{
			this.missionPercentChanceText.set_color(Color.get_green());
			this.missionPercentChanceLabel.set_color(Color.get_green());
		}
		if (this.m_percentChance < 100 && newChance >= 100)
		{
			Main.instance.m_UISound.Play_100Percent();
		}
		else if (this.m_percentChance < 200 && newChance >= 200)
		{
			Main.instance.m_UISound.Play_200Percent();
		}
		else if (newChance > this.m_percentChance)
		{
		}
		if (StaticDB.rewardPackDB.GetRecord(record.OvermaxRewardPackID) == null)
		{
			return;
		}
		if (record.OvermaxRewardPackID > 0)
		{
			string @string = StaticDB.GetString("BONUS", "Bonus:");
			this.m_bonusLootChanceText.set_text(string.Concat(new object[]
			{
				"<color=#ffff00ff>",
				@string,
				" </color>\n<color=#ff8600ff>",
				Math.Max(0, newChance - 100),
				"%</color>"
			}));
			if (PersistentMissionData.missionDictionary.ContainsKey(this.m_currentGarrMissionID))
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(this.m_currentGarrMissionID);
				if (jamGarrisonMobileMission.OvermaxReward.Length > 0)
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
							CurrencyTypesRec record2 = StaticDB.currencyTypesDB.GetRecord(jamGarrisonMissionReward.CurrencyType);
							int rewardQuantity = (int)((ulong)jamGarrisonMissionReward.CurrencyQuantity / (ulong)(((record2.Flags & 8u) == 0u) ? 1L : 100L));
							this.m_bonusMissionRewardDisplay.InitReward(MissionRewardDisplay.RewardType.currency, jamGarrisonMissionReward.CurrencyType, rewardQuantity, 0, 0);
						}
					}
				}
			}
		}
		this.m_percentChance = newChance;
	}

	public void AssignCombatAlly()
	{
		this.StartMission();
	}

	public void StartMission()
	{
		if (this.m_isOverMaxChampionSoftCap || this.m_needMoreResources || this.m_needAtLeastOneChampion)
		{
			if (this.m_isOverMaxChampionSoftCap)
			{
				AllPopups.instance.ShowGenericPopupFull(StaticDB.GetString("TOO_MANY_CHAMPIONS", null));
			}
			else if (this.m_needAtLeastOneChampion)
			{
				AllPopups.instance.ShowGenericPopupFull(StaticDB.GetString("NEED_A_CHAMPION", null));
			}
			else if (this.m_needMoreResources)
			{
				AllPopups.instance.ShowGenericPopupFull(StaticDB.GetString("NEED_MORE_RESOURCES", null));
			}
			return;
		}
		Main.instance.m_UISound.Play_StartMission();
		MissionFollowerSlot[] componentsInChildren = base.get_gameObject().GetComponentsInChildren<MissionFollowerSlot>(true);
		List<ulong> list = new List<ulong>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			int currentGarrFollowerID = componentsInChildren[i].GetCurrentGarrFollowerID();
			if (PersistentFollowerData.followerDictionary.ContainsKey(currentGarrFollowerID))
			{
				JamGarrisonFollower jamGarrisonFollower = PersistentFollowerData.followerDictionary.get_Item(currentGarrFollowerID);
				list.Add(jamGarrisonFollower.DbID);
			}
		}
		Main.instance.StartMission(this.m_currentGarrMissionID, list.ToArray());
		Main.instance.allPanels.ShowAdventureMap();
		AdventureMapPanel.instance.SelectMissionFromMap(0);
		AdventureMapPanel.instance.SelectMissionFromList(0);
		AdventureMapPanel.instance.SetSelectedIconContainer(null);
	}

	public void ShowMissionDescriptionPopup()
	{
		AllPopups.instance.ShowMissionDescriptionTooltip(this.m_currentGarrMissionID);
	}

	public CombatAllyMissionState GetCombatAllyMissionState()
	{
		return this.m_combatAllyMissionState;
	}

	public void SetCombatAllyMissionState(CombatAllyMissionState state)
	{
		this.m_combatAllyMissionState = state;
		switch (state)
		{
		case CombatAllyMissionState.notAvailable:
		{
			Text[] componentsInChildren = this.m_combatAllyNotAvailableStuff.GetComponentsInChildren<Text>();
			if (componentsInChildren[0] != null)
			{
				componentsInChildren[0].set_text(StaticDB.GetString("COMBAT_ALLY_UNAVAILABLE", null));
			}
			this.m_combatAllyNotAvailableStuff.SetActive(true);
			this.m_combatAllyAvailableStuff.SetActive(false);
			break;
		}
		case CombatAllyMissionState.available:
			this.m_combatAllyNotAvailableStuff.SetActive(false);
			this.m_combatAllyAvailableStuff.SetActive(true);
			this.m_assignCombatAllyStuff.SetActive(true);
			this.m_unassignCombatAllyStuff.SetActive(false);
			break;
		case CombatAllyMissionState.inProgress:
		{
			this.m_combatAllyNotAvailableStuff.SetActive(false);
			this.m_combatAllyAvailableStuff.SetActive(true);
			int follower = 0;
			using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator = PersistentFollowerData.followerDictionary.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JamGarrisonFollower current = enumerator.get_Current();
					if (current.CurrentMissionID == this.m_currentGarrMissionID)
					{
						Debug.LogWarning("Combat Ally ID FOUND: " + current.GarrFollowerID);
						follower = current.GarrFollowerID;
						break;
					}
				}
			}
			MissionFollowerSlot[] componentsInChildren2 = base.get_gameObject().GetComponentsInChildren<MissionFollowerSlot>(true);
			Debug.LogWarning("missionfollowerSlots found: " + componentsInChildren2.Length);
			componentsInChildren2[0].SetFollower(follower);
			this.m_assignCombatAllyStuff.SetActive(false);
			this.m_unassignCombatAllyStuff.SetActive(true);
			break;
		}
		}
	}

	public void UnassignCombatAlly()
	{
		Main.instance.CompleteMission(this.m_currentGarrMissionID);
		this.SetCombatAllyMissionState(CombatAllyMissionState.available);
		MissionFollowerSlot[] componentsInChildren = base.get_gameObject().GetComponentsInChildren<MissionFollowerSlot>(true);
		componentsInChildren[0].SetFollower(0);
	}

	public int GetCurrentMissionID()
	{
		return this.m_currentGarrMissionID;
	}

	public static float ComputeFollowerBias(JamGarrisonFollower follower, int followerLevel, int followerItemLevel, int garrMissionID)
	{
		bool flag = (follower.Flags & 8) != 0;
		if (flag)
		{
			return 0f;
		}
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		if (record == null)
		{
			return 0f;
		}
		float num = MissionDetailView.ComputeFollowerLevelBias(follower, followerLevel, record.TargetLevel);
		num += MissionDetailView.ComputeFollowerItemLevelBias(follower, followerItemLevel, (int)record.GarrFollowerTypeID, (int)record.TargetItemLevel);
		return Mathf.Clamp(num, -1f, 1f);
	}

	public static int GarrisonFollower_GetMaxFollowerLevel(int garrFollowerTypeID)
	{
		int maxLevel = 0;
		StaticDB.garrFollowerLevelXPDB.EnumRecords(delegate(GarrFollowerLevelXPRec garrFollowerLevelXPRec)
		{
			if ((ulong)garrFollowerLevelXPRec.GarrFollowerTypeID == (ulong)((long)garrFollowerTypeID) && (maxLevel == 0 || (ulong)garrFollowerLevelXPRec.FollowerLevel > (ulong)((long)maxLevel)))
			{
				maxLevel = (int)garrFollowerLevelXPRec.FollowerLevel;
			}
			return true;
		});
		return maxLevel;
	}

	private static float ComputeFollowerItemLevelBias(JamGarrisonFollower follower, int followerItemLevel, int garrFollowerTypeID, int targetItemLevel)
	{
		float num = 0f;
		if (follower.FollowerLevel == MissionDetailView.GarrisonFollower_GetMaxFollowerLevel(garrFollowerTypeID))
		{
			int num2;
			int num3;
			MissionDetailView.GarrisonFollower_GetFollowerBiasConstants(follower, out num2, out num3);
			int num4 = (targetItemLevel <= 0) ? 600 : targetItemLevel;
			float num5 = (float)(followerItemLevel - num4);
			num5 /= (float)num3;
			num += num5;
		}
		return num;
	}

	private static float ComputeFollowerLevelBias(JamGarrisonFollower follower, int followerLevel, int targetLevel)
	{
		int num;
		int num2;
		MissionDetailView.GarrisonFollower_GetFollowerBiasConstants(follower, out num, out num2);
		float num3 = (float)(followerLevel - targetLevel);
		return num3 / (float)num;
	}

	private static float ComputeBiasValue(float par, float max, float bias)
	{
		if (bias < 0f)
		{
			return par + bias * par;
		}
		return par + bias * (max - par);
	}

	private static void GarrisonFollower_GetFollowerBiasConstants(JamGarrisonFollower follower, out int out_levelRangeBias, out int out_itemLevelRangeBias)
	{
		out_levelRangeBias = 1;
		out_itemLevelRangeBias = 1;
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(follower.GarrFollowerID);
		if (record == null)
		{
			return;
		}
		GarrFollowerTypeRec record2 = StaticDB.garrFollowerTypeDB.GetRecord((int)record.GarrFollowerTypeID);
		if (record2 == null)
		{
			return;
		}
		out_levelRangeBias = (int)((record2.LevelRangeBias >= 1u) ? record2.LevelRangeBias : 1u);
		out_itemLevelRangeBias = (int)((record2.ItemLevelRangeBias >= 1u) ? record2.ItemLevelRangeBias : 1u);
	}

	public void NotifyFollowerSlotsChanged()
	{
		if (this.FollowerSlotsChangedAction != null)
		{
			this.FollowerSlotsChangedAction.Invoke();
		}
	}
}
