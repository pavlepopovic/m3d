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

    [Header("Dialog Boxes")]
    [UnityEngine.Serialization.FormerlySerializedAs("tutorial")]
    public GameObject Tutorial;
    [UnityEngine.Serialization.FormerlySerializedAs("pausedialogbox")]
    public GameObject PausedDialogBox;
    [UnityEngine.Serialization.FormerlySerializedAs("levelcompletebox")]
    public GameObject LevelCompleteBox;
    public GameObject LevelFailFullBoard;

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
    public Text MagnetsTextValue;
    [UnityEngine.Serialization.FormerlySerializedAs("freezedelayvalue")]
    public int FreezeDelayValue;

    public GameObject HintAddButton;
    public GameObject TimeFreezeAddButton;
    [UnityEngine.Serialization.FormerlySerializedAs("RewardTimeFreezeDialogbox")]
    public GameObject RewardTimeFreezeDialogBox;

    #endregion

    private int m_GamePlayLevelValue;
    private int m_TotalObjects;

    internal void FailLevelDueToFullBoard()
    {
        FindObjectOfType<LevelTimer>().FreezeTimeBool = true;
        LevelFailFullBoard.SetActive(true);
    }

    public void CleanBoard()
    {
        // todo: implement
        Debug.Log("Clean board button pressed! Implement me!");
    }

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
        m_GamePlayLevelValue = PrefManager.GetLevelsValue();
        if (m_GamePlayLevelValue == 1)
        {
            Tutorial.gameObject.SetActive(true);
        }
        if (m_GamePlayLevelValue >= 3)
        {
            HintLocked.SetActive(false);
            HintUnlocked.SetActive(true);
        }
        if (m_GamePlayLevelValue >= 5)
        {
            TimeFreezeUnlock.SetActive(true);
            TimeFreezeLocked.SetActive(false);
        }

        CreateLevel();        
    }

    void CreateLevel()
    {
        m_TotalObjects = Levels.LevelData[m_GamePlayLevelValue - 1].TotalObjects.Length;
        LevelValueText.text = "LV." + m_GamePlayLevelValue.ToString();
        LevelFailTimerText.text = Levels.LevelData[m_GamePlayLevelValue - 1].minute + ":" + Levels.LevelData[m_GamePlayLevelValue - 1].seconds+ "Min";
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < Levels.LevelData[m_GamePlayLevelValue - 1].TotalObjects.Length; i++)
            {
                GameObject g = Instantiate(Levels.LevelData[m_GamePlayLevelValue - 1].TotalObjects[i].gameObject, gameObject.transform);
                g.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.2f, 1.5f), Random.Range(-2.0f, 1.0f));
            }
        }
    }

    void Update()
    {
        FreezeTextValue.text = "" + PrefManager.GetFreezeValue();
        MagnetsTextValue.text = "" + PrefManager.GetMagnetsValue();

        if (PrefManager.GetFreezeValue() <= 0)
        {
            TimeFreezeAddButton.SetActive(true);
        }
        else
        {
            TimeFreezeAddButton.SetActive(false);
        }
        if (PrefManager.GetMagnetsValue() <= 0)
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

            TimerText.text = FindObjectOfType<LevelTimer>().SadaMinute + ":" + FindObjectOfType<LevelTimer>().SadaSeconds + " Min";
            m_GamePlayLevelValue++;
            PrefManager.SetLevelsValue(m_GamePlayLevelValue);
            PrefManager.IncrementNumUpgradeStars();

            SoundManager.instance.PlayStarCollectSound();

            // Setting Reward Coin Value
            int rewardCoinValue = m_TotalObjects * 10;
            RewardCoinValue.text = "Earned " + rewardCoinValue + " Coins";
            int coinvalue = PrefManager.GetCoinsValue();
            coinvalue += rewardCoinValue;
            PrefManager.SetCoinsValue(coinvalue);

            ScoreText.text = m_TotalObjects.ToString();
            LevelCompleteBox.SetActive(true);

            int showRateUsDialogueValue = PrefManager.GetCurrentRateUsValue();
            if (showRateUsDialogueValue == ShowRateUsAfterlevels)
            {
                ShowRateUsDialogBox.SetActive(true);
                showRateUsDialogueValue = 0;
                PrefManager.SetCurrentRateUsValue(showRateUsDialogueValue);
            }
            else
            {             
                showRateUsDialogueValue++;
                PrefManager.SetCurrentRateUsValue(showRateUsDialogueValue);
            }
        }
    }

    public void SkipLevelValue()
    {
        m_GamePlayLevelValue= PrefManager.GetLevelsValue();
        m_GamePlayLevelValue++;
        PrefManager.SetLevelsValue(m_GamePlayLevelValue);
        SceneManager.LoadScene("gameplay");
    }

    public void OnMagnetClick()
    {
        int numMagnets = PrefManager.GetMagnetsValue();

        if (numMagnets > 0)
        {
            if (MatchBoard.s_Instance.SafeToAddNewItemToBoard)
            {
                StartCoroutine(MatchBoard.s_Instance.ProcessMagnet());

                // Magnets can be changed inside ProcessMagnet(),
                // hence we are not decrementing them, but rather reading them from
                // a persistent storage
                PrefManager.SetMagnetsValue(PrefManager.GetMagnetsValue());
                MagnetsTextValue.text = numMagnets.ToString();
            }
        }
    }

    void MakeTimeScaleZero()
    {
        Time.timeScale = 0f;
    }

    public void OnPauseClick()
    {
        SoundManager.instance.PlayButtonSound();
        Time.timeScale = 0f;
        PausedDialogBox.SetActive(true);
    }

    public void OnResumeClick()
    {
        SoundManager.instance.PlayButtonSound();
        Time.timeScale = 1f;
        PausedDialogBox.SetActive(false);
    }

    public void OnHomeClick()
    {
        SoundManager.instance.PlayButtonSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu");
    }

    public void OnNextClick()
    {
        SoundManager.instance.PlayButtonSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameplay");
    }

    public void OnRateUsClick()
    {
        SoundManager.instance.PlayButtonSound();
        Application.OpenURL("https://play.google.com/store/apps/details?id="+ Application.identifier);
    }

    public void OnRetryClick()
    {
        SoundManager.instance.PlayButtonSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("gameplay");
    }

    public void FreezeTime()
    {
        if (PrefManager.GetFreezeValue() > 0)
        {
            Freeze.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            int freezevalue = PrefManager.GetFreezeValue();
            freezevalue--;
            PrefManager.SetFreezeValue(freezevalue);
            SoundManager.instance.PlayButtonSound();
            Time.timeScale = 1f;
            FindObjectOfType<LevelTimer>().FreezeTimeBool = true;
            Freeze.SetActive(true);
        }
        else
        {
            RewardTimeFreezeDialogBox.SetActive(true);
        }
    }

    public void AddCoinHints()
    {
        int coinvalue = PrefManager.GetCoinsValue();
        if (coinvalue >= 1000)
        {
            coinvalue = coinvalue - 1000;
            PrefManager.SetCoinsValue(coinvalue);
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
        int hintsValue = PrefManager.GetMagnetsValue();
        hintsValue += numHintsToAdd;
        PrefManager.SetMagnetsValue(hintsValue);
    }

    public void AddCoinTimeFreeze()
    {
        int coinvalue = PrefManager.GetCoinsValue();
        if (coinvalue >= 1000)
        {
            coinvalue = coinvalue - 1000;
            PrefManager.SetCoinsValue(coinvalue);
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
        int freezeValue = PrefManager.GetFreezeValue();
        freezeValue += numFreezesToAdd;
        PrefManager.SetFreezeValue(freezeValue);
    }

    void DisableMessageDialogBox()
    {
        PopUpDialogBox.SetActive(false);
    }

    public void RateUsNow()
    {
        int coinValue = PrefManager.GetCoinsValue();
        coinValue += 300;
        PrefManager.SetCoinsValue(coinValue);

        SoundManager.instance.PlayButtonSound();
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
}
