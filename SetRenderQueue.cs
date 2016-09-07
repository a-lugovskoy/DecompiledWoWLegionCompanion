using System;
using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]
public class SetRenderQueue : MonoBehaviour
{
	[SerializeField]
	protected int[] m_queues = new int[]
	{
		3000
	};

	protected void Awake()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material[] materials = componentsInChildren[i].get_materials();
			int num = 0;
			while (num < materials.Length && num < this.m_queues.Length)
			{
				materials[num].set_renderQueue(this.m_queues[num]);
				num++;
			}
		}
	}
}
