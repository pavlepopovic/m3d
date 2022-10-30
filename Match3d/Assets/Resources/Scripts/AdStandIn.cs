using UnityEngine;

public class AdStandIn : MonoBehaviour
{
    public void StandInRewardedAd(int n)
    {
        switch (n)
        {
            case 1:
                FindObjectOfType<MainMenuManager>().GiveCoins(100);
                break;
            case 2:
                FindObjectOfType<GameManager>().SkipLevelValue();
                break;
            case 3:
                FindObjectOfType<GameManager>().GiveHints(1);
                break;
            case 4:
                FindObjectOfType<GameManager>().GiveFreezeTime(1);
                break;
            case 5:
                FindObjectOfType<leveltimer>().ReviveButton();
                break;
            default:
                UnityEngine.Assertions.Assert.IsTrue(false);
                break;
        }
    }
}

