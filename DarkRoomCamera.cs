using System;
using UnityEngine;

public class DarkRoomCamera : MonoBehaviour
{
	public GameObject m_drCanvasObject;

	private int wtf;

	private void Awake()
	{
		this.wtf = 0;
	}

	private void OnPostRender()
	{
		if (++this.wtf <= 1)
		{
			return;
		}
	}
}
