using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{

    [SerializeField] CinematicManager _cineManager;
    [SerializeField] GameObject _objectToDisable;

    private void OnEnable()
    {
        if (_cineManager != null)
        {
            Invoke("LaunchCinematic", 0.75f);
        }
    }

    private void LaunchCinematic()
    {
        _objectToDisable.SetActive(false);
        _cineManager.StartCinematicOnTrigger();
    }
}
