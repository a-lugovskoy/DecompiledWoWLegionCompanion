using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderHallMultiPanel : MonoBehaviour
{
	public Text m_titleText;

	public Text m_troopButtonText;

	public Text m_followerButtonText;

	public Text m_allyButtonText;

	public Text m_talentButtonText;

	public Text m_worldMapButtonText;

	public OrderHallNavButton m_defaultNavButton;

	public AutoCenterScrollRect m_autoCenterScrollRect;

	public float m_navButtonInitialEntranceDelay;

	public float m_navButtonEntranceDelay;

	private void Start()
	{
		if (this.m_titleText != null)
		{
			this.m_titleText.set_font(GeneralHelpers.LoadStandardFont());
			this.m_titleText.set_text(StaticDB.GetString("CLASS_ORDER_HALL", null));
		}
		this.m_troopButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_troopButtonText.set_text(StaticDB.GetString("RECRUIT", null));
		this.m_followerButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_followerButtonText.set_text(StaticDB.GetString("FOLLOWERS", null));
		this.m_allyButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_allyButtonText.set_text(StaticDB.GetString("MISSIONS", null));
		this.m_talentButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_talentButtonText.set_text(StaticDB.GetString("RESEARCH", null));
		this.m_worldMapButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_worldMapButtonText.set_text(StaticDB.GetString("WORLD_MAP", null));
		Text[] componentsInChildren = base.GetComponentsInChildren<Text>(true);
		Text[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Text text = array[i];
			if (text.get_text() == "Abilities")
			{
				text.set_text(StaticDB.GetString("ABILITIES", null));
			}
			else if (text.get_text() == "Counters:")
			{
				text.set_text(StaticDB.GetString("COUNTERS", null) + ":");
			}
		}
		this.m_defaultNavButton.SelectMe();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}
