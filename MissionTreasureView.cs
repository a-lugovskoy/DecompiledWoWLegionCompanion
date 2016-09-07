using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionTreasureView : MonoBehaviour
{
	public Text m_chanceText;

	private void Start()
	{
		this.m_chanceText.set_text(StaticDB.GetString("CHANCE", null));
	}

	private void Update()
	{
	}
}
