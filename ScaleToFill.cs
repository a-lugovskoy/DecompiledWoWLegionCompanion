using System;
using UnityEngine;

public class ScaleToFill : MonoBehaviour
{
	private float nativeHeight;

	private void Start()
	{
		RectTransform component = base.GetComponent<RectTransform>();
		this.nativeHeight = component.get_rect().get_height();
	}

	private void Update()
	{
		RectTransform component = base.get_transform().get_parent().get_gameObject().GetComponent<RectTransform>();
		float num = component.get_rect().get_height() / this.nativeHeight;
		base.get_transform().set_localScale(new Vector3(num, num, num));
	}
}
