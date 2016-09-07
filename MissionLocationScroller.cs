using System;
using UnityEngine;

public class MissionLocationScroller : MonoBehaviour
{
	public float scrollSpeed;

	public float imageWidth;

	private RectTransform m_myRT;

	private void Awake()
	{
		this.m_myRT = base.GetComponent<RectTransform>();
	}

	private void Update()
	{
		Vector2 anchoredPosition = this.m_myRT.get_anchoredPosition();
		anchoredPosition.x += this.scrollSpeed * Time.get_deltaTime();
		this.m_myRT.set_anchoredPosition(anchoredPosition);
		if (this.m_myRT.get_anchoredPosition().x <= -this.imageWidth * 0.5f * this.m_myRT.get_localScale().x)
		{
			anchoredPosition = this.m_myRT.get_anchoredPosition();
			anchoredPosition.x = 0f;
			this.m_myRT.set_anchoredPosition(anchoredPosition);
		}
	}
}
