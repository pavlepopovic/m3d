using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 RotationValues;
    public bool applyforce;

    [HideInInspector]
    public bool HintBool;

    private bool m_UpPositionBool;
    private Rigidbody m_RigidBody;
    private Vector3 m_ScreenPoint;
    private Vector3 m_Offset;

    private float m_OriginalScaleX;
    private float m_OriginalScaleY;
    private float m_OriginalScaleZ;
     
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
        m_OriginalScaleX = m_RigidBody.gameObject.transform.localScale.x;
        m_OriginalScaleX = m_RigidBody.gameObject.transform.localScale.x;
        m_OriginalScaleX = m_RigidBody.gameObject.transform.localScale.x;

        // Get the minimum and maximum position values according to the screen size represented by the main camera.
        m_ClampMinX = Camera.main.ScreenToWorldPoint(new Vector2(0 + m_ClampMarginMinX, 0)).x+ m_OffsetXMinValue;        
        m_ClampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - m_ClampMarginMaxX, 0)).x+ m_OffsetXMaxValue;
        m_ClampMinY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0 + m_ClampMarginMinY)).z+ m_OffsetYMinValue;
        m_ClampMaxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height + m_ClampMarginMaxY)).z+ m_OffsetYMaxValue;

        Invoke("MakeSpawnFalse", 1f);
    }

    private void LateUpdate()
    {
        if (m_UpPositionBool == false)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.4f, 1f),
             Mathf.Clamp(transform.position.y, -1f, 1f), Mathf.Clamp(transform.position.z, -3f, 3f));
        }

        if (applyforce == true && HintBool != true)
        {
            GetComponent<Rigidbody>().AddForce(Physics.gravity * 100f, ForceMode.Acceleration);
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

    public void OnMouseDown()
    {
        m_RigidBody.useGravity = false;
        m_RigidBody.constraints = RigidbodyConstraints.None;
        m_ScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        m_Offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z));
    }

    public void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + m_Offset;
        m_RigidBody.position = cursorPosition;
        m_RigidBody.constraints = RigidbodyConstraints.None;

        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        transform.Rotate(0f, 1f, 0f);
        //Change Value of Selection height from here
        m_RigidBody.MovePosition(new Vector3(m_RigidBody.position.x, 3.5f, m_RigidBody.position.z));
        transform.position = new Vector3(m_RigidBody.position.x, 3.5f, m_RigidBody.position.z);
    }

    public void OnMouseUp()
    {
        m_RigidBody.useGravity = true;
        m_RigidBody.constraints = RigidbodyConstraints.None;
        applyforce = true;
        Invoke("MakeForceFalse", 0.07f);

        transform.localScale = new Vector3(m_OriginalScaleX , m_OriginalScaleY, m_OriginalScaleZ);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void MakeForceFalse()
    {
        applyforce = false;
    }

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
