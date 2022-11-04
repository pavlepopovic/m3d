using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 RotationValues;

    [HideInInspector]
    public bool HintBool;

    private bool m_UpPositionBool;
    private Rigidbody m_RigidBody;
    private MeshCollider m_MeshCollider;
    private GameObject m_MatchSlot;

    private float m_ClampMarginMinX = 0.0f;
    private float m_ClampMarginMaxX = 0.0f;
    private float m_ClampMarginMinY = 0.0f;
    private float m_ClampMarginMaxY = 0.0f;

    private float m_OffsetXMinValue = 0.18f;
    private float m_OffsetXMaxValue = -0.15f;
    private float m_OffsetYMinValue = 0.8f;
    private float m_OffsetYMaxValue = -0.25f;

    // The minimum and maximum values which the object can go
    private float m_ClampMinX;
    private float m_ClampMaxX;
    private float m_ClampMinY;
    private float m_ClampMaxY;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_MeshCollider = GetComponent<MeshCollider>();

        UnityEngine.Assertions.Assert.IsNotNull(m_RigidBody, "RigidBody is null!");
        UnityEngine.Assertions.Assert.IsNotNull(m_MeshCollider, "MeshCollider is null!");

        // Get the minimum and maximum position values according to the screen size represented by the main camera.
        m_ClampMinX = Camera.main.ScreenToWorldPoint(new Vector2(0 + m_ClampMarginMinX, 0)).x + m_OffsetXMinValue;        
        m_ClampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - m_ClampMarginMaxX, 0)).x+ m_OffsetXMaxValue;
        m_ClampMinY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0 + m_ClampMarginMinY)).z + m_OffsetYMinValue;
        m_ClampMaxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height + m_ClampMarginMaxY)).z+ m_OffsetYMaxValue;

        m_MatchSlot = null;
        Invoke("MakeSpawnFalse", 1f);
    }

    private void LateUpdate()
    {
        if (m_UpPositionBool == false)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.4f, 1f),
             Mathf.Clamp(transform.position.y, -1f, 1f), Mathf.Clamp(transform.position.z, -3f, 3f));
        }

        if (transform.position.x < m_ClampMinX)
        {
            // If the object position tries to exceed the left screen bound clamp the min x position to 0.
            // The maximum x position won't be clamped so the object can move to the right.
            // rb.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            transform.position = new Vector3(m_ClampMinX, transform.position.y, transform.position.z);
        }

        if (transform.position.x > m_ClampMaxX)
        {
            // Same goes here
            transform.position = new Vector3(m_ClampMaxX, transform.position.y, transform.position.z);
        }
        if (transform.position.z < m_ClampMinY)
        {
            // Same goes here
            transform.position = new Vector3(transform.position.x, transform.position.y, m_ClampMinY);
        }
        if (transform.position.z > m_ClampMaxY)
        {
            // Same goes here
            transform.position = new Vector3(transform.position.x, transform.position.y, m_ClampMaxY);
        }
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the Collider.
    public void OnMouseDown()
    {
        UnityEngine.Assertions.Assert.IsNull(m_MatchSlot);
        m_MatchSlot = MatchCheck.s_Instance.GetEmptySlot();
        if (m_MatchSlot != null)
        {
            m_RigidBody.useGravity = false;
        }
    }

    // OnMouseDrag is called when the user has clicked on a Collider and is still holding down the mouse.
    public void OnMouseDrag()
    {
        if (m_MatchSlot == null)
        {
            return;
        }

        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        transform.Rotate(0f, 1f, 0f);

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cursorPosition.x, 3.5f, cursorPosition.z);
    }

    // OnMouseUp is called when the user has released the mouse button.
    public void OnMouseUp()
    {
        if (m_MatchSlot != null)
        {
            StartCoroutine(MoveToSlot(0.05f, m_MatchSlot));
            m_MatchSlot = null;
        }
    }

    IEnumerator MoveToSlot(float delayTime, GameObject matchSlot)
    {
        UnityEngine.Assertions.Assert.IsNotNull(matchSlot, "Slot is null!");
        yield return new WaitForSeconds(delayTime);
        m_MeshCollider.enabled = false;
        
        SetRotation();

        float startTime = Time.time;
        float interpolatedRatio = 0.0f;
        Vector3 startingPosition = transform.position;
        Vector3 endingPosition = matchSlot.transform.position;

        Vector3 startingScale = transform.localScale;
        Vector3 endingScale = Vector3.one;

        while(Time.time - startTime <= 0.5 || interpolatedRatio != 1.0f)
        {
            interpolatedRatio = Mathf.Min(2 * (Time.time - startTime), 1.0f);
            transform.position = Vector3.Lerp(startingPosition, endingPosition, interpolatedRatio);
            transform.localScale = Vector3.Lerp(startingScale, endingScale, interpolatedRatio);
            yield return null;
        }

        UnityEngine.Assertions.Assert.AreEqual(interpolatedRatio, 1.0f, "Interpolated ratio is not 1!");
        m_MeshCollider.enabled = true;
    }

    // Happening during spawn - to be investigated, and hopefully removed
    void MakeSpawnFalse()
    {
        m_UpPositionBool = true;
    }

    public void SetRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(
            RotationValues.x,
            RotationValues.y,
            RotationValues.z);
    }
}
