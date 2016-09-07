using System;
using UnityEngine;
using WowStatConstants;

public class MissionPanelSlider : MonoBehaviour
{
	public MissionDetailView m_missionDetailView;

	public FollowerDetailView m_followerDetailView;

	public bool m_isVertical;

	public bool m_disablePreview;

	public bool m_usedForMissionList;

	public SliderPanel m_sliderPanel;

	private int m_garrFollowerID;

	private void OnEnable()
	{
		this.m_sliderPanel = base.GetComponent<SliderPanel>();
		this.m_sliderPanel.m_masterCanvasGroup.set_alpha(0f);
		AdventureMapPanel expr_26 = AdventureMapPanel.instance;
		expr_26.OnZoomOutMap = (Action)Delegate.Combine(expr_26.OnZoomOutMap, new Action(this.OnZoomOutMap));
		if (this.m_usedForMissionList)
		{
			AdventureMapPanel expr_57 = AdventureMapPanel.instance;
			expr_57.MissionSelectedFromListAction = (Action<int>)Delegate.Combine(expr_57.MissionSelectedFromListAction, new Action<int>(this.HandleMissionChanged));
		}
		else
		{
			AdventureMapPanel expr_82 = AdventureMapPanel.instance;
			expr_82.MissionMapSelectionChangedAction = (Action<int>)Delegate.Combine(expr_82.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		}
		AdventureMapPanel expr_A8 = AdventureMapPanel.instance;
		expr_A8.OnShowMissionRewardPanel = (Action<bool>)Delegate.Combine(expr_A8.OnShowMissionRewardPanel, new Action<bool>(this.OnShowMissionRewardPanel));
		SliderPanel expr_CF = this.m_sliderPanel;
		expr_CF.SliderPanelMaximizedAction = (Action)Delegate.Combine(expr_CF.SliderPanelMaximizedAction, new Action(this.OnSliderPanelMaximized));
		SliderPanel expr_F6 = this.m_sliderPanel;
		expr_F6.SliderPanelBeginMinimizeAction = (Action)Delegate.Combine(expr_F6.SliderPanelBeginMinimizeAction, new Action(this.RevealMap));
		SliderPanel expr_11D = this.m_sliderPanel;
		expr_11D.SliderPanelBeginDragAction = (Action)Delegate.Combine(expr_11D.SliderPanelBeginDragAction, new Action(this.RevealMap));
		SliderPanel expr_144 = this.m_sliderPanel;
		expr_144.SliderPanelBeginShrinkToPreviewPositionAction = (Action)Delegate.Combine(expr_144.SliderPanelBeginShrinkToPreviewPositionAction, new Action(this.RevealMap));
		SliderPanel expr_16B = this.m_sliderPanel;
		expr_16B.SliderPanelFinishMinimizeAction = (Action)Delegate.Combine(expr_16B.SliderPanelFinishMinimizeAction, new Action(this.HandleSliderPanelFinishMinimize));
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideSliderPanel, this.m_sliderPanel.get_gameObject());
	}

	private void OnDisable()
	{
		AdventureMapPanel expr_05 = AdventureMapPanel.instance;
		expr_05.OnZoomOutMap = (Action)Delegate.Remove(expr_05.OnZoomOutMap, new Action(this.OnZoomOutMap));
		if (this.m_usedForMissionList)
		{
			AdventureMapPanel expr_36 = AdventureMapPanel.instance;
			expr_36.MissionSelectedFromListAction = (Action<int>)Delegate.Remove(expr_36.MissionSelectedFromListAction, new Action<int>(this.HandleMissionChanged));
		}
		else
		{
			AdventureMapPanel expr_61 = AdventureMapPanel.instance;
			expr_61.MissionMapSelectionChangedAction = (Action<int>)Delegate.Remove(expr_61.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		}
		AdventureMapPanel expr_87 = AdventureMapPanel.instance;
		expr_87.OnShowMissionRewardPanel = (Action<bool>)Delegate.Remove(expr_87.OnShowMissionRewardPanel, new Action<bool>(this.OnShowMissionRewardPanel));
		SliderPanel expr_AE = this.m_sliderPanel;
		expr_AE.SliderPanelMaximizedAction = (Action)Delegate.Remove(expr_AE.SliderPanelMaximizedAction, new Action(this.OnSliderPanelMaximized));
		SliderPanel expr_D5 = this.m_sliderPanel;
		expr_D5.SliderPanelBeginMinimizeAction = (Action)Delegate.Remove(expr_D5.SliderPanelBeginMinimizeAction, new Action(this.RevealMap));
		SliderPanel expr_FC = this.m_sliderPanel;
		expr_FC.SliderPanelBeginDragAction = (Action)Delegate.Remove(expr_FC.SliderPanelBeginDragAction, new Action(this.RevealMap));
		SliderPanel expr_123 = this.m_sliderPanel;
		expr_123.SliderPanelBeginShrinkToPreviewPositionAction = (Action)Delegate.Remove(expr_123.SliderPanelBeginShrinkToPreviewPositionAction, new Action(this.RevealMap));
		SliderPanel expr_14A = this.m_sliderPanel;
		expr_14A.SliderPanelFinishMinimizeAction = (Action)Delegate.Remove(expr_14A.SliderPanelFinishMinimizeAction, new Action(this.HandleSliderPanelFinishMinimize));
		Main.instance.m_backButtonManager.PopBackAction();
	}

	private void OnSliderPanelMaximized()
	{
	}

	private void HandleSliderPanelFinishMinimize()
	{
		AdventureMapPanel.instance.SelectMissionFromMap(0);
	}

	private void RevealMap()
	{
	}

	private void OnFollowerDetailViewSliderPanelMaximized()
	{
		this.m_missionDetailView.m_topLevelDetailViewCanvasGroup.set_alpha(0f);
	}

	private void RevealMissionDetails()
	{
		this.m_missionDetailView.m_topLevelDetailViewCanvasGroup.set_alpha(1f);
	}

	public void OnZoomOutMap()
	{
		this.m_sliderPanel.HideSliderPanel();
	}

	public void HandleMissionChanged(int garrMissionID)
	{
		if (garrMissionID > 0)
		{
			if (this.m_disablePreview)
			{
				this.m_sliderPanel.MaximizeSliderPanel();
			}
			else
			{
				this.m_sliderPanel.ShowSliderPanel();
			}
		}
		else
		{
			this.m_sliderPanel.HideSliderPanel();
		}
		iTween.StopByName(base.get_gameObject(), "bounce");
		if (!this.m_disablePreview)
		{
			iTween.PunchPosition(base.get_gameObject(), iTween.Hash(new object[]
			{
				"name",
				"bounce",
				"y",
				16,
				"time",
				2.2,
				"delay",
				4,
				"looptype",
				"loop"
			}));
		}
	}

	private void OnShowMissionRewardPanel(bool show)
	{
		if (show)
		{
			this.m_sliderPanel.HideSliderPanel();
		}
	}

	public void StopTheBounce()
	{
		iTween.StopByName(base.get_gameObject(), "bounce");
	}

	public void PlayMinimizeSound()
	{
		Main.instance.m_UISound.Play_DefaultNavClick();
	}
}
