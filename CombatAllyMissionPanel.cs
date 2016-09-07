using System;
using System.Collections;
using UnityEngine;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class CombatAllyMissionPanel : MonoBehaviour
{
	public MissionDetailView m_missionDetailView;

	public SliderPanel m_sliderPanel;

	public void Show()
	{
		int num = 0;
		CombatAllyMissionState combatAllyMissionState = CombatAllyMissionState.notAvailable;
		IEnumerator enumerator = PersistentMissionData.missionDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)enumerator.get_Current();
				GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(jamGarrisonMobileMission.MissionRecID);
				if (record != null)
				{
					if ((record.Flags & 16u) != 0u)
					{
						num = jamGarrisonMobileMission.MissionRecID;
						if (jamGarrisonMobileMission.MissionState == 1)
						{
							combatAllyMissionState = CombatAllyMissionState.inProgress;
						}
						else
						{
							combatAllyMissionState = CombatAllyMissionState.available;
						}
						break;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		if (num > 0)
		{
			this.m_missionDetailView.HandleMissionSelected(num);
		}
		this.m_missionDetailView.SetCombatAllyMissionState(combatAllyMissionState);
		this.m_sliderPanel.MaximizeSliderPanel();
	}

	public void Hide()
	{
		this.m_sliderPanel.HideSliderPanel();
	}

	private void Update()
	{
	}
}
