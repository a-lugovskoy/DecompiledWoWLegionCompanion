using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowerMissionButtonView : MonoBehaviour
{
	public Text followerMissionToggleButtonText;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ShowMissionList()
	{
		Main.instance.allPanels.ShowAdventureMap();
	}
}
