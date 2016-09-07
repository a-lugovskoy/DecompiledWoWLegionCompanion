using System;
using UnityEngine;
using UnityEngine.UI;

public class ZoneMissionOverview : MonoBehaviour
{
	public ZoneMissionStat statDisplay_AvailableMissions;

	public ZoneMissionStat statDisplay_InProgressMissions;

	public ZoneMissionStat statDisplay_CompleteMissions;

	public ZoneMissionStat statDisplay_WorldQuests;

	public GameObject m_zoneNameArea;

	public GameObject m_statsArea;

	public GameObject m_bountyButtonRoot;

	public GameObject m_anonymousBountyButtonRoot;

	public int[] m_areaID;

	public string zoneNameTag;

	public Text zoneNameText;

	private static PinchZoomContentManager m_pinchZoomManager;

	private void OnEnable()
	{
		if (ZoneMissionOverview.m_pinchZoomManager == null)
		{
			ZoneMissionOverview.m_pinchZoomManager = base.get_gameObject().GetComponentInParent<PinchZoomContentManager>();
		}
		PinchZoomContentManager expr_25 = ZoneMissionOverview.m_pinchZoomManager;
		expr_25.ZoomFactorChanged = (Action)Delegate.Combine(expr_25.ZoomFactorChanged, new Action(this.OnZoomChanged));
	}

	private void OnDisable()
	{
		PinchZoomContentManager expr_05 = ZoneMissionOverview.m_pinchZoomManager;
		expr_05.ZoomFactorChanged = (Action)Delegate.Remove(expr_05.ZoomFactorChanged, new Action(this.OnZoomChanged));
	}

	private void Start()
	{
		if (this.zoneNameTag.get_Length() > 0)
		{
			this.zoneNameText.set_text(StaticDB.GetString(this.zoneNameTag, null));
		}
		else
		{
			this.m_zoneNameArea.SetActive(false);
			this.m_statsArea.SetActive(false);
		}
	}

	private void Update()
	{
	}

	private void OnZoomChanged()
	{
		CanvasGroup component = base.get_gameObject().GetComponent<CanvasGroup>();
		MapInfo componentInParent = base.get_gameObject().GetComponentInParent<MapInfo>();
		component.set_alpha((componentInParent.m_maxZoomFactor - ZoneMissionOverview.m_pinchZoomManager.m_zoomFactor) / (componentInParent.m_maxZoomFactor - 1f));
		if (component.get_alpha() < 0.99f)
		{
			component.set_interactable(false);
		}
		else
		{
			component.set_interactable(true);
		}
	}
}
