using UnityEngine;
using GoogleMobileAds.Api;

public class admobads : MonoBehaviour 
{	
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

    int advalue;

	void Start()
	{
        MobileAds.Initialize(initStatus => { });
			requestintersitial();
        rewardbasedad = new RewardedAd(admobrewardedvideoid);
        // Called when the user should be rewarded for interacting with the ad.
        rewardbasedad.OnUserEarnedReward += HandleUserEarnedReward;
        requestrewardbasedvideo();
	}

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (advalue == 1)
        {
			UnityEngine.Assertions.Assert.IsFalse(true, "Unexpected add: 1");
        }
		else if (advalue == 2)
        {
			UnityEngine.Assertions.Assert.IsFalse(true, "Unexpected add: 2");
        }
        else if(advalue == 3)
        {
			UnityEngine.Assertions.Assert.IsFalse(true, "Unexpected add: 3");
        }
        else if (advalue == 4)
        {
			UnityEngine.Assertions.Assert.IsFalse(true, "Unexpected add: 4");
		}
		else if (advalue == 5)
        {
			UnityEngine.Assertions.Assert.IsFalse(true, "Unexpected add: 5");
        }

        requestrewardbasedvideo();
    }
    private void requestrewardbasedvideo()
	{
		AdRequest request = new AdRequest.Builder().Build();
        rewardbasedad.LoadAd(request);
    }

	public void showad(int n)
	{
        advalue = n;

        if (rewardbasedad.IsLoaded())
		{
            rewardbasedad.Show();
		}
	}

	public void requestbanner()
	{
		if (bannerview == null)
		{
			bannerview = new BannerView (bannerid, AdSize.SmartBanner, AdPosition.Top);
				bannerrequest = new AdRequest.Builder ().Build();
			bannerview.LoadAd (bannerrequest);
		}
		else
		{
			showBanner();
		}
	}

	public void requestintersitial()
	{
		interstitial = new InterstitialAd (intersitialid);
		bannerrequest =new AdRequest.Builder().Build();
		interstitial.LoadAd (bannerrequest);
	}

	public void showinterstialad()
	{
		interstitial.Show();
	}

	public void HideBanner()
	{
		if(bannerrequest!=null)
		{
		   bannerview.Hide();
		}
	}
	public void showBanner()
	{			
		if (bannerrequest != null)
		{
			bannerview.Show();
		} else
		{
			requestbanner();
		}
	}
	public void OnDestroybanner()
	{
		if (bannerrequest != null)
		{
			bannerview.Destroy();
		}
	}
}
