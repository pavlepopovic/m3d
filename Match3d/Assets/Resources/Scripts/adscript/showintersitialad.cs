using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showintersitialad : MonoBehaviour
{
    public void OnEnable()
    {
        
        GameObject.FindObjectOfType<admobads>().showinterstialad();
    }
    
}
