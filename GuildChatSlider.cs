using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildChatSlider : MonoBehaviour
{
	public enum SLASH_CMD
	{
		SYSTEM = 0,
		SAY = 1,
		PARTY = 2,
		RAID = 3,
		GUILD = 4,
		OFFICER = 5,
		YELL = 6,
		WHISPER = 7,
		WHISPER_FOREIGN = 8,
		WHISPER_INFORM = 9,
		EMOTE = 10,
		TEXT_EMOTE = 11,
		MONSTER_SAY = 12,
		MONSTER_PARTY = 13,
		MONSTER_YELL = 14,
		MONSTER_WHISPER = 15,
		MONSTER_EMOTE = 16,
		SEND_CHANNEL = 17,
		JOIN_CHANNEL = 18,
		LEAVE_CHANNEL = 19,
		LIST_CHANNEL = 20,
		CHANNEL_NOTICE = 21,
		CHANNEL_NOTICE_USER = 22,
		SEND_AFK = 23,
		SEND_DND = 24,
		IGNORED = 25,
		SKILL = 26,
		LOOT = 27,
		MONEY = 28,
		OPENING = 29,
		TRADESKILLS = 30,
		PET_INFO = 31,
		COMBAT_MISC_INFO = 32,
		COMBAT_MSG_XP_GAIN = 33,
		COMBAT_MSG_HONOR_GAIN = 34,
		COMBAT_MSG_FACTION_CHANGE = 35,
		BG_SYSTEM_NEUTRAL = 36,
		BG_SYSTEM_ALLIANCE = 37,
		BG_SYSTEM_HORDE = 38,
		RAID_LEADER = 39,
		RAID_WARNING = 40,
		RAID_BOSS_EMOTE = 41,
		RAID_BOSS_WHISPER = 42,
		SPAM_FILTER = 43,
		RESTRICTED = 44,
		BATTLENET = 45,
		ACHIEVEMENT = 46,
		GUILD_ACHIEVEMENT = 47,
		COMBAT_MSG_ARENA_POINTS_GAIN = 48,
		PARTY_LEADER = 49,
		TARGET_ICONS = 50,
		BN_WHISPER = 51,
		BN_WHISPER_INFORM = 52,
		BN_CONVERSATION = 53,
		BN_CONVERSATION_NOTICE = 54,
		BN_CONVERSATION_LIST = 55,
		BN_INLINE_TOAST_ALERT = 56,
		BN_INLINE_TOAST_BROADCAST = 57,
		BN_INLINE_TOAST_BROADCAST_INFORM = 58,
		BN_INLINE_TOAST_CONVERSATION = 59,
		BN_WHISPER_PLAYER_OFFLINE = 60,
		MSG_COMBAT_GUILD_XP_GAIN = 61,
		CURRENCY = 62,
		QUEST_BOSS_EMOTE = 63,
		PET_BATTLE_COMBAT_LOG = 64,
		PET_BATTLE_INFO = 65,
		INSTANCE_CHAT = 66,
		INSTANCE_CHAT_LEADER = 67
	}

	[Header("General Stuff")]
	public GameObject chatRootObj;

	[Header("Chat Stuff")]
	public GameObject m_chatViewObj;

	public Text conversationText;

	public InputField textToSend;

	public Text m_numGuildMatesOnlineText;

	[Header("Guild Member Stuff")]
	public GameObject m_guildMemberViewObj;

	public Text m_guildMemberText;

	private void Start()
	{
		Main.instance.SetChatScript(this);
		this.ShowGuildChat();
	}

	private void Update()
	{
	}

	public void OnSendText()
	{
		if (this.textToSend.get_text().get_Length() == 0)
		{
			return;
		}
		Main.instance.SendGuildChat(this.textToSend.get_text());
		this.textToSend.set_text(string.Empty);
		this.textToSend.Select();
		this.textToSend.ActivateInputField();
	}

	public void OnReceiveText(string sender, string text)
	{
		Text expr_06 = this.conversationText;
		string text2 = expr_06.get_text();
		expr_06.set_text(string.Concat(new string[]
		{
			text2,
			"[",
			sender,
			"]: ",
			text,
			"\n"
		}));
	}

	public void Show()
	{
		this.chatRootObj.SetActive(true);
		this.chatRootObj.GetComponent<SliderPanel>().ShowSliderPanel();
	}

	public void Hide()
	{
		this.chatRootObj.GetComponent<SliderPanel>().HideSliderPanel();
	}

	public void Toggle()
	{
		if (this.chatRootObj.get_activeSelf())
		{
			this.Hide();
		}
		else
		{
			this.Show();
		}
	}

	public void UpdateGuildMateRoster()
	{
		if (GuildData.guildMemberDictionary.get_Count() == 0)
		{
			this.m_numGuildMatesOnlineText.set_text(string.Empty);
		}
		else
		{
			this.m_numGuildMatesOnlineText.set_text(string.Empty + GuildData.guildMemberDictionary.get_Count());
		}
		this.m_guildMemberText.set_text(string.Empty);
		using (Dictionary<string, GuildData.GuildMember>.Enumerator enumerator = GuildData.guildMemberDictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, GuildData.GuildMember> current = enumerator.get_Current();
				Text expr_76 = this.m_guildMemberText;
				expr_76.set_text(expr_76.get_text() + current.get_Value().m_mobileGuildMember.Name + "\n");
			}
		}
	}

	public void ShowGuildMemberList()
	{
		this.m_chatViewObj.SetActive(false);
		this.m_guildMemberViewObj.SetActive(true);
	}

	public void ShowGuildChat()
	{
		this.m_chatViewObj.SetActive(true);
		this.m_guildMemberViewObj.SetActive(false);
	}
}
