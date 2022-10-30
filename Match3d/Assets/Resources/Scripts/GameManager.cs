using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance = null;

    public int ShowRateUsAfterlevels;

    [Header("Level Editor")]
    [UnityEngine.Serialization.FormerlySerializedAs("levels")]
    public leveleditor levels;
    [UnityEngine.Serialization.FormerlySerializedAs("levelValuetxt")]
    public Text levelValuetxt;
    [Header("Spawnpoint")]
    [UnityEngine.Serialization.FormerlySerializedAs("Spawnpoint")]
    public GameObject Spawnpoint;
    int levelvalue,totalobjects;
    [UnityEngine.Serialization.FormerlySerializedAs("holdercube")]
    public GameObject holdercube;

    [Header("dialogeboxes")]
    [UnityEngine.Serialization.FormerlySerializedAs("tutorial")]
    public GameObject tutorial;
    [UnityEngine.Serialization.FormerlySerializedAs("pausedialogbox")]
    public GameObject pausedialogbox;
    [UnityEngine.Serialization.FormerlySerializedAs("levelcompletebox")]
    public GameObject levelcompletebox;
    [UnityEngine.Serialization.FormerlySerializedAs("levelfailbox")]
    public GameObject levelfailbox;
    public Text ScoreText;
    public Text TimerText;
    [UnityEngine.Serialization.FormerlySerializedAs("levelfailtimertext")]
    public Text levelfailtimertext;
    public Text RewardCoinValue;
    [UnityEngine.Serialization.FormerlySerializedAs("ShowRateUsDialogbox")]
    public GameObject ShowRateUsDialogbox;
    [UnityEngine.Serialization.FormerlySerializedAs("Popupdialogbox")]
    public GameObject Popupdialogbox;

    [Header("FreezeAndHintImageAndText")]
    public GameObject HintLocked;
    public GameObject HintUnlocked;
    public GameObject TimeFreezeUnlock;
    public GameObject TimeFreezeLocked;
    [UnityEngine.Serialization.FormerlySerializedAs("freeze")]
    public GameObject freeze;
    [UnityEngine.Serialization.FormerlySerializedAs("freezetextvalue")]
    public Text freezetextvalue;
    [UnityEngine.Serialization.FormerlySerializedAs("hinttextvalue")]
    public Text hinttextvalue;
    [UnityEngine.Serialization.FormerlySerializedAs("freezedelayvalue")]
    public int freezedelayvalue;

    public GameObject HintAddButton;
    public GameObject TimeFreezeAddButton;
    public GameObject RewardTimeFreezeDialogbox;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelvalue = prefmanager.instance.Getlevelsvalue();
        if (levelvalue == 1)
        {
            tutorial.gameObject.SetActive(true);
        }
        if (levelvalue >= 3)
        {
            HintLocked.SetActive(false);
            HintUnlocked.SetActive(true);
        }
        if (levelvalue >= 5)
        {
            TimeFreezeUnlock.SetActive(true);
            TimeFreezeLocked.SetActive(false);
        }

        CreateLevel();
        
        Invoke("Makecubediable", 0.5f);
    }

    void Makecubediable()
    {
        holdercube.gameObject.SetActive(false);
    }

    void CreateLevel()
    {
        totalobjects = levels.LevelData[levelvalue-1].TotalObjects.Length;
        levelValuetxt.text = "LV." + levelvalue.ToString();
        levelfailtimertext.text = levels.LevelData[levelvalue - 1].minute+ ":" + levels.LevelData[levelvalue - 1].seconds+"Min";
        SpawnObjects();
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for(int i=0;i< levels.LevelData[levelvalue-1].TotalObjects.Length; i++)
        {
            GameObject g=Instantiate(levels.LevelData[levelvalue-1].TotalObjects[i].gameObject,Spawnpoint.transform);
            g.transform.position = new Vector3(g.transform.position.x + Random.Range(-1, 1), g.transform.position.y, g.transform.position.z + Random.Range(0, -1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        freezetextvalue.text =""+ prefmanager.instance.Getfreezevalue();
        hinttextvalue.text = "" + prefmanager.instance.Gethintvalue();

        if (prefmanager.instance.Getfreezevalue() <= 0)
        {
            TimeFreezeAddButton.SetActive(true);
        }
        else
        {
            TimeFreezeAddButton.SetActive(false);
        }
        if (prefmanager.instance.Gethintvalue() <= 0)
        {
            HintAddButton.SetActive(true);
        }
        else
        {
            HintAddButton.SetActive(false);
        }
    }

    public void checklevelcomplete(int collectedobjectvalue)
    {
        if (collectedobjectvalue == totalobjects)
        {
            Invoke("maketimescalezero", 1f);

            TimerText.text = FindObjectOfType<leveltimer>().SadaMinute + ":" + FindObjectOfType<leveltimer>().SadaSeconds + " Min";
            levelvalue++;
            prefmanager.instance.Setlevelsvalue(levelvalue);

            SoundManager.instance.PlaystarcollectSOund();

            //Setting Reward Coin Value
            int rewardcoinvalue = totalobjects * 10;
            RewardCoinValue.text = "Earned " + rewardcoinvalue + " Coins";
            int coinvalue = prefmanager.instance.Getcoinsvalue();
            coinvalue += rewardcoinvalue;
            prefmanager.instance.SetcoinsValue(coinvalue);

            ScoreText.text = totalobjects.ToString();

            levelcompletebox.SetActive(true);

            int Showrateusdialogvalue = prefmanager.instance.GetCurrentrateusvalue();
            if (Showrateusdialogvalue == ShowRateUsAfterlevels)
            {
                ShowRateUsDialogbox.SetActive(true);
                Showrateusdialogvalue = 0;
                prefmanager.instance.SetCurrentrateusvalue(Showrateusdialogvalue);
            }
            else
            {             
                Showrateusdialogvalue++;
                prefmanager.instance.SetCurrentrateusvalue(Showrateusdialogvalue);
            }
        }
    }

    public void Skiplevelvalue()
    {
        levelvalue= prefmanager.instance.Getlevelsvalue();
        levelvalue++;
        prefmanager.instance.Setlevelsvalue(levelvalue);
        SceneManager.LoadScene("gameplay");
    }

    void maketimescalezero()
    {
        Time.timeScale = 0f;
    }

    public void Onpauseclick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 0f;
        pausedialogbox.SetActive(true);
    }

    public void Onresumeclick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        pausedialogbox.SetActive(false);
    }

    public void OnHomeclick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu");
    }

    public void OnNextclick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameplay");
    }

    public void OnRateUsclick()
    {
        SoundManager.instance.PlayButtonSOund();
        Application.OpenURL("https://play.google.com/store/apps/details?id="+ Application.identifier);
    }

    public void Onretryclick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameplay");
    }

    public void freezetime()
    {
        if (prefmanager.instance.Getfreezevalue() > 0)
        {
            freeze.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            int freezevalue = prefmanager.instance.Getfreezevalue();
            freezevalue--;
            prefmanager.instance.Setfreezevalue(freezevalue);
            SoundManager.instance.PlayButtonSOund();
            Time.timeScale = 1f;
            FindObjectOfType<leveltimer>().FreezeTimeBool = true;
            freeze.SetActive(true);
        }
        else
        {
            RewardTimeFreezeDialogbox.SetActive(true);
        }
    }

    public void AddCoinHints()
    {
        int coinvalue = prefmanager.instance.Getcoinsvalue();
        if(coinvalue >= 1000)
        {
            coinvalue = coinvalue - 1000;
            prefmanager.instance.SetcoinsValue(coinvalue);
            GivetHints(3);
            Popupdialogbox.SetActive(true);
            Popupdialogbox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You Got 3 Hints";
            Invoke("DisaableMessageDialogBox", 1f);
        }
        else
        {
            Popupdialogbox.SetActive(true);
            Popupdialogbox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Not Enough Coins";
            Invoke("DisaableMessageDialogBox", 1f);
        }
    }

    public void GivetHints(int hintvalue)
    {
        int hintsvalue = prefmanager.instance.Gethintvalue();
        hintsvalue += hintvalue;
        prefmanager.instance.SetHintValue(hintsvalue);
    }

    public void AddCoinTimeFreeze()
    {
        int coinvalue = prefmanager.instance.Getcoinsvalue();
        if (coinvalue >= 1000)
        {
            coinvalue = coinvalue - 1000;
            prefmanager.instance.SetcoinsValue(coinvalue);
            GiveFreezetime(3);
            Popupdialogbox.SetActive(true);
            Popupdialogbox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You Got 3 Time Freezes";
            Invoke("DisaableMessageDialogBox", 1f);
        }
        else
        {
            Popupdialogbox.SetActive(true);
            Popupdialogbox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Not Enough Coins";
            Invoke("DisaableMessageDialogBox", 1f);
        }
    }

    public void GiveFreezetime(int freezeassigningvalue)
    {
        int freezevalue = prefmanager.instance.Getfreezevalue();
        freezevalue += freezeassigningvalue;
        prefmanager.instance.Setfreezevalue(freezevalue);
    }

    void DisaableMessageDialogBox()
    {
        Popupdialogbox.SetActive(false);
    }

    public void RateUsNow()
    {
        int coinvalue = prefmanager.instance.Getcoinsvalue();
        coinvalue += 300;
        prefmanager.instance.SetcoinsValue(coinvalue);

        SoundManager.instance.PlayButtonSOund();
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
}
