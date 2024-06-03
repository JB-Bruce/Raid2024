using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] CinematicManager _cineManager;

    private void OnEnable()
    {
        if (_cineManager != null)
        {
            _cineManager.StartCinematicOnTrigger();
        }
    }
}
