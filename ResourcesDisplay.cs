using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesDisplay : MonoBehaviour
{
	public Text m_yourResourcesLabel;

	public Image m_currencyIcon;

	private int m_currencyAmount;

	public Text m_currencyAmountText;

	public Text m_researchText;

	public Text m_costText;

	private void OnEnable()
	{
		this.UpdateCurrencyDisplayAmount();
		Main expr_0B = Main.instance;
		expr_0B.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(expr_0B.GarrisonDataResetFinishedAction, new Action(this.UpdateCurrencyDisplayAmount));
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.GarrisonDataResetFinishedAction = (Action)Delegate.Remove(expr_05.GarrisonDataResetFinishedAction, new Action(this.UpdateCurrencyDisplayAmount));
	}

	private void Start()
	{
		this.UpdateCurrencyDisplayAmount();
		Sprite sprite = GeneralHelpers.LoadCurrencyIcon(1220);
		if (sprite != null)
		{
			this.m_currencyIcon.set_sprite(sprite);
		}
		this.m_yourResourcesLabel.set_text(StaticDB.GetString("YOUR_RESOURCES", null));
		if (this.m_researchText != null)
		{
			this.m_researchText.set_text(StaticDB.GetString("RESEARCH_TIME", null));
		}
		if (this.m_costText != null)
		{
			this.m_costText.set_text(StaticDB.GetString("COST", null));
		}
	}

	private void UpdateCurrencyDisplayAmount()
	{
		this.m_currencyAmountText.set_text(GarrisonStatus.Resources().ToString("N0"));
	}
}
