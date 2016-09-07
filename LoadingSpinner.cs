using System;
using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
	public GameObject[] m_objectsToSpin;

	public float m_spinSpeed;

	private void Update()
	{
		GameObject[] objectsToSpin = this.m_objectsToSpin;
		for (int i = 0; i < objectsToSpin.Length; i++)
		{
			GameObject gameObject = objectsToSpin[i];
			gameObject.get_transform().Rotate(0f, 0f, this.m_spinSpeed * Time.get_deltaTime());
		}
	}
}
