using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;

public class WorldQuestPanel : MonoBehaviour
{
	public SliderPanel m_sliderPanel;

	public Text m_worldQuestNameText;

	public Text m_worldQuestDescriptionText;

	public Text m_worldQuestTimeText;

	public MissionRewardDisplay m_missionRewardDisplayPrefab;

	public GameObject m_lootGroupObj;

	private int m_questID;

	public int QuestID
	{
		get
		{
			return this.m_questID;
		}
		set
		{
			this.m_questID = value;
			MissionRewardDisplay[] componentsInChildren = this.m_lootGroupObj.GetComponentsInChildren<MissionRewardDisplay>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
				}
			}
			if (this.m_questID > 0)
			{
				MobileWorldQuest mobileWorldQuest = (MobileWorldQuest)WorldQuestData.worldQuestDictionary.get_Item(this.m_questID);
				if (mobileWorldQuest != null)
				{
					this.m_worldQuestNameText.set_text(mobileWorldQuest.QuestTitle);
					this.m_worldQuestDescriptionText.set_text(mobileWorldQuest.QuestTitle);
					int num = (int)((long)mobileWorldQuest.EndTime - GarrisonStatus.CurrentTime());
					if (num < 0)
					{
						num = 0;
					}
					Duration duration = new Duration(num);
					this.m_worldQuestTimeText.set_text(duration.DurationString);
					MissionRewardDisplay.InitWorldQuestRewards(mobileWorldQuest, this.m_missionRewardDisplayPrefab.get_gameObject(), this.m_lootGroupObj.get_transform());
				}
			}
		}
	}

	private void Awake()
	{
		this.m_sliderPanel = base.GetComponent<SliderPanel>();
		AdventureMapPanel expr_11 = AdventureMapPanel.instance;
		expr_11.OnZoomOutMap = (Action)Delegate.Combine(expr_11.OnZoomOutMap, new Action(this.OnZoomOutMap));
		AdventureMapPanel expr_37 = AdventureMapPanel.instance;
		expr_37.MissionMapSelectionChangedAction = (Action<int>)Delegate.Combine(expr_37.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		AdventureMapPanel expr_5D = AdventureMapPanel.instance;
		expr_5D.OnShowMissionRewardPanel = (Action<bool>)Delegate.Combine(expr_5D.OnShowMissionRewardPanel, new Action<bool>(this.OnShowMissionRewardPanel));
		AdventureMapPanel expr_83 = AdventureMapPanel.instance;
		expr_83.WorldQuestChangedAction = (Action<int>)Delegate.Combine(expr_83.WorldQuestChangedAction, new Action<int>(this.HandleWorldQuestChanged));
	}

	public void OnZoomOutMap()
	{
		this.m_sliderPanel.HideSliderPanel();
	}

	public void HandleMissionChanged(int garrMissionID)
	{
		if (garrMissionID != 0)
		{
			this.m_sliderPanel.HideSliderPanel();
		}
	}

	private void OnShowMissionRewardPanel(bool show)
	{
		this.m_sliderPanel.HideSliderPanel();
	}

	private void HandleWorldQuestChanged(int worldQuestID)
	{
		this.QuestID = worldQuestID;
		if (this.QuestID != 0)
		{
			this.m_sliderPanel.ShowSliderPanel();
		}
		else
		{
			this.m_sliderPanel.HideSliderPanel();
		}
	}
}
