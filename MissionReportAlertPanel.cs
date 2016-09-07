using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;

public class MissionReportAlertPanel : MonoBehaviour
{
	public GameObject missionReportView;

	public GameObject allianceCommander;

	public GameObject hordeCommander;

	public Text completedMissionsText;

	public GameObject missionResultsView;

	public GameObject missionRewardsIconArea;

	public GameObject missionListItemPrefab;

	public GameObject missionRewardResultsDisplayPrefab;

	public GameObject completedMissionListContents;

	public GameObject okButton;

	public Canvas mainCanvas;

	private Dictionary<int, bool> _requestedMissionCollection;

	private Dictionary<int, bool> GetRequestedMissionCollectionDictionary()
	{
		if (this._requestedMissionCollection == null)
		{
			this._requestedMissionCollection = new Dictionary<int, bool>();
		}
		return this._requestedMissionCollection;
	}

	private void Awake()
	{
	}

	private void Update()
	{
		this.completedMissionsText.set_text(string.Empty + PersistentMissionData.GetNumCompletedMissions() + " Completed Missions");
	}

	private void OnEnable()
	{
		this.GetRequestedMissionCollectionDictionary().Clear();
		this.okButton.SetActive(false);
		this.mainCanvas.set_renderMode(1);
		if (GarrisonStatus.Faction() == PVP_FACTION.HORDE)
		{
			this.hordeCommander.SetActive(true);
			this.allianceCommander.SetActive(false);
		}
		else
		{
			this.hordeCommander.SetActive(false);
			this.allianceCommander.SetActive(true);
		}
		this.completedMissionsText.set_text(string.Empty + PersistentMissionData.GetNumCompletedMissions() + " Completed Missions");
		MissionListItem[] componentsInChildren = this.completedMissionListContents.GetComponentsInChildren<MissionListItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
		}
		MissionRewardDisplay[] componentsInChildren2 = this.missionRewardsIconArea.GetComponentsInChildren<MissionRewardDisplay>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			Object.DestroyImmediate(componentsInChildren2[j].get_gameObject());
		}
		this.missionReportView.SetActive(true);
		this.missionResultsView.SetActive(false);
	}

	private void OnDisable()
	{
		this.mainCanvas.set_renderMode(0);
	}

	private bool MissionIsOnCompletedMissionList(int garrMissionID)
	{
		MissionListItem[] componentsInChildren = this.completedMissionListContents.GetComponentsInChildren<MissionListItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].garrMissionID == garrMissionID)
			{
				return true;
			}
		}
		return false;
	}

	private void PopulateCompletedMissionList()
	{
		IEnumerator enumerator = PersistentMissionData.missionDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)enumerator.get_Current();
				if ((jamGarrisonMobileMission.MissionState == 2 || jamGarrisonMobileMission.MissionState == 6) && !this.MissionIsOnCompletedMissionList(jamGarrisonMobileMission.MissionRecID))
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.missionListItemPrefab);
					gameObject.get_transform().SetParent(this.completedMissionListContents.get_transform(), false);
					MissionListItem component = gameObject.GetComponent<MissionListItem>();
					component.Init(jamGarrisonMobileMission.MissionRecID);
					component.isResultsItem = true;
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

	private void CollectFirstCompletedMission()
	{
		MissionListItem[] componentsInChildren = this.completedMissionListContents.GetComponentsInChildren<MissionListItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(componentsInChildren[i].garrMissionID);
			if (!this.GetRequestedMissionCollectionDictionary().ContainsKey(componentsInChildren[i].garrMissionID) && PersistentMissionData.missionDictionary.ContainsKey(componentsInChildren[i].garrMissionID) && jamGarrisonMobileMission.MissionState == 2)
			{
				this.GetRequestedMissionCollectionDictionary().Add(componentsInChildren[i].garrMissionID, true);
				Main.instance.ClaimMissionBonus(componentsInChildren[i].garrMissionID);
				break;
			}
		}
	}

	public void CompleteAllMissions()
	{
		Main.instance.CompleteAllMissions();
		this.missionReportView.SetActive(false);
		this.missionResultsView.SetActive(true);
		this.PopulateCompletedMissionList();
		this.CollectFirstCompletedMission();
	}

	public void OnMissionStatusChanged()
	{
		this.PopulateCompletedMissionList();
		this.CollectFirstCompletedMission();
		int num = 0;
		MissionListItem[] componentsInChildren = this.completedMissionListContents.GetComponentsInChildren<MissionListItem>(true);
		MissionRewardDisplay[] componentsInChildren2 = this.missionRewardsIconArea.get_transform().GetComponentsInChildren<MissionRewardDisplay>(true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren2[i].get_gameObject());
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(componentsInChildren[j].garrMissionID);
			if (PersistentMissionData.missionDictionary.ContainsKey(componentsInChildren[j].garrMissionID) && jamGarrisonMobileMission.MissionState == 6)
			{
				componentsInChildren[j].inProgressDarkener.SetActive(true);
				componentsInChildren[j].missionResultsText.get_gameObject().SetActive(true);
				if (this.GetRequestedMissionCollectionDictionary().ContainsKey(componentsInChildren[j].garrMissionID))
				{
					componentsInChildren[j].missionResultsText.set_text("<color=#00ff00ff>SUCCEEDED!</color>");
					MissionRewardDisplay[] componentsInChildren3 = componentsInChildren[j].missionRewardGroup.GetComponentsInChildren<MissionRewardDisplay>(true);
					for (int k = 0; k < componentsInChildren3.Length; k++)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.missionRewardResultsDisplayPrefab);
						gameObject.get_transform().SetParent(this.missionRewardsIconArea.get_transform(), false);
					}
				}
				else
				{
					componentsInChildren[j].missionResultsText.set_text("<color=#ff0000ff>FAILED</color>");
				}
			}
			else
			{
				num++;
			}
			if (num == 0)
			{
				this.okButton.SetActive(true);
			}
		}
	}

	public void ShowMissionListAndRefreshData()
	{
		Debug.Log("Request Data Refresh");
		Main.instance.MobileRequestData();
		Main.instance.allPanels.ShowAdventureMap();
	}
}
