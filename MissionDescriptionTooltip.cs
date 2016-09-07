using System;
using UnityEngine;
using UnityEngine.UI;
using WowStaticData;

public class MissionDescriptionTooltip : MonoBehaviour
{
	public Image m_missionIcon;

	public Text m_missionName;

	public Text m_missionDescription;

	public void OnEnable()
	{
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
	}

	public void SetMission(int garrMissionID)
	{
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(garrMissionID);
		this.m_missionName.set_text(record.Name);
		this.m_missionDescription.set_text(record.Description);
	}
}
