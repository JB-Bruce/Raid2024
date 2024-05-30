using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<RestrictedArea> restrictedAreas = new List<RestrictedArea>();
    public float mapSize = 100f;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

}

[System.Serializable]
public struct RestrictedArea
{
    public Transform areaOrigine;
    public float areaRadius;
}

