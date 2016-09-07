using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStaticData;

public class MissionListItem : MonoBehaviour
{
	public Text missionNameText;

	public Text missionLevelText;

	public Text missionTimeRemainingText;

	public Text missionResultsText;

	public Text rareMissionText;

	public Image levelBG;

	public GameObject inProgressDarkener;

	public Image[] locationImages;

	public Image m_missionTypeImage;

	public int garrMissionID;

	public int locationIndex;

	public GameObject missionRewardGroup;

	public GameObject missionRewardDisplayPrefab;

	public bool isResultsItem;

	private int missionDurationInSeconds;

	private long missionStartedTime;

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.missionTimeRemainingText.get_gameObject().get_activeSelf())
		{
			return;
		}
		long num = GarrisonStatus.CurrentTime() - this.missionStartedTime;
		long num2 = (long)this.missionDurationInSeconds - num;
		num2 = ((num2 <= 0L) ? 0L : num2);
		Duration duration = new Duration((int)num2);
		this.missionTimeRemainingText.set_text(duration.DurationString + " <color=#ff0000ff>(In Progress)</color>");
	}

	public void OnTap()
	{
		if (this.isResultsItem)
		{
			return;
		}
	}

	public void Init(int missionRecID)
	{
		this.garrMissionID = missionRecID;
		GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(this.garrMissionID);
		if (!PersistentMissionData.missionDictionary.ContainsKey(this.garrMissionID))
		{
			return;
		}
		this.missionDurationInSeconds = record.MissionDuration;
		JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)PersistentMissionData.missionDictionary.get_Item(this.garrMissionID);
		this.missionStartedTime = jamGarrisonMobileMission.StartTime;
		Duration duration = new Duration(record.MissionDuration);
		string text = (duration.Hours < 2) ? "<color=#ffffffff>" : "<color=#ff8600ff>";
		long num = GarrisonStatus.CurrentTime() - this.missionStartedTime;
		long num2 = (long)this.missionDurationInSeconds - num;
		num2 = ((num2 <= 0L) ? 0L : num2);
		bool flag = jamGarrisonMobileMission.MissionState == 1 && num2 > 0L;
		this.missionNameText.set_text(record.Name + ((!flag) ? (" (" + text + duration.DurationString + "</color>)") : string.Empty));
		this.missionLevelText.set_text(string.Empty + record.TargetLevel);
		this.inProgressDarkener.SetActive(flag);
		this.missionTimeRemainingText.get_gameObject().SetActive(flag);
		this.missionDurationInSeconds = record.MissionDuration;
		this.missionResultsText.get_gameObject().SetActive(false);
		this.isResultsItem = false;
		MissionRewardDisplay.InitMissionRewards(this.missionRewardDisplayPrefab, this.missionRewardGroup.get_transform(), jamGarrisonMobileMission.Reward);
		for (int i = 0; i < this.locationImages.Length; i++)
		{
			if (this.locationImages[i] != null)
			{
				this.locationImages[i].get_gameObject().SetActive(false);
			}
		}
		Image image = null;
		uint uiTextureKitID = record.UiTextureKitID;
		switch (uiTextureKitID)
		{
		case 101u:
			image = this.locationImages[1];
			this.locationIndex = 1;
			break;
		case 102u:
			image = this.locationImages[10];
			this.locationIndex = 10;
			break;
		case 103u:
			image = this.locationImages[3];
			this.locationIndex = 3;
			break;
		case 104u:
			image = this.locationImages[4];
			this.locationIndex = 4;
			break;
		case 105u:
			image = this.locationImages[9];
			this.locationIndex = 9;
			break;
		case 106u:
			image = this.locationImages[7];
			this.locationIndex = 7;
			break;
		case 107u:
			image = this.locationImages[8];
			this.locationIndex = 8;
			break;
		default:
			switch (uiTextureKitID)
			{
			case 203u:
				image = this.locationImages[2];
				this.locationIndex = 2;
				break;
			case 204u:
				image = this.locationImages[6];
				this.locationIndex = 6;
				break;
			case 205u:
				image = this.locationImages[5];
				this.locationIndex = 5;
				break;
			default:
				if (uiTextureKitID != 164u)
				{
					if (uiTextureKitID != 165u)
					{
						this.locationIndex = 0;
					}
					else
					{
						image = this.locationImages[11];
						this.locationIndex = 11;
					}
				}
				else
				{
					image = this.locationImages[0];
					this.locationIndex = 0;
				}
				break;
			}
			break;
		}
		if (image != null)
		{
			image.get_gameObject().SetActive(true);
		}
		GarrMissionTypeRec record2 = StaticDB.garrMissionTypeDB.GetRecord((int)record.GarrMissionTypeID);
		this.m_missionTypeImage.set_overrideSprite(TextureAtlas.instance.GetAtlasSprite((int)record2.UiTextureAtlasMemberID));
		if ((record.Flags & 1u) != 0u)
		{
			this.rareMissionText.get_gameObject().SetActive(true);
			Color color = this.levelBG.get_color();
			color.r = 0f;
			color.g = 0.211f;
			color.b = 0.506f;
			this.levelBG.set_color(color);
		}
		else
		{
			this.rareMissionText.get_gameObject().SetActive(false);
		}
	}
}
