using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showrewardedvideoad : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnShowrewardedad(int n)
    {
        GameObject.FindObjectOfType<admobads>().showad(n);
    }
}
