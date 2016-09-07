using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderPanel : MonoBehaviour
{
	public float m_previewSeconds;

	public float m_maximizeSeconds;

	public float m_hideSeconds;

	public bool m_isHorizontal;

	private Vector2 m_startingPos;

	private Vector2 m_startingPointerPos;

	public int m_startingVerticalOffset;

	public int m_missionPanelSliderPreviewHeight;

	public int m_missionPanelSliderFullHeight;

	public bool m_stretchAbovePreviewHight;

	public int m_missionPanelSliderPreviewWidth;

	public int m_missionPanelSliderFullWidth;

	public bool m_stretchAbovePreviewWidth;

	public bool m_hidePreviewWhenMaximized;

	public CanvasGroup m_masterCanvasGroup;

	public CanvasGroup m_previewCanvasGroup;

	public CanvasGroup m_maximizedCanvasGroup;

	public Action SliderPanelMaximizedAction;

	public Action SliderPanelBeginMinimizeAction;

	public Action SliderPanelFinishMinimizeAction;

	public Action SliderPanelBeginDragAction;

	public Action SliderPanelBeginShrinkToPreviewPositionAction;

	private bool m_busyMoving;

	private bool m_movementIsPending;

	private bool m_pendingMovementIsToShowPreview;

	private bool m_pendingMovementIsToMaximize;

	private bool m_pendingMovementIsToHide;

	private bool m_isShowing;

	private void OnEnable()
	{
		this.m_busyMoving = false;
		this.m_movementIsPending = false;
		this.m_isShowing = false;
	}

	private void Update()
	{
		if (this.m_movementIsPending && !this.m_busyMoving)
		{
			this.m_movementIsPending = false;
			if (this.m_pendingMovementIsToShowPreview)
			{
				this.ShowSliderPanel();
			}
			else if (this.m_pendingMovementIsToMaximize)
			{
				this.MaximizeSliderPanel();
			}
			else if (this.m_pendingMovementIsToHide)
			{
				this.HideSliderPanel();
			}
		}
	}

	public void OnBeginDrag(BaseEventData eventData)
	{
		if (this.SliderPanelBeginDragAction != null)
		{
			this.SliderPanelBeginDragAction.Invoke();
		}
		this.m_startingPos = base.get_transform().get_localPosition();
		Vector2 startingPointerPos = AdventureMapPanel.instance.ScreenPointToLocalPointInMapViewRT(((PointerEventData)eventData).get_position());
		this.m_startingPointerPos = startingPointerPos;
		this.m_busyMoving = false;
		this.m_movementIsPending = false;
		iTween.Stop(base.get_gameObject());
	}

	public void OnDrag(BaseEventData eventData)
	{
		if (!this.m_isHorizontal)
		{
			float num = AdventureMapPanel.instance.ScreenPointToLocalPointInMapViewRT(((PointerEventData)eventData).get_position()).y - this.m_startingPointerPos.y + this.m_startingPos.y;
			float num2 = (float)this.m_missionPanelSliderFullHeight / 2f;
			num = Mathf.Min(num, num2);
			base.get_transform().set_localPosition(new Vector3(base.get_transform().get_localPosition().x, num, base.get_transform().get_localPosition().z));
			RectTransform component = base.GetComponent<RectTransform>();
			if (this.m_stretchAbovePreviewHight)
			{
				if (num > (float)this.m_missionPanelSliderPreviewHeight - num2)
				{
					Vector2 sizeDelta = component.get_sizeDelta();
					sizeDelta.y = num2 + num;
					component.set_sizeDelta(sizeDelta);
				}
				else
				{
					Vector2 sizeDelta2 = component.get_sizeDelta();
					sizeDelta2.y = (float)this.m_missionPanelSliderPreviewHeight;
					component.set_sizeDelta(sizeDelta2);
				}
			}
		}
		else
		{
			float num3 = AdventureMapPanel.instance.ScreenPointToLocalPointInMapViewRT(((PointerEventData)eventData).get_position()).x - this.m_startingPointerPos.x + this.m_startingPos.x;
			float num4 = (float)this.m_missionPanelSliderFullWidth / 2f;
			num3 = Mathf.Min(num3, num4);
			base.get_transform().set_localPosition(new Vector3(num3, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z));
			RectTransform component2 = base.GetComponent<RectTransform>();
			if (this.m_stretchAbovePreviewWidth)
			{
				if (num3 > (float)this.m_missionPanelSliderPreviewWidth - num4)
				{
					Vector2 sizeDelta3 = component2.get_sizeDelta();
					sizeDelta3.x = num4 + num3;
					component2.set_sizeDelta(sizeDelta3);
				}
				else
				{
					Vector2 sizeDelta4 = component2.get_sizeDelta();
					sizeDelta4.x = (float)this.m_missionPanelSliderPreviewWidth;
					component2.set_sizeDelta(sizeDelta4);
				}
			}
		}
	}

	private void PanelSliderBottomTweenCallback(Vector2 val)
	{
		RectTransform component = base.GetComponent<RectTransform>();
		component.set_anchoredPosition(new Vector2(val.x, val.y));
		if (!this.m_isHorizontal)
		{
			if (this.m_stretchAbovePreviewHight)
			{
				if (val.y > (float)this.m_missionPanelSliderPreviewHeight)
				{
					Vector2 sizeDelta = component.get_sizeDelta();
					sizeDelta.y = val.y;
					component.set_sizeDelta(sizeDelta);
				}
				else
				{
					Vector2 sizeDelta2 = component.get_sizeDelta();
					sizeDelta2.y = (float)this.m_missionPanelSliderPreviewHeight;
					component.set_sizeDelta(sizeDelta2);
				}
			}
		}
		else if (this.m_stretchAbovePreviewWidth)
		{
			if (val.x > (float)this.m_missionPanelSliderPreviewWidth)
			{
				Vector2 sizeDelta3 = component.get_sizeDelta();
				sizeDelta3.x = val.x;
				component.set_sizeDelta(sizeDelta3);
			}
			else
			{
				Vector2 sizeDelta4 = component.get_sizeDelta();
				sizeDelta4.x = (float)this.m_missionPanelSliderPreviewWidth;
				component.set_sizeDelta(sizeDelta4);
			}
		}
		this.m_startingPos = val;
		if (this.m_hidePreviewWhenMaximized && !this.m_isHorizontal)
		{
			float num = (val.y - (float)this.m_missionPanelSliderPreviewHeight) / (float)(this.m_missionPanelSliderFullHeight - this.m_missionPanelSliderPreviewHeight);
			if (this.m_previewCanvasGroup != null)
			{
				this.m_previewCanvasGroup.set_alpha(1f - num);
			}
			if (this.m_maximizedCanvasGroup != null)
			{
				this.m_maximizedCanvasGroup.set_alpha(num);
			}
		}
	}

	private void DisableSliderPanel()
	{
		if (this.m_masterCanvasGroup != null)
		{
			this.m_masterCanvasGroup.set_alpha(0f);
		}
		this.m_busyMoving = false;
		this.m_isShowing = false;
		if (this.SliderPanelFinishMinimizeAction != null)
		{
			this.SliderPanelFinishMinimizeAction.Invoke();
		}
	}

	private void OnDoneSlidingInPreview()
	{
		this.m_busyMoving = false;
		this.m_isShowing = true;
		if (this.m_hidePreviewWhenMaximized && !this.m_isHorizontal && this.m_previewCanvasGroup != null)
		{
			this.m_previewCanvasGroup.set_blocksRaycasts(true);
		}
	}

	private void OnDoneSlidingInMaximize()
	{
		this.m_busyMoving = false;
		this.m_isShowing = true;
		if (this.m_hidePreviewWhenMaximized && !this.m_isHorizontal)
		{
			if (this.m_previewCanvasGroup != null)
			{
				this.m_previewCanvasGroup.set_alpha(0f);
				this.m_previewCanvasGroup.set_blocksRaycasts(false);
			}
			if (this.m_maximizedCanvasGroup != null)
			{
				this.m_maximizedCanvasGroup.set_alpha(1f);
			}
		}
		if (this.SliderPanelMaximizedAction != null)
		{
			this.SliderPanelMaximizedAction.Invoke();
		}
	}

	public void MissionPanelSlider_HandleAutopositioning_Bottom()
	{
		float num = 5f;
		RectTransform component = base.GetComponent<RectTransform>();
		if (!this.m_isHorizontal)
		{
			if (this.m_startingPos.y < (float)this.m_missionPanelSliderPreviewHeight + num)
			{
				if (component.get_anchoredPosition().y < (float)this.m_missionPanelSliderPreviewHeight - num)
				{
					this.HideSliderPanel();
					return;
				}
				this.MaximizeSliderPanel();
				return;
			}
		}
		else if (this.m_startingPos.x < (float)this.m_missionPanelSliderPreviewWidth + num)
		{
			if (component.get_anchoredPosition().x < (float)this.m_missionPanelSliderPreviewWidth - num)
			{
				this.HideSliderPanel();
				return;
			}
			this.MaximizeSliderPanel();
			return;
		}
		if (this.SliderPanelBeginShrinkToPreviewPositionAction != null)
		{
			this.SliderPanelBeginShrinkToPreviewPositionAction.Invoke();
		}
		this.ShowSliderPanel();
	}

	public void MaximizeSliderPanel()
	{
		if (this.m_busyMoving)
		{
			this.m_movementIsPending = true;
			this.m_pendingMovementIsToShowPreview = false;
			this.m_pendingMovementIsToMaximize = true;
			this.m_pendingMovementIsToHide = false;
			return;
		}
		this.m_busyMoving = true;
		if (this.m_masterCanvasGroup != null)
		{
			this.m_masterCanvasGroup.set_alpha(1f);
		}
		Vector2 vector = new Vector2((float)this.m_missionPanelSliderFullWidth, (float)(this.m_missionPanelSliderFullHeight + this.m_startingVerticalOffset));
		Vector2 anchoredPosition = base.GetComponent<RectTransform>().get_anchoredPosition();
		anchoredPosition.y += (float)this.m_startingVerticalOffset;
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"Slide Mission Details In (Bottom)",
			"from",
			anchoredPosition,
			"to",
			vector,
			"easeType",
			"easeOutCubic",
			"time",
			(this.m_maximizeSeconds <= 0f) ? 0.5f : this.m_maximizeSeconds,
			"onupdate",
			"PanelSliderBottomTweenCallback",
			"oncomplete",
			"OnDoneSlidingInMaximize"
		}));
	}

	public void ShowSliderPanel()
	{
		if (this.m_busyMoving)
		{
			this.m_movementIsPending = true;
			this.m_pendingMovementIsToShowPreview = true;
			this.m_pendingMovementIsToMaximize = false;
			this.m_pendingMovementIsToHide = false;
			return;
		}
		this.m_busyMoving = true;
		if (this.m_masterCanvasGroup != null)
		{
			this.m_masterCanvasGroup.set_alpha(1f);
		}
		Vector2 vector = new Vector2((float)this.m_missionPanelSliderPreviewWidth, (float)this.m_missionPanelSliderPreviewHeight);
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"Slide Mission Details In (Bottom)",
			"from",
			base.GetComponent<RectTransform>().get_anchoredPosition(),
			"to",
			vector,
			"easeType",
			"easeOutCubic",
			"time",
			(this.m_previewSeconds <= 0f) ? 0.5f : this.m_previewSeconds,
			"onupdate",
			"PanelSliderBottomTweenCallback",
			"oncomplete",
			"OnDoneSlidingInPreview"
		}));
	}

	public void HideSliderPanel()
	{
		if (this.m_busyMoving)
		{
			this.m_movementIsPending = true;
			this.m_pendingMovementIsToShowPreview = false;
			this.m_pendingMovementIsToMaximize = false;
			this.m_pendingMovementIsToHide = true;
			return;
		}
		this.m_busyMoving = true;
		if (this.SliderPanelBeginMinimizeAction != null)
		{
			this.SliderPanelBeginMinimizeAction.Invoke();
		}
		Vector2 vector = new Vector2(0f, (float)this.m_startingVerticalOffset);
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"Slide Mission Details Out (Bottom)",
			"from",
			base.GetComponent<RectTransform>().get_anchoredPosition(),
			"to",
			vector,
			"easeType",
			"easeOutCubic",
			"time",
			(this.m_hideSeconds <= 0f) ? 0.5f : this.m_hideSeconds,
			"onupdate",
			"PanelSliderBottomTweenCallback",
			"oncomplete",
			"DisableSliderPanel"
		}));
	}

	public bool IsShowing()
	{
		return this.m_isShowing;
	}

	public bool IsBusyMoving()
	{
		return this.m_busyMoving;
	}
}
