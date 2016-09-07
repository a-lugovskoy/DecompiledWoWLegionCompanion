using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;
using WowStaticData;

public class AdventureMapPanel : MonoBehaviour
{
	public enum eZone
	{
		Azsuna = 0,
		BrokenShore = 1,
		HighMountain = 2,
		Stormheim = 3,
		Suramar = 4,
		ValShara = 5,
		None = 6,
		NumZones = 7
	}

	public bool m_testEnableDetailedZoneMaps;

	public bool m_testEnableAutoZoomInOut;

	public bool m_testEnableTapToZoomOut;

	public float m_testMissionIconScale;

	public Action<float> TestIconSizeChanged;

	public Camera m_mainCamera;

	public PinchZoomContentManager m_pinchZoomContentManager;

	public RectTransform m_mapViewRT;

	public RectTransform m_mapAndRewardParentViewRT;

	public RectTransform m_mapViewContentsRT;

	public Image m_worldMapLowDetail;

	public MapInfo m_mainMapInfo;

	public GameObject m_AdvMapMissionSitePrefab;

	public GameObject m_AdvMapWorldQuestPrefab;

	public GameObject m_bountySitePrefab;

	public GameObject m_missionAndWordQuestArea;

	public ZoneButton m_zoneButtonAzsuna;

	public ZoneButton m_zoneButtonBrokenShore;

	public ZoneButton m_zoneButtonHighmountain;

	public ZoneButton m_zoneButtonStormheim;

	public ZoneButton m_zoneButtonSuramar;

	public ZoneButton m_zoneButtonValShara;

	public ZoneButton[] m_allZoneButtons;

	public ZoneMissionOverview[] m_allZoneMissionOverviews;

	public GameObject m_missionRewardResultsDisplayPrefab;

	public GameObject m_zoneLabel;

	public Text m_zoneLabelText;

	public AdventureMapPanel.eZone m_zoneID;

	public static AdventureMapPanel instance;

	public ZoneButton m_lastTappedZoneButton;

	public ZoneButton m_currentVisibleZone;

	public GuildChatSlider m_guildChatSlider_Bottom;

	private int m_currentMapMission;

	public Action<int> MissionSelectedFromMapAction;

	public Action<int> MissionMapSelectionChangedAction;

	private int m_currentListMission;

	public Action<int> MissionSelectedFromListAction;

	private int m_currentWorldQuest;

	public Action<int> WorldQuestChangedAction;

	public Action OnZoomOutMap;

	public Action<int> OnAddMissionLootToRewardPanel;

	public Action<bool> OnShowMissionRewardPanel;

	public Action OnInitMissionSites;

	public Action MapFiltersChanged;

	private int m_followerToInspect;

	public Action<int> FollowerToInspectChangedAction;

	public Action DeselectAllFollowerListItemsAction;

	public Action<bool> OnShowFollowerDetails;

	public Action<int, bool> OnMissionFollowerSlotChanged;

	public Action<int, int, bool> ShowMissionResultAction;

	private StackableMapIconContainer m_iconContainer;

	public Action<StackableMapIconContainer> SelectedIconContainerChanged;

	public float m_secondsMissionHasBeenSelected;

	public CanvasGroup m_topLevelMapCanvasGroup;

	public RectTransform m_parentViewRT;

	private Vector2 m_multiPanelViewSizeDelta;

	public PlayerInfoDisplay m_playerInfoDisplay;

	private bool[] m_mapFilters;

	private void OnEnable()
	{
		this.MapFiltersChanged = (Action)Delegate.Combine(this.MapFiltersChanged, new Action(this.UpdateWorldQuests));
	}

	private void OnDisable()
	{
		this.MapFiltersChanged = (Action)Delegate.Remove(this.MapFiltersChanged, new Action(this.UpdateWorldQuests));
	}

	public void DeselectAllFollowerListItems()
	{
		if (this.DeselectAllFollowerListItemsAction != null)
		{
			this.DeselectAllFollowerListItemsAction.Invoke();
		}
	}

	public void ShowFollowerDetails(bool show)
	{
		if (this.OnShowFollowerDetails != null)
		{
			this.OnShowFollowerDetails.Invoke(show);
		}
	}

	public void SetFollowerToInspect(int garrFollowerID)
	{
		this.m_followerToInspect = garrFollowerID;
		if (this.FollowerToInspectChangedAction != null)
		{
			this.FollowerToInspectChangedAction.Invoke(garrFollowerID);
		}
	}

