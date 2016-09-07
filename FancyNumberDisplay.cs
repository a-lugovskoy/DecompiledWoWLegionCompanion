using System;
using UnityEngine;
using UnityEngine.UI;

public class FancyNumberDisplay : MonoBehaviour
{
	public Text m_numberText;

	public int m_actualValue;

	public float m_duration;

	public iTween.EaseType m_fillEaseType;

	public Action TimerStartedAction;

	public Action<int> TimerUpdateAction;

	public Action TimerEndedAction;

	private int m_currentValue;

	private bool m_instant;

	private bool m_initialized;

	private bool m_startedTimer;

	private float m_timeRemainingUntilStartTimer;

	public void SetValue(int newValue, float delayStartTimerTime = 0f)
	{
		this.SetValue(newValue, false, delayStartTimerTime);
	}

	public void SetValue(int newValue, bool instant, float delayStartTimerTime = 0f)
	{
		this.m_actualValue = newValue;
		this.m_instant = instant;
		this.m_timeRemainingUntilStartTimer = delayStartTimerTime;
		this.m_startedTimer = false;
		if (instant)
		{
			this.m_currentValue = newValue;
			this.m_numberText.set_text(string.Empty + newValue);
		}
		this.m_initialized = true;
	}

	private void Update()
	{
		if (this.m_initialized && !this.m_instant && !this.m_startedTimer)
		{
			this.m_timeRemainingUntilStartTimer -= Time.get_deltaTime();
			if (this.m_timeRemainingUntilStartTimer <= 0f)
			{
				if (this.TimerStartedAction != null)
				{
					this.TimerStartedAction.Invoke();
				}
				iTween.Stop(base.get_gameObject());
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"name",
					"Fancy Number Display",
					"from",
					this.m_currentValue,
					"to",
					this.m_actualValue,
					"easeType",
					this.m_fillEaseType,
					"time",
					this.m_duration,
					"onupdate",
					"TimerUpdateCB",
					"oncomplete",
					"TimerEndedCB"
				}));
				this.m_startedTimer = true;
			}
		}
	}

	private void TimerUpdateCB(int newValue)
	{
		this.m_currentValue = newValue;
		this.m_numberText.set_text(string.Empty + newValue);
		if (this.TimerUpdateAction != null)
		{
			this.TimerUpdateAction.Invoke(newValue);
		}
	}

	private void TimerEndedCB()
	{
		this.m_currentValue = this.m_actualValue;
		this.m_numberText.set_text(string.Empty + this.m_currentValue);
		if (this.TimerEndedAction != null)
		{
			this.TimerEndedAction.Invoke();
		}
	}
}
