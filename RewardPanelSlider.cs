using System;
using UnityEngine;
using WowJamMessages;
using WowStaticData;

public class RewardPanelSlider : MonoBehaviour
{
	public SliderPanel m_sliderPanel;

	public GameObject m_rewardIconArea;

	public bool m_isVertical;

	private void Awake()
	{
		this.ClearRewardIcons();
		this.m_sliderPanel = base.GetComponent<SliderPanel>();
		AdventureMapPanel expr_17 = AdventureMapPanel.instance;
		expr_17.OnZoomOutMap = (Action)Delegate.Combine(expr_17.OnZoomOutMap, new Action(this.OnZoomOutMap));
		AdventureMapPanel expr_3D = AdventureMapPanel.instance;
		expr_3D.MissionMapSelectionChangedAction = (Action<int>)Delegate.Combine(expr_3D.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		AdventureMapPanel expr_63 = AdventureMapPanel.instance;
		expr_63.OnAddMissionLootToRewardPanel = (Action<int>)Delegate.Combine(expr_63.OnAddMissionLootToRewardPanel, new Action<int>(this.OnAddMissionLootToRewardPanel));
		AdventureMapPanel expr_89 = AdventureMapPanel.instance;
		expr_89.OnShowMissionRewardPanel = (Action<bool>)Delegate.Combine(expr_89.OnShowMissionRewardPanel, new Action<bool>(this.OnShowMissionRewardPanel));
	}

	private void OnZoomOutMap()
	{
		this.m_sliderPanel.HideSliderPanel();
	}

	public void ClearRewardIcons()
	{
		MissionRewardDisplay[] componentsInChildren = this.m_rewardIconArea.GetComponentsInChildren<MissionRewardDisplay>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
		}
	}

	public void OnShowMissionRewardPanel(bool show)
	{
		if (show)
		{
			this.m_sliderPanel.ShowSliderPanel();
		}
		else
		{
			this.m_sliderPanel.HideSliderPanel();
		}
	}

	public void OnAddMissionLootToRewardPanel(int garrMissionID)
	{
		JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(garrMissionID);
		MissionRewardDisplay.InitMissionRewards(AdventureMapPanel.instance.m_missionRewardResultsDisplayPrefab, this.m_rewardIconArea.get_transform(), jamGarrisonMobileMission.Reward);
		if (jamGarrisonMobileMission.MissionState != 3)
		{
			return;
		}
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		if (record == null)
		{
			return;
		}
		if (StaticDB.rewardPackDB.GetRecord(record.OvermaxRewardPackID) == null)
		{
			return;
		}
		if (jamGarrisonMobileMission.OvermaxReward.Length > 0)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(AdventureMapPanel.instance.m_missionRewardResultsDisplayPrefab);
			gameObject.get_transform().SetParent(this.m_rewardIconArea.get_transform(), false);
			MissionRewardDisplay component = gameObject.GetComponent<MissionRewardDisplay>();
			component.InitReward(MissionRewardDisplay.RewardType.item, jamGarrisonMobileMission.OvermaxReward[0].ItemID, (int)jamGarrisonMobileMission.OvermaxReward[0].ItemQuantity, 0, jamGarrisonMobileMission.OvermaxReward[0].ItemFileDataID);
		}
	}

	public void OnShowMissionRewardSlider(bool show)
	{
		if (show)
		{
			this.m_sliderPanel.ShowSliderPanel();
		}
		else
		{
			this.m_sliderPanel.HideSliderPanel();
		}
	}

	private void HandleMissionChanged(int garrMissionID)
	{
		this.m_sliderPanel.HideSliderPanel();
	}
}
