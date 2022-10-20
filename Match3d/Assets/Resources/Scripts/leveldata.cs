using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    [System.Serializable]
    public class TotalBottles
    {
        public string Name;
        [Tooltip("Enter Value of Total Colors In bottles from 0 to 4")]
        public Color[] ColorsInBottle;
    }

    [System.Serializable]
    public class leveldata
    {
        public string Levelname;
    [Header("leveltimer")]
    public int minute;
    public int seconds;
    public GameObject[] TotalObjects;
    }

