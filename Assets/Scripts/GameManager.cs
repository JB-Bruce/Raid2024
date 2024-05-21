using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<RestrictedArea> restrictedAreas = new List<RestrictedArea>();


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {

        restrictedAreas.Add(new RestrictedArea
        {
            areaOrigine = Vector3.zero,
            areaRadius = 50
        });
    }

}


public struct RestrictedArea
{
    public Vector3 areaOrigine;
    public float areaRadius;
}

