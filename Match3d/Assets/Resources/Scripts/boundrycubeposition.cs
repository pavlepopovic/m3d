using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundrycubeposition : MonoBehaviour
{
    public GameObject leftcube, rightcube;


    float
         clampMarginMinX = 0.0f,
         clampMarginMaxX = 0.0f,
         clampMarginMinY = 0.0f,
         clampMarginMaxY = 0.0f;


    float offsetxminvalue = 0f;
    float offsetxmaxvalue = -0f;
    float offsetyminvalue = 0.8f;
    float offsetymaxvalue = -0.25f;


    // The minimum and maximum values which the object can go
    private float
        m_clampMinX,
        m_clampMaxX,
        m_clampMinY,
        m_clampMaxY;
    // Start is called before the first frame update
    void Start()
    {
        // Get the minimum and maximum position values according to the screen size represented by the main camera.
        m_clampMinX = Camera.main.ScreenToWorldPoint(new Vector2(0 + clampMarginMinX, 0)).x + offsetxminvalue;

        m_clampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - clampMarginMaxX, 0)).x + offsetxmaxvalue;
        m_clampMinY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0 + clampMarginMinY)).z + offsetyminvalue;
        m_clampMaxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height + clampMarginMaxY)).z + offsetymaxvalue;

        rightcube.transform.position = new Vector3(m_clampMaxX, rightcube.transform.position.y, rightcube. transform.position.z);
        leftcube.transform.position = new Vector3(m_clampMinX, leftcube.transform.position.y, leftcube.transform.position.z);
    }
}

