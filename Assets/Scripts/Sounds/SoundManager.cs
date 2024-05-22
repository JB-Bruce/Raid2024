using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("References: ")]
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;

    [Header("Clips: ")]
    [SerializeField] Sounds[] musics, sfxs;

    SettingsMenu settingsMenu;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float mainVolume = 1f; // Global volume multiplier

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        settingsMenu = SettingsMenu.instance;
        PlayMusic("Theme");
    }

    public void PlayMusic(string _name)
    {
        Sounds _s = Array.Find(musics, _x =>  _x.name == _name);
        if (_s == null) //If doesn't find music by name
        {
            Debug.Log("Music not Found");
        }
        else //else play the music by name
        {
            musicPlayer.clip = _s.clip;
            musicPlayer.Play();
        }
    }

    public void PlaySFX(string _name)
    {
        Sounds _s = Array.Find(sfxs, _x => _x.name == _name);
        if (_s == null) //If doesn't find sfx by name
        {
            Debug.Log("SFX not Found");
        }
        else //else play the sfx by name
        {
            sfxPlayer.PlayOneShot(_s.clip);
        }
    }

    public void ToggleMusic() //Allows muting Music
    {
        musicPlayer.mute = !musicPlayer.mute; 
    }

    public void ToggleSFX() //Allows muting sfx
    {
        sfxPlayer.mute = !sfxPlayer.mute;
    }

    public void MusicVolume(float volume) //Changes the Music's volume
    {
        musicVolume = volume;
        UpdateVolumes();
    }

    public void SFXVolume(float volume) //Changes the SFX's volume
    {
        sfxVolume = volume;
        UpdateVolumes();
    }

    public void MainVolume (float volume) //Changes the Main volume
    {
        mainVolume = volume;
        UpdateVolumes();
    }

    private void UpdateVolumes() //Updates the volume changes
    {
        musicPlayer.volume = musicVolume * mainVolume;
        sfxPlayer.volume = sfxVolume * mainVolume;
    }
}
