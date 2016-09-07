using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using WowStaticData;

public class UiAnimation : MonoBehaviour
{
	private enum State
	{
		Stopped = 0,
		Stopping = 1,
		Paused = 2,
		Playing = 3
	}

	public abstract class UiAnimElement
	{
		[XmlAttribute("childKey")]
		public string m_childKey;

		[XmlAttribute("smoothing")]
		public string m_smoothing;

		[XmlAttribute("duration")]
		public float m_duration;

		[XmlAttribute("startDelay")]
		public float m_startDelay;

		[XmlAttribute("order")]
		public int m_order;

		public object m_texture;

		public bool m_smoothIn;

		public bool m_smoothOut;

		public void SetSmoothing()
		{
			if (this.m_smoothing == "IN")
			{
				this.m_smoothIn = true;
				this.m_smoothOut = false;
			}
			else if (this.m_smoothing == "OUT")
			{
				this.m_smoothIn = false;
				this.m_smoothOut = true;
			}
			else if (this.m_smoothing == "IN_OUT")
			{
				this.m_smoothIn = true;
				this.m_smoothOut = true;
			}
			else
			{
				this.m_smoothIn = false;
				this.m_smoothOut = false;
			}
		}

		public abstract void Update(float elapsedTime, float maxTime, bool reverse);

		public abstract void Reset();

		public float GetUnitProgress(float elapsedTime, float maxTime, bool reverse, out bool update)
		{
			update = true;
			float num2;
			if (reverse)
			{
				float num = maxTime - (this.m_startDelay + this.m_duration);
				num2 = elapsedTime - num;
				if (num2 < 0f)
				{
					update = false;
					return 0f;
				}
				if (num2 > this.m_duration)
				{
					update = false;
					return 1f;
				}
			}
			else
			{
				num2 = elapsedTime - this.m_startDelay;
				if (num2 < 0f)
				{
					update = false;
					return 0f;
				}
				if (num2 > this.m_duration)
				{
					update = false;
					return 1f;
				}
			}
			if (num2 <= 0f)
			{
				return 0f;
			}
			if (num2 < Mathf.Epsilon)
			{
				return 1f;
			}
			float num3 = num2 / this.m_duration;
			num3 = Mathf.Clamp01(num3);
			if (!this.m_smoothIn && !this.m_smoothOut)
			{
				if (reverse)
				{
					num3 = 1f - num3;
				}
				return num3;
			}
			if (this.m_smoothIn && num3 <= 0.5f)
			{
				num3 = 0.5f * (1f + Mathf.Sin((1f - 2f * num3) * -0.5f * 3.14159274f));
			}
			else if (this.m_smoothOut && num3 > 0.5f)
			{
				num3 = 0.5f + 0.5f * Mathf.Sin(2f * (num3 - 0.5f) * 0.5f * 3.14159274f);
			}
			num3 = Mathf.Clamp01(num3);
			if (reverse)
			{
				num3 = 1f - num3;
			}
			return num3;
		}

		public float GetTotalTime()
		{
			return this.m_startDelay + this.m_duration;
		}
	}

	public class UiRotation : UiAnimation.UiAnimElement
	{
		[XmlAttribute("degrees")]
		public float m_degrees;

		public override void Update(float elapsedTime, float maxTime, bool reverse)
		{
			bool flag;
			float unitProgress = base.GetUnitProgress(elapsedTime, maxTime, reverse, out flag);
			if (!flag)
			{
				return;
			}
			UiAnimation.UiTexture uiTexture = (UiAnimation.UiTexture)this.m_texture;
			Quaternion localRotation = uiTexture.m_image.get_transform().get_localRotation();
			Vector3 eulerAngles = localRotation.get_eulerAngles();
			eulerAngles.z = this.m_degrees * unitProgress;
			localRotation.set_eulerAngles(eulerAngles);
			uiTexture.m_image.get_transform().set_localRotation(localRotation);
		}

		public override void Reset()
		{
		}
	}

