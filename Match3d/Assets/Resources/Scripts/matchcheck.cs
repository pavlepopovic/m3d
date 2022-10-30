using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCheck : MonoBehaviour
{    
    [Header("Destination point")]
    public GameObject PointA;
    public GameObject PointB;

    [UnityEngine.Serialization.FormerlySerializedAs("Matchpoint")]
    public GameObject MatchPoint;

    [Header("Match Anim")]
    [UnityEngine.Serialization.FormerlySerializedAs("matchanim")]
    public GameObject MatchAnim;
    [UnityEngine.Serialization.FormerlySerializedAs("Staranim")]
    public GameObject StarAnim;
    [UnityEngine.Serialization.FormerlySerializedAs("Starvalue")]
    public Text StarValue;

    [Header("Hint Stuff")]
    [UnityEngine.Serialization.FormerlySerializedAs("hintbutton")]
    public Button HintButton;
    [UnityEngine.Serialization.FormerlySerializedAs("Addrewardhintdialogbox")]
    public GameObject AddRewardHintDialogBox;
    [UnityEngine.Serialization.FormerlySerializedAs("Spawnpoint")]
    public GameObject SpawnPoint;

    private List<GameObject> m_HintObjects = new List<GameObject>();
    private List<GameObject> m_PlaceObject = new List<GameObject>();
    private int m_CollectedObjectValue = 0;
    private int m_StarValue = 0;

    private void OnTriggerStay(Collider other)
    {
        if(m_PlaceObject.Count == 0 || (other.gameObject.name.Contains(m_PlaceObject[0].name) == true))
        {
            return;
        }
        other.GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 1) * 120 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_PlaceObject.Count == 0)
        {           
            other.gameObject.transform.position = PointA.transform.position;
            other.gameObject.transform.rotation = PointA.transform.rotation;
            other.gameObject.GetComponent<Item>().SetRotation(); 
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;           
            m_PlaceObject.Add(other.gameObject);          
        }
        else if(other.gameObject.name.Contains(m_PlaceObject[0].name) == true)
        {
            other.gameObject.transform.position = PointB.transform.position;
            other.gameObject.transform.rotation = PointB.transform.rotation;
            other.gameObject.GetComponent<Item>().SetRotation();
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.gameObject.GetComponent<MeshCollider>().enabled = false;
            m_PlaceObject.Add(other.gameObject);
            m_PlaceObject[0].GetComponent<MeshCollider>().enabled = false;
            m_PlaceObject[1].GetComponent<MeshCollider>().enabled = false;

            Invoke("CollectObject", 0.2f);
            Invoke("DisableAnim", 0.4f);
            StartCoroutine(MoveToMatchPoint(0.01f, m_PlaceObject[0], m_PlaceObject[1]));

            SoundManager.instance.PlayBottleFillSound();
            Invoke("DisableStarAnim", 1f);
            MatchAnim.GetComponent<Animator>().SetBool("collect", true);
            StarAnim.SetActive(true);            
        }
        else
        {
            other.gameObject.transform.position = PointB.transform.position;
            other.gameObject.transform.rotation = PointB.transform.rotation;

            SoundManager.instance.PlayWrongMatchSound();
            if(PrefManager.s_Instance.GetVibrationsValue() == 1)
            {
                Handheld.Vibrate();
            }
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 1) * 120 * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_PlaceObject.Contains(other.gameObject))
        {             
            m_PlaceObject.Remove(other.gameObject);             
        }
    }

    void CollectObject()
    {
        Destroy(m_PlaceObject[0].gameObject);       
        Destroy(m_PlaceObject[1].gameObject);
        m_PlaceObject.Clear();
        m_HintObjects.Clear();
    }
   
    void DisableAnim()
    {
        MatchAnim.GetComponent<Animator>().SetBool("collect", false);
    }

    void DisableStarAnim()
    {
        StarAnim.SetActive(false);
        m_StarValue++;
        StarValue.text = m_StarValue.ToString();
        m_CollectedObjectValue++;
        GameManager.s_Instance.CheckLevelComplete(m_CollectedObjectValue);
    }

    public void OnHintClick()
    {
        if (PrefManager.s_Instance.GetHintValue() > 0)           
        {
            HintButton.interactable = false;
            int hintvalue = PrefManager.s_Instance.GetHintValue();
            hintvalue--;
            PrefManager.s_Instance.SetHintValue(hintvalue);

            FindGameObjects();
            if (m_PlaceObject.Count != 1)
            {
                TranslateObjectToPoint(m_HintObjects[0], PointA.transform.GetChild(0).gameObject);
                Invoke("PlaceOtherObject", 1f);
            }
            else
            {
                PlaceOtherObject();
            }
        }
        else
        {
            AddRewardHintDialogBox.SetActive(true);
        }
    }

    void PlaceOtherObject()
    {
        TranslateObjectToPoint(m_HintObjects[1],PointB.transform.GetChild(0).gameObject);
        Invoke("MakeHintButtonInteractable", 2f);
    }

    void MakeHintButtonInteractable()
    {
        HintButton.interactable = true;
    }

    void FindGameObjects()
    {
        int i = 0;
        m_HintObjects.Clear();
        if (m_PlaceObject.Count == 1)
        {
            m_HintObjects.Add(m_PlaceObject[0].gameObject);
        }
        else
        {
            i++;
            m_HintObjects.Add(SpawnPoint.transform.GetChild(0).gameObject);
        }
        
        while(i < SpawnPoint.transform.childCount)
        {
            if(SpawnPoint.transform.GetChild(i).name== m_HintObjects[0].name )
            {
                if (m_PlaceObject.Count==1)
                {
                    if(!ReferenceEquals(SpawnPoint.transform.GetChild(i).gameObject,m_PlaceObject[0].gameObject))
                    {
                        m_HintObjects.Add(SpawnPoint.transform.GetChild(i).gameObject);
                        break;
                    }
                }
                else
                {
                    m_HintObjects.Add(SpawnPoint.transform.GetChild(i).gameObject);
                    break;
                }
            }
            i++;
        }
    }

    // Moving object to center of point to give merging illusion
    IEnumerator MoveToMatchPoint(float delayTime, GameObject a, GameObject b)
    {
        yield return new WaitForSeconds(delayTime); // start at time X
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 0.17)
        { 
            // until one second passed
            a.transform.position = Vector3.Lerp(a.transform.position, MatchPoint.transform.position, Time.time - startTime); // lerp from A to B in one second
            b.transform.position = Vector3.Lerp(b.transform.position, MatchPoint.transform.position, Time.time - startTime); // lerp from A to B in one second
            yield return 0.17f; // wait for next frame           
        }
    }

    void TranslateObjectToPoint(GameObject g, GameObject point)
    {
        g.gameObject.GetComponent<Item>().HintBool = true;
        g.gameObject.GetComponent<Item>().OnMouseDown();
        g.gameObject.GetComponent<Item>().OnMouseDrag();
        StartCoroutine(WaitAndMove(0.05f,g, point));
    }

    IEnumerator WaitAndMove(float delayTime, GameObject g, GameObject point)
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
