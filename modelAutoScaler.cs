using System;
using UnityEngine;

public class modelAutoScaler : MonoBehaviour
{
	public Camera mainCamera;

	public bool isPortrait;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.isPortrait)
		{
			float num = 0.75f / this.mainCamera.get_aspect();
			base.get_gameObject().get_transform().set_localScale(new Vector3(num, num, num));
		}
		else
		{
			float num2 = 1.33333337f / this.mainCamera.get_aspect();
			base.get_gameObject().get_transform().set_localScale(new Vector3(num2, num2, num2));
		}
	}
}