	public int GetFollowerToInspect()
	{
		return this.m_followerToInspect;
	}

	public void EnableMapFilter(MapFilterType mapFilterType, bool enable)
	{
		for (int i = 0; i < this.m_mapFilters.Length; i++)
		{
			this.m_mapFilters[i] = false;
		}
		this.m_mapFilters[(int)mapFilterType] = true;
		AllPopups.instance.m_optionsDialog.SyncWithOptions();
		if (this.MapFiltersChanged != null)
		{
			this.MapFiltersChanged.Invoke();
		}
	}

	public bool IsFilterEnabled(MapFilterType mapFilterType)
	{
		return this.m_mapFilters[(int)mapFilterType];
	}

	public void AddMissionLootToRewardPanel(int garrMissionID)
	{
		if (this.OnAddMissionLootToRewardPanel != null)
		{
			this.OnAddMissionLootToRewardPanel.Invoke(garrMissionID);
		}
	}

	public void ShowRewardPanel(bool show)
	{
		if (this.OnShowMissionRewardPanel != null)
		{
			this.OnShowMissionRewardPanel.Invoke(show);
		}
	}

	public int GetCurrentMapMission()
	{
		return this.m_currentMapMission;
	}

	public void SelectMissionFromMap(int garrMissionID)
	{
		if (this.m_currentMapMission != garrMissionID)
		{
			this.m_secondsMissionHasBeenSelected = 0f;
			this.m_currentMapMission = garrMissionID;
			if (this.MissionMapSelectionChangedAction != null)
			{
				this.MissionMapSelectionChangedAction.Invoke(this.m_currentMapMission);
			}
		}
		if (this.MissionSelectedFromMapAction != null)
		{
			this.MissionSelectedFromMapAction.Invoke(this.m_currentMapMission);
		}
		if (garrMissionID > 0)
		{
			this.SelectWorldQuest(0);
		}
	}

	public int GetCurrentListMission()
	{
		return this.m_currentListMission;
	}

	public void SelectMissionFromList(int garrMissionID)
	{
		this.m_currentListMission = garrMissionID;
		if (this.MissionSelectedFromListAction != null)
		{
			this.MissionSelectedFromListAction.Invoke(garrMissionID);
		}
	}

	public int GetCurrentWorldQuest()
	{
		return this.m_currentWorldQuest;
	}

	public void SelectWorldQuest(int worldQuestID)
	{
		this.m_currentWorldQuest = worldQuestID;
		if (this.WorldQuestChangedAction != null)
		{
			this.WorldQuestChangedAction.Invoke(this.m_currentWorldQuest);
		}
		if (worldQuestID > 0)
		{
			this.SelectMissionFromMap(0);
		}
	}

	public void SetMissionIconScale(float val)
	{
		this.m_testMissionIconScale = val;
		if (this.TestIconSizeChanged != null)
		{
			this.TestIconSizeChanged.Invoke(this.m_testMissionIconScale);
		}
	}

	public void SetSelectedIconContainer(StackableMapIconContainer container)
	{
		this.m_iconContainer = container;
		if (this.SelectedIconContainerChanged != null)
		{
			this.SelectedIconContainerChanged.Invoke(container);
		}
	}

	public StackableMapIconContainer GetSelectedIconContainer()
	{
		return this.m_iconContainer;
	}

	private void Awake()
	{
		AdventureMapPanel.instance = this;
		this.m_zoneID = AdventureMapPanel.eZone.None;
		this.m_testMissionIconScale = 1f;
		this.m_mapFilters = new bool[7];
		for (int i = 0; i < this.m_mapFilters.Length; i++)
		{
			this.m_mapFilters[i] = false;
		}
		this.EnableMapFilter(MapFilterType.All, true);
		AllPanels.instance.m_missionResultsPanel.get_gameObject().SetActive(true);
		Main.instance.RequestWorldQuests();
	}

