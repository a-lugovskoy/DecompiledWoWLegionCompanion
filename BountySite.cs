using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;

public class BountySite : MonoBehaviour
{
	public Image m_emissaryIcon;

	public Text m_invalidFileDataID;

	public Image m_errorImage;

	private MobileWorldQuestBounty m_bounty;

	private void Awake()
	{
	}

	public void SetBounty(MobileWorldQuestBounty bounty)
	{
		this.m_bounty = bounty;
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, bounty.IconFileDataID);
		if (sprite != null)
		{
			this.m_invalidFileDataID.get_gameObject().SetActive(false);
			this.m_emissaryIcon.set_sprite(sprite);
		}
		else
		{
			this.m_invalidFileDataID.get_gameObject().SetActive(true);
			this.m_invalidFileDataID.set_text(string.Empty + bounty.IconFileDataID);
		}
	}

	public void OnTap()
	{
		UiAnimMgr.instance.PlayAnim("MinimapPulseAnim", base.get_transform(), Vector3.get_zero(), 3f, 0f);
		AllPopups.instance.ShowBountyInfoTooltip(this.m_bounty);
	}

	private void Update()
	{
	}
}
