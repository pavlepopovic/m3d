using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{


    public Vector3 RotationValues;
    public bool applyforce;

    [HideInInspector]
    public bool HintBool;

    bool uppositionbool;
    Rigidbody rb;
    Vector3 Screenpoint;
    Vector3 offset;

    float orginalscalex, orginalscaley, orginalscalez;
    

    float
         clampMarginMinX = 0.0f,
         clampMarginMaxX = 0.0f,
         clampMarginMinY = 0.0f,
         clampMarginMaxY = 0.0f;


     float offsetxminvalue= 0.18f;
     float offsetxmaxvalue= -0.15f;
     float offsetyminvalue=0.8f;
     float offsetymaxvalue= -0.25f;


    // The minimum and maximum values which the object can go
    private float
        m_clampMinX,
        m_clampMaxX,
        m_clampMinY,
        m_clampMaxY;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        orginalscalex = rb.gameObject.transform.localScale.x;
        orginalscalex = rb.gameObject.transform.localScale.x;
        orginalscalex = rb.gameObject.transform.localScale.x;


        // Get the minimum and maximum position values according to the screen size represented by the main camera.
        m_clampMinX = Camera.main.ScreenToWorldPoint(new Vector2(0 + clampMarginMinX, 0)).x+ offsetxminvalue;
        
        m_clampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - clampMarginMaxX, 0)).x+ offsetxmaxvalue;
        m_clampMinY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0 + clampMarginMinY)).z+ offsetyminvalue;
        m_clampMaxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height + clampMarginMaxY)).z+ offsetymaxvalue;
        Invoke("makespawnfalse", 1f);
      //  print(m_clampMinX + "m_clampMinX" + m_clampMaxX + "m_clampMaxX" + m_clampMinY + "m_clampMinY" + m_clampMaxY + "m_clampMaxY");
    }
    void makespawnfalse()
    {
        uppositionbool = true;
    }


    private void LateUpdate()
    {
        if (uppositionbool == false)
        {

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.4f, 1f),
             Mathf.Clamp(transform.position.y, -1f, 1f), Mathf.Clamp(transform.position.z, -3f, 3f));
        }


        if (applyforce == true&&HintBool!=true)
        {
            GetComponent<Rigidbody>().AddForce(Physics.gravity*100f, ForceMode.Acceleration);
        }

        if (transform.position.x < m_clampMinX)
            {
                // If the object position tries to exceed the left screen bound clamp the min x position to 0.
                // The maximum x position won't be clamped so the object can move to the right.
                //  rb.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                transform.position = new Vector3(m_clampMinX, transform.position.y, transform.position.z);
            

            }

            if (transform.position.x > m_clampMaxX)
            {
                // Same goes here
                transform.position = new Vector3(m_clampMaxX, transform.position.y, transform.position.z);
            
                //    print("Keep running");
            }
            if (transform.position.z < m_clampMinY)
            {
                // Same goes here
                transform.position = new Vector3(transform.position.x, transform.position.y, m_clampMinY);
           

                print("Keep running");
            }
            if (transform.position.z > m_clampMaxY)
            {
                // Same goes here
                transform.position = new Vector3(transform.position.x, transform.position.y, m_clampMaxY);
            
                // print("Player position");
            }
       // }



    }
    // Update is called once per frame
    void Update()
    {
       

    }
    public void OnMouseDown()
    {

       
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None;
        Screenpoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Screenpoint.z));
    }

    public void OnMouseDrag()
    {
       // applyforce = true;
        Vector3 cursorpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Screenpoint.z);
        Vector3 Cursorposition = Camera.main.ScreenToWorldPoint(cursorpoint) + offset;
        rb.position = Cursorposition;
        rb.constraints = RigidbodyConstraints.None;

           transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        transform.Rotate(0f, 1f, 0f);
        //Change Value of Selection height from here
        rb.MovePosition(new Vector3(rb.position.x, 3.5f, rb.position.z));
        transform.position = new Vector3(rb.position.x, 3.5f, rb.position.z);

    }
    public void OnMouseUp()
    {
       
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        applyforce = true;
        Invoke("Makeforcefalse", 0.07f);

          transform.localScale = new Vector3(orginalscalex , orginalscaley, orginalscalez);

         transform.localScale = new Vector3(1f, 1f, 1f);
    }
    void Makeforcefalse()
    {
        applyforce = false;
    }



    public void SetRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(
       RotationValues.x,
       RotationValues.y,
        RotationValues.z);
    }




}
