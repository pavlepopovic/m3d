using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class requestintersitialad : MonoBehaviour
{
    public void OnEnable()
    {
        GameObject.FindObjectOfType<admobads>().requestintersitial();
    }
}
