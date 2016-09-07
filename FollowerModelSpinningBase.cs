using System;
using UnityEngine;

public class FollowerModelSpinningBase : MonoBehaviour
{
	public float inititalYRotation;

	private float initialTouchX;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnBeginDrag()
	{
		this.inititalYRotation = base.get_transform().get_localEulerAngles().y;
		if (Input.get_touchCount() > 0)
		{
			this.initialTouchX = Input.GetTouch(0).get_position().x;
		}
		else
		{
			this.initialTouchX = Input.get_mousePosition().x;
		}
	}

	public void OnDrag()
	{
		float x;
		if (Input.get_touchCount() > 0)
		{
			x = Input.GetTouch(0).get_position().x;
		}
		else
		{
			x = Input.get_mousePosition().x;
		}
		float num = (this.initialTouchX - x) / (float)Screen.get_width();
		num *= 2f;
		base.get_transform().set_localRotation(Quaternion.get_identity());
		base.get_transform().Rotate(0f, this.inititalYRotation + num * 360f, 0f, 1);
	}
}
