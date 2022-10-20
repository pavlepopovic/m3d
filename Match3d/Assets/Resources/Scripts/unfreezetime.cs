using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unfreezetime : MonoBehaviour
{
    public float freezevalue;
    private void OnEnable()
    {
        freezevalue = GameManager.instance.freezedelayvalue;
    }
    // Update is called once per frame
    void Update()
    {
        freezevalue -= Time.deltaTime;
        gameObject.GetComponent<Text>().text = "" + Mathf.Round(freezevalue);
        if (freezevalue <= 0)
        {
           GameManager.instance. freeze.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            GameObject.FindObjectOfType<leveltimer>().freezetimebool = false;
            GameObject.FindObjectOfType<leveltimer>().CountDown();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
