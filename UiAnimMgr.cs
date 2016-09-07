using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimMgr
{
	private class AnimData
	{
		public TextAsset m_sourceData;

		public string m_animName;

		public List<GameObject> m_activeObjects;

		public Stack<GameObject> m_availableObjects;
	}

	public class UiAnimHandle
	{
		private UiAnimation m_anim;

		private int m_ID;

		public UiAnimHandle(UiAnimation anim)
		{
			this.m_anim = anim;
			this.m_ID = anim.m_ID;
		}

		public UiAnimation GetAnim()
		{
			if (this.m_anim == null)
			{
				return null;
			}
			if (this.m_anim.m_ID == this.m_ID)
			{
				return this.m_anim;
			}
			return null;
		}
	}

	private static UiAnimMgr s_instance;

	private static bool s_initialized;

	private Dictionary<string, UiAnimMgr.AnimData> m_animData;

	private int m_idIndex;

	private GameObject m_parentObj;

	public Material m_blendMaterial
	{
		get;
		private set;
	}

	public Material m_additiveMaterial
	{
		get;
		private set;
	}

	public static UiAnimMgr instance
	{
		get
		{
			if (UiAnimMgr.s_instance == null)
			{
				UiAnimMgr.s_instance = new UiAnimMgr();
				UiAnimMgr.s_instance.InitAnimMgr();
			}
			return UiAnimMgr.s_instance;
		}
	}

	private void InitAnimMgr()
	{
		if (UiAnimMgr.s_initialized)
		{
			Debug.Log("Warning: AnimMgr already initialized.");
			return;
		}
		this.m_parentObj = new GameObject();
		this.m_parentObj.set_name("UiAnimMgr Parent");
		this.m_additiveMaterial = (Resources.Load("Materials/UiAdditive") as Material);
		this.m_blendMaterial = (Resources.Load("Materials/UiBlend") as Material);
		this.m_animData = new Dictionary<string, UiAnimMgr.AnimData>();
		TextAsset[] array = Resources.LoadAll<TextAsset>("UiAnimations");
		uint num = 0u;
		while ((ulong)num < (ulong)((long)array.Length))
		{
			UiAnimMgr.AnimData animData = new UiAnimMgr.AnimData();
			animData.m_sourceData = array[(int)((UIntPtr)num)];
			animData.m_animName = array[(int)((UIntPtr)num)].get_name();
			animData.m_activeObjects = new List<GameObject>();
			animData.m_availableObjects = new Stack<GameObject>();
			this.m_animData.Add(array[(int)((UIntPtr)num)].get_name(), animData);
			GameObject gameObject = this.CreateAnimObj(array[(int)((UIntPtr)num)].get_name(), true);
			gameObject.SetActive(false);
			gameObject.get_transform().SetParent(this.m_parentObj.get_transform());
			num += 1u;
		}
		this.m_idIndex = 0;
		UiAnimMgr.s_initialized = true;
	}

	public TextAsset GetSourceData(string key)
	{
		UiAnimMgr.AnimData animData = null;
		this.m_animData.TryGetValue(key, ref animData);
		if (animData == null)
		{
			return null;
		}
		return animData.m_sourceData;
	}

	public UiAnimMgr.UiAnimHandle PlayAnim(string animName, Transform parent, Vector3 localPos, float localScale, float fadeTime = 0f)
	{
		GameObject gameObject = this.CreateAnimObj(animName, false);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.get_transform().SetParent(parent, false);
		gameObject.get_transform().set_localPosition(localPos);
		gameObject.get_transform().set_localScale(new Vector3(localScale, localScale, localScale));
		UiAnimation component = gameObject.GetComponent<UiAnimation>();
		component.Play(fadeTime);
		return new UiAnimMgr.UiAnimHandle(component);
	}

	private GameObject CreateAnimObj(string animName, bool createForInit = false)
	{
		UiAnimMgr.AnimData animData;
		this.m_animData.TryGetValue(animName, ref animData);
		if (animData == null)
		{
			return null;
		}
		GameObject gameObject = null;
		if (!createForInit && animData.m_availableObjects.get_Count() > 0)
		{
			gameObject = animData.m_availableObjects.Pop();
		}
		if (gameObject != null)
		{
			if (animData.m_activeObjects.Contains(gameObject))
			{
				Debug.Log("Error! new anim object already in active object list.");
			}
			else
			{
				animData.m_activeObjects.Add(gameObject);
			}
			gameObject.SetActive(true);
			UiAnimation component = gameObject.GetComponent<UiAnimation>();
			component.Reset();
			component.m_ID = this.GetNextID();
			return gameObject;
		}
		gameObject = new GameObject();
		if (createForInit)
		{
			animData.m_availableObjects.Push(gameObject);
		}
		else if (animData.m_activeObjects.Contains(gameObject))
		{
			Debug.Log("Error! new anim object already in active object list.");
		}
		else
		{
			animData.m_activeObjects.Add(gameObject);
		}
		CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
		canvasGroup.set_blocksRaycasts(false);
		canvasGroup.set_interactable(false);
		gameObject.set_name(animName);
		UiAnimation uiAnimation = gameObject.AddComponent<UiAnimation>();
		uiAnimation.m_ID = this.GetNextID();
		uiAnimation.Deserialize(animName);
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.SetSizeWithCurrentAnchors(0, uiAnimation.GetFrameWidth());
		rectTransform.SetSizeWithCurrentAnchors(1, uiAnimation.GetFrameHeight());
		using (Dictionary<string, UiAnimation.UiTexture>.ValueCollection.Enumerator enumerator = uiAnimation.m_textures.get_Values().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UiAnimation.UiTexture current = enumerator.get_Current();
				GameObject gameObject2 = new GameObject();
				gameObject2.set_name(current.m_parentKey + "_Texture");
				gameObject2.get_transform().SetParent(gameObject.get_transform(), false);
				current.m_image = gameObject2.AddComponent<Image>();
				current.m_image.set_sprite(current.m_sprite);
				current.m_image.get_canvasRenderer().SetAlpha(current.m_alpha);
				if (current.m_alphaMode == "ADD")
				{
					current.m_image.set_material(new Material(UiAnimMgr.instance.m_additiveMaterial));
				}
				else
				{
					current.m_image.set_material(new Material(UiAnimMgr.instance.m_blendMaterial));
				}
				current.m_image.get_material().set_mainTexture(current.m_sprite.get_texture());
				RectTransform component2 = gameObject2.GetComponent<RectTransform>();
				int num;
				if (current.m_anchor != null && current.m_anchor.relativePoint != null)
				{
					string text = current.m_anchor.relativePoint;
					if (text != null)
					{
						if (UiAnimMgr.<>f__switch$map9 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
							dictionary.Add("TOP", 0);
							dictionary.Add("BOTTOM", 1);
							dictionary.Add("LEFT", 2);
							dictionary.Add("RIGHT", 3);
							dictionary.Add("CENTER", 4);
							dictionary.Add("TOPLEFT", 5);
							dictionary.Add("TOPRIGHT", 6);
							dictionary.Add("BOTTOMLEFT", 7);
							dictionary.Add("BOTTOMRIGHT", 8);
							UiAnimMgr.<>f__switch$map9 = dictionary;
						}
						if (UiAnimMgr.<>f__switch$map9.TryGetValue(text, ref num))
						{
							switch (num)
							{
							case 0:
								component2.set_anchorMin(new Vector2(0.5f, 1f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 1:
								component2.set_anchorMin(new Vector2(0.5f, 0f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 2:
								component2.set_anchorMin(new Vector2(0f, 0.5f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 3:
								component2.set_anchorMin(new Vector2(1f, 0.5f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 4:
								component2.set_anchorMin(new Vector2(0.5f, 0.5f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 5:
								component2.set_anchorMin(new Vector2(0f, 1f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 6:
								component2.set_anchorMin(new Vector2(1f, 1f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 7:
								component2.set_anchorMin(new Vector2(0f, 0f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							case 8:
								component2.set_anchorMin(new Vector2(1f, 0f));
								component2.set_anchorMax(component2.get_anchorMin());
								break;
							}
						}
					}
				}
				Vector2 anchoredPosition = default(Vector2);
				if (current.m_anchor != null && current.m_anchor.point != null)
				{
					string text = current.m_anchor.point;
					if (text != null)
					{
						if (UiAnimMgr.<>f__switch$mapA == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
							dictionary.Add("TOP", 0);
							dictionary.Add("BOTTOM", 1);
							dictionary.Add("LEFT", 2);
							dictionary.Add("RIGHT", 3);
							dictionary.Add("CENTER", 4);
							dictionary.Add("TOPLEFT", 5);
							dictionary.Add("TOPRIGHT", 6);
							dictionary.Add("BOTTOMLEFT", 7);
							dictionary.Add("BOTTOMRIGHT", 8);
							UiAnimMgr.<>f__switch$mapA = dictionary;
						}
						if (UiAnimMgr.<>f__switch$mapA.TryGetValue(text, ref num))
						{
							switch (num)
							{
							case 0:
								anchoredPosition.Set(0f, -0.5f * current.m_image.get_sprite().get_rect().get_height());
								break;
							case 1:
								anchoredPosition.Set(0f, 0.5f * current.m_image.get_sprite().get_rect().get_height());
								break;
							case 2:
								anchoredPosition.Set(0.5f * current.m_image.get_sprite().get_rect().get_width(), 0f);
								break;
							case 3:
								anchoredPosition.Set(-0.5f * current.m_image.get_sprite().get_rect().get_width(), 0f);
								break;
							case 5:
								anchoredPosition.Set(0.5f * current.m_image.get_sprite().get_rect().get_width(), -0.5f * current.m_image.get_sprite().get_rect().get_height());
								break;
							case 6:
								anchoredPosition.Set(-0.5f * current.m_image.get_sprite().get_rect().get_width(), -0.5f * current.m_image.get_sprite().get_rect().get_height());
								break;
							case 7:
								anchoredPosition.Set(0.5f * current.m_image.get_sprite().get_rect().get_width(), 0.5f * current.m_image.get_sprite().get_rect().get_height());
								break;
							case 8:
								anchoredPosition.Set(-0.5f * current.m_image.get_sprite().get_rect().get_width(), 0.5f * current.m_image.get_sprite().get_rect().get_height());
								break;
							}
						}
					}
				}
				component2.set_anchoredPosition(anchoredPosition);
				component2.SetSizeWithCurrentAnchors(0, current.m_image.get_sprite().get_rect().get_width());
				component2.SetSizeWithCurrentAnchors(1, current.m_image.get_sprite().get_rect().get_height());
			}
		}
		using (Dictionary<string, UiAnimation.UiTexture>.ValueCollection.Enumerator enumerator2 = uiAnimation.m_textures.get_Values().GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				UiAnimation.UiTexture current2 = enumerator2.get_Current();
				RectTransform rectTransform2 = current2.m_image.get_rectTransform();
				current2.m_localPosition = rectTransform2.get_localPosition();
			}
		}
		return gameObject;
	}

	public void AnimComplete(UiAnimation script)
	{
		GameObject gameObject = script.get_gameObject();
		UiAnimMgr.AnimData animData;
		this.m_animData.TryGetValue(gameObject.get_name(), ref animData);
		if (animData == null)
		{
			Debug.Log("Error! UiAnimMgr could not find completed anim " + gameObject.get_name());
			return;
		}
		if (!animData.m_activeObjects.Remove(gameObject))
		{
			Debug.Log("Error! anim obj " + gameObject.get_name() + "not in UiAnimMgr active list");
		}
		animData.m_availableObjects.Push(gameObject);
		gameObject.SetActive(false);
		gameObject.get_transform().SetParent(this.m_parentObj.get_transform());
		UiAnimation component = gameObject.GetComponent<UiAnimation>();
		component.m_ID = 0;
	}

	public int GetNumActiveAnims()
	{
		int num = 0;
		using (Dictionary<string, UiAnimMgr.AnimData>.Enumerator enumerator = this.m_animData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, UiAnimMgr.AnimData> current = enumerator.get_Current();
				num += current.get_Value().m_activeObjects.get_Count();
			}
		}
		return num;
	}

	public int GetNumAvailableAnims()
	{
		int num = 0;
		using (Dictionary<string, UiAnimMgr.AnimData>.Enumerator enumerator = this.m_animData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, UiAnimMgr.AnimData> current = enumerator.get_Current();
				num += current.get_Value().m_availableObjects.get_Count();
			}
		}
		return num;
	}

	private int GetNextID()
	{
		this.m_idIndex++;
		return this.m_idIndex;
	}
}
