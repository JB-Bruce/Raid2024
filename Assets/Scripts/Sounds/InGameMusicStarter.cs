using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusicStarter : MonoBehaviour
{
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager = SoundManager.instance;

        _soundManager.PlayMusicFromPlaylist("InGame");
    }
}
