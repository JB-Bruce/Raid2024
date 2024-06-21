using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UnitSoundPlayer : MonoBehaviour
{
    public AudioSource unitAudioSource;

    public static UnitSoundPlayer instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

    }

    private void Start()
    {
        unitAudioSource = GetComponent<AudioSource>();
    }
}
