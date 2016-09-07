using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStaticData;

public class MissionListPanel : MonoBehaviour
{
	public Text characterNameText;

	public Text characterLevelAndClassText;

	public Image availableMissionsButtonHighlight;

	public Text availableMissionsButtonText;

	public MissionListView availableMissionListView;

	public GameObject availableMissionListContents;

	public Image inProgressMissionsButtonHighlight;

	public Text inProgressMissionsButtonText;

	public MissionListView inProgressMissionListView;

	public GameObject inProgressMissionListContents;

	public GameObject CompleteMissionCheatPopup;

	private int m_currentCheatMissionID;

	private void Start()
	{
		this.m_currentCheatMissionID = 0;
	}

	private void Update()
	{
		IEnumerator enumerator = PersistentMissionData.missionDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)enumerator.get_Current();
				bool flag = false;
				GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(jamGarrisonMobileMission.MissionRecID);
				if (record != null)
				{
					if (record.GarrFollowerTypeID == 4u)
					{
						if (jamGarrisonMobileMission.MissionState == 1)
						{
							long num = GarrisonStatus.CurrentTime() - jamGarrisonMobileMission.StartTime;
							long num2 = jamGarrisonMobileMission.MissionDuration - num;
							if (num2 <= 0L)
							{
								flag = true;
							}
						}
						if (jamGarrisonMobileMission.MissionState == 2)
						{
							flag = true;
						}
						if (flag)
						{
						}
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
	}

	public void ShowAvailableMissionList()
	{
		this.availableMissionListView.get_gameObject().SetActive(true);
		this.inProgressMissionListView.get_gameObject().SetActive(false);
		this.availableMissionsButtonHighlight.get_gameObject().SetActive(true);
		this.inProgressMissionsButtonHighlight.get_gameObject().SetActive(false);
	}

	public void ShowInProgressMissionList()
	{
		this.availableMissionListView.get_gameObject().SetActive(false);
		this.inProgressMissionListView.get_gameObject().SetActive(true);
		this.availableMissionsButtonHighlight.get_gameObject().SetActive(false);
		this.inProgressMissionsButtonHighlight.get_gameObject().SetActive(true);
	}

	public void OnUIRefresh()
	{
		this.availableMissionListView.OnUIRefresh();
		this.inProgressMissionListView.OnUIRefresh();
		MissionListItem[] componentsInChildren = this.availableMissionListContents.GetComponentsInChildren<MissionListItem>(true);
		MissionListItem[] componentsInChildren2 = this.inProgressMissionListContents.GetComponentsInChildren<MissionListItem>(true);
		this.availableMissionsButtonText.set_text("Available - " + componentsInChildren.Length);
		this.inProgressMissionsButtonText.set_text("In Progress - " + componentsInChildren2.Length);
	}

	public void ShowCheatCompleteMission(int garrMissionID)
	{
		this.m_currentCheatMissionID = garrMissionID;
		this.CompleteMissionCheatPopup.SetActive(true);
		this.CompleteMissionCheatPopup.get_transform().SetAsLastSibling();
	}

	public void CheatCompleteMission()
	{
		if (this.m_currentCheatMissionID > 0)
		{
			Main.instance.ExpediteMissionCheat(this.m_currentCheatMissionID);
		}
		this.m_currentCheatMissionID = 0;
		this.CompleteMissionCheatPopup.SetActive(false);
	}

	public void CancelCheatCompleteMission()
	{
		this.CompleteMissionCheatPopup.SetActive(false);
	}

	public void SetCharacterName(string name)
	{
		this.characterNameText.set_text(name);
	}

	public void SetCharacterLevelAndClass(int level, int charClassID)
	{
	}
}
