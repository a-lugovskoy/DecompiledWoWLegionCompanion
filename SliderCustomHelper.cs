using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderCustomHelper : MonoBehaviour
{
	public string m_baseTitleString;

	public Text m_titleText;

	public Text m_minText;

	public Text m_maxText;

	private Slider m_slider;

	private void Start()
	{
		Slider component = base.GetComponent<Slider>();
		this.m_minText.set_text(string.Empty + component.get_minValue());
		this.m_maxText.set_text(string.Empty + component.get_maxValue());
		this.OnValueChanged(component.get_value());
	}

	public void OnValueChanged(float val)
	{
		this.m_titleText.set_text(string.Format("{0} ({1:F2})", this.m_baseTitleString, val));
	}
}
