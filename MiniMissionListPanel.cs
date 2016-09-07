using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStaticData;

public class MiniMissionListPanel : MonoBehaviour
{
	public RectTransform m_parentViewRT;

	public RectTransform m_panelViewRT;

	public GameObject m_miniMissionListItemPrefab;

	public GameObject m_availableMissionListScrollView;

	public GameObject m_availableMission_listContents;

	public GameObject m_inProgressMissionListScrollView;

	public GameObject m_inProgressMission_listContents;

	public Button m_availableMissionsTabButton;

	public Text m_availableMissionsTabLabel;

	public Image m_availableMissionsTabSelectedImage;

	public Button m_inProgressMissionsTabButton;

	public Text m_inProgressMissionsTabLabel;

	public Image m_inProgressMissionsTabSelectedImage;

	public Text m_noMissionsAvailableLabel;

	public Text m_noMissionsInProgressLabel;

	public CombatAllyListItem m_combatAllyListItem;

	private Vector2 m_multiPanelViewSizeDelta;

	private void Awake()
	{
		this.m_availableMissionsTabLabel.set_font(GeneralHelpers.LoadFancyFont());
		this.m_inProgressMissionsTabLabel.set_font(GeneralHelpers.LoadFancyFont());
		this.m_noMissionsAvailableLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_noMissionsAvailableLabel.set_text(StaticDB.GetString("NO_MISSIONS_AVAILABLE", "No missions are currently available."));
		this.m_noMissionsInProgressLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_noMissionsInProgressLabel.set_text(StaticDB.GetString("NO_MISSIONS_IN_PROGRESS", "No missions are currently in progress."));
	}

	public void OnEnable()
	{
		Main expr_05 = Main.instance;
		expr_05.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(expr_05.GarrisonDataResetFinishedAction, new Action(this.HandleGarrisonDataResetFinished));
		Main expr_2B = Main.instance;
		expr_2B.MissionAddedAction = (Action<int, int>)Delegate.Combine(expr_2B.MissionAddedAction, new Action<int, int>(this.HandleMissionAdded));
		this.InitMissionList();
		this.ShowAvailableMissionList();
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.GarrisonDataResetFinishedAction = (Action)Delegate.Remove(expr_05.GarrisonDataResetFinishedAction, new Action(this.HandleGarrisonDataResetFinished));
		Main expr_2B = Main.instance;
		expr_2B.MissionAddedAction = (Action<int, int>)Delegate.Remove(expr_2B.MissionAddedAction, new Action<int, int>(this.HandleMissionAdded));
	}

	public void ShowAvailableMissionList()
	{
		this.m_availableMissionListScrollView.SetActive(true);
		this.m_inProgressMissionListScrollView.SetActive(false);
		this.m_availableMissionsTabSelectedImage.get_gameObject().SetActive(true);
		this.m_inProgressMissionsTabSelectedImage.get_gameObject().SetActive(false);
	}

	public void ShowInProgressMissionList()
	{
		this.m_availableMissionListScrollView.SetActive(false);
		this.m_inProgressMissionListScrollView.SetActive(true);
		this.m_availableMissionsTabSelectedImage.get_gameObject().SetActive(false);
		this.m_inProgressMissionsTabSelectedImage.get_gameObject().SetActive(true);
	}

	private void Update()
	{
		if (this.m_panelViewRT.get_sizeDelta().x != this.m_parentViewRT.get_rect().get_width())
		{
			this.m_multiPanelViewSizeDelta = this.m_panelViewRT.get_sizeDelta();
			this.m_multiPanelViewSizeDelta.x = this.m_parentViewRT.get_rect().get_width();
			this.m_panelViewRT.set_sizeDelta(this.m_multiPanelViewSizeDelta);
		}
	}

	private void HandleGarrisonDataResetFinished()
	{
		this.InitMissionList();
	}

	private void HandleMissionAdded(int garrMissionID, int result)
	{
		this.InitMissionList();
	}

