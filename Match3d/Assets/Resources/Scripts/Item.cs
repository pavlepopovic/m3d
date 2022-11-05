using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 RotationValues;

    [HideInInspector]
    public bool HintBool;

    private Rigidbody m_RigidBody;
    private MeshCollider m_MeshCollider;

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
    }

    private void LateUpdate()
    {
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
        // Potentialy problematic if multiple objects are on top of each other
        if (MatchBoard.s_Instance.IsThereAtLeastOnePlaceOnBoard())
        {
            m_RigidBody.useGravity = false;
        }
    }

    // OnMouseDrag is called when the user has clicked on a Collider and is still holding down the mouse.
    public void OnMouseDrag()
    {
        if (!MatchBoard.s_Instance.IsThereAtLeastOnePlaceOnBoard())
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
        if (MatchBoard.s_Instance.IsThereAtLeastOnePlaceOnBoard())
        {
            MatchBoard.s_Instance.MoveItemToSlot(gameObject);
        }
    }

    public void SetRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(
            RotationValues.x,
            RotationValues.y,
            RotationValues.z);
    }
}
