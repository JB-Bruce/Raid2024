using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UnitSoundPlayer : MonoBehaviour
{
    public AudioSource unitAudioSource;

    private void Start()
    {
        unitAudioSource = GetComponent<AudioSource>();
        if (unitAudioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the GameObject.");
        }
    }
}
