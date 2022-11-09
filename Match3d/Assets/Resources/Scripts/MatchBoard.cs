using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls matching mechanisms in game
public class MatchBoard : MonoBehaviour
{
    public static MatchBoard s_Instance = null;
    public const int k_MatchSlotsLength = 7;

    public bool SafeToAddNewItemToBoard { get; private set; }

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
        SafeToAddNewItemToBoard = true;
    }

    private void OnDestroy()
    {
        UnityEngine.Assertions.Assert.IsNotNull(s_Instance);
        s_Instance = null;
    }

    // Checks if there are two same items on board, and if we can pull third
    private bool PullOneMissingItem(Item[] allItems, Item[] itemsToPullOut)
    {
        // Find two of the same items in the array
        GameObject firstItem = null;
        GameObject secondItem = null;
        for (int i = 0; i < k_MatchSlotsLength - 1; i++)
        {
            if (m_MatchSlotItems[i] == null || m_MatchSlotItems[i + 1] == null)
            {
                break;
            }
            if (m_MatchSlotItems[i].name == m_MatchSlotItems[i + 1].name)
            {
                firstItem = m_MatchSlotItems[i];
                secondItem = m_MatchSlotItems[i + 1];
                break;
            }
        }

        // No matching items in array
        if (firstItem == null)
        {
            return false;
        }

        // Find the third item
        for (int i = 0; i < allItems.Length; i++)
        {
            Item item = allItems[i];
            if ((item.name == firstItem.name) && (item.gameObject != firstItem) && (item.gameObject != secondItem))
            {
                itemsToPullOut[0] = item;
                break;
            }
        }

        UnityEngine.Assertions.Assert.IsNotNull(itemsToPullOut[0], "Third item is null");
        return true;
    }

    // Checks if there is at least one item on board, and 2 free spaces, if so, we can pull other two
    private bool PullTwoMissingItems(Item[] allItems, Item[] itemsToPullOut)
    {
        // If the last two places on the board aren't null, we can't do anything
        if (m_MatchSlotItems[k_MatchSlotsLength - 1] != null || m_MatchSlotItems[k_MatchSlotsLength - 2] != null)
        {
            return false;
        }

        // Otherwise, get the first item and match it
        if (m_MatchSlotItems[0] == null)
        {
            return false;
        }

        Item item = m_MatchSlotItems[0].GetComponent<Item>();

        // find other two items
        for (int i = 0, j = 0; i < allItems.Length; i++)
        {
            if (allItems[i].name == item.name && allItems[i] != item)
            {
                itemsToPullOut[j++] = allItems[i];
            }
            if (j == 2)
            {
                break;
            }
        }

        UnityEngine.Assertions.Assert.IsNotNull(itemsToPullOut[0], "First item is null");
        UnityEngine.Assertions.Assert.IsNotNull(itemsToPullOut[1], "Second item is null");
        return true;
    }

    // Checks if it is possible to pull 3 items on board, only possible if the board is empty
    private bool PullThreeMissingItems(Item[] allItems, Item[] itemsToPullOut)
    {
        if (m_MatchSlotItems[0] != null)
        {
            return false;
        }

        Item targetItem = allItems[0];
        itemsToPullOut[0] = targetItem;
        for (int i = 1, j = 1; i < allItems.Length; i++)
        {
            if (allItems[i].name == targetItem.name)
            {
                itemsToPullOut[j++] = allItems[i];
            }
            if (j == 3)
            {
                break;
            }
        }
        return true;
    }

    public IEnumerator ProcessMagnet()
    {
        if (!SafeToAddNewItemToBoard)
            yield break;

        UnityEngine.Assertions.Assert.IsTrue(IsThereAtLeastOnePlaceOnBoard(), "No room when calling magnet");

        var itemsToPull = new Item[3];
        var allItems = FindObjectsOfType<Item>();

        bool canPull = PullOneMissingItem(allItems, itemsToPull);
        if (!canPull)
        {
            canPull = PullTwoMissingItems(allItems, itemsToPull);
            if (!canPull)
            {
                canPull = PullThreeMissingItems(allItems, itemsToPull);
                if (!canPull)
                {
                    SafeToAddNewItemToBoard = true;
                    yield break;
                }
            }
        }

        PrefManager.DecrementMagnets();

        for (int i = 0; i < 3; i++)
        {
            if (itemsToPull[i] != null)
            {
                SafeToAddNewItemToBoard = false;
                for (int j = 0; j < k_MatchSlotsLength; j++)
                {
                    if (m_MatchSlotItems[j] != null)
                    {
                        if (itemsToPull[i].gameObject == m_MatchSlotItems[j].gameObject)
                        {
                            UnityEngine.Assertions.Assert.IsFalse(true, "AA");
                        }
                    }
                }
                yield return PlaceItemInMatchBoardArrayIEnumerator(itemsToPull[i].gameObject);
            }
        }
    }

    public void PlaceItemInMatchBoardArray(GameObject item)
    {
        int itemIndex = PlaceItemInMatchBoardArrayImpl(item);
        StartCoroutine(ResolveMatchBoard(itemIndex));
    }

    private IEnumerator PlaceItemInMatchBoardArrayIEnumerator(GameObject item)
    {
        int itemIndex = PlaceItemInMatchBoardArrayImpl(item);
        yield return ResolveMatchBoard(itemIndex);
    }

    private int PlaceItemInMatchBoardArrayImpl(GameObject item)
    {
        UnityEngine.Assertions.Assert.IsTrue(IsThereAtLeastOnePlaceOnBoard(), "No place on board!");
        UnityEngine.Assertions.Assert.IsNotNull(item);
        UnityEngine.Assertions.Assert.IsNotNull(item.GetComponent<Item>());
        UnityEngine.Assertions.Assert.IsNotNull(item.GetComponent<Rigidbody>());
        UnityEngine.Assertions.Assert.IsNotNull(item.GetComponent<MeshCollider>());

        item.GetComponent<Item>().SetRotation();
        item.GetComponent<MeshCollider>().enabled = false;
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        SafeToAddNewItemToBoard = false;

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
                itemIndex = i + 1;
                if (m_MatchSlotItems[itemIndex] != null && m_MatchSlotItems[itemIndex].name == item.name)
                {
                    itemIndex++;
                }
                MoveItemsOnePlaceToTheRightInArray(itemIndex);
                UnityEngine.Assertions.Assert.IsNull(m_MatchSlotItems[itemIndex]);

                m_MatchSlotItems[itemIndex] = item;
                break;
            }
        }

        UnityEngine.Assertions.Assert.IsTrue(itemIndex != -1, "Item index wrongly calculated");
        return itemIndex;
    }

    private void MoveItemsOnePlaceToTheRightInArray(int startingIndex)
    {
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
        else
        {
            SafeToAddNewItemToBoard = true;
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
        if (newItemIndex <= 1)
        {
            // Nothing happens
            yield break;
        }

        GameObject newItem = m_MatchSlotItems[newItemIndex];
        GameObject secondItem = m_MatchSlotItems[newItemIndex - 1];
        GameObject thirdItem = m_MatchSlotItems[newItemIndex - 2];

        UnityEngine.Assertions.Assert.IsNotNull(newItem);
        UnityEngine.Assertions.Assert.IsNotNull(secondItem);
        UnityEngine.Assertions.Assert.IsNotNull(thirdItem);

        if (newItem.name != secondItem.name || newItem.name != thirdItem.name)
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
        yield return MatchObjects(newItem, secondItem, thirdItem);
    }

    private IEnumerator MatchObjects(GameObject a, GameObject b, GameObject c)
    {
        SoundManager.instance.PlayBottleFillSound();
 
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        float interpolationRatio = 0.0f;

        Vector3 startingPositionNewItem = a.transform.position;
        Vector3 startingPositionSecondItem = b.transform.position;
        Vector3 startingPositionThirdItem = c.transform.position;
        Vector3 matchPosition = (startingPositionNewItem + startingPositionSecondItem + startingPositionThirdItem) / 3.0f;

        while (Time.time - startTime <= 0.20f || interpolationRatio != 1.0f)
        {
            interpolationRatio = Mathf.Min(5 * (Time.time - startTime), 1.0f);
            a.transform.position = Vector3.Lerp(startingPositionNewItem, matchPosition, interpolationRatio);
            b.transform.position = Vector3.Lerp(startingPositionSecondItem, matchPosition, interpolationRatio);
            c.transform.position = Vector3.Lerp(startingPositionThirdItem, matchPosition, interpolationRatio);
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
            GameObject secondItem = m_MatchSlotItems[newItemIndex - 1];
            GameObject thirdItem = m_MatchSlotItems[newItemIndex - 2];

            Destroy(newItem);
            Destroy(secondItem);
            Destroy(thirdItem);
        }

        // set board pieces to null
        {
            m_MatchSlotItems[newItemIndex] = null;
            m_MatchSlotItems[newItemIndex - 1] = null;
            m_MatchSlotItems[newItemIndex - 2] = null;
        }
        
        // make array valid
        {
            for (int i = newItemIndex + 1; i < k_MatchSlotsLength; i++)
            {
                m_MatchSlotItems[i - 3] = m_MatchSlotItems[i];
            }

            // make last three items null, as they represent free space now
            m_MatchSlotItems[k_MatchSlotsLength - 1] = null;
            m_MatchSlotItems[k_MatchSlotsLength - 2] = null;
            m_MatchSlotItems[k_MatchSlotsLength - 3] = null;
        }

        // finally, move items to their place on board properly
        {
            Vector3[] startingPositions = new Vector3[k_MatchSlotsLength - 1 - newItemIndex];
            for (int i = newItemIndex - 2, j = 0; j < startingPositions.Length; i++, j++)
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
                for (int i = newItemIndex - 2, j = 0; i < k_MatchSlotsLength; i++, j++)
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
        SafeToAddNewItemToBoard = true;
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
