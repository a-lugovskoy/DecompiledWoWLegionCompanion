using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowJamMessages.MobilePlayerJSON;

public class TroopsPanel : MonoBehaviour
{
	public GameObject m_troopsListItemPrefab;

	public GameObject m_troopsListContents;

	public float m_listItemInitialEntranceDelay;

	public float m_listItemEntranceDelay;

	public Text m_noRecruitsYetMessage;

	public RectTransform m_panelViewRT;

	public RectTransform m_parentViewRT;

	private Vector2 m_multiPanelViewSizeDelta;

	private void Awake()
	{
		this.m_noRecruitsYetMessage.set_font(GeneralHelpers.LoadStandardFont());
		this.m_noRecruitsYetMessage.set_text(StaticDB.GetString("NO_RECRUITS_AVAILABLE_YET", "You have no recruits available yet."));
		this.InitList();
	}

	public void OnEnable()
	{
		Main expr_05 = Main.instance;
		expr_05.CreateShipmentResultAction = (Action<int>)Delegate.Combine(expr_05.CreateShipmentResultAction, new Action<int>(this.HandleRecruitResult));
		Main expr_2B = Main.instance;
		expr_2B.FollowerDataChangedAction = (Action)Delegate.Combine(expr_2B.FollowerDataChangedAction, new Action(this.HandleFollowerDataChanged));
		Main expr_51 = Main.instance;
		expr_51.ShipmentTypesUpdatedAction = (Action)Delegate.Combine(expr_51.ShipmentTypesUpdatedAction, new Action(this.InitList));
		Main expr_77 = Main.instance;
		expr_77.StartLogOutAction = (Action)Delegate.Combine(expr_77.StartLogOutAction, new Action(this.HandleStartLogout));
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.CreateShipmentResultAction = (Action<int>)Delegate.Remove(expr_05.CreateShipmentResultAction, new Action<int>(this.HandleRecruitResult));
		Main expr_2B = Main.instance;
		expr_2B.FollowerDataChangedAction = (Action)Delegate.Remove(expr_2B.FollowerDataChangedAction, new Action(this.HandleFollowerDataChanged));
		Main expr_51 = Main.instance;
		expr_51.ShipmentTypesUpdatedAction = (Action)Delegate.Remove(expr_51.ShipmentTypesUpdatedAction, new Action(this.InitList));
		Main expr_77 = Main.instance;
		expr_77.StartLogOutAction = (Action)Delegate.Remove(expr_77.StartLogOutAction, new Action(this.HandleStartLogout));
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

	private void HandleFollowerDataChanged()
	{
		this.InitList();
		TroopsListItem[] componentsInChildren = this.m_troopsListContents.GetComponentsInChildren<TroopsListItem>();
		TroopsListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			TroopsListItem troopsListItem = array[i];
			troopsListItem.HandleFollowerDataChanged();
		}
	}

	private void HandleStartLogout()
	{
		TroopsListItem[] componentsInChildren = this.m_troopsListContents.GetComponentsInChildren<TroopsListItem>();
		TroopsListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			TroopsListItem troopsListItem = array[i];
			Object.DestroyImmediate(troopsListItem.get_gameObject());
		}
	}

	private void InitList()
	{
		MobileClientShipmentType[] availableShipmentTypes = PersistentShipmentData.GetAvailableShipmentTypes();
		if (availableShipmentTypes == null || availableShipmentTypes.Length == 0)
		{
			this.m_noRecruitsYetMessage.get_gameObject().SetActive(true);
		}
		else
		{
			this.m_noRecruitsYetMessage.get_gameObject().SetActive(false);
		}
		TroopsListItem[] componentsInChildren = this.m_troopsListContents.GetComponentsInChildren<TroopsListItem>();
		TroopsListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			TroopsListItem troopsListItem = array[i];
			bool flag = true;
			if (availableShipmentTypes != null)
			{
				MobileClientShipmentType[] array2 = availableShipmentTypes;
				for (int j = 0; j < array2.Length; j++)
				{
					MobileClientShipmentType mobileClientShipmentType = array2[j];
					if (troopsListItem.GetCharShipmentTypeID() == mobileClientShipmentType.CharShipmentID)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				Object.DestroyImmediate(troopsListItem.get_gameObject());
			}
		}
		if (availableShipmentTypes == null)
		{
			return;
		}
		componentsInChildren = this.m_troopsListContents.GetComponentsInChildren<TroopsListItem>();
		for (int k = 0; k < availableShipmentTypes.Length; k++)
		{
			bool flag2 = false;
			TroopsListItem[] array3 = componentsInChildren;
			for (int l = 0; l < array3.Length; l++)
			{
				TroopsListItem troopsListItem2 = array3[l];
				if (troopsListItem2.GetCharShipmentTypeID() == availableShipmentTypes[k].CharShipmentID)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_troopsListItemPrefab);
				gameObject.get_transform().SetParent(this.m_troopsListContents.get_transform(), false);
				TroopsListItem component = gameObject.GetComponent<TroopsListItem>();
				component.SetCharShipment(availableShipmentTypes[k]);
				FancyEntrance component2 = component.GetComponent<FancyEntrance>();
				component2.m_timeToDelayEntrance = this.m_listItemInitialEntranceDelay + this.m_listItemEntranceDelay * (float)k;
				component2.Activate();
			}
		}
	}

	private void HandleRecruitResult(int result)
	{
		if (result == 0)
		{
			MobilePlayerRequestShipments obj = new MobilePlayerRequestShipments();
			Login.instance.SendToMobileServer(obj);
		}
	}
}
