using System;
using UnityEngine;

public class SimpleSpinner : MonoBehaviour
{
	public float m_spinSpeed;

	private void Update()
	{
		base.get_transform().Rotate(0f, 0f, this.m_spinSpeed * Time.get_deltaTime());
	}
}
