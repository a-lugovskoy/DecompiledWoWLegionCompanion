using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAlphaSetter : MonoBehaviour
{
	public Image m_image;

	public CanvasGroup m_canvasGroup;

	public void SetAlpha(float alpha)
	{
		if (this.m_image != null)
		{
			Color color = this.m_image.get_color();
			color.a = alpha;
			this.m_image.set_color(color);
		}
		if (this.m_canvasGroup != null)
		{
			this.m_canvasGroup.set_alpha(alpha);
		}
	}

	public void SetAlphaZero()
	{
		if (this.m_image != null)
		{
			Color color = this.m_image.get_color();
			color.a = 0f;
			this.m_image.set_color(color);
		}
		if (this.m_canvasGroup != null)
		{
			this.m_canvasGroup.set_alpha(0f);
		}
	}
}
