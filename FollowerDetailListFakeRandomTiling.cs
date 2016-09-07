using System;
using UnityEngine;

public class FollowerDetailListFakeRandomTiling : MonoBehaviour
{
	private void Start()
	{
		RectTransform component = base.get_gameObject().GetComponent<RectTransform>();
		component.set_offsetMin(new Vector2(Random.Range(-300f, -2000f), component.get_offsetMin().y));
		component.set_offsetMax(new Vector2(-component.get_offsetMin().x, component.get_offsetMax().y));
		if (Random.Range(0, 1) == 0)
		{
			component.set_localScale(new Vector3(-1f * component.get_localScale().x, component.get_localScale().y, component.get_localScale().z));
		}
		if (Random.Range(0, 1) == 0)
		{
			component.set_localScale(new Vector3(component.get_localScale().x, -1f * component.get_localScale().y, component.get_localScale().z));
		}
	}
}
