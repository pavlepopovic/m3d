using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefmanager : MonoBehaviour
{
    public static prefmanager instance = null;

    int hintvalue, addcubevalue, unlockedlevelvalue, coinsvalue, heartvalue;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //for setting and getting hint values
    public int Gethintvalue()
    {
        return PlayerPrefs.GetInt("hintpref", 5);
    }
    public void SetHintValue(int value)
    {
        PlayerPrefs.SetInt("hintpref", value);
    }





    //for getting and Setting extra cubes

    public int Gettubevalue()
    {
        return PlayerPrefs.GetInt("tubepref", 5);
    }
    public void SettubeValue(int value)
    {
        PlayerPrefs.SetInt("tubepref", value);
    }





    //for getting and setting coinsvalue
    public int Getcoinsvalue()
    {
        return PlayerPrefs.GetInt("coinsvalue", 0);
    }
    public void SetcoinsValue(int value)
    {
        PlayerPrefs.SetInt("coinsvalue", value);
    }





    //unlock level value container
    public int Getlevelsvalue()
    {
        return PlayerPrefs.GetInt("level", 1);
    }
    public void Setlevelsvalue(int value)
    {
        PlayerPrefs.SetInt("level", value);
    }

    //music pref value

    public int Getmusicsvalue()
    {
        return PlayerPrefs.GetInt("musicvalue", 1);
    }
    public void Setmusicsvalue(int value)
    {
        PlayerPrefs.SetInt("musicvalue", value);
    }

    //sound pref value

    public int Getsoundsvalue()
    {
        return PlayerPrefs.GetInt("soundvalue", 1);
    }
    public void Setsoundsvalue(int value)
    {
        PlayerPrefs.SetInt("soundvalue", value);
    }

    //vibration pref value

    public int Getvibrationsvalue()
    {
        return PlayerPrefs.GetInt("vibrationvalue", 1);
    }
    public void Setvibrationvalue(int value)
    {
        PlayerPrefs.SetInt("vibrationvalue", value);
    }





    //Freeze pref value

    public int Getfreezevalue()
    {
        return PlayerPrefs.GetInt("freezevalue", 5);
    }
    public void Setfreezevalue(int value)
    {
        PlayerPrefs.SetInt("freezevalue", value);
    }



    //rate us pref value

    public int GetCurrentrateusvalue()
    {
        int value = PlayerPrefs.GetInt("rateusvalue", 0);
        return value;
    }
    public void SetCurrentrateusvalue(int n)
    {
        PlayerPrefs.SetInt("rateusvalue", n);       
    }







}
