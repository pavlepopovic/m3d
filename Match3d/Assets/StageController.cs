using UnityEngine;

public class StageController : MonoBehaviour
{
    public int StageValue;

    public void Start()
    {
        UnityEngine.Assertions.Assert.IsTrue(StageValue != 0);
    }

    public void OnStageObjectiveClick(int childIndex)
    {
        UnityEngine.Assertions.Assert.IsTrue(PrefManager.CanDecrementUpgradeStars());
        UnityEngine.Assertions.Assert.AreEqual(PrefManager.GetStageObjectiveState(StageValue, childIndex), 0);

        PrefManager.DecrementUpgradeStarsAndIncrementStageProgress();
        PrefManager.SetStageObjectiveState(StageValue, childIndex, 1);
        FindObjectOfType<MainMenuManager>().ResolveGameState();
    }
}
