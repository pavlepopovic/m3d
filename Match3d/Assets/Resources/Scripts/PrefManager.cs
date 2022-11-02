using UnityEngine;

public static class PrefManager
{
    public static void Reset()
    {
        SetLevelsValue(1);
        SetNumUpgradeStars(0);
        SetStageValue(1);
        SetHintValue(5);
        SetCoinsValue(1000);
        SetMusicValue(1);
        SetSoundsValue(1);
        SetVibrationValue(1);
        SetFreezeValue(5);
        SetCurrentRateUsValue(0);
        SetStageProgress(0);
        for (int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                SetStageObjectiveState(i, j, 0);
            }
        }
    }

    #region Getters

    public static int GetStageObjectiveState(int stage, int childIndex)
    {
        return PlayerPrefs.GetInt($"stageobjectivestate{stage}{childIndex}");
    }

    public static int GetNumUpgradeStars()
    {
        return PlayerPrefs.GetInt("upgradestars");
    }

    public static int GetStageValue()
    {
        return PlayerPrefs.GetInt("stage");
    }

    public static int GetStageProgress()
    {
        return PlayerPrefs.GetInt("stageprogress");
    }

    public static bool CanAdvanceToNextStage()
    {
        return GetStageProgress() == 5;
    }

    public static int GetHintValue()
    {
        return PlayerPrefs.GetInt("hintpref", 5);
    }

    public static int GetCoinsValue()
    {
        return PlayerPrefs.GetInt("coinsvalue", 0);
    }

    public static int GetLevelsValue()
    {
        return PlayerPrefs.GetInt("level", 1);
    }

    public static int GetMusicValue()
    {
        return PlayerPrefs.GetInt("musicvalue", 1);
    }

    public static int GetSoundsValue()
    {
        return PlayerPrefs.GetInt("soundvalue", 1);
    }

    public static int GetVibrationsValue()
    {
        return PlayerPrefs.GetInt("vibrationvalue", 1);
    }

    public static int GetFreezeValue()
    {
        return PlayerPrefs.GetInt("freezevalue", 5);
    }

    public static int GetCurrentRateUsValue()
    {
        int value = PlayerPrefs.GetInt("rateusvalue", 0);
        return value;
    }

    #endregion

    #region Setters

    public static void SetStageObjectiveState(int stage, int childIndex, int value)
    {
        PlayerPrefs.SetInt($"stageobjectivestate{stage}{childIndex}", value);
    }

    public static void SetNumUpgradeStars(int numStars)
    {
        PlayerPrefs.SetInt("upgradestars", numStars);
    }

    public static void SetStageValue(int stageValue)
    {
        PlayerPrefs.SetInt("stage", stageValue);
    }

    public static void SetHintValue(int value)
    {
        PlayerPrefs.SetInt("hintpref", value);
    }

    public static void SetCoinsValue(int value)
    {
        PlayerPrefs.SetInt("coinsvalue", value);
    }

    public static void SetLevelsValue(int value)
    {
        PlayerPrefs.SetInt("level", value);
    }

    public static void SetMusicValue(int value)
    {
        PlayerPrefs.SetInt("musicvalue", value);
    }

    public static void SetSoundsValue(int value)
    {
        PlayerPrefs.SetInt("soundvalue", value);
    }

    public static void SetVibrationValue(int value)
    {
        PlayerPrefs.SetInt("vibrationvalue", value);
    }

    public static void SetFreezeValue(int value)
    {
        PlayerPrefs.SetInt("freezevalue", value);
    }

    public static void SetCurrentRateUsValue(int value)
    {
        PlayerPrefs.SetInt("rateusvalue", value);
    }

    public static void SetStageProgress(int value)
    {
        PlayerPrefs.SetInt("stageprogress", value);
    }

    #endregion

    #region Incrementers

    public static void IncrementNumUpgradeStars()
    {
        int currentNumStars = PlayerPrefs.GetInt("upgradestars");
        PlayerPrefs.SetInt("upgradestars", currentNumStars + 1);
    }

    public static void IncrementStage()
    {
        int currentStage = GetStageValue();
        PlayerPrefs.SetInt("stage", currentStage + 1);
    }

    public static void IncrementStageProgress()
    {
        int stageProgressValue = PlayerPrefs.GetInt("stageprogress");
        PlayerPrefs.SetInt("stageprogress", stageProgressValue + 1);
    }

    #endregion

    #region Decrementers

    public static bool CanDecrementUpgradeStars()
    {
        return GetNumUpgradeStars() > 0;
    }

    public static void DecrementUpgradeStarsAndIncrementStageProgress()
    {
        int currentNumStars = PlayerPrefs.GetInt("upgradestars");
        if (currentNumStars > 0)
        {
            PlayerPrefs.SetInt("upgradestars", currentNumStars - 1);
            IncrementStageProgress();
        }
    }

    #endregion

    #region Logic

    public static void AdvanceToNextStage()
    {
        UnityEngine.Assertions.Assert.IsTrue(CanAdvanceToNextStage());
        IncrementStage();
        SetStageProgress(0);
    }

    #endregion

}
