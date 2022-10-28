using UnityEngine;

public class showrewardedvideoad : MonoBehaviour
{
    public void OnShowrewardedad(int n)
    {
        switch(n)
        {
            case 5:
                FindObjectOfType<leveltimer>().ReviveButton();
                break;
            default:
                UnityEngine.Assertions.Assert.IsTrue(false);
                break;
        }
    }
}
