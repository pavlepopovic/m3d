using UnityEngine;
using UnityEngine.UI;

public class UnfreezeTime : MonoBehaviour
{
    public float FreezeValue;

    private void OnEnable()
    {
        FreezeValue = GameManager.s_Instance.freezedelayvalue;
    }

    void Update()
    {
        FreezeValue -= Time.deltaTime;
        gameObject.GetComponent<Text>().text = "" + Mathf.Round(FreezeValue);

        if (FreezeValue <= 0)
        {
            GameManager.s_Instance.freeze.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            FindObjectOfType<leveltimer>().FreezeTimeBool = false;
            FindObjectOfType<leveltimer>().CountDown();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
