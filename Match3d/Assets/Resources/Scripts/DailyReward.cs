using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
	private ulong m_LastChestOpen;
	private int m_FirstTimeRewardValue;

	public Button Button;
	public Text ChestTimer;
	public GameObject DailyRewardDialogBox;
	public float MilisecondsToWait = 5000.0f;
    
	void Start()
	{
		m_FirstTimeRewardValue = PlayerPrefs.GetInt("firsttimerewardvalue", 0);
        if (m_FirstTimeRewardValue == 0)
        {
			DailyRewardDialogBox.SetActive(true);
			m_FirstTimeRewardValue = 1;
			PlayerPrefs.SetInt("firsttimerewardvalue", m_FirstTimeRewardValue);
		}

		m_LastChestOpen =ulong.Parse(PlayerPrefs.GetString ("lastchestopen"));
		if(!IsChestReady())
		{
			Button.interactable = false;
        }
        else
        {
			DailyRewardDialogBox.SetActive(true);
			ChestTimer.text = "Collect your Daily Reward!";
        }
	}
	
	void Update()
	{
		if(!Button.IsInteractable())
		{
			if (IsChestReady())
			{
				Button.interactable = true;
				return;
			}

			ulong dif = (ulong) (DateTime.Now.Ticks) - m_LastChestOpen;
			ulong m = dif / TimeSpan.TicksPerMillisecond;
			float secondslefts = (float)(MilisecondsToWait - m) / 1000f;

			string t = "";
            {
				//Hours
				t += ((int)secondslefts / 3600).ToString() + "h";
				secondslefts -= ((int)secondslefts / 3600) * 3600;

				//Minutes
				t += ((int)secondslefts / 60).ToString("00") + "m";

				//Seconds
				t += (secondslefts % 60).ToString("00") + "s";
			}
			
            ChestTimer.text = t;
        }
	}

	public void OnCollectClick()
	{
		SoundManager.instance.PlayButtonSound();

		int coin = PrefManager.s_Instance.GetCoinsValue();
		coin += 100;
		PrefManager.s_Instance.SetCoinsValue(coin);

		TimeCheckChest();
	}

	private void TimeCheckChest()
	{
		m_LastChestOpen = (ulong)(DateTime.Now.Ticks);
		Debug.Log(DateTime.Now.Ticks.ToString());
		PlayerPrefs.SetString("lastchestopen", m_LastChestOpen.ToString());
		Button.interactable = false;
	}

	private bool IsChestReady()
	{
		ulong dif = (ulong) (DateTime.Now.Ticks) - m_LastChestOpen;
		ulong m = dif / TimeSpan.TicksPerMillisecond;
		float secondslefts = (float)(MilisecondsToWait - m) / 1000f;
		if (secondslefts < 0) 
		{
			DailyRewardDialogBox.SetActive(true);
			ChestTimer.text = "Collect your Daily Reward!";           
            return true;
		}
		else
        {
			return false;
		}
	}	
}

