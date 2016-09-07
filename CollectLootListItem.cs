using System;
using UnityEngine;
using UnityEngine.UI;

public class CollectLootListItem : MonoBehaviour
{
	public Text completedMissionsText;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CompleteAllMissions()
	{
		Main.instance.CompleteAllMissions();
	}
}