	public class UiScale : UiAnimation.UiAnimElement
	{
		[XmlAttribute("fromScaleX")]
		public float m_fromScaleX;

		[XmlAttribute("toScaleX")]
		public float m_toScaleX;

		[XmlAttribute("fromScaleY")]
		public float m_fromScaleY;

		[XmlAttribute("toScaleY")]
		public float m_toScaleY;

		public override void Update(float elapsedTime, float maxTime, bool reverse)
		{
			bool flag;
			float unitProgress = base.GetUnitProgress(elapsedTime, maxTime, reverse, out flag);
			if (!flag)
			{
				return;
			}
			Vector3 localScale;
			localScale.x = this.m_fromScaleX + (this.m_toScaleX - this.m_fromScaleX) * unitProgress;
			localScale.y = this.m_fromScaleY + (this.m_toScaleY - this.m_fromScaleY) * unitProgress;
			localScale.z = 1f;
			UiAnimation.UiTexture uiTexture = (UiAnimation.UiTexture)this.m_texture;
			uiTexture.m_image.get_transform().set_localScale(localScale);
		}

		public override void Reset()
		{
		}
	}

	public class UiAlpha : UiAnimation.UiAnimElement
	{
		[XmlAttribute("fromAlpha")]
		public float m_fromAlpha;

		[XmlAttribute("toAlpha")]
		public float m_toAlpha;

		public override void Update(float elapsedTime, float maxTime, bool reverse)
		{
			bool flag;
			float unitProgress = base.GetUnitProgress(elapsedTime, maxTime, reverse, out flag);
			if (!flag)
			{
				return;
			}
			float alpha = this.m_fromAlpha + (this.m_toAlpha - this.m_fromAlpha) * unitProgress;
			UiAnimation.UiTexture uiTexture = (UiAnimation.UiTexture)this.m_texture;
			uiTexture.m_image.get_canvasRenderer().SetAlpha(alpha);
		}

		public override void Reset()
		{
		}
	}

	public class UiTranslation : UiAnimation.UiAnimElement
	{
		[XmlAttribute("offsetX")]
		public float m_offsetX;

		[XmlAttribute("offsetY")]
		public float m_offsetY;

		public override void Update(float elapsedTime, float maxTime, bool reverse)
		{
			bool flag;
			float unitProgress = base.GetUnitProgress(elapsedTime, maxTime, reverse, out flag);
			if (!flag)
			{
				return;
			}
			UiAnimation.UiTexture uiTexture = (UiAnimation.UiTexture)this.m_texture;
			RectTransform rectTransform = uiTexture.m_image.get_rectTransform();
			Vector2 localPosition = uiTexture.m_localPosition;
			localPosition.x += this.m_offsetX * unitProgress;
			localPosition.y += this.m_offsetY * unitProgress;
			rectTransform.set_localPosition(localPosition);
		}

		public override void Reset()
		{
			UiAnimation.UiTexture uiTexture = (UiAnimation.UiTexture)this.m_texture;
			Vector2 localPosition = uiTexture.m_localPosition;
			RectTransform rectTransform = uiTexture.m_image.get_rectTransform();
			rectTransform.set_localPosition(localPosition);
		}
	}

	public class UiSourceAnimGroup
	{
		[XmlAttribute("parentKey")]
		public string parentKey;

		[XmlAttribute("looping")]
		public string looping;

		[XmlElement("Alpha")]
		public List<UiAnimation.UiAlpha> m_alphas = new List<UiAnimation.UiAlpha>();

		[XmlElement("Scale")]
		public List<UiAnimation.UiScale> m_scales = new List<UiAnimation.UiScale>();

		[XmlElement("Rotation")]
		public List<UiAnimation.UiRotation> m_rotations = new List<UiAnimation.UiRotation>();

		[XmlElement("Translation")]
		public List<UiAnimation.UiTranslation> m_translations = new List<UiAnimation.UiTranslation>();
	}

