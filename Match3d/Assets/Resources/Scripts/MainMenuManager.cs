using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Loading;

    [UnityEngine.Serialization.FormerlySerializedAs("privacypolicy")]
    public string PrivacyPolicy;

    [UnityEngine.Serialization.FormerlySerializedAs("Levelfillbar")]
    public Image LevelFillBar;

    // Level on screen
    [UnityEngine.Serialization.FormerlySerializedAs("levelValue")]
    public Text StageValue;
    [Header("Dialogs")]
    public GameObject Setting;
    public GameObject Shop;
    public GameObject Share;
    [UnityEngine.Serialization.FormerlySerializedAs("Dailygift")]
    public GameObject DailyGift;
    [UnityEngine.Serialization.FormerlySerializedAs("levelplay")]
    public GameObject LevelPlay;

    // level when pressing play, when boosters spawn
    // unused as of now, use when implementing boosters
    [UnityEngine.Serialization.FormerlySerializedAs("leveltext")]
    public Text LevelText;

    [Header("Upgrade stars")]
    public Text UpgradeStarsText;
    public Button UpgradeStarsButton;

    [Header("Coins And LifeValue")]
    [UnityEngine.Serialization.FormerlySerializedAs("cointext")]
    public Text CoinText;
    [UnityEngine.Serialization.FormerlySerializedAs("shopcointext")]
    public Text ShopCoinText;

    [Header("Setting icons")]
    [UnityEngine.Serialization.FormerlySerializedAs("onoff")]
    public Sprite[] OnOff;
    [UnityEngine.Serialization.FormerlySerializedAs("music")]
    public Image Music;
    [UnityEngine.Serialization.FormerlySerializedAs("sound")]
    public Image Sound;
    [UnityEngine.Serialization.FormerlySerializedAs("vibration")]
    public Image Vibration;

    void Start()
    {
        LevelText.text = "Level " + PrefManager.GetLevelsValue(); // The level value which pops up if boosters are to be selected (not implemented)
        SettingsDialog();
        ResolveGameState();
    }
    
    void Update()
    {
        CoinText.text = PrefManager.GetCoinsValue().ToString();
        ShopCoinText.text= PrefManager.GetCoinsValue().ToString();
    }

    // Supposed to bring up stuff like boosters - not implemented
    public void OnMainMenuPlayClick()
    {
        SoundManager.instance.PlayButtonSound();
        LevelPlay.gameObject.SetActive(true);
    }

    public void OnPlayClick()
    {
        Loading.SetActive(true);
        SoundManager.instance.PlayButtonSound();
        SceneManager.LoadScene("gameplay");
    }

    public void OnUpgradeStarClick()
    {
        UnityEngine.Assertions.Assert.IsTrue(PrefManager.CanDecrementUpgradeStars());
        PrefManager.DecrementUpgradeStarsAndIncrementStageProgress();
        ResolveGameState();
    }

    void ResolveGameState()
    {
        // stage and stage progress
        {
            if (PrefManager.CanAdvanceToNextStage())
            {
                PrefManager.AdvanceToNextStage();
            }

            float currentStageLevel = PrefManager.GetStageValue();
            StageValue.text = currentStageLevel.ToString();
            LevelFillBar.fillAmount = PrefManager.GetStageProgress() / 5.0f;
        }

        // stars
        {
            int numStars = PrefManager.GetNumUpgradeStars();
            UpgradeStarsText.text = numStars.ToString();
            UpgradeStarsButton.interactable = numStars > 0;
        }
    }

    void SettingsDialog()
    {
        Music.sprite = OnOff[PrefManager.GetMusicValue()];
        if (PrefManager.GetMusicValue() == 1)
        {
            Music.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Music.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        Sound.sprite = OnOff[PrefManager.GetSoundsValue()];
        if (PrefManager.GetMusicValue() == 1)
        {
            Sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        Vibration.sprite = OnOff[PrefManager.GetVibrationsValue()];
        if (PrefManager.GetMusicValue() == 1)
        {
            Sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
    }

    public void OnSettingsDialogClick()
    {
        SoundManager.instance.PlayButtonSound();
        Setting.gameObject.SetActive(true);
    }

    public void OnSettingsDialogCloseClick()
    {
        SoundManager.instance.PlayButtonSound();
        Setting.gameObject.SetActive(false);
    }

    public void OnShopClick()
    {
        SoundManager.instance.PlayButtonSound();
        Shop.gameObject.SetActive(true);
    }

    public void OnShareClick()
    {
        SoundManager.instance.PlayButtonSound();
        Share.gameObject.SetActive(true);
    }

    public void OnShareCloseClick()
    {
        SoundManager.instance.PlayButtonSound();
        Share.gameObject.SetActive(false);
    }

    public void OnHomeClick()
    {
        SoundManager.instance.PlayButtonSound();
        Shop.gameObject.SetActive(false);
    }

    public void OnMusicClick()
    {
        if (PrefManager.GetMusicValue() == 1)
        {
            PrefManager.SetMusicValue(0);
            Music.sprite = OnOff[PrefManager.GetMusicValue()];
            Music.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            PrefManager.SetMusicValue(1);
            Music.sprite = OnOff[PrefManager.GetMusicValue()];
            Music.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        SoundManager.instance.SetMusicSource();
    }

    public void OnSoundClick()
    {
        if (PrefManager.GetSoundsValue() == 1)
        {
            PrefManager.SetSoundsValue(0);
            Sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            PrefManager.SetSoundsValue(1);
            Sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        Sound.sprite = OnOff[PrefManager.GetSoundsValue()];
        SoundManager.instance.SetSoundSource();
    }

    public void OnVibrationClick()
    {
        if (PrefManager.GetVibrationsValue() == 1)
        {
            PrefManager.SetVibrationValue(0);
            Vibration.sprite = OnOff[PrefManager.GetVibrationsValue()];
            Vibration.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            PrefManager.SetVibrationValue(1);
            Vibration.sprite = OnOff[PrefManager.GetVibrationsValue()];
            Vibration.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
    }

    public void OnPrivacyClick()
    {
        Application.OpenURL(PrivacyPolicy);
    }

    public void RateUS()
    {
        Application.OpenURL(Application.identifier);
    }

    public void GiveCoins(int value)
    {
        int coinvalue= PrefManager.GetCoinsValue();
        coinvalue += value;
        PrefManager.SetCoinsValue(coinvalue);

        CoinText.text = PrefManager.GetCoinsValue().ToString();
    }
}
