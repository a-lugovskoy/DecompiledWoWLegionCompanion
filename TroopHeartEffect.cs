using System;
using UnityEngine;
using UnityEngine.UI;

public class TroopHeartEffect : MonoBehaviour
{
	public Image m_image;

	public CanvasGroup m_canvasGroup;

	public float m_targetScale;

	public void SetHeartEffectProgress(float progress)
	{
		if (this.m_image != null)
		{
			Color color = this.m_image.get_color();
			color.a = 1f - progress;
			this.m_image.set_color(color);
		}
		if (this.m_canvasGroup != null)
		{
			this.m_canvasGroup.set_alpha(1f - progress);
		}
		base.get_transform().set_localScale(Vector3.get_one() * (1f + progress * (this.m_targetScale - 1f)));
	}

	public void FinishHeartEffect()
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
		base.get_transform().set_localScale(Vector3.get_one() * this.m_targetScale);
	}
}
