using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlaylistStarter : MonoBehaviour
{
    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = SoundManager.instance;

        _soundManager.PlayMusicFromPlaylist("MainMenu");
    }
}
