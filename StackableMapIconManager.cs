using System;
using System.Collections.Generic;
using UnityEngine;

public class StackableMapIconManager : MonoBehaviour
{
	public GameObject m_stackableMapContainerPrefab;

	private List<StackableMapIconContainer> m_containers;

	private static StackableMapIconManager s_instance;

	private void Awake()
	{
		StackableMapIconManager.s_instance = this;
		this.m_containers = new List<StackableMapIconContainer>();
	}

	public static void RegisterStackableMapIcon(StackableMapIcon icon)
	{
		if (StackableMapIconManager.s_instance == null)
		{
			Debug.LogError("ERROR: RegisterStackableMapIcon with null s_instance");
			return;
		}
		if (icon == null)
		{
			Debug.LogError("ERROR: RegisterStackableMapIcon with null icon");
			return;
		}
		bool flag = false;
		using (List<StackableMapIconContainer>.Enumerator enumerator = StackableMapIconManager.s_instance.m_containers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StackableMapIconContainer current = enumerator.get_Current();
				if (!(current == null))
				{
					if (current.get_gameObject().get_activeSelf())
					{
						Rect worldRect = current.GetWorldRect();
						if (icon.GetWorldRect().Overlaps(worldRect))
						{
							current.AddStackableMapIcon(icon);
							icon.SetContainer(current);
							StackableMapIconManager.s_instance.m_containers.Add(current);
							flag = true;
							break;
						}
					}
				}
			}
		}
		if (!flag)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(StackableMapIconManager.s_instance.m_stackableMapContainerPrefab);
			gameObject.get_transform().SetParent(icon.get_transform().get_parent(), false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			RectTransform component2 = icon.GetComponent<RectTransform>();
			component.set_anchorMin(component2.get_anchorMin());
			component.set_anchorMax(component2.get_anchorMax());
			component.set_sizeDelta(icon.m_iconBoundsRT.get_sizeDelta());
			component.set_anchoredPosition(Vector2.get_zero());
			StackableMapIconContainer component3 = gameObject.GetComponent<StackableMapIconContainer>();
			if (component3 != null)
			{
				component3.AddStackableMapIcon(icon);
				icon.SetContainer(component3);
				StackableMapIconManager.s_instance.m_containers.Add(component3);
			}
			else
			{
				Debug.LogError("ERROR: containerObj has no StackableMapIconContainer!!");
			}
		}
	}

	public static void RemoveStackableMapIconContainer(StackableMapIconContainer container)
	{
		if (StackableMapIconManager.s_instance != null && StackableMapIconManager.s_instance.m_containers.Contains(container))
		{
			StackableMapIconManager.s_instance.m_containers.Remove(container);
		}
	}
}
