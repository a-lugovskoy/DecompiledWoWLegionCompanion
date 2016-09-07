using System;
using UnityEngine;

public class ZoneMap : MonoBehaviour
{
	public string m_zoneName;

	public GameObject m_missionIconArea;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		AdventureMapPanel.instance.m_pinchZoomContentManager.SetZoom(1f, base.get_transform().get_position(), false);
		AdventureMapPanel.instance.m_mapViewContentsRT.set_localPosition(Vector3.get_zero());
	}

	public void SetAdventureMapZoom(float zoomFactor)
	{
		this.m_missionIconArea.get_transform().set_localScale(new Vector3(zoomFactor, zoomFactor, zoomFactor));
	}

	private void OverZoomOut()
	{
		AdventureMapPanel.instance.ShowWorldMap(true);
		base.get_gameObject().SetActive(false);
	}
}
