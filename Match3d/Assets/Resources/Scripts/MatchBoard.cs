using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Controls matching mechanisms in game
public class MatchBoard : MonoBehaviour
{
    public static MatchBoard s_Instance = null;
    public const int k_MatchSlotsLength = 7;

    [Header("Match slots")]
    public GameObject[] MatchSlots;

    [Header("Animation")]
    public GameObject StarAnim;

    [Header("Star text")]
    public Text StarValue;

    private GameObject[] m_MatchSlotItems;
    private int m_StarValue = 0;

    private int m_NewItemIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(MatchSlots, "MatchSlots is null!");
        UnityEngine.Assertions.Assert.AreEqual(MatchSlots.Length, k_MatchSlotsLength, "MatchSlots inapropriate length");
        UnityEngine.Assertions.Assert.IsNull(s_Instance);

        m_MatchSlotItems = new GameObject[k_MatchSlotsLength];
        s_Instance = this;
    }

    private void OnDestroy()
    {
        UnityEngine.Assertions.Assert.IsNotNull(s_Instance);
        s_Instance = null;
    }

    public void MoveItemToSlot(GameObject item)
    {
        UnityEngine.Assertions.Assert.IsNotNull(item);
        UnityEngine.Assertions.Assert.IsNotNull(item.GetComponent<Item>());
        UnityEngine.Assertions.Assert.IsNotNull(item.GetComponent<Rigidbody>());
        UnityEngine.Assertions.Assert.IsNotNull(item.GetComponent<MeshCollider>());

        item.GetComponent<Item>().SetRotation();
        item.GetComponent<MeshCollider>().enabled = false;
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // place accordingly
        PlaceItemInMatchBoardArray(item);
        
        // check for match
    }

    private void PlaceItemInMatchBoardArray(GameObject item)
    {
        UnityEngine.Assertions.Assert.IsTrue(IsThereAtLeastOnePlaceOnBoard(), "No place on board!");

        int itemIndex = -1;
        for (int i = 0; i < k_MatchSlotsLength; i++)
        {
            if (m_MatchSlotItems[i] == null)
            {
                m_MatchSlotItems[i] = item;
                itemIndex = i;
                break;
            }
            else if (m_MatchSlotItems[i].name == item.name)
            {
                // In this case, we need to move all other items to the right
                MoveItemsOnePlaceToTheRightInArray(i + 1);
                UnityEngine.Assertions.Assert.IsNull(m_MatchSlotItems[i + 1]);

                m_MatchSlotItems[i + 1] = item;
                itemIndex = i + 1;
                break;
            }
        }

        UnityEngine.Assertions.Assert.IsTrue(itemIndex != -1, "Item index wrongly calculated");
        StartCoroutine(ResolveMatchBoard(itemIndex));
    }

    private void MoveItemsOnePlaceToTheRightInArray(int startingIndex)
    {
        UnityEngine.Assertions.Assert.IsTrue(startingIndex < k_MatchSlotsLength - 1);

        GameObject[] helpArray = new GameObject[k_MatchSlotsLength - 1 - startingIndex];
        for (int i = 0; i < helpArray.Length; i++)
        {
            helpArray[i] = m_MatchSlotItems[startingIndex + i];
        }

        // Move objects one place to the right
        for (int i = 0; i < helpArray.Length; i++)
        {
            m_MatchSlotItems[startingIndex + i + 1] = helpArray[i];
        }

        // Invalidate slot
        m_MatchSlotItems[startingIndex] = null;
    }

    private IEnumerator ResolveMatchBoard(int newItemIndex)
    {
        yield return MoveItemsToCorrectPositionsOnBoard(newItemIndex);

        yield return MatchIfPossible(newItemIndex);

        // Todo - move objects back where they belong, and cleanup board
    }

    private IEnumerator MoveItemsToCorrectPositionsOnBoard(int startingItemIndex)
    {
        float startTime = Time.time;
        float interpolationRatio = 0.0f;

        // StartingItem is specific, as it needs to move twice slower then other items on board
        GameObject startingItem = m_MatchSlotItems[startingItemIndex];

        int numNonNullElementsToTheRight = 0;
        {
            for (int i = startingItemIndex; i < k_MatchSlotsLength; i++)
            {
                if ((m_MatchSlotItems[i]) != null)
                {
                    numNonNullElementsToTheRight++;
                }
                else
                {
                    break;
                }
            }
        }
        
        Vector3[] startingPositions = new Vector3[numNonNullElementsToTheRight];
        Vector3[] endingPositions = new Vector3[numNonNullElementsToTheRight];
        for (int i = 0; i < numNonNullElementsToTheRight;i++)
        {
            int slotPosition = startingItemIndex + i;
            startingPositions[i] = m_MatchSlotItems[slotPosition].transform.position;
            endingPositions[i] = MatchSlots[slotPosition].transform.position;
        }

        // we are changing the scale of only the first item (indexed by startingItemIndex)
        // as others on board have proper scale already
        Vector3 startingItemStartingScale = startingItem.transform.localScale;
        Vector3 startingItemEndingScale = new Vector3(0.8f, 0.8f, 0.8f);

        while (Time.time - startTime <= 0.25f || interpolationRatio != 1.0f)
        {
            interpolationRatio = Mathf.Min(4 * (Time.time - startTime), 1.0f);
            m_MatchSlotItems[startingItemIndex].transform.position = Vector3.Lerp(startingPositions[0], endingPositions[0], interpolationRatio);
            m_MatchSlotItems[startingItemIndex].transform.localScale = Vector3.Lerp(startingItemStartingScale, startingItemEndingScale, interpolationRatio);

            // Move items on board twice as fast
            float interpolationRatioForOtherItems = Mathf.Min(2 * interpolationRatio, 1.0f);
            if (interpolationRatioForOtherItems <= 1.0f)
            {
                for (int i = 1; i < startingPositions.Length; i++)
                {
                    m_MatchSlotItems[startingItemIndex + i].transform.position = Vector3.Lerp(startingPositions[i], endingPositions[i], interpolationRatioForOtherItems);
                }
            }
            yield return null;
        }

        UnityEngine.Assertions.Assert.AreEqual(interpolationRatio, 1.0f);
    }

    private IEnumerator MatchIfPossible(int newItemIndex)
    {
        if (newItemIndex == 0)
        {
            // Nothing happens
            yield break;
        }

        GameObject newItem = m_MatchSlotItems[newItemIndex];
        GameObject adjacentItem = m_MatchSlotItems[newItemIndex - 1];
        if (adjacentItem == null || newItem.name != adjacentItem.name)
        {
            // Loss
            if(!IsThereAtLeastOnePlaceOnBoard())
            {
                // ...
            }
            
            yield break;
        }

        // match
        {
            SoundManager.instance.PlayBottleFillSound();
            StarAnim.SetActive(true);
            Invoke("DisableStarAnim", 1f);

            m_NewItemIndex = newItemIndex;
            Invoke("CollectObjects", 0.25f);

            float startTime = Time.time; // Time.time contains current frame time, so remember starting point
            float interpolationRatio = 0.0f;

            Vector3 startingPositionNewItem = newItem.transform.position;
            Vector3 startingPositionAdjacentItem = adjacentItem.transform.position;
            Vector3 endingPosition = (startingPositionNewItem + startingPositionAdjacentItem) / 2.0f;

            while (Time.time - startTime <= 0.20f || interpolationRatio != 1.0f)
            {
                interpolationRatio = Mathf.Min(5 * (Time.time - startTime), 1.0f);
                newItem.transform.position = Vector3.Lerp(startingPositionNewItem, endingPosition, interpolationRatio); // lerp from A to B in one second
                adjacentItem.transform.position = Vector3.Lerp(startingPositionAdjacentItem, endingPosition, interpolationRatio); // lerp from A to B in one second
                yield return null;
            }
        }

    }

    void DisableStarAnim()
    {
        StarAnim.SetActive(false);
        m_StarValue++;
        StarValue.text = m_StarValue.ToString();
        //m_CollectedObjectValue++;
        //GameManager.s_Instance.CheckLevelComplete(m_CollectedObjectValue);
    }

    private void CollectObjects()
    {
        UnityEngine.Assertions.Assert.IsTrue(m_NewItemIndex > 0);

        GameObject newItem = m_MatchSlotItems[m_NewItemIndex];
        GameObject adjacentItem = m_MatchSlotItems[m_NewItemIndex - 1];

        Destroy(newItem);
        Destroy(adjacentItem);
    }

    public bool IsThereAtLeastOnePlaceOnBoard()
    {
        for (int i = 0; i < k_MatchSlotsLength; i++)
        {
            if (m_MatchSlotItems[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
