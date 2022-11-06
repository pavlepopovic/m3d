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

    [Header("Star Animation")]
    public GameObject StarPrefab;
    public GameObject StarEndPoint;
    public GameObject Canvas;

    [Header("Star text")]
    public Text StarValue;

    private GameObject[] m_MatchSlotItems;
    private int m_StarValue = 0;
    private int m_NumCollectedObjects = 0;

    // Needed as a workaround as coroutines don't return values.
    private bool m_DidMatchHappen = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(MatchSlots, "MatchSlots is null!");
        UnityEngine.Assertions.Assert.AreEqual(MatchSlots.Length, k_MatchSlotsLength, "MatchSlots inapropriate length");
        UnityEngine.Assertions.Assert.IsNull(s_Instance, "Instance is not null!");
        UnityEngine.Assertions.Assert.IsNotNull(StarEndPoint, "StarEndPoint is null!");
        UnityEngine.Assertions.Assert.IsNotNull(StarPrefab, "StarPrefab is null");
        UnityEngine.Assertions.Assert.IsNotNull(Canvas, "Canvas is null");

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

        PlaceItemInMatchBoardArray(item);
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

        if (m_DidMatchHappen == true)
        {
            // increase score, destroy matched items, and move others in proper place
            yield return CleanBoardAfterMatch(newItemIndex);
        }
    }

    private IEnumerator MoveItemsToCorrectPositionsOnBoard(int startingItemIndex)
    {
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

        float startTime = Time.time;
        float interpolationRatio = 0.0f;
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
        m_DidMatchHappen = false;
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
                GameManager.s_Instance.FailLevelDueToFullBoard();
            }
            
            yield break;
        }

        m_DidMatchHappen = true;

        // match
        yield return MatchObjects(newItem, adjacentItem);
    }

    private IEnumerator MatchObjects(GameObject a, GameObject b)
    {
        SoundManager.instance.PlayBottleFillSound();
 
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        float interpolationRatio = 0.0f;

        Vector3 startingPositionNewItem = a.transform.position;
        Vector3 startingPositionAdjacentItem = b.transform.position;
        Vector3 matchPosition = (startingPositionNewItem + startingPositionAdjacentItem) / 2.0f;

        while (Time.time - startTime <= 0.20f || interpolationRatio != 1.0f)
        {
            interpolationRatio = Mathf.Min(5 * (Time.time - startTime), 1.0f);
            a.transform.position = Vector3.Lerp(startingPositionNewItem, matchPosition, interpolationRatio); // lerp from A to B in one second
            b.transform.position = Vector3.Lerp(startingPositionAdjacentItem, matchPosition, interpolationRatio); // lerp from A to B in one second
            yield return null;
        }

        StartCoroutine(MoveStarAndIncreaseScore(matchPosition));

        UnityEngine.Assertions.Assert.AreEqual(interpolationRatio, 1.0f);
    }

    private IEnumerator CleanBoardAfterMatch(int newItemIndex)
    {
        // Destroy items with a slight delay
        yield return new WaitForSeconds(0.05f);

        // destroy matched items
        {
            GameObject newItem = m_MatchSlotItems[newItemIndex];
            GameObject adjacentItem = m_MatchSlotItems[newItemIndex - 1];

            Destroy(newItem);
            Destroy(adjacentItem);
        }

        // set board pieces to null
        {
            m_MatchSlotItems[newItemIndex] = null;
            m_MatchSlotItems[newItemIndex - 1] = null;
        }
        
        // make array valid
        {
            for (int i = newItemIndex + 1; i < k_MatchSlotsLength; i++)
            {
                m_MatchSlotItems[i - 2] = m_MatchSlotItems[i];
            }

            // make last two items null, as they represent free space now
            m_MatchSlotItems[k_MatchSlotsLength - 1] = null;
            m_MatchSlotItems[k_MatchSlotsLength - 2] = null;
        }

        // finally, move items to their place on board properly
        {
            Vector3[] startingPositions = new Vector3[k_MatchSlotsLength - 1 - newItemIndex];
            for (int i = newItemIndex - 1, j = 0; j < startingPositions.Length; i++, j++)
            {
                if (m_MatchSlotItems[i] != null)
                {
                    startingPositions[j] = m_MatchSlotItems[i].transform.position;
                }
            }

            float interpolationRatio = 0.0f;
            float startTime = Time.time;
            while (Time.time - startTime <= 0.25f || interpolationRatio != 1.0f)
            {
                interpolationRatio = Mathf.Min(4 * (Time.time - startTime), 1.0f);
                for (int i = newItemIndex - 1, j = 0; i < k_MatchSlotsLength; i++, j++)
                {
                    if (m_MatchSlotItems[i] != null)
                    {
                        m_MatchSlotItems[i].transform.position = Vector3.Lerp(startingPositions[j], MatchSlots[i].transform.position, interpolationRatio);
                    }
                }
                yield return null;
            }

            UnityEngine.Assertions.Assert.AreEqual(interpolationRatio, 1.0f);
        }
    }

    private IEnumerator MoveStarAndIncreaseScore(Vector3 starSpawnPosition)
    {
        GameObject star = Instantiate(StarPrefab, Canvas.transform);
        RectTransform starRectTransform = star.GetComponent<RectTransform>();

        // Set proper star position in canvas
        // Don't know what this exactly does, copied it from internet.
        {
            RectTransform canvasTransform = Canvas.GetComponent<RectTransform>();
            Vector2 uiOffset = new Vector2(canvasTransform.sizeDelta.x / 2.0f, canvasTransform.sizeDelta.y / 2.0f);
            Vector2 viewPortSpawnPosition = Camera.main.WorldToViewportPoint(starSpawnPosition);
            Vector2 proportionalPosition = new Vector2(viewPortSpawnPosition.x * canvasTransform.sizeDelta.x, viewPortSpawnPosition.y * canvasTransform.sizeDelta.y);
            starRectTransform.localPosition = proportionalPosition - uiOffset;
        }

        float interpolationRatio = 0.0f;
        float startTime = Time.time;
        RectTransform endPointRectTransform = StarEndPoint.GetComponent<RectTransform>();
        Vector2 endPoint = endPointRectTransform.position;
        Vector2 startPoint = starRectTransform.position;
        while (Time.time - startTime <= 1.0f || interpolationRatio != 1.0f)
        {
            interpolationRatio = Mathf.Min(Time.time - startTime, 1.0f);
            star.GetComponent<RectTransform>().position = Vector2.Lerp(startPoint, endPoint, interpolationRatio);
            yield return null;
        }

        UnityEngine.Assertions.Assert.AreEqual(1.0f, interpolationRatio);
        Destroy(star);

        // Update score
        m_StarValue++;
        StarValue.text = m_StarValue.ToString();
        m_NumCollectedObjects++;
        GameManager.s_Instance.CheckLevelComplete(m_NumCollectedObjects);
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
}
