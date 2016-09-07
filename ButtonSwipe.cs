using System;
using UnityEngine;

public class ButtonSwipe : MonoBehaviour
{
	public float m_initialX;

	public float m_initialTouchX;

	public float m_currentTouchX;

	public void OnBeginDrag()
	{
		RectTransform component = base.GetComponent<RectTransform>();
		this.m_initialX = component.get_localPosition().x;
		if (Input.get_touchCount() > 0)
		{
			this.m_initialTouchX = Input.GetTouch(0).get_position().x;
		}
		else
		{
			this.m_initialTouchX = Input.get_mousePosition().x;
		}
	}

	public void OnDrag()
	{
		if (Input.get_touchCount() > 0)
		{
			this.m_currentTouchX = Input.GetTouch(0).get_position().x;
		}
		else
		{
			this.m_currentTouchX = Input.get_mousePosition().x;
		}
		float num = this.m_currentTouchX - this.m_initialTouchX;
		RectTransform component = base.GetComponent<RectTransform>();
		Vector3 localPosition = component.get_localPosition();
		localPosition.x = this.m_initialX + num;
		component.set_localPosition(localPosition);
	}

	public void OnEndDrag()
	{
	}
}
