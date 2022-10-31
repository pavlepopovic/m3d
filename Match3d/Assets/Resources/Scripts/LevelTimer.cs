using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
	[UnityEngine.Serialization.FormerlySerializedAs("restart")]
	public GameObject Restart;
	[UnityEngine.Serialization.FormerlySerializedAs("level")]
	public leveleditor Level;
	public int SadaSeconds;
	public int SadaMinute;	
	public bool FreezeTimeBool;

	private Text m_TimeText;
	private int m_LevelSectionTimer;
	private bool m_Flag = true;
	private bool m_FirstTime = false;
	private int m_Minutes;
	private int m_Seconds;

	// Use this for initialization
	void Start()
	{
		m_TimeText = GetComponent<Text>();

		m_LevelSectionTimer = PrefManager.GetLevelsValue();
		Time.timeScale = 1;
		if (!m_FirstTime)
        {
			m_Minutes = Level.LevelData[m_LevelSectionTimer - 1].minute;
			m_Seconds = Level.LevelData[m_LevelSectionTimer - 1].seconds;
			m_FirstTime = true;
		}

		StartCoroutine(Wait());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_Seconds < 10)
		{
			m_TimeText.text = (m_Minutes + ":0" + m_Seconds);
		}
		if(m_Seconds > 9)
		{
			m_TimeText.text = (m_Minutes + ":" + m_Seconds);
		}
	}

	public void ReviveButton()
    {
		m_Seconds = 60;
		CountDown();
		Restart.SetActive(false);
	}

	public void CountDown()
	{
		if (SadaSeconds >= 60)
		{
			SadaMinute++;
			SadaSeconds = 0;
		}

		if (m_Seconds <= 0)
		{
			m_Minutes--;
			m_Seconds = 60;			
		}

		if(m_Minutes >= 0)
		{
			m_Seconds -= 1;
			SadaSeconds += 1;
		}

		if(m_Minutes <= 0 && m_Seconds <= 0)
		{
            Time.timeScale = 0f;
			Restart.SetActive(true);
			StopTimer();
		}
		else
		{
			Start();
		}
		if ((m_Minutes==3) && (m_Seconds<=30) && m_Flag)
		{
			m_Flag = false;
		}
	}

	public void MinusMinute()
	{
		m_Minutes -= 1;
	}

	public IEnumerator Wait()
	{
		yield return new WaitForSeconds(1);
		if (FreezeTimeBool == false)
		{
			CountDown();
		}
	}

	public void StopTimer()
	{
		m_Seconds = 0;
		m_Minutes = 0;
	}
}



