using UnityEngine;

// This script is attached to the collider below the floor.
// It's purpose is to check if a penetration happens, which it does by checking if
// there is a trigger event.
public class FloorPenetrationChecker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        UnityEngine.Assertions.Assert.IsTrue(false, $"{collision.gameObject.name} has fell below the floor!");
    }
}