	public void InitMissionList()
	{
		this.m_combatAllyListItem.get_gameObject().SetActive(false);
		MiniMissionListItem[] componentsInChildren = this.m_availableMission_listContents.GetComponentsInChildren<MiniMissionListItem>(true);
		MiniMissionListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			MiniMissionListItem miniMissionListItem = array[i];
			bool flag = true;
			if (PersistentMissionData.missionDictionary.ContainsKey(miniMissionListItem.GetMissionID()))
			{
				JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(miniMissionListItem.GetMissionID());
				if (jamGarrisonMobileMission.MissionState == 0)
				{
					flag = false;
					miniMissionListItem.UpdateMechanicPreview(false, jamGarrisonMobileMission);
				}
			}
			if (flag)
			{
				Object.DestroyImmediate(miniMissionListItem.get_gameObject());
			}
		}
		componentsInChildren = this.m_inProgressMission_listContents.GetComponentsInChildren<MiniMissionListItem>(true);
		MiniMissionListItem[] array2 = componentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			MiniMissionListItem miniMissionListItem2 = array2[j];
			bool flag2 = true;
			if (PersistentMissionData.missionDictionary.ContainsKey(miniMissionListItem2.GetMissionID()))
			{
				JamGarrisonMobileMission jamGarrisonMobileMission2 = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(miniMissionListItem2.GetMissionID());
				if (jamGarrisonMobileMission2.MissionState != 0)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				Object.DestroyImmediate(miniMissionListItem2.get_gameObject());
			}
		}
		MiniMissionListItem[] componentsInChildren2 = this.m_availableMission_listContents.GetComponentsInChildren<MiniMissionListItem>(true);
		MiniMissionListItem[] componentsInChildren3 = this.m_inProgressMission_listContents.GetComponentsInChildren<MiniMissionListItem>(true);
		IEnumerator enumerator = PersistentMissionData.missionDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission jamGarrisonMobileMission3 = (JamGarrisonMobileMission)enumerator.get_Current();
				bool flag3 = false;
				MiniMissionListItem[] array3 = componentsInChildren2;
				for (int k = 0; k < array3.Length; k++)
				{
					MiniMissionListItem miniMissionListItem3 = array3[k];
					if (miniMissionListItem3.GetMissionID() == jamGarrisonMobileMission3.MissionRecID)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					MiniMissionListItem[] array4 = componentsInChildren3;
					for (int l = 0; l < array4.Length; l++)
					{
						MiniMissionListItem miniMissionListItem4 = array4[l];
						if (miniMissionListItem4.GetMissionID() == jamGarrisonMobileMission3.MissionRecID)
						{
							flag3 = true;
							break;
						}
					}
				}
				if (!flag3)
				{
					GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(jamGarrisonMobileMission3.MissionRecID);
					if (record == null)
					{
						Debug.LogWarning("Mission Not Found: ID " + jamGarrisonMobileMission3.MissionRecID);
					}
					else if (record.GarrFollowerTypeID == 4u)
					{
						if ((record.Flags & 16u) != 0u)
						{
							this.m_combatAllyListItem.get_gameObject().SetActive(true);
						}
						else
						{
							GameObject gameObject = Object.Instantiate<GameObject>(this.m_miniMissionListItemPrefab);
							if (jamGarrisonMobileMission3.MissionState == 0)
							{
								gameObject.get_transform().SetParent(this.m_availableMission_listContents.get_transform(), false);
							}
							else
							{
								gameObject.get_transform().SetParent(this.m_inProgressMission_listContents.get_transform(), false);
							}
							MiniMissionListItem component = gameObject.GetComponent<MiniMissionListItem>();
							component.SetMission(jamGarrisonMobileMission3);
							AutoHide autoHide = gameObject.AddComponent<AutoHide>();
							autoHide.m_clipRT = base.get_gameObject().GetComponent<RectTransform>();
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
		componentsInChildren2 = this.m_availableMission_listContents.GetComponentsInChildren<MiniMissionListItem>(true);
		componentsInChildren3 = this.m_inProgressMission_listContents.GetComponentsInChildren<MiniMissionListItem>(true);
		int num = componentsInChildren2.Length;
		int num2 = componentsInChildren3.Length;
		this.m_availableMissionsTabLabel.set_text(StaticDB.GetString("AVAILABLE", null) + " - " + num);
		this.m_inProgressMissionsTabLabel.set_text(StaticDB.GetString("IN_PROGRESS", null) + " - " + num2);
		this.m_noMissionsAvailableLabel.get_gameObject().SetActive(num == 0);
		this.m_noMissionsInProgressLabel.get_gameObject().SetActive(num2 == 0);
	}
}
