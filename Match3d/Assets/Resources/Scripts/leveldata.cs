using UnityEngine;

[System.Serializable]
public class leveldata
{
    public string Levelname;

    [Header("leveltimer")]
    public int minute;
    public int seconds;
    public GameObject[] TotalObjects;
}

