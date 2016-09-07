using System;
using UnityEngine;
using UnityEngine.UI;

public class StackableMapIconContainer : MonoBehaviour
{
	public GameObject m_countainerPreviewIconsGroup;

	public Text m_iconCount;

	public GameObject m_multiRoot;

	public CanvasGroup m_multiRootCanvasGroup;

	public GameObject m_singleRoot;

	public GameObject m_deadRoot;

	private bool m_explodedListIsVisible;

	private void OnEnable()
	{
		this.ShowExplodedList(false);
		AdventureMapPanel expr_0C = AdventureMapPanel.instance;
		expr_0C.SelectedIconContainerChanged = (Action<StackableMapIconContainer>)Delegate.Combine(expr_0C.SelectedIconContainerChanged, new Action<StackableMapIconContainer>(this.HandleSelectedIconContainerChanged));
	}

	private void OnDisable()
	{
		AdventureMapPanel expr_05 = AdventureMapPanel.instance;
		expr_05.SelectedIconContainerChanged = (Action<StackableMapIconContainer>)Delegate.Remove(expr_05.SelectedIconContainerChanged, new Action<StackableMapIconContainer>(this.HandleSelectedIconContainerChanged));
	}

	private void OnDestroy()
	{
		StackableMapIconManager.RemoveStackableMapIconContainer(this);
	}

	public void ShowExplodedList(bool show)
	{
		this.m_explodedListIsVisible = show;
		this.m_multiRootCanvasGroup.set_alpha((!show) ? 0f : 1f);
		this.m_multiRootCanvasGroup.set_interactable(show);
		this.m_multiRootCanvasGroup.set_blocksRaycasts(show);
	}

	private bool ExplodedListIsVisible()
	{
		return this.m_explodedListIsVisible;
	}

	private void HandleSelectedIconContainerChanged(StackableMapIconContainer container)
	{
		if (container == this)
		{
			if (this.GetCount() > 1)
			{
				this.ShowExplodedList(true);
			}
		}
		else
		{
			this.ShowExplodedList(false);
		}
	}

	public Rect GetWorldRect()
	{
		Vector3[] array = new Vector3[4];
		RectTransform component = base.GetComponent<RectTransform>();
		component.GetWorldCorners(array);
		float num = array[2].x - array[0].x;
		float num2 = array[2].y - array[0].y;
		float zoomFactor = AdventureMapPanel.instance.m_pinchZoomContentManager.m_zoomFactor;
		num *= zoomFactor;
		num2 *= zoomFactor;
		Rect result = new Rect(array[0].x, array[0].y, num, num2);
		return result;
	}

	public void AddStackableMapIcon(StackableMapIcon icon)
	{
		StackableMapIcon componentInChildren = this.m_singleRoot.GetComponentInChildren<StackableMapIcon>(true);
		StackableMapIcon[] componentsInChildren = this.m_multiRoot.GetComponentsInChildren<StackableMapIcon>(true);
		bool flag = true;
		if (componentInChildren == null && componentsInChildren.Length == 0)
		{
			icon.get_transform().SetParent(this.m_singleRoot.get_transform(), false);
			icon.get_transform().get_transform().set_localPosition(Vector3.get_zero());
		}
		else if (componentInChildren != null && componentsInChildren.Length == 0)
		{
			componentInChildren.get_transform().SetParent(this.m_multiRoot.get_transform(), false);
			icon.get_transform().SetParent(this.m_multiRoot.get_transform(), false);
			flag = false;
		}
		else
		{
			icon.get_transform().SetParent(this.m_multiRoot.get_transform(), false);
			flag = false;
		}
		this.ShowExplodedList(false);
		if (flag)
		{
			this.m_countainerPreviewIconsGroup.SetActive(false);
		}
		else
		{
			this.m_countainerPreviewIconsGroup.SetActive(true);
		}
		componentsInChildren = this.m_multiRoot.GetComponentsInChildren<StackableMapIcon>(true);
		this.m_iconCount.set_text(string.Empty + componentsInChildren.Length);
		base.get_gameObject().set_name("IconContainer (" + ((componentsInChildren.Length <= 0) ? "Single" : (string.Empty + componentsInChildren.Length)) + ")");
		base.get_gameObject().SetActive(true);
	}

	public void RemoveStackableMapIcon(StackableMapIcon icon)
	{
		StackableMapIcon componentInChildren = this.m_singleRoot.GetComponentInChildren<StackableMapIcon>(true);
		StackableMapIcon[] componentsInChildren = this.m_multiRoot.GetComponentsInChildren<StackableMapIcon>(true);
		bool flag = false;
		if (componentsInChildren.Length == 2)
		{
			componentsInChildren[0].get_transform().SetParent(this.m_singleRoot.get_transform(), false);
			componentsInChildren[0].get_transform().set_localPosition(Vector3.get_zero());
			componentsInChildren[1].get_transform().SetParent(this.m_singleRoot.get_transform(), false);
			componentsInChildren[1].get_transform().set_localPosition(Vector3.get_zero());
			flag = true;
		}
		else if (componentInChildren != null)
		{
			base.get_gameObject().SetActive(false);
			return;
		}
		this.ShowExplodedList(false);
		if (flag)
		{
			this.m_countainerPreviewIconsGroup.SetActive(false);
		}
		else
		{
			this.m_countainerPreviewIconsGroup.SetActive(true);
		}
		int num = componentsInChildren.Length - 1;
		this.m_iconCount.set_text(string.Empty + num);
		base.get_gameObject().set_name("IconContainer (" + ((num <= 0) ? "Single" : (string.Empty + num)) + ")");
	}

	public void PlayTapSound()
	{
		Main.instance.m_UISound.Play_SelectWorldQuest();
	}

	public void ToggleIconList()
	{
		this.PlayTapSound();
		this.ShowExplodedList(!this.m_multiRoot.get_activeSelf());
		if (this.m_multiRoot.get_activeSelf())
		{
			AdventureMapPanel.instance.SetSelectedIconContainer(this);
			UiAnimMgr.instance.PlayAnim("MinimapPulseAnim", base.get_transform(), Vector3.get_zero(), 3f, 0f);
		}
		else
		{
			AdventureMapPanel.instance.SetSelectedIconContainer(null);
		}
	}

	public int GetCount()
	{
		StackableMapIcon componentInChildren = this.m_singleRoot.GetComponentInChildren<StackableMapIcon>(true);
		StackableMapIcon[] componentsInChildren = this.m_multiRoot.GetComponentsInChildren<StackableMapIcon>(true);
		int num = 0;
		if (componentInChildren != null)
		{
			num++;
		}
		return num + componentsInChildren.Length;
	}
}
