using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 RotationValues;

    [HideInInspector]
    public bool HintBool;

    private Rigidbody m_RigidBody;
    private MeshCollider m_MeshCollider;
    private bool m_PickedUp;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_MeshCollider = GetComponent<MeshCollider>();
        m_PickedUp = false;

        UnityEngine.Assertions.Assert.IsNotNull(m_RigidBody, "RigidBody is null!");
        UnityEngine.Assertions.Assert.IsNotNull(m_MeshCollider, "MeshCollider is null!");
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the Collider.
    public void OnMouseDown()
    {
        // Potentialy problematic if multiple objects are on top of each other
        if (MatchBoard.s_Instance.IsThereAtLeastOnePlaceOnBoard() && MatchBoard.s_Instance.SafeToAddNewItemToBoard)
        {
            m_PickedUp = true;
            m_RigidBody.useGravity = false;
        }
    }

    // OnMouseDrag is called when the user has clicked on a Collider and is still holding down the mouse.
    public void OnMouseDrag()
    {
        if (!m_PickedUp)
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
        if (m_PickedUp)
        {
            MatchBoard.s_Instance.PlaceItemInMatchBoardArray(gameObject);
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
