using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("References: ")]
    public AudioSource _musicPlayer;
    public AudioSource _sfxPlayer;

    [Header("Clips: ")]
    [SerializeField] Sounds[] _musics, _sfxs;
    [SerializeField] Playlist[] _playlists;

    [Header("Toggle Buttons: ")]
    [SerializeField] GameObject _mainButton;
    [SerializeField] GameObject _musicButton;
    [SerializeField] GameObject _sfxButton;

    [Header("Toggle Buttons Sprites: ")]
    [SerializeField] Sprite _muteSprite;
    [SerializeField] Sprite _unmuteSprite;

    SettingsMenu _settingsMenu;

    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;
    private float _mainVolume = 1f; // Global volume multiplier

    private int _mainMuteChecker = 1;
    private int _musicMuteChecker = 1;
    private int _sfxMuteChecker = 1;

    private Coroutine _currentPlaylistCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _settingsMenu = SettingsMenu.instance;

        if (PlayerPrefs.GetInt("mainToggle", 1) == 0)
        {
            UpdateButtonSprite(_mainButton, _musicPlayer.mute, ref _mainMuteChecker);
        }
        if (PlayerPrefs.GetInt("musicToggle", 1) == 0)
        {
            ToggleMusic();
        }
        if (PlayerPrefs.GetInt("sfxToggle", 1) == 0)
        {
            ToggleSFX();
        }
    }

    private void Update()
    {
        if (_mainMuteChecker == 1 && _musicMuteChecker == 0 && _sfxMuteChecker == 0)
        {
            UpdateButtonSprite(_mainButton, _musicPlayer.mute, ref _mainMuteChecker);

            PlayerPrefs.SetInt("mainToggle", _mainMuteChecker);
        }
    }

    public void PlayMusic(string _name)
    {
        Sounds _s = Array.Find(_musics, _x => _x.name == _name);
        if (_s == null) // If doesn't find music by name
        {
            Debug.Log("Music not Found");
        }
        else // else play the music by name
        {
            _musicPlayer.clip = _s.clip;
            _musicPlayer.Play();
        }
    }

    public void PlaySFX(string _name, AudioSource audioSource)
    {
        Sounds _s = Array.Find(_sfxs, _x => _x.name == _name);
        if (_s == null) // If doesn't find sfx by name
        {
            Debug.Log("SFX not Found");
        }
        else // else play the sfx by name
        {
            audioSource.PlayOneShot(_s.clip);
        }
    }

    public void ToggleMain() // Allows muting both Music and SFX
    {
        if ((_mainMuteChecker == 1 && _musicMuteChecker == 1 && _sfxMuteChecker == 0) || (_mainMuteChecker == 0 && _musicMuteChecker == 0 && _sfxMuteChecker == 1))
        {
            ToggleMusic();
        }
        else if ((_mainMuteChecker == 1 && _musicMuteChecker == 0 && _sfxMuteChecker == 1) || (_mainMuteChecker == 0 && _musicMuteChecker == 1 && _sfxMuteChecker == 0))
        {
            ToggleSFX();
        }
        else
        {
            ToggleMusic();
            ToggleSFX();
        }

        UpdateButtonSprite(_mainButton, _musicPlayer.mute, ref _mainMuteChecker);

        PlayerPrefs.SetInt("mainToggle", _mainMuteChecker);
    }

    public void ToggleMusic() // Allows muting Music
    {
        _musicPlayer.mute = !_musicPlayer.mute;

        if (_musicMuteChecker == 0 && _mainMuteChecker == 0)
        {
            UpdateButtonSprite(_mainButton, _musicPlayer.mute, ref _mainMuteChecker);
        }

        UpdateButtonSprite(_musicButton, _musicPlayer.mute, ref _musicMuteChecker);
        PlayerPrefs.SetInt("musicToggle", _musicMuteChecker);
    }

    public void ToggleSFX() // Allows muting sfx
    {
        _sfxPlayer.mute = !_sfxPlayer.mute;

        if (_sfxMuteChecker == 0 && _mainMuteChecker == 0)
        {
            UpdateButtonSprite(_mainButton, _sfxPlayer.mute, ref _mainMuteChecker);
        }

        UpdateButtonSprite(_sfxButton, _sfxPlayer.mute, ref _sfxMuteChecker);
        PlayerPrefs.SetInt("sfxToggle", _sfxMuteChecker);
    }

    private void UpdateButtonSprite(GameObject button, bool isMuted, ref int muteChecker) // Changes the sprite of the Toggle Buttons from mute to unmuted and vice versa
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? _unmuteSprite : _muteSprite;
            muteChecker = isMuted ? 0 : 1;
        }
        else
        {
            Debug.LogWarning("No Image Component has been found on the button");
        }
    }

    public void MusicVolume(float volume) // Changes the Music's volume
    {
        _musicVolume = volume;
        UpdateVolumes();
    }

    public void SFXVolume(float volume) // Changes the SFX's volume
    {
        _sfxVolume = volume;
        UpdateVolumes();
    }

    public void MainVolume(float volume) // Changes the Main volume
    {
        _mainVolume = volume;
        UpdateVolumes();
    }

    private void UpdateVolumes() // Updates the volume changes
    {
        _musicPlayer.volume = _musicVolume * _mainVolume;
        _sfxPlayer.volume = _sfxVolume * _mainVolume;
    }

    public void PlayMusicFromPlaylist(string playlistName)
    {
        Playlist playlist = Array.Find(_playlists, p => p.name == playlistName);
        if (playlist == null)
        {
            return;
        }

        // Stop the current coroutine if it's running
        if (_currentPlaylistCoroutine != null)
        {
            StopCoroutine(_currentPlaylistCoroutine);
        }

        _currentPlaylistCoroutine = StartCoroutine(PlayRandomSoundFromPlaylist(playlist));
    }

    private IEnumerator PlayRandomSoundFromPlaylist(Playlist playlist)
    {
        while (true)
        {
            if (playlist.sounds.Count == 0)
            {
                yield break;
            }

            Sounds randomSound = playlist.sounds[UnityEngine.Random.Range(0, playlist.sounds.Count)];
            _musicPlayer.clip = randomSound.clip;
            _musicPlayer.Play();

            yield return new WaitForSeconds(_musicPlayer.clip.length);
        }
    }
}
