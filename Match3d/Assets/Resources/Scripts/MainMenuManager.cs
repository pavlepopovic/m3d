using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Loading;

    public string privacypolicy;

    public Image Levelfillbar;
    public Text levelValue;
    [Header("Dialogs")]
    public GameObject Setting;
    public GameObject Shop;
    public GameObject Share;
    public GameObject Dailygift;
    public GameObject levelplay;
    public Text leveltext;

    [Header("Coins And LifeValue")]
    public Text cointext;
    public Text shopcointext;

    [Header("Setting icons")]
    public Sprite[] onoff;
    public Image music, sound, vibration;

    int levelvalue;

    void Start()
    {
        levelvalue = prefmanager.instance.Getlevelsvalue();
        leveltext.text = "Level " + levelvalue;
        prefmanager.instance.Setlevelsvalue(levelvalue);
        SettingsDialog();
        SetLevelProgress();
    }
    
    void Update()
    {
        cointext.text = prefmanager.instance.Getcoinsvalue().ToString();
        shopcointext.text= prefmanager.instance.Getcoinsvalue().ToString();
    }

    // Supposed to bring up stuff like boosters - not implemented
    public void OnMainMenuPlayClick()
    {
        SoundManager.instance.PlayButtonSOund();
        levelplay.gameObject.SetActive(true);
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
        levelValue.text = currentlevelvalue.ToString();
        var  fillvalue=currentlevelvalue / 100f;
        Levelfillbar.fillAmount = fillvalue;
    }

    void SettingsDialog()
    {
        music.sprite = onoff[prefmanager.instance.Getmusicsvalue()];
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            music.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            music.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        sound.sprite = onoff[prefmanager.instance.Getsoundsvalue()];
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        vibration.sprite = onoff[prefmanager.instance.Getvibrationsvalue()];
        if (prefmanager.instance.Getmusicsvalue() == 1)
        {
            sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
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
            music.sprite = onoff[prefmanager.instance.Getmusicsvalue()];
            music.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            prefmanager.instance.Setmusicsvalue(1);
            music.sprite = onoff[prefmanager.instance.Getmusicsvalue()];
            music.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        SoundManager.instance.SetMusicSource();
    }

    public void OnSoundClick()
    {
        if (prefmanager.instance.Getsoundsvalue() == 1)
        {
            prefmanager.instance.Setsoundsvalue(0);
            sound.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            prefmanager.instance.Setsoundsvalue(1);
            sound.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
        sound.sprite = onoff[prefmanager.instance.Getsoundsvalue()];
        SoundManager.instance.SetSoundSource();
    }

    public void OnVibrationClick()
    {
        if (prefmanager.instance.Getvibrationsvalue() == 1)
        {
            prefmanager.instance.Setvibrationvalue(0);
            vibration.sprite = onoff[prefmanager.instance.Getvibrationsvalue()];
            vibration.transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            prefmanager.instance.Setvibrationvalue(1);
            vibration.sprite = onoff[prefmanager.instance.Getvibrationsvalue()];
            vibration.transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
    }

    public void OnPrivacyclick()
    {
        Application.OpenURL(privacypolicy);
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

        cointext.text = prefmanager.instance.Getcoinsvalue().ToString();
    }
}
