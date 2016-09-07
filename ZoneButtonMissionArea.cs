using System;
using UnityEngine;

public class ZoneButtonMissionArea : MonoBehaviour
{
	private static PinchZoomContentManager m_pinchZoomManager;

	private void OnEnable()
	{
		if (ZoneButtonMissionArea.m_pinchZoomManager == null)
		{
			ZoneButtonMissionArea.m_pinchZoomManager = base.get_gameObject().GetComponentInParent<PinchZoomContentManager>();
		}
		PinchZoomContentManager expr_25 = ZoneButtonMissionArea.m_pinchZoomManager;
		expr_25.ZoomFactorChanged = (Action)Delegate.Combine(expr_25.ZoomFactorChanged, new Action(this.OnZoomChanged));
	}

	private void OnDisable()
	{
		PinchZoomContentManager expr_05 = ZoneButtonMissionArea.m_pinchZoomManager;
		expr_05.ZoomFactorChanged = (Action)Delegate.Remove(expr_05.ZoomFactorChanged, new Action(this.OnZoomChanged));
	}

	private void OnZoomChanged()
	{
		MapInfo componentInParent = base.get_gameObject().GetComponentInParent<MapInfo>();
		CanvasGroup component = base.get_gameObject().GetComponent<CanvasGroup>();
		component.set_alpha((ZoneButtonMissionArea.m_pinchZoomManager.m_zoomFactor - 1f) / (componentInParent.m_maxZoomFactor - 1f));
		component.set_blocksRaycasts(component.get_alpha() > 0.99f);
	}
}
