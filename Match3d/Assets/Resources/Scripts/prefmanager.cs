using UnityEngine;

public class PrefManager : MonoBehaviour
{
    public static PrefManager s_Instance = null;

    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // For setting and getting hint values
    public int GetHintValue()
    {
        return PlayerPrefs.GetInt("hintpref", 5);
    }

    public void SetHintValue(int value)
    {
        PlayerPrefs.SetInt("hintpref", value);
    }

    // for getting and setting coins value
    public int GetCoinsValue()
    {
        return PlayerPrefs.GetInt("coinsvalue", 0);
    }

    public void SetCoinsValue(int value)
    {
        PlayerPrefs.SetInt("coinsvalue", value);
    }

    // Unlock level value container
    public int GetLevelsValue()
    {
        return PlayerPrefs.GetInt("level", 1);
    }

    public void SetLevelsValue(int value)
    {
        PlayerPrefs.SetInt("level", value);
    }

    // Music pref value
    public int GetMusicValue()
    {
        return PlayerPrefs.GetInt("musicvalue", 1);
    }

    public void SetMusicValue(int value)
    {
        PlayerPrefs.SetInt("musicvalue", value);
    }

    // Sound pref value
    public int GetSoundsValue()
    {
        return PlayerPrefs.GetInt("soundvalue", 1);
    }

    public void SetSoundsValue(int value)
    {
        PlayerPrefs.SetInt("soundvalue", value);
    }

    // Vibration pref value
    public int GetVibrationsValue()
    {
        return PlayerPrefs.GetInt("vibrationvalue", 1);
    }

    public void SetVibrationValue(int value)
    {
        PlayerPrefs.SetInt("vibrationvalue", value);
    }

    // Freeze pref value
    public int GetFreezeValue()
    {
        return PlayerPrefs.GetInt("freezevalue", 5);
    }

    public void SetFreezeValue(int value)
    {
        PlayerPrefs.SetInt("freezevalue", value);
    }

    // Rate us pref value
    public int GetCurrentRateUsValue()
    {
        int value = PlayerPrefs.GetInt("rateusvalue", 0);
        return value;
    }

    public void SetCurrentRateUsValue(int value)
    {
        PlayerPrefs.SetInt("rateusvalue", value);       
    }
}
