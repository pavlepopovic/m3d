using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showbanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        GameObject.FindObjectOfType<admobads>().showBanner ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
