using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class admobads : MonoBehaviour {
	bool hidebanner;
	
	public string appid;
	public string bannerid;
	public string intersitialid;
	//rewardedvideo ad
	public string admobrewardedvideoid;
	private RewardedAd rewardbasedad;
	//end
	private BannerView bannerview;
	InterstitialAd interstitial;
	AdRequest bannerrequest;
	AdRequest interstialrequest;
	string agefilter;
	Scene m_scene;
	int privacyvalue;
	int removeadvalue;
    int advalue;
	// Use this for initialization
	void Start () {
		
		agefilter = PlayerPrefs.GetString ("agecheck");
		privacyvalue = PlayerPrefs.GetInt ("privacyvalue",0);

        MobileAds.Initialize(initStatus => { });
        print ("againrun");
	     //	requestbanner ();
			requestintersitial ();
        this.rewardbasedad = new RewardedAd(admobrewardedvideoid);
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardbasedad.OnUserEarnedReward += HandleUserEarnedReward;
        this.requestrewardbasedvideo ();
	}
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (advalue == 1)
        {
            GameObject.FindObjectOfType<MainMenuManager>().GiveCoins(100);

        }else if (advalue == 2)
        {
            GameObject.FindObjectOfType<GameManager>().Skiplevelvalue();
        }
        else if(advalue == 3)
        {
            GameObject.FindObjectOfType<GameManager>().GivetHints(1);
        }
        else if (advalue == 4)
        {
            GameObject.FindObjectOfType<GameManager>().GiveFreezetime(1);
        }
        else if (advalue == 5)
        {
            GameObject.FindObjectOfType<leveltimer>().ReviveButton();
        }


        this.requestrewardbasedvideo();

    }
    private void requestrewardbasedvideo(){
		//	AdRequest request = new AdRequest.Builder ().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build ();
		//	AdRequest request = new AdRequest.Builder().AddExtra("max_ad_content_rating", agefilter).Build();
		AdRequest request = new AdRequest.Builder().Build();
        this.rewardbasedad.LoadAd(request);
    }
	//show rewarded video ad

	public void showad(int n){
        advalue = n;

        if (rewardbasedad.IsLoaded()){
            

            rewardbasedad.Show ();
		}
	}
	void Update(){
		//loading = Resources.FindObjectsOfTypeAll (typeof(GameObject));
		m_scene = SceneManager.GetActiveScene ();
		//if(m_scene.name=="Main"){
			removeadvalue = PlayerPrefs.GetInt ("removeads",0);
			if(bannerview!=null){
			//	bannerview.SetPosition (AdPosition.Top);
			//	showBanner ();
            }
            else
            {
              //  requestbanner();
            }
	//	}
			
	}
	public void requestbanner(){
		removeadvalue = PlayerPrefs.GetInt ("removeads",0);

			if (bannerview == null) {
				agefilter = PlayerPrefs.GetString ("agecheck");
			    bannerview = new BannerView (bannerid, AdSize.SmartBanner, AdPosition.Top);
			//	bannerrequest = new AdRequest.Builder ().AddTestDevice ("2077ef9a63d2b398840261c8221a0c9b").Build ();
			//		bannerrequest = new AdRequest.Builder().AddExtra("max_ad_content_rating", agefilter).Build();
					bannerrequest = new AdRequest.Builder ().Build ();
				bannerview.LoadAd (bannerrequest);
			} else {
				showBanner ();
			}


	}

	//hide banner ads

	public void requestintersitial(){


			agefilter = PlayerPrefs.GetString ("agecheck");
			interstitial = new InterstitialAd (intersitialid);
		//	bannerrequest = new AdRequest.Builder ().AddTestDevice ("2077ef9a63d2b398840261c8221a0c9b").Build ();
			//bannerrequest =new AdRequest.Builder().AddExtra("max_ad_content_rating", agefilter).Build();
			bannerrequest =new AdRequest.Builder().Build();
			interstitial.LoadAd (bannerrequest);
		}




	public void showinterstialad(){

			interstitial.Show ();

	}
//	public void loadinterstitialad(){
//		interstitial = new InterstitialAd (intersitialid);
//		interstialrequest=new AdRequest.Builder().AddExtra("max_ad_content_rating", agefilter).Build();
//		interstitial.LoadAd (interstialrequest);
//	}
	public void HideBanner(){
		if(bannerrequest!=null){
		      bannerview.Hide ();
		}
	}
	public void showBanner(){
		
			
				if (bannerrequest != null) {
					bannerview.Show ();
				} else {
					requestbanner ();
				}

		
	}
	public void OnDestroybanner(){
		if (bannerrequest != null) {
			bannerview.Destroy ();
		}
	}

}
