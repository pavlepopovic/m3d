using UnityEngine;

public class BoundryCubePosition : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("leftcube")]
    public GameObject LeftCube;
    [UnityEngine.Serialization.FormerlySerializedAs("rightcube")]
    public GameObject RightCube;

    private const float k_ClampMarginMinX = 0.0f;
    private const float k_ClampMarginMaxX = 0.0f;
    private const float k_OffsetXMinValue = 0.0f;
    private const float k_OffsetXMaxValue = 0.0f;

    void Start()
    {
        float clampMinX = Camera.main.ScreenToWorldPoint(new Vector2(0 + k_ClampMarginMinX, 0)).x + k_OffsetXMinValue;
        float clampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - k_ClampMarginMaxX, 0)).x + k_OffsetXMaxValue;

        RightCube.transform.position = new Vector3(clampMaxX, RightCube.transform.position.y, RightCube. transform.position.z);
        LeftCube.transform.position = new Vector3(clampMinX, LeftCube.transform.position.y, LeftCube.transform.position.z);
    }
}

