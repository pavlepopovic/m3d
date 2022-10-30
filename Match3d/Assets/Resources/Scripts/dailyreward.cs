using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class dailyreward : MonoBehaviour {
	int firsttimerewardvalue;
	public GameObject dailyrewarddialogbox;

	public float mstowait=5000.0f;
	public Button button;

	private ulong lastchestopen;
	public Text  chesttimer2;
    
    
    
	// Use this for initialization
	void Start () {

		firsttimerewardvalue= PlayerPrefs.GetInt("firsttimerewardvalue", 0);
        if (firsttimerewardvalue == 0)
        {
			dailyrewarddialogbox.SetActive(true);
			firsttimerewardvalue = 1;
			PlayerPrefs.SetInt("firsttimerewardvalue", firsttimerewardvalue);


		}
		lastchestopen =ulong.Parse(PlayerPrefs.GetString ("lastchestopen"));


		if(!ischestready()){
			button.interactable = false;
        }
        else
        {

			dailyrewarddialogbox.SetActive(true);
			chesttimer2.text = "Collect your Daily Reward!";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!button.IsInteractable()){
			if (ischestready()) {
				button.interactable = true;

				return;
			}
			//setting the timer
			ulong dif = (ulong) (DateTime.Now.Ticks) - lastchestopen;
			//		Debug.Log (dif);
			ulong m = dif / TimeSpan.TicksPerMillisecond;
			float secondslefts = (float)(mstowait - m) / 1000f;
//			if (valueforrespin == 1) {
//				secondslefts = 0f;
//				valueforrespin = 0;
//			}
			string t = "";
			//Hours
			t+=((int)secondslefts/3600).ToString()+"h";
			secondslefts-=((int)secondslefts/3600)*3600;
			//Minutes

			t+=((int)secondslefts/60).ToString("00")+"m";
			//Seconds
			t+=(secondslefts%60).ToString("00")+"s";
			
            chesttimer2.text = t;
        }

	
	}

	public void timecheckchest(){
		lastchestopen = (ulong)(DateTime.Now.Ticks);
		Debug.Log (DateTime.Now.Ticks.ToString());
		PlayerPrefs.SetString ("lastchestopen",lastchestopen.ToString());
		button.interactable = false;
		
	}
	private bool ischestready(){
		ulong dif = (ulong) (DateTime.Now.Ticks) - lastchestopen;
//		Debug.Log (dif);
		ulong m = dif / TimeSpan.TicksPerMillisecond;
		float secondslefts = (float)(mstowait - m) / 1000f;
		if (secondslefts < 0) {
			dailyrewarddialogbox.SetActive(true);
			chesttimer2.text = "Collect your Daily Reward!";
           
            return true;
		}
		else
			return false;
		}
	
    public void Oncollectclick()
    {
        SoundManager.instance.PlayButtonSOund();

		int coin = prefmanager.instance.Getcoinsvalue();
       coin += 100;
		prefmanager.instance.SetcoinsValue(coin);
       
        timecheckchest();
    }
	
	}