	private void Start()
	{
		this.m_pinchZoomContentManager.SetZoom(1f, false);
		this.m_guildChatSlider_Bottom.Hide();
		StackableMapIconContainer[] componentsInChildren = this.m_missionAndWordQuestArea.GetComponentsInChildren<StackableMapIconContainer>(true);
		StackableMapIconContainer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			StackableMapIconContainer stackableMapIconContainer = array[i];
			Object.DestroyImmediate(stackableMapIconContainer.get_gameObject());
		}
		this.InitMissionSites();
		this.UpdateWorldQuests();
		this.HandleBountyInfoUpdated();
		Main expr_65 = Main.instance;
		expr_65.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(expr_65.GarrisonDataResetFinishedAction, new Action(this.InitMissionSites));
		Main expr_8B = Main.instance;
		expr_8B.MissionAddedAction = (Action<int, int>)Delegate.Combine(expr_8B.MissionAddedAction, new Action<int, int>(this.HandleMissionAdded));
		Main expr_B1 = Main.instance;
		expr_B1.BountyInfoUpdatedAction = (Action)Delegate.Combine(expr_B1.BountyInfoUpdatedAction, new Action(this.HandleBountyInfoUpdated));
	}

	private void HandleMissionAdded(int garrMissionID, int result)
	{
		Debug.LogWarning("MISSION ADDED!!");
	}

	private void CreateMissionSite(int garrMissionID)
	{
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		if (record == null)
		{
			Debug.LogWarning("Mission Not Found: ID " + garrMissionID);
			return;
		}
		if (record.GarrFollowerTypeID != 4u)
		{
			return;
		}
		if ((record.Flags & 16u) != 0u)
		{
			return;
		}
		if (!PersistentMissionData.missionDictionary.ContainsKey(garrMissionID))
		{
			return;
		}
		JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(garrMissionID);
		if (jamGarrisonMobileMission.MissionState == 0)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(AdventureMapPanel.instance.m_AdvMapMissionSitePrefab);
		gameObject.get_transform().SetParent(this.m_missionAndWordQuestArea.get_transform(), false);
		float num = 1.84887111f;
		float num2 = record.Mappos_x * num;
		float num3 = record.Mappos_y * -num;
		float num4 = -272.5694f;
		float num5 = 1318.388f;
		num2 += num4;
		num3 += num5;
		float width = this.m_worldMapLowDetail.get_sprite().get_textureRect().get_width();
		float height = this.m_worldMapLowDetail.get_sprite().get_textureRect().get_height();
		Vector2 vector = new Vector3(num2 / width, num3 / height);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.set_anchorMin(vector);
		component.set_anchorMax(vector);
		component.set_anchoredPosition(Vector2.get_zero());
		AdventureMapMissionSite component2 = gameObject.GetComponent<AdventureMapMissionSite>();
		component2.SetMission(record.ID);
		StackableMapIcon component3 = gameObject.GetComponent<StackableMapIcon>();
		if (component3 != null)
		{
			component3.RegisterWithManager();
		}
	}

	private void InitMissionSites()
	{
		if (this.OnInitMissionSites != null)
		{
			this.OnInitMissionSites.Invoke();
		}
		AdventureMapMissionSite[] componentsInChildren = this.m_missionAndWordQuestArea.get_transform().GetComponentsInChildren<AdventureMapMissionSite>(true);
		AdventureMapMissionSite[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			AdventureMapMissionSite adventureMapMissionSite = array[i];
			if (adventureMapMissionSite != null)
			{
				Object.DestroyImmediate(adventureMapMissionSite.get_gameObject());
			}
		}
		IEnumerator enumerator = PersistentMissionData.missionDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)enumerator.get_Current();
				this.CreateMissionSite(jamGarrisonMobileMission.MissionRecID);
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
	}

	public void UpdateWorldQuests()
	{
		AdventureMapWorldQuest[] componentsInChildren = this.m_missionAndWordQuestArea.GetComponentsInChildren<AdventureMapWorldQuest>(true);
		AdventureMapWorldQuest[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			AdventureMapWorldQuest adventureMapWorldQuest = array[i];
			Object.DestroyImmediate(adventureMapWorldQuest.get_gameObject());
		}
		IEnumerator enumerator = WorldQuestData.worldQuestDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MobileWorldQuest mobileWorldQuest = (MobileWorldQuest)enumerator.get_Current();
				if (!this.IsFilterEnabled(MapFilterType.All))
				{
					bool matchesFilter = false;
					if (this.IsFilterEnabled(MapFilterType.ArtifactPower))
					{
						MobileWorldQuestReward[] item = mobileWorldQuest.Item;
						for (int j = 0; j < item.Length; j++)
						{
							MobileWorldQuestReward mobileWorldQuestReward = item[j];
							StaticDB.itemEffectDB.EnumRecordsByParentID(mobileWorldQuestReward.RecordID, delegate(ItemEffectRec itemEffectRec)
							{
								StaticDB.spellEffectDB.EnumRecordsByParentID(itemEffectRec.SpellID, delegate(SpellEffectRec spellEffectRec)
								{
									if (spellEffectRec.Effect == 240)
									{
										matchesFilter = true;
										return false;
									}
									return true;
								});
								return !matchesFilter;
							});
						}
					}
					if (this.IsFilterEnabled(MapFilterType.OrderResources))
					{
						MobileWorldQuestReward[] currency = mobileWorldQuest.Currency;
						for (int k = 0; k < currency.Length; k++)
						{
							MobileWorldQuestReward mobileWorldQuestReward2 = currency[k];
							if (mobileWorldQuestReward2.RecordID == 1220)
							{
								matchesFilter = true;
								break;
							}
						}
					}
					if (this.IsFilterEnabled(MapFilterType.Gold) && mobileWorldQuest.Money > 0)
					{
						matchesFilter = true;
					}
					if (this.IsFilterEnabled(MapFilterType.Gear))
					{
						MobileWorldQuestReward[] item2 = mobileWorldQuest.Item;
						for (int l = 0; l < item2.Length; l++)
						{
							MobileWorldQuestReward mobileWorldQuestReward3 = item2[l];
							ItemRec record = StaticDB.itemDB.GetRecord(mobileWorldQuestReward3.RecordID);
							if (record != null && (record.ClassID == 2 || record.ClassID == 3 || record.ClassID == 4 || record.ClassID == 6))
							{
								matchesFilter = true;
								break;
							}
						}
					}
					if (this.IsFilterEnabled(MapFilterType.ProfessionMats))
					{
						MobileWorldQuestReward[] item3 = mobileWorldQuest.Item;
						for (int m = 0; m < item3.Length; m++)
						{
							MobileWorldQuestReward mobileWorldQuestReward4 = item3[m];
							ItemRec record2 = StaticDB.itemDB.GetRecord(mobileWorldQuestReward4.RecordID);
							if (record2 != null && record2.ClassID == 7)
							{
								matchesFilter = true;
								break;
							}
						}
					}
					if (this.IsFilterEnabled(MapFilterType.PetCharms))
					{
						MobileWorldQuestReward[] item4 = mobileWorldQuest.Item;
						for (int n = 0; n < item4.Length; n++)
						{
							MobileWorldQuestReward mobileWorldQuestReward5 = item4[n];
							if (mobileWorldQuestReward5.RecordID == 116415)
							{
								matchesFilter = true;
								break;
							}
						}
					}
					if (!matchesFilter)
					{
						continue;
					}
				}
				GameObject gameObject = Object.Instantiate<GameObject>(AdventureMapPanel.instance.m_AdvMapWorldQuestPrefab);
				gameObject.get_transform().SetParent(this.m_missionAndWordQuestArea.get_transform(), false);
				float num = 0.10271506f;
				float num2 = (float)mobileWorldQuest.StartLocationY * -num;
				float num3 = (float)mobileWorldQuest.StartLocationX * num;
				float num4 = 1036.88037f;
				float num5 = 597.2115f;
				num2 += num4;
				num3 += num5;
				float width = this.m_worldMapLowDetail.get_sprite().get_textureRect().get_width();
				float height = this.m_worldMapLowDetail.get_sprite().get_textureRect().get_height();
				Vector2 vector = new Vector3(num2 / width, num3 / height);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.set_anchorMin(vector);
				component.set_anchorMax(vector);
				component.set_anchoredPosition(Vector2.get_zero());
				AdventureMapWorldQuest component2 = gameObject.GetComponent<AdventureMapWorldQuest>();
				component2.SetQuestID(mobileWorldQuest.QuestID);
				StackableMapIcon component3 = gameObject.GetComponent<StackableMapIcon>();
				if (component3 != null)
				{
					component3.RegisterWithManager();
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
	}

	private void MissionPanelSliderLeftTweenCallback(float val)
	{
		this.m_mapAndRewardParentViewRT.set_offsetMin(new Vector2(val, this.m_mapAndRewardParentViewRT.get_offsetMin().y));
		MapInfo componentInChildren = base.GetComponentInChildren<MapInfo>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.CalculateFillScale();
		this.m_pinchZoomContentManager.SetZoom(this.m_pinchZoomContentManager.m_zoomFactor, false);
	}

	private void Update()
	{
		this.m_currentVisibleZone = null;
		if (this.m_currentMapMission > 0)
		{
			this.m_secondsMissionHasBeenSelected += Time.get_deltaTime();
		}
		if (this.m_mapAndRewardParentViewRT.get_sizeDelta().x != this.m_parentViewRT.get_rect().get_width())
		{
			this.m_multiPanelViewSizeDelta = this.m_mapAndRewardParentViewRT.get_sizeDelta();
			this.m_multiPanelViewSizeDelta.x = this.m_parentViewRT.get_rect().get_width();
			this.m_mapAndRewardParentViewRT.set_sizeDelta(this.m_multiPanelViewSizeDelta);
		}
	}

	private void ZoomOutTweenCallback(float newZoomFactor)
	{
		this.m_pinchZoomContentManager.SetZoom(newZoomFactor, true);
	}

	private void ZoomInTweenCallback(float newZoomFactor)
	{
		this.m_pinchZoomContentManager.SetZoom(newZoomFactor, false);
	}

	public void CenterAndZoomOut()
	{
		AutoCenterItem componentInParent = base.get_gameObject().GetComponentInParent<AutoCenterItem>();
		if (componentInParent && componentInParent.IsCentered())
		{
			this.CenterAndZoom(Vector2.get_zero(), null, false);
		}
	}

	public void CenterAndZoomIn()
	{
		if (Input.get_touchCount() != 1)
		{
			return;
		}
		Vector2 position = Input.GetTouch(0).get_position();
		this.CenterAndZoom(position, null, true);
	}

	public void ShowWorldMap(bool show)
	{
		this.m_mainMapInfo.get_gameObject().SetActive(show);
	}

	public void CenterAndZoom(Vector2 tapPos, ZoneButton zoneButton, bool zoomIn)
	{
		this.m_lastTappedZoneButton = zoneButton;
		Vector3[] array = new Vector3[4];
		this.m_mapViewRT.GetWorldCorners(array);
		float num = array[2].x - array[0].x;
		float num2 = array[2].y - array[0].y;
		Vector2 vector;
		vector.x = array[0].x + num * 0.5f;
		vector.y = array[0].y + num2 * 0.5f;
		Vector3[] array2 = new Vector3[4];
		this.m_mapViewContentsRT.GetWorldCorners(array2);
		float num3 = array2[2].x - array2[0].x;
		float num4 = array2[2].y - array2[0].y;
		Vector2 vector2;
		vector2.x = array2[0].x + num3 * 0.5f;
		vector2.y = array2[0].y + num4 * 0.5f;
		MapInfo componentInChildren = base.GetComponentInChildren<MapInfo>();
		if (componentInChildren == null)
		{
			return;
		}
		if (zoomIn)
		{
			if (this.m_pinchZoomContentManager.m_zoomFactor < 1.001f)
			{
				Main.instance.m_UISound.Play_MapZoomIn();
			}
			Vector2 vector3 = tapPos - vector2;
			vector3 *= componentInChildren.m_maxZoomFactor / this.m_pinchZoomContentManager.m_zoomFactor;
			Vector2 vector4 = vector2 + vector3;
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"name",
				"Zoom View In",
				"from",
				this.m_pinchZoomContentManager.m_zoomFactor,
				"to",
				componentInChildren.m_maxZoomFactor,
				"easeType",
				"easeOutCubic",
				"time",
				0.8f,
				"onupdate",
				"ZoomInTweenCallback"
			}));
			iTween.MoveBy(this.m_mapViewContentsRT.get_gameObject(), iTween.Hash(new object[]
			{
				"name",
				"Pan View To Point (in)",
				"x",
				vector.x - vector4.x,
				"y",
				vector.y - vector4.y,
				"easeType",
				"easeOutQuad",
				"time",
				0.8f
			}));
		}
		else
		{
			if (this.OnZoomOutMap != null)
			{
				this.OnZoomOutMap.Invoke();
			}
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"name",
				"Zoom View Out",
				"from",
				this.m_pinchZoomContentManager.m_zoomFactor,
				"to",
				componentInChildren.m_minZoomFactor,
				"easeType",
				"easeOutCubic",
				"time",
				0.8f,
				"onupdate",
				"ZoomOutTweenCallback"
			}));
			iTween.MoveTo(this.m_mapViewContentsRT.get_gameObject(), iTween.Hash(new object[]
			{
				"name",
				"Pan View To Point (out)",
				"x",
				vector.x,
				"y",
				vector.y,
				"easeType",
				"easeOutQuad",
				"time",
				0.8f
			}));
		}
	}

	public Vector2 ScreenPointToLocalPointInMapViewRT(Vector2 screenPoint)
	{
		Vector2 result;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_mapViewRT, screenPoint, this.m_mainCamera, ref result);
		return result;
	}

	public void MissionFollowerSlotChanged(int garrFollowerID, bool inParty)
	{
		if (this.OnMissionFollowerSlotChanged != null)
		{
			this.OnMissionFollowerSlotChanged.Invoke(garrFollowerID, inParty);
		}
	}

	public void HandleBountyInfoUpdated()
	{
		BountySite[] componentsInChildren = this.m_missionAndWordQuestArea.get_transform().GetComponentsInChildren<BountySite>(true);
		BountySite[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			BountySite bountySite = array[i];
			Object.DestroyImmediate(bountySite.get_gameObject());
		}
		IEnumerator enumerator = PersistentBountyData.bountyDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MobileWorldQuestBounty mobileWorldQuestBounty = (MobileWorldQuestBounty)enumerator.get_Current();
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_bountySitePrefab);
				BountySite component = gameObject.GetComponent<BountySite>();
				component.SetBounty(mobileWorldQuestBounty);
				gameObject.set_name("BountySite " + mobileWorldQuestBounty.QuestID);
				RectTransform component2 = gameObject.GetComponent<RectTransform>();
				gameObject.get_transform().SetParent(this.m_missionAndWordQuestArea.get_transform(), false);
				component2.set_anchorMin(new Vector2(0.5f, 0.5f));
				component2.set_anchorMax(new Vector2(0.5f, 0.5f));
				QuestV2Rec record = StaticDB.questDB.GetRecord(mobileWorldQuestBounty.QuestID);
				bool flag = true;
				ZoneMissionOverview zoneMissionOverview = null;
				int questSortID = record.QuestSortID;
				if (questSortID == 7502)
				{
					goto IL_1AA;
				}
				if (questSortID != 7503)
				{
					if (questSortID != 7334)
					{
						if (questSortID != 7541)
						{
							if (questSortID != 7558)
							{
								if (questSortID != 7637)
								{
									if (questSortID == 8147)
									{
										goto IL_1AA;
									}
									flag = false;
								}
								else
								{
									zoneMissionOverview = this.m_allZoneMissionOverviews[4];
								}
							}
							else
							{
								zoneMissionOverview = this.m_allZoneMissionOverviews[5];
							}
						}
						else
						{
							zoneMissionOverview = this.m_allZoneMissionOverviews[3];
						}
					}
					else
					{
						zoneMissionOverview = this.m_allZoneMissionOverviews[0];
					}
				}
				else
				{
					zoneMissionOverview = this.m_allZoneMissionOverviews[2];
				}
				IL_1C1:
				if (flag)
				{
					if (zoneMissionOverview.zoneNameTag.get_Length() > 0)
					{
						gameObject.get_transform().SetParent(zoneMissionOverview.m_bountyButtonRoot.get_transform(), false);
					}
					else
					{
						gameObject.get_transform().SetParent(zoneMissionOverview.m_anonymousBountyButtonRoot.get_transform(), false);
					}
					gameObject.get_transform().set_localPosition(Vector3.get_zero());
					component.m_errorImage.get_gameObject().SetActive(false);
				}
				else
				{
					gameObject.get_transform().set_localPosition(new Vector3(0f, 0f, 0f));
					component.m_errorImage.get_gameObject().SetActive(true);
				}
				StackableMapIcon component3 = gameObject.GetComponent<StackableMapIcon>();
				if (component3 != null)
				{
					component3.RegisterWithManager();
					continue;
				}
				continue;
				IL_1AA:
				zoneMissionOverview = this.m_allZoneMissionOverviews[6];
				goto IL_1C1;
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
	}

	public void HideRecentCharacterPanel()
	{
		this.m_playerInfoDisplay.HideRecentCharacterPanel();
	}
}