	public class UiAnimGroup
	{
		public string m_parentKey;

		public bool m_looping;

		public bool m_bounce;

		public bool m_bounceBack;

		public float m_startTime;

		public float m_maxTime;

		public List<UiAnimation.UiAnimElement> m_elements = new List<UiAnimation.UiAnimElement>();

		public void Reset()
		{
			this.m_startTime = Time.get_timeSinceLevelLoad();
			this.m_bounceBack = false;
			using (List<UiAnimation.UiAnimElement>.Enumerator enumerator = this.m_elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UiAnimation.UiAnimElement current = enumerator.get_Current();
					current.Reset();
				}
			}
		}

		public bool Update(bool stopping)
		{
			float num = Time.get_timeSinceLevelLoad() - this.m_startTime;
			using (List<UiAnimation.UiAnimElement>.Enumerator enumerator = this.m_elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UiAnimation.UiAnimElement current = enumerator.get_Current();
					if (!stopping || !(current is UiAnimation.UiAlpha))
					{
						current.Update(num, this.m_maxTime, this.m_bounce && this.m_bounceBack);
					}
				}
			}
			if (num >= this.m_maxTime && this.m_looping)
			{
				this.m_startTime = Time.get_timeSinceLevelLoad();
				if (this.m_bounce)
				{
					this.m_bounceBack = !this.m_bounceBack;
				}
				else
				{
					this.Reset();
				}
				return false;
			}
			return num >= this.m_maxTime;
		}
	}

	public class UiAnim
	{
		[XmlElement("AnimationGroup")]
		public List<UiAnimation.UiSourceAnimGroup> groups = new List<UiAnimation.UiSourceAnimGroup>();
	}

	public class UiAnchor
	{
		[XmlAttribute("point")]
		public string point;

		[XmlAttribute("relativePoint")]
		public string relativePoint;

		[XmlAttribute("x")]
		public float x;

		[XmlAttribute("y")]
		public float y;
	}

	public class UiSourceTexture
	{
		[XmlAttribute("parentKey")]
		public string m_parentKey;

		[XmlAttribute("hidden")]
		public bool m_hidden;

		[XmlAttribute("alpha")]
		public float m_alpha;

		[XmlAttribute("alphaMode")]
		public string m_alphaMode;

		[XmlAttribute("atlas")]
		public string m_atlas;

		[XmlAttribute("useAtlasSize")]
		public bool m_useAtlasSize;

		[XmlArray("Anchors"), XmlArrayItem("Anchor")]
		public List<UiAnimation.UiAnchor> m_anchors = new List<UiAnimation.UiAnchor>();
	}

	public class UiTexture
	{
		public string m_parentKey;

		public bool m_hidden;

		public float m_alpha;

		public string m_alphaMode;

		public string m_atlas;

		public bool m_useAtlasSize;

		public UiAnimation.UiAnchor m_anchor;

		public int m_textureAtlasMemberID;

		public Sprite m_sprite;

		public Image m_image;

		public Vector2 m_localPosition;
	}

	public class UiLayer
	{
		[XmlAttribute("level")]
		public string level;

		[XmlElement("Texture")]
		public List<UiAnimation.UiSourceTexture> textures = new List<UiAnimation.UiSourceTexture>();
	}

	public class UiFrame
	{
		[XmlAttribute("hidden")]
		public bool hidden;

		[XmlAttribute("parent")]
		public string parent;

		[XmlAttribute("parentKey")]
		public string parentKey;

		[XmlAttribute("alpha")]
		public float alpha;

		[XmlElement("Size")]
		public UiAnimation.UiSize size = new UiAnimation.UiSize();

		[XmlArray("Layers"), XmlArrayItem("Layer")]
		public List<UiAnimation.UiLayer> layers = new List<UiAnimation.UiLayer>();

		[XmlElement("Animations")]
		public UiAnimation.UiAnim animation = new UiAnimation.UiAnim();

		[XmlAttribute("name")]
		public string name;
	}

	public class UiSize
	{
		[XmlAttribute("x")]
		public float x;

		[XmlAttribute("y")]
		public float y;
	}

	[XmlRoot("Ui")]
	public class UiSourceAnimation
	{
		[XmlElement("Frame")]
		public UiAnimation.UiFrame frame;
	}

	public Dictionary<string, UiAnimation.UiTexture> m_textures = new Dictionary<string, UiAnimation.UiTexture>();

	private List<UiAnimation.UiAnimGroup> m_groups = new List<UiAnimation.UiAnimGroup>();

	private UiAnimation.UiFrame m_frame;

	private UiAnimation.State m_state;

	private float m_fadeTime;

	private float m_fadeStart;

	public int m_ID;

	private float m_fadeAlphaScalar = 1f;

	private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
	{
	}

	private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
	{
	}

	public float GetFrameWidth()
	{
		return this.m_frame.size.x;
	}

	public float GetFrameHeight()
	{
		return this.m_frame.size.y;
	}

	public void Deserialize(string animName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(UiAnimation.UiSourceAnimation));
		xmlSerializer.add_UnknownNode(new XmlNodeEventHandler(this.serializer_UnknownNode));
		xmlSerializer.add_UnknownAttribute(new XmlAttributeEventHandler(this.serializer_UnknownAttribute));
		TextAsset sourceData = UiAnimMgr.instance.GetSourceData(animName);
		if (sourceData == null)
		{
			Debug.Log("Could not find asset " + animName);
			return;
		}
		MemoryStream memoryStream = new MemoryStream(sourceData.get_bytes());
		UiAnimation.UiSourceAnimation uiSourceAnimation = xmlSerializer.Deserialize(memoryStream) as UiAnimation.UiSourceAnimation;
		memoryStream.Close();
		if (uiSourceAnimation == null)
		{
			Debug.Log("No ui animation.");
			return;
		}
		this.m_frame = uiSourceAnimation.frame;
		using (List<UiAnimation.UiSourceAnimGroup>.Enumerator enumerator = uiSourceAnimation.frame.animation.groups.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UiAnimation.UiSourceAnimGroup current = enumerator.get_Current();
				UiAnimation.UiAnimGroup uiAnimGroup = new UiAnimation.UiAnimGroup();
				uiAnimGroup.m_parentKey = current.parentKey;
				uiAnimGroup.m_bounceBack = false;
				if (current.looping == null)
				{
					uiAnimGroup.m_looping = false;
					uiAnimGroup.m_bounce = false;
				}
				else if (current.looping == "REPEAT")
				{
					uiAnimGroup.m_looping = true;
					uiAnimGroup.m_bounce = false;
				}
				else if (current.looping == "BOUNCE")
				{
					uiAnimGroup.m_looping = true;
					uiAnimGroup.m_bounce = true;
				}
				using (List<UiAnimation.UiScale>.Enumerator enumerator2 = current.m_scales.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UiAnimation.UiScale current2 = enumerator2.get_Current();
						if (current2.m_childKey != null)
						{
							current2.SetSmoothing();
							uiAnimGroup.m_elements.Add(current2);
						}
					}
				}
				using (List<UiAnimation.UiAlpha>.Enumerator enumerator3 = current.m_alphas.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						UiAnimation.UiAlpha current3 = enumerator3.get_Current();
						if (current3.m_childKey != null)
						{
							current3.SetSmoothing();
							uiAnimGroup.m_elements.Add(current3);
						}
					}
				}
				using (List<UiAnimation.UiRotation>.Enumerator enumerator4 = current.m_rotations.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						UiAnimation.UiRotation current4 = enumerator4.get_Current();
						if (current4.m_childKey != null)
						{
							current4.SetSmoothing();
							uiAnimGroup.m_elements.Add(current4);
						}
					}
				}
				using (List<UiAnimation.UiTranslation>.Enumerator enumerator5 = current.m_translations.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						UiAnimation.UiTranslation current5 = enumerator5.get_Current();
						if (current5.m_childKey != null)
						{
							current5.SetSmoothing();
							uiAnimGroup.m_elements.Add(current5);
						}
					}
				}
				this.m_groups.Add(uiAnimGroup);
			}
		}
		using (List<UiAnimation.UiLayer>.Enumerator enumerator6 = uiSourceAnimation.frame.layers.GetEnumerator())
		{
			while (enumerator6.MoveNext())
			{
				UiAnimation.UiLayer current6 = enumerator6.get_Current();
				using (List<UiAnimation.UiSourceTexture>.Enumerator enumerator7 = current6.textures.GetEnumerator())
				{
					UiAnimation.UiSourceTexture texture;
					while (enumerator7.MoveNext())
					{
						texture = enumerator7.get_Current();
						if (texture.m_parentKey != null)
						{
							UiAnimation.UiTexture uiTexture;
							this.m_textures.TryGetValue(texture.m_parentKey, ref uiTexture);
							if (uiTexture != null)
							{
								Debug.Log("Found duplicate texture " + texture.m_parentKey);
							}
							else
							{
								int textureAtlasMemberID = 0;
								StaticDB.uiTextureAtlasMemberDB.EnumRecords(delegate(UiTextureAtlasMemberRec memberRec)
								{
									if (memberRec.CommittedName != null && texture.m_atlas != null && memberRec.CommittedName.ToLower() == texture.m_atlas.ToLower())
									{
										textureAtlasMemberID = memberRec.ID;
										return false;
									}
									return true;
								});
								if (textureAtlasMemberID > 0)
								{
									Sprite sprite = TextureAtlas.GetSprite(textureAtlasMemberID);
									if (sprite != null)
									{
										UiAnimation.UiTexture uiTexture2 = new UiAnimation.UiTexture();
										uiTexture2.m_alpha = texture.m_alpha;
										uiTexture2.m_alphaMode = texture.m_alphaMode;
										uiTexture2.m_anchor = ((texture.m_anchors.get_Count() <= 0) ? null : texture.m_anchors.ToArray()[0]);
										uiTexture2.m_atlas = texture.m_atlas;
										uiTexture2.m_hidden = texture.m_hidden;
										uiTexture2.m_parentKey = texture.m_parentKey;
										uiTexture2.m_sprite = sprite;
										this.m_textures.Add(texture.m_parentKey, uiTexture2);
									}
									else
									{
										Debug.Log(string.Concat(new object[]
										{
											"Could not find sprite for textureAtlasMemberID ",
											textureAtlasMemberID,
											" in Ui Animation ",
											animName
										}));
									}
								}
							}
						}
					}
				}
			}
		}
		List<UiAnimation.UiAnimElement> list = new List<UiAnimation.UiAnimElement>();
		using (List<UiAnimation.UiAnimGroup>.Enumerator enumerator8 = this.m_groups.GetEnumerator())
		{
			while (enumerator8.MoveNext())
			{
				UiAnimation.UiAnimGroup current7 = enumerator8.get_Current();
				current7.m_maxTime = 0f;
				using (List<UiAnimation.UiAnimElement>.Enumerator enumerator9 = current7.m_elements.GetEnumerator())
				{
					while (enumerator9.MoveNext())
					{
						UiAnimation.UiAnimElement current8 = enumerator9.get_Current();
						UiAnimation.UiTexture uiTexture3 = null;
						this.m_textures.TryGetValue(current8.m_childKey, ref uiTexture3);
						if (uiTexture3 != null)
						{
							current8.m_texture = uiTexture3;
							float totalTime = current8.GetTotalTime();
							if (totalTime > current7.m_maxTime)
							{
								current7.m_maxTime = totalTime;
							}
						}
						else
						{
							list.Add(current8);
							Debug.Log("Removing element with childKey " + current8.m_childKey + ", no associated texture was found.");
						}
					}
				}
				using (List<UiAnimation.UiAnimElement>.Enumerator enumerator10 = list.GetEnumerator())
				{
					while (enumerator10.MoveNext())
					{
						UiAnimation.UiAnimElement current9 = enumerator10.get_Current();
						current7.m_elements.Remove(current9);
					}
				}
			}
		}
	}

	private bool IsTextureReferenced(string parentKey)
	{
		using (List<UiAnimation.UiAnimGroup>.Enumerator enumerator = this.m_groups.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UiAnimation.UiAnimGroup current = enumerator.get_Current();
				using (List<UiAnimation.UiAnimElement>.Enumerator enumerator2 = current.m_elements.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UiAnimation.UiAnimElement current2 = enumerator2.get_Current();
						if (current2.m_childKey == parentKey)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public void Reset()
	{
		using (List<UiAnimation.UiAnimGroup>.Enumerator enumerator = this.m_groups.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UiAnimation.UiAnimGroup current = enumerator.get_Current();
				current.Reset();
			}
		}
	}

	public void Play(float fadeTime = 0f)
	{
		switch (this.m_state)
		{
		case UiAnimation.State.Stopped:
		case UiAnimation.State.Stopping:
			this.Reset();
			break;
		case UiAnimation.State.Playing:
			return;
		}
		this.m_state = UiAnimation.State.Playing;
		this.m_fadeTime = fadeTime;
		if (fadeTime > 0f)
		{
			this.m_fadeAlphaScalar = 0f;
			this.m_fadeStart = Time.get_timeSinceLevelLoad();
			this.Update();
		}
	}

	public void Stop(float fadeTime = 0f)
	{
		if (fadeTime > Mathf.Epsilon)
		{
			this.m_state = UiAnimation.State.Stopping;
			this.m_fadeTime = fadeTime;
			this.m_fadeStart = Time.get_timeSinceLevelLoad();
		}
		else
		{
			this.m_state = UiAnimation.State.Stopped;
			this.m_fadeTime = 0f;
			UiAnimMgr.instance.AnimComplete(this);
		}
	}

	private void Update()
	{
		if (this.m_state != UiAnimation.State.Playing && this.m_state != UiAnimation.State.Stopping)
		{
			return;
		}
		bool flag = true;
		using (List<UiAnimation.UiAnimGroup>.Enumerator enumerator = this.m_groups.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UiAnimation.UiAnimGroup current = enumerator.get_Current();
				if (!current.Update(this.m_state == UiAnimation.State.Stopping))
				{
					flag = false;
				}
			}
		}
		bool flag2 = false;
		if (this.m_state == UiAnimation.State.Playing && this.m_fadeTime > 0f)
		{
			float num = (Time.get_timeSinceLevelLoad() - this.m_fadeStart) / this.m_fadeTime;
			if (num >= 1f)
			{
				this.m_fadeTime = 0f;
				num = 1f;
			}
			flag2 = true;
			this.m_fadeAlphaScalar = num;
		}
		if (this.m_state == UiAnimation.State.Stopping)
		{
			flag = false;
			float num2 = (Time.get_timeSinceLevelLoad() - this.m_fadeStart) / this.m_fadeTime;
			if (num2 >= 1f)
			{
				num2 = 1f;
				flag = true;
			}
			num2 = 1f - num2;
			this.m_fadeAlphaScalar = num2;
			flag2 = true;
		}
		if (flag2)
		{
			using (Dictionary<string, UiAnimation.UiTexture>.ValueCollection.Enumerator enumerator2 = this.m_textures.get_Values().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UiAnimation.UiTexture current2 = enumerator2.get_Current();
					current2.m_image.get_canvasRenderer().SetAlpha(current2.m_alpha * this.m_fadeAlphaScalar);
				}
			}
		}
		if (flag)
		{
			this.m_state = UiAnimation.State.Stopped;
			this.m_fadeTime = 0f;
			UiAnimMgr.instance.AnimComplete(this);
		}
	}
}
