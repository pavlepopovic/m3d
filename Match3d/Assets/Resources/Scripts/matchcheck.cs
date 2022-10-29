using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class matchcheck : MonoBehaviour
{
    int collectedobjectvalue=0;
    
    [Header("Object that have Placed")]
    public List<GameObject> PlaceObject = new List<GameObject>();
    [Header("Destination point")]
    public GameObject PointA;
    public GameObject PointB;
    public GameObject Matchpoint;

    [Header("Match Anim")]
    public GameObject matchanim;
    public GameObject Staranim;
    public Text Starvalue;
    int starvalue=0;
    [Header("Hint Stuff")]
    public Button hintbutton;
    public GameObject Addrewardhintdialogbox;
    public GameObject Spawnpoint;
    public int automovingobjectspeed;
    public List<GameObject> HintObjects = new List<GameObject>();


    bool firstplace, secondplace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlaceObject.Count == 0)
        {

        }
        else if (other.gameObject.name.Contains(PlaceObject[0].name) == true)
        {
        }
        else
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 1) * 120 * Time.deltaTime;
        }
    }

            private void OnTriggerEnter(Collider other)
    {
        if (PlaceObject.Count == 0)
        {
           
                other.gameObject.transform.position = PointA.transform.position;
                other.gameObject.transform.rotation = PointA.transform.rotation;
            other.gameObject.GetComponent<Item>().SetRotation(); 
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
           
                PlaceObject.Add(other.gameObject);
          
        }else if (other.gameObject.name.Contains(PlaceObject[0].name)==true)
        {
            
               
                other.gameObject.transform.position = PointB.transform.position;
                other.gameObject.transform.rotation = PointB.transform.rotation;
            other.gameObject.GetComponent<Item>().SetRotation();
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            other.gameObject.GetComponent<MeshCollider>().enabled = false;

           

            PlaceObject.Add(other.gameObject);

            PlaceObject[0].GetComponent<MeshCollider>().enabled = false;
            PlaceObject[1].GetComponent<MeshCollider>().enabled = false;

            Invoke("collectObject", 0.2f);
                Invoke("disableanim", 0.4f);


                StartCoroutine(movetomatchpoint(0.01f, PlaceObject[0], PlaceObject[1]));

            SoundManager.instance.PlaybottlfillSOund();
                Invoke("disablestaranim", 1f);

                matchanim.GetComponent<Animator>().SetBool("collect", true);
                Staranim.SetActive(true);
            
        }
        else
        {
            other.gameObject.transform.position = PointB.transform.position;
            other.gameObject.transform.rotation = PointB.transform.rotation;

            SoundManager.instance.wrongmatchsound();
            if(prefmanager.instance.Getvibrationsvalue()==1)
            {
                Handheld.Vibrate();
            }
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 1) * 120 * Time.deltaTime;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (PlaceObject.Contains(other.gameObject))
        {
             
            PlaceObject.Remove(other.gameObject);
             
        }
    }


    //Collect And Destroy Objects
    void collectObject()
    {

        Destroy(PlaceObject[0].gameObject);
       
            Destroy(PlaceObject[1].gameObject);
       
       
            PlaceObject.Clear();
            HintObjects.Clear();
      
    }
   
    void disableanim()
    {
        matchanim.GetComponent<Animator>().SetBool("collect", false);
    }

    void disablestaranim()
    {
        Staranim.SetActive(false);
        starvalue++;
        Starvalue.text = starvalue.ToString();
        collectedobjectvalue++;
        GameManager.instance.checklevelcomplete(collectedobjectvalue);
    }

    public void OnhintClick()
    {
        if (prefmanager.instance.Gethintvalue() > 0)
           
            {

            hintbutton.interactable = false;
            int hintvalue = prefmanager.instance.Gethintvalue();
            hintvalue--;
            prefmanager.instance.SetHintValue(hintvalue);


            FindGameObjects();
            if (PlaceObject.Count != 1)
            {
                translateobjecttopoint(HintObjects[0], PointA.transform.GetChild(0).gameObject);
                Invoke("PlaceOtherObject", 1f);
            }
            else
            {
                PlaceOtherObject();
            }
        }
        else
        {
            Addrewardhintdialogbox.SetActive(true);
        }
    }

    void PlaceOtherObject()
    {
        translateobjecttopoint(HintObjects[1],PointB.transform.GetChild(0).gameObject);
        Invoke("MakeHintButtonInteractable", 2f);
    }

    void MakeHintButtonInteractable()
    {
        hintbutton.interactable = true;
    }

    void FindGameObjects()
    {
        int i = 0;
        HintObjects.Clear();
        if (PlaceObject.Count == 1)
        {
            HintObjects.Add(PlaceObject[0].gameObject);
        }
        else
        {
            i++;
            HintObjects.Add(Spawnpoint.transform.GetChild(0).gameObject);
        }
        
        while( i < Spawnpoint.transform.childCount)
        {
            if(Spawnpoint.transform.GetChild(i).name== HintObjects[0].name )
            {
                if (PlaceObject.Count==1)
                {
                    if(!GameObject.ReferenceEquals(Spawnpoint.transform.GetChild(i).gameObject,PlaceObject[0].gameObject))
                    {
                        HintObjects.Add(Spawnpoint.transform.GetChild(i).gameObject);
                        break;
                    }
                }
                else
                {
                    HintObjects.Add(Spawnpoint.transform.GetChild(i).gameObject);
                    break;
                }
            }
            i++;
        }
    }

    //MOving Object to Center of point to give Merging illusion
    IEnumerator movetomatchpoint(float delayTime, GameObject a, GameObject b)
    {
        yield return new WaitForSeconds(delayTime); // start at time X
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 0.17)
        { // until one second passed
            a.transform.position = Vector3.Lerp(a.transform.position, Matchpoint.transform.position, Time.time - startTime); // lerp from A to B in one second
            b.transform.position = Vector3.Lerp(b.transform.position, Matchpoint.transform.position, Time.time - startTime); // lerp from A to B in one second
            yield return 0.17f; // wait for next frame           
        }
    }

    void translateobjecttopoint(GameObject g,GameObject point)
    {
        g.gameObject.GetComponent<Item>().HintBool = true;
        g.gameObject.GetComponent<Item>().OnMouseDown();
        g.gameObject.GetComponent<Item>().OnMouseDrag();
        StartCoroutine(WaitAndMove(0.05f,g, point));
    }

    IEnumerator WaitAndMove(float delayTime,GameObject g,GameObject point)
    {
        yield return new WaitForSeconds(delayTime); // start at time X
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 0.5)
        { // until one second passed
            g.transform.position = Vector3.Lerp(g.transform.position, point.transform.position, Time.time - startTime); // lerp from A to B in one second
            yield return 0.5f; // wait for next frame
            g.gameObject.GetComponent<Item>().OnMouseUp();
        }
    }
}
