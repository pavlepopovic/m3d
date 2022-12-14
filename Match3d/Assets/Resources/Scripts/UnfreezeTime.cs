using UnityEngine;
using UnityEngine.UI;

public class UnfreezeTime : MonoBehaviour
{
    public float FreezeValue;

    private void OnEnable()
    {
        FreezeValue = GameManager.s_Instance.FreezeDelayValue;
    }

    void Update()
    {
        FreezeValue -= Time.deltaTime;
        gameObject.GetComponent<Text>().text = "" + Mathf.Round(FreezeValue);

        if (FreezeValue <= 0)
        {
            GameManager.s_Instance.Freeze.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            FindObjectOfType<LevelTimer>().FreezeTimeBool = false;
            FindObjectOfType<LevelTimer>().CountDown();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
