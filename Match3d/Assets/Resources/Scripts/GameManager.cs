using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance = null;

    public int ShowRateUsAfterlevels;

    #region Authoring

    [Header("Level Editor")]
    [UnityEngine.Serialization.FormerlySerializedAs("levels")]
    public leveleditor Levels;
    [UnityEngine.Serialization.FormerlySerializedAs("levelValuetxt")]
    public Text LevelValueText;
    [Header("Spawn Point")]
    [UnityEngine.Serialization.FormerlySerializedAs("Spawnpoint")]
    public GameObject SpawnPoint;
    [UnityEngine.Serialization.FormerlySerializedAs("holdercube")]
    public GameObject HolderCube;

    [Header("dialogeboxes")]
    [UnityEngine.Serialization.FormerlySerializedAs("tutorial")]
    public GameObject Tutorial;
    [UnityEngine.Serialization.FormerlySerializedAs("pausedialogbox")]
    public GameObject PausedDialogBox;
    [UnityEngine.Serialization.FormerlySerializedAs("levelcompletebox")]
    public GameObject LevelCompleteBox;
    [UnityEngine.Serialization.FormerlySerializedAs("levelfailbox")]
    public GameObject LevelFailBox;
    public Text ScoreText;
    public Text TimerText;
    [UnityEngine.Serialization.FormerlySerializedAs("levelfailtimertext")]
    public Text LevelFailTimerText;
    public Text RewardCoinValue;
    [UnityEngine.Serialization.FormerlySerializedAs("ShowRateUsDialogbox")]
    public GameObject ShowRateUsDialogBox;
    [UnityEngine.Serialization.FormerlySerializedAs("Popupdialogbox")]
    public GameObject PopUpDialogBox;

    [Header("Freeze And Hint Image And Text")]
    public GameObject HintLocked;
    public GameObject HintUnlocked;
    public GameObject TimeFreezeUnlock;
    public GameObject TimeFreezeLocked;
    [UnityEngine.Serialization.FormerlySerializedAs("freeze")]
    public GameObject Freeze;
    [UnityEngine.Serialization.FormerlySerializedAs("freezetextvalue")]
    public Text FreezeTextValue;
    [UnityEngine.Serialization.FormerlySerializedAs("hinttextvalue")]
    public Text HintTextValue;
    [UnityEngine.Serialization.FormerlySerializedAs("freezedelayvalue")]
    public int FreezeDelayValue;

    public GameObject HintAddButton;
    public GameObject TimeFreezeAddButton;
    [UnityEngine.Serialization.FormerlySerializedAs("RewardTimeFreezeDialogbox")]
    public GameObject RewardTimeFreezeDialogBox;

    #endregion

    private int m_LevelValue;
    private int m_TotalObjects;

    private void Awake()
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

    void Start()
    {
        m_LevelValue = prefmanager.instance.Getlevelsvalue();
        if (m_LevelValue == 1)
        {
            Tutorial.gameObject.SetActive(true);
        }
        if (m_LevelValue >= 3)
        {
            HintLocked.SetActive(false);
            HintUnlocked.SetActive(true);
        }
        if (m_LevelValue >= 5)
        {
            TimeFreezeUnlock.SetActive(true);
            TimeFreezeLocked.SetActive(false);
        }

        CreateLevel();
        
        Invoke("MakeCubeInactive", 0.5f);
    }

    void MakeCubeInactive()
    {
        HolderCube.gameObject.SetActive(false);
    }

    void CreateLevel()
    {
        m_TotalObjects = Levels.LevelData[m_LevelValue - 1].TotalObjects.Length;
        LevelValueText.text = "LV." + m_LevelValue.ToString();
        LevelFailTimerText.text = Levels.LevelData[m_LevelValue - 1].minute + ":" + Levels.LevelData[m_LevelValue - 1].seconds+ "Min";
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < Levels.LevelData[m_LevelValue - 1].TotalObjects.Length; i++)
            {
                GameObject g = Instantiate(Levels.LevelData[m_LevelValue - 1].TotalObjects[i].gameObject, SpawnPoint.transform);
                g.transform.position = new Vector3(g.transform.position.x + Random.Range(-1, 1), g.transform.position.y, g.transform.position.z + Random.Range(0, -1));
            }
        }
    }

    void Update()
    {
        FreezeTextValue.text = "" + prefmanager.instance.Getfreezevalue();
        HintTextValue.text = "" + prefmanager.instance.Gethintvalue();

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

    public void CheckLevelComplete(int collectedObjectValue)
    {
        if (collectedObjectValue == m_TotalObjects)
        {
            Invoke("MakeTimeScaleZero", 1f);

            TimerText.text = FindObjectOfType<leveltimer>().SadaMinute + ":" + FindObjectOfType<leveltimer>().SadaSeconds + " Min";
            m_LevelValue++;
            prefmanager.instance.Setlevelsvalue(m_LevelValue);

            SoundManager.instance.PlaystarcollectSOund();

            // Setting Reward Coin Value
            int rewardCoinValue = m_TotalObjects * 10;
            RewardCoinValue.text = "Earned " + rewardCoinValue + " Coins";
            int coinvalue = prefmanager.instance.Getcoinsvalue();
            coinvalue += rewardCoinValue;
            prefmanager.instance.SetcoinsValue(coinvalue);

            ScoreText.text = m_TotalObjects.ToString();
            LevelCompleteBox.SetActive(true);

            int showRateUsDialogueValue = prefmanager.instance.GetCurrentrateusvalue();
            if (showRateUsDialogueValue == ShowRateUsAfterlevels)
            {
                ShowRateUsDialogBox.SetActive(true);
                showRateUsDialogueValue = 0;
                prefmanager.instance.SetCurrentrateusvalue(showRateUsDialogueValue);
            }
            else
            {             
                showRateUsDialogueValue++;
                prefmanager.instance.SetCurrentrateusvalue(showRateUsDialogueValue);
            }
        }
    }

    public void SkipLevelValue()
    {
        m_LevelValue= prefmanager.instance.Getlevelsvalue();
        m_LevelValue++;
        prefmanager.instance.Setlevelsvalue(m_LevelValue);
        SceneManager.LoadScene("gameplay");
    }

    void MakeTimeScaleZero()
    {
        Time.timeScale = 0f;
    }

    public void OnPauseClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 0f;
        PausedDialogBox.SetActive(true);
    }

    public void OnResumeClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        PausedDialogBox.SetActive(false);
    }

    public void OnHomeClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu");
    }

    public void OnNextClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameplay");
    }

    public void OnRateUsClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Application.OpenURL("https://play.google.com/store/apps/details?id="+ Application.identifier);
    }

    public void OnRetryClick()
    {
        SoundManager.instance.PlayButtonSOund();
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameplay");
    }

    public void FreezeTime()
    {
        if (prefmanager.instance.Getfreezevalue() > 0)
        {
            Freeze.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            int freezevalue = prefmanager.instance.Getfreezevalue();
            freezevalue--;
            prefmanager.instance.Setfreezevalue(freezevalue);
            SoundManager.instance.PlayButtonSOund();
            Time.timeScale = 1f;
            FindObjectOfType<leveltimer>().FreezeTimeBool = true;
            Freeze.SetActive(true);
        }
        else
        {
            RewardTimeFreezeDialogBox.SetActive(true);
        }
    }

    public void AddCoinHints()
    {
        int coinvalue = prefmanager.instance.Getcoinsvalue();
        if(coinvalue >= 1000)
        {
            coinvalue = coinvalue - 1000;
            prefmanager.instance.SetcoinsValue(coinvalue);
            GiveHints(3);
            PopUpDialogBox.SetActive(true);
            PopUpDialogBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You Got 3 Hints";
            Invoke("DisableMessageDialogBox", 1f);
        }
        else
        {
            PopUpDialogBox.SetActive(true);
            PopUpDialogBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Not Enough Coins";
            Invoke("DisableMessageDialogBox", 1f);
        }
    }

    public void GiveHints(int numHintsToAdd)
    {
        int hintsValue = prefmanager.instance.Gethintvalue();
        hintsValue += numHintsToAdd;
        prefmanager.instance.SetHintValue(hintsValue);
    }

    public void AddCoinTimeFreeze()
    {
        int coinvalue = prefmanager.instance.Getcoinsvalue();
        if (coinvalue >= 1000)
        {
            coinvalue = coinvalue - 1000;
            prefmanager.instance.SetcoinsValue(coinvalue);
            GiveFreezeTime(3);
            PopUpDialogBox.SetActive(true);
            PopUpDialogBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You Got 3 Time Freezes";
            Invoke("DisableMessageDialogBox", 1f);
        }
        else
        {
            PopUpDialogBox.SetActive(true);
            PopUpDialogBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Not Enough Coins";
            Invoke("DisableMessageDialogBox", 1f);
        }
    }

    public void GiveFreezeTime(int numFreezesToAdd)
    {
        int freezeValue = prefmanager.instance.Getfreezevalue();
        freezeValue += numFreezesToAdd;
        prefmanager.instance.Setfreezevalue(freezeValue);
    }

    void DisableMessageDialogBox()
    {
        PopUpDialogBox.SetActive(false);
    }

    public void RateUsNow()
    {
        int coinValue = prefmanager.instance.Getcoinsvalue();
        coinValue += 300;
        prefmanager.instance.SetcoinsValue(coinValue);

        SoundManager.instance.PlayButtonSOund();
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
}
