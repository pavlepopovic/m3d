using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leveltimer : MonoBehaviour {
	public GameObject restart;

	public bool freezetimebool;


	public leveleditor level;
	private Text time;
	float tim;
	int levelselectiontimer;
	bool flag=true;
	int i=0;
	public int Minutes;
	public int Seconds;
	bool firsttime;
	public int sadaminute, sadasecond;


	// Use this for initialization
	void Start () {
		time = GetComponent<Text> ();
		levelselectiontimer = prefmanager.instance.Getlevelsvalue();
        //levelselectiontimer = 1;
		Time.timeScale = 1;
		if (i == 0)
		{
			Minutes = level.LevelData[levelselectiontimer - 1].minute;
		  Seconds = level.LevelData[levelselectiontimer - 1].seconds;
			i++;
		}


		/*
		if (levelselectiontimer == 1) {
			if(i==0) {
				Minutes = 1;
				i++;
			}
		} */
		StartCoroutine(Wait());
	}
	
	// Update is called once per frame
	void Update ()
	{
		



//				tim -= Time.deltaTime;
//				time.text = "" + Mathf.Round (tim);
//				if (tim <= 1) {
//					Time.timeScale = 0;
//					restart.SetActive (true);
//					flag = false;
//					} else {
//				Time.timeScale = 1;
//				}
			//		}
		if(Seconds < 10)
		{
			time.text = (Minutes + ":0" + Seconds);
		}
		if(Seconds > 9)
		{
			time.text = (Minutes + ":" + Seconds);
		}

	}

	public void ReviveButton()
    {
		Seconds = 60;
		CountDown();
		restart.SetActive(false);
	}





	public void CountDown()
	{
		if (sadasecond >= 60)
		{
			PlusMinute();
			sadasecond = 0;
		}

		



		if (Seconds <= 0)
		{
			MinusMinute();
			Seconds = 60;
			
		}
		if(Minutes >= 0)
		{
			MinusSeconds();
		}
		if(Minutes <= 0 && Seconds <= 0)
		{
            //			Gads.ShowBanner ();
            Time.timeScale = 0f;
			restart.SetActive (true);
			//	Application.LoadLevel ("LevelFail");
			StopTimer();
		}
		else
		{
			Start ();
		}
		if ((Minutes==3) && (Seconds<=30) && flag) {
			flag = false;
			//				Uads.showUnityAd ();
		}
	}
	public void MinusMinute()
	{
		Minutes -= 1;
        
		


	}
	public void PlusMinute()
    {
		sadaminute += 1;
	}







	public void MinusSeconds()
	{
		Seconds -= 1;
		sadasecond += 1;
	}
	public void PlusSeconds()
	{
		
		sadasecond += 1;
	}



	public IEnumerator Wait()
	{
		yield return new WaitForSeconds(1);
		if (freezetimebool == false)
		{
			CountDown();
		}
	}
	public void StopTimer()
	{
		Seconds = 0;
		Minutes = 0;
	}









}



