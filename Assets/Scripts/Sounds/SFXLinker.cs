using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLinker : MonoBehaviour
{
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager = SoundManager.instance;
    }

    public void GetPlaySFX(string name)
    {
        _soundManager.PlaySFX(name, _soundManager._sfxPlayer);
    }
}
