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
    [UnityEngine.Serialization.FormerlySerializedAs("levelValue")]
    public Text LevelValue;
    [Header("Dialogs")]
    public GameObject Setting;
    public GameObject Shop;
    public GameObject Share;
    [UnityEngine.Serialization.FormerlySerializedAs("Dailygift")]
    public GameObject DailyGift;
    [UnityEngine.Serialization.FormerlySerializedAs("levelplay")]
    public GameObject LevelPlay;
    [UnityEngine.Serialization.FormerlySerializedAs("leveltext")]
    public Text LevelText;

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

    private int m_LevelValue;

    void Start()
    {
        m_LevelValue = prefmanager.instance.Getlevelsvalue();
        LevelText.text = "Level " + m_LevelValue;
        prefmanager.instance.Setlevelsvalue(m_LevelValue);
        SettingsDialog();
        SetLevelProgress();
    }
    
    void Update()
    {
        CoinText.text = prefmanager.instance.Getcoinsvalue().ToString();
        ShopCoinText.text= prefmanager.instance.Getcoinsvalue().ToString();
    }

    // Supposed to bring up stuff like boosters - not implemented
    public void OnMainMenuPlayClick()
    {
        SoundManager.instance.PlayButtonSOund();
        LevelPlay.gameObject.SetActive(true);
    }

    public void OnPlayClick()
    {
        Loading.SetActive(true);
        SoundManager.instance.PlayButtonSOund();
        SceneManager.LoadScene("gameplay");
    }

    void SetLevelProgress()
    {
        float currentlevelvalue = prefmanager.instance.Getlevelsvalue();
        LevelValue.text = currentlevelvalue.ToString();
        var  fillvalue=currentlevelvalue / 100f;
        LevelFillBar.fillAmount = fillvalue;
    }

    void SettingsDialog()
    {
        Music.sprite = OnOff[prefmanager.instance.Getmusicsvalue()];
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            Music.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Music.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        Sound.sprite = OnOff[prefmanager.instance.Getsoundsvalue()];
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            Sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        Vibration.sprite = OnOff[prefmanager.instance.Getvibrationsvalue()];
        if (prefmanager.instance.Getmusicsvalue() == 1)
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
        SoundManager.instance.PlayButtonSOund();
        Setting.gameObject.SetActive(true);
    }

    public void OnSettingsDialogCloseClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Setting.gameObject.SetActive(false);
    }

    public void OnShopClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Shop.gameObject.SetActive(true);
    }

    public void OnShareClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Share.gameObject.SetActive(true);
    }

    public void OnShareCloseClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Share.gameObject.SetActive(false);
    }

    public void OnHomeClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Shop.gameObject.SetActive(false);
    }

    public void OnMusicClick()
    {
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            prefmanager.instance.Setmusicsvalue(0);
            Music.sprite = OnOff[prefmanager.instance.Getmusicsvalue()];
            Music.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            prefmanager.instance.Setmusicsvalue(1);
            Music.sprite = OnOff[prefmanager.instance.Getmusicsvalue()];
            Music.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        SoundManager.instance.SetMusicSource();
    }

    public void OnSoundClick()
    {
        if (prefmanager.instance.Getsoundsvalue() == 1)
        {
            prefmanager.instance.Setsoundsvalue(0);
            Sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            prefmanager.instance.Setsoundsvalue(1);
            Sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        Sound.sprite = OnOff[prefmanager.instance.Getsoundsvalue()];
        SoundManager.instance.SetSoundSource();
    }

    public void OnVibrationClick()
    {
        if (prefmanager.instance.Getvibrationsvalue() == 1)
        {
            prefmanager.instance.Setvibrationvalue(0);
            Vibration.sprite = OnOff[prefmanager.instance.Getvibrationsvalue()];
            Vibration.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            prefmanager.instance.Setvibrationvalue(1);
            Vibration.sprite = OnOff[prefmanager.instance.Getvibrationsvalue()];
            Vibration.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
    }

    public void OnPrivacyclick()
    {
        Application.OpenURL(PrivacyPolicy);
    }

    public void RateUS()
    {
        Application.OpenURL(Application.identifier);
    }

    public void GiveCoins(int value)
    {
        int coinvalue= prefmanager.instance.Getcoinsvalue();
        coinvalue += value;
        prefmanager.instance.SetcoinsValue(coinvalue);

        CoinText.text = prefmanager.instance.Getcoinsvalue().ToString();
    }
}
