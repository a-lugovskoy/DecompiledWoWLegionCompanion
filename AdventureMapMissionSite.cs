using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class AdventureMapMissionSite : MonoBehaviour
{
	public Image m_errorImage;

	public CanvasGroup m_availableMissionGroup;

	public CanvasGroup m_inProgressMissionGroup;

	public CanvasGroup m_completeMissionGroup;

	public Image m_availableMissionTypeIcon;

	public Image m_inProgressMissionTypeIcon;

	public Text m_missingMissionTypeIconErrorText;

	public Text m_missionLevelText;

	public Text m_missionTimeRemainingText;

	public Image m_followerPortraitRingImage;

	public Image m_followerPortraitImage;

	public CanvasGroup m_missionSiteGroup;

	public RectTransform m_myRT;

	public int m_areaID;

	public Transform m_selectedEffectRoot;

	public Image m_selectionRing;

	public RectTransform m_zoomScaleRoot;

	public bool m_isStackablePreview;

	public GameObject[] m_stuffToHideInPreviewMode;

	private int m_garrMissionID;

	private int m_missionDurationInSeconds;

	private long m_missionStartedTime;

	private bool m_claimedMyLoot;

	private bool m_showedMyLoot;

	private bool m_isSupportMission;

	private bool m_autoCompletedSupportMission;

	public Text m_missionCompleteText;

	private UiAnimMgr.UiAnimHandle m_selectedEffectAnimHandle;

	private void OnEnable()
	{
		AdventureMapPanel expr_05 = AdventureMapPanel.instance;
		expr_05.TestIconSizeChanged = (Action<float>)Delegate.Combine(expr_05.TestIconSizeChanged, new Action<float>(this.OnTestIconSizeChanged));
		Main expr_2B = Main.instance;
		expr_2B.ClaimMissionBonusResultAction = (Action<int, bool, int>)Delegate.Combine(expr_2B.ClaimMissionBonusResultAction, new Action<int, bool, int>(this.HandleClaimMissionBonusResult));
		Main expr_51 = Main.instance;
		expr_51.CompleteMissionResultAction = (Action<int, int, int>)Delegate.Combine(expr_51.CompleteMissionResultAction, new Action<int, int, int>(this.HandleCompleteMissionResult));
		PinchZoomContentManager expr_7C = AdventureMapPanel.instance.m_pinchZoomContentManager;
		expr_7C.ZoomFactorChanged = (Action)Delegate.Combine(expr_7C.ZoomFactorChanged, new Action(this.HandleZoomChanged));
	}

	private void OnDisable()
	{
		AdventureMapPanel expr_05 = AdventureMapPanel.instance;
		expr_05.TestIconSizeChanged = (Action<float>)Delegate.Remove(expr_05.TestIconSizeChanged, new Action<float>(this.OnTestIconSizeChanged));
		Main expr_2B = Main.instance;
		expr_2B.ClaimMissionBonusResultAction = (Action<int, bool, int>)Delegate.Remove(expr_2B.ClaimMissionBonusResultAction, new Action<int, bool, int>(this.HandleClaimMissionBonusResult));
		Main expr_51 = Main.instance;
		expr_51.CompleteMissionResultAction = (Action<int, int, int>)Delegate.Remove(expr_51.CompleteMissionResultAction, new Action<int, int, int>(this.HandleCompleteMissionResult));
		PinchZoomContentManager expr_7C = AdventureMapPanel.instance.m_pinchZoomContentManager;
		expr_7C.ZoomFactorChanged = (Action)Delegate.Remove(expr_7C.ZoomFactorChanged, new Action(this.HandleZoomChanged));
	}

	private void OnTestIconSizeChanged(float newScale)
	{
		base.get_transform().set_localScale(Vector3.get_one() * newScale);
	}

	private void HandleZoomChanged()
	{
		this.m_zoomScaleRoot.set_sizeDelta(this.m_myRT.get_sizeDelta() * AdventureMapPanel.instance.m_pinchZoomContentManager.m_zoomFactor);
	}

	private void Awake()
	{
		this.m_selectionRing.get_gameObject().SetActive(false);
		AdventureMapPanel expr_16 = AdventureMapPanel.instance;
		expr_16.MissionMapSelectionChangedAction = (Action<int>)Delegate.Combine(expr_16.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		this.m_missionCompleteText.set_text(StaticDB.GetString("MISSION_COMPLETE", null));
		this.m_isStackablePreview = false;
	}

	private void Update()
	{
		this.UpdateMissionRemainingTimeDisplay();
		Vector3[] array = new Vector3[4];
		AdventureMapPanel.instance.m_mapViewRT.GetWorldCorners(array);
		float num = array[2].x - array[0].x;
		float num2 = array[2].y - array[0].y;
		Rect rect = new Rect(array[0].x, array[0].y, num, num2);
		Vector3[] array2 = new Vector3[4];
		this.m_myRT.GetWorldCorners(array2);
		float num3 = array2[2].x - array2[0].x;
		float num4 = array2[2].y - array2[0].y;
		Rect rect2 = new Rect(array2[0].x, array2[0].y, num3, num4);
		if (!rect.Overlaps(rect2))
		{
			if (AdventureMapPanel.instance.GetCurrentMapMission() == this.m_garrMissionID)
			{
				AdventureMapPanel.instance.SelectMissionFromMap(0);
			}
			StackableMapIcon component = base.GetComponent<StackableMapIcon>();
			if (component != null && AdventureMapPanel.instance.GetSelectedIconContainer() == component.GetContainer())
			{
				AdventureMapPanel.instance.SetSelectedIconContainer(null);
			}
			return;
		}
	}

	private void UpdateMissionRemainingTimeDisplay()
	{
		if (!this.m_inProgressMissionGroup.get_gameObject().get_activeSelf())
		{
			return;
		}
		if (this.m_missionSiteGroup != null && this.m_missionSiteGroup.get_alpha() < 0.1f)
		{
			return;
		}
		long num = GarrisonStatus.CurrentTime() - this.m_missionStartedTime;
		long num2 = (long)this.m_missionDurationInSeconds - num;
		num2 = ((num2 <= 0L) ? 0L : num2);
		if (!this.m_isSupportMission)
		{
			Duration duration = new Duration((int)num2);
			this.m_missionTimeRemainingText.set_text(duration.DurationString);
		}
		if (num2 == 0L)
		{
			if (this.m_isSupportMission)
			{
				if (!this.m_autoCompletedSupportMission)
				{
					if (AdventureMapPanel.instance.ShowMissionResultAction != null)
					{
						AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_garrMissionID, 1, false);
					}
					Main.instance.CompleteMission(this.m_garrMissionID);
					this.m_autoCompletedSupportMission = true;
				}
			}
			else
			{
				this.m_availableMissionGroup.get_gameObject().SetActive(false);
				this.m_inProgressMissionGroup.get_gameObject().SetActive(false);
				this.m_completeMissionGroup.get_gameObject().SetActive(true);
			}
		}
	}

	public void SetMission(int garrMissionID)
	{
		base.get_gameObject().set_name("AdvMapMissionSite " + garrMissionID);
		if (!PersistentMissionData.missionDictionary.ContainsKey(garrMissionID))
		{
			return;
		}
		this.m_garrMissionID = garrMissionID;
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		if (record == null || !PersistentMissionData.missionDictionary.ContainsKey(garrMissionID))
		{
			return;
		}
		this.m_areaID = record.AreaID;
		this.m_isSupportMission = false;
		if ((record.Flags & 16u) != 0u)
		{
			this.m_isSupportMission = true;
			this.m_missionTimeRemainingText.set_text("Fortified");
		}
		GarrMissionTypeRec record2 = StaticDB.garrMissionTypeDB.GetRecord((int)record.GarrMissionTypeID);
		if (record2.UiTextureAtlasMemberID > 0u)
		{
			Sprite atlasSprite = TextureAtlas.instance.GetAtlasSprite((int)record2.UiTextureAtlasMemberID);
			if (atlasSprite != null)
			{
				this.m_availableMissionTypeIcon.set_sprite(atlasSprite);
				this.m_inProgressMissionTypeIcon.set_sprite(atlasSprite);
			}
		}
		JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(garrMissionID);
		if (jamGarrisonMobileMission.MissionState == 1 || jamGarrisonMobileMission.MissionState == 2)
		{
			this.m_missionDurationInSeconds = (int)jamGarrisonMobileMission.MissionDuration;
		}
		else
		{
			this.m_missionDurationInSeconds = record.MissionDuration;
		}
		this.m_missionStartedTime = jamGarrisonMobileMission.StartTime;
		this.m_availableMissionGroup.get_gameObject().SetActive(jamGarrisonMobileMission.MissionState == 0);
		this.m_inProgressMissionGroup.get_gameObject().SetActive(jamGarrisonMobileMission.MissionState == 1);
		this.m_completeMissionGroup.get_gameObject().SetActive(jamGarrisonMobileMission.MissionState == 2 || jamGarrisonMobileMission.MissionState == 3);
		if (jamGarrisonMobileMission.MissionState == 1)
		{
			using (Dictionary<int, JamGarrisonFollower>.Enumerator enumerator = PersistentFollowerData.followerDictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, JamGarrisonFollower> current = enumerator.get_Current();
					if (current.get_Value().CurrentMissionID == garrMissionID)
					{
						GarrFollowerRec record3 = StaticDB.garrFollowerDB.GetRecord(current.get_Value().GarrFollowerID);
						if (record3 != null)
						{
							Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.PortraitIcons, (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record3.AllianceIconFileDataID : record3.HordeIconFileDataID);
							if (sprite != null)
							{
								this.m_followerPortraitImage.set_sprite(sprite);
							}
							this.m_followerPortraitRingImage.GetComponent<Image>().set_enabled(true);
							break;
						}
					}
				}
			}
		}
		this.m_missionLevelText.set_text(string.Empty + record.TargetLevel + ((record.TargetLevel != 110) ? string.Empty : (" (" + record.TargetItemLevel + ")")));
		this.UpdateMissionRemainingTimeDisplay();
	}

	public void HandleMissionChanged(int newMissionID)
	{
		if (this.m_isStackablePreview || this.m_garrMissionID == 0)
		{
			return;
		}
		if (this.m_selectedEffectAnimHandle != null)
		{
			UiAnimation anim = this.m_selectedEffectAnimHandle.GetAnim();
			if (anim != null)
			{
				anim.Stop(0.5f);
			}
		}
		if (newMissionID == this.m_garrMissionID)
		{
			this.m_selectedEffectAnimHandle = UiAnimMgr.instance.PlayAnim("MinimapLoopPulseAnim", this.m_selectedEffectRoot, Vector3.get_zero(), 2.5f, 0f);
		}
		if (this.m_selectionRing != null)
		{
			this.m_selectionRing.get_gameObject().SetActive(newMissionID == this.m_garrMissionID);
		}
	}

	public void OnTapAvailableMission()
	{
		AdventureMapPanel.instance.SelectMissionFromMap(this.m_garrMissionID);
		this.JustZoomToMission();
	}

	public void JustZoomToMission()
	{
		UiAnimMgr.instance.PlayAnim("MinimapPulseAnim", base.get_transform(), Vector3.get_zero(), 3f, 0f);
		Main.instance.m_UISound.Play_SelectMission();
		if (StaticDB.garrMissionDB.GetRecord(this.m_garrMissionID) == null)
		{
			return;
		}
		AdventureMapPanel instance = AdventureMapPanel.instance;
		StackableMapIcon component = base.GetComponent<StackableMapIcon>();
		StackableMapIconContainer stackableMapIconContainer = null;
		if (component != null)
		{
			stackableMapIconContainer = component.GetContainer();
			AdventureMapPanel.instance.SetSelectedIconContainer(stackableMapIconContainer);
		}
		Vector2 tapPos;
		if (stackableMapIconContainer != null)
		{
			tapPos = new Vector2(stackableMapIconContainer.get_transform().get_position().x, stackableMapIconContainer.get_transform().get_position().y);
		}
		else
		{
			tapPos = new Vector2(base.get_transform().get_position().x, base.get_transform().get_position().y);
		}
		instance.CenterAndZoom(tapPos, null, true);
	}

	public void OnTapCompletedMission()
	{
		UiAnimMgr.instance.PlayAnim("MinimapPulseAnim", base.get_transform(), Vector3.get_zero(), 3f, 0f);
		Main.instance.m_UISound.Play_SelectMission();
		if (AdventureMapPanel.instance.ShowMissionResultAction != null)
		{
			AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_garrMissionID, 1, false);
		}
		Main.instance.CompleteMission(this.m_garrMissionID);
	}

	public void HandleCompleteMissionResult(int garrMissionID, int result, int missionSuccessChance)
	{
		if (garrMissionID == this.m_garrMissionID)
		{
			this.OnMissionStatusChanged(false);
		}
	}

	public void HandleClaimMissionBonusResult(int garrMissionID, bool awardOvermax, int result)
	{
		if (garrMissionID == this.m_garrMissionID)
		{
			if (result == 0)
			{
				this.OnMissionStatusChanged(awardOvermax);
			}
			else
			{
				Debug.LogWarning("CLAIM MISSION FAILED! Result = " + (GARRISON_RESULT)result);
			}
		}
	}

	public void OnMissionStatusChanged(bool awardOvermax = false)
	{
		JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(this.m_garrMissionID);
		if (jamGarrisonMobileMission.MissionState == 6 && !this.m_claimedMyLoot)
		{
			Debug.Log("OnMissionStatusChanged() MISSION FAILED " + this.m_garrMissionID);
			this.m_claimedMyLoot = true;
			this.ShowMissionFailure();
			return;
		}
		if (!this.m_claimedMyLoot)
		{
			if (jamGarrisonMobileMission.MissionState == 2 || jamGarrisonMobileMission.MissionState == 3)
			{
				Main.instance.ClaimMissionBonus(this.m_garrMissionID);
				this.m_claimedMyLoot = true;
			}
			return;
		}
		if (!this.m_showedMyLoot)
		{
			this.ShowMissionSuccess(awardOvermax);
			this.m_showedMyLoot = true;
		}
	}

	private void ShowMissionFailure()
	{
		if (AdventureMapPanel.instance.ShowMissionResultAction != null)
		{
			AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_garrMissionID, 3, false);
		}
		Object.Destroy(base.get_gameObject());
	}

	private void ShowMissionSuccess(bool awardOvermax)
	{
		if (AdventureMapPanel.instance.ShowMissionResultAction != null)
		{
			AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_garrMissionID, 2, awardOvermax);
		}
		Object.Destroy(base.get_gameObject());
	}

	public void ShowInProgressMissionDetails()
	{
		Main.instance.m_UISound.Play_SelectWorldQuest();
		if (AdventureMapPanel.instance.ShowMissionResultAction != null)
		{
			AdventureMapPanel.instance.ShowMissionResultAction.Invoke(this.m_garrMissionID, 0, false);
		}
	}

	public void SetPreviewMode(bool isPreview)
	{
		this.m_isStackablePreview = isPreview;
		GameObject[] stuffToHideInPreviewMode = this.m_stuffToHideInPreviewMode;
		for (int i = 0; i < stuffToHideInPreviewMode.Length; i++)
		{
			GameObject gameObject = stuffToHideInPreviewMode[i];
			gameObject.SetActive(!isPreview);
		}
	}
}
