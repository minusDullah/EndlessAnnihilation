using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{

    [SerializeField] public AudioMixer _MasterMixer;
    
    private float startingVolume;
    public Slider[] slider;

    private void Start()
    {
        UpdateMasterSlider(slider[0]);
        UpdateMusicSlider(slider[1]);
        UpdatePlayerAudioSlider(slider[3]);
        UpdateEnemySlider(slider[4]);
    }

    public void UpdateMasterSlider(Slider volume)
    {
        _MasterMixer.GetFloat("Master", out startingVolume);
        _MasterMixer.SetFloat("Master", startingVolume);
        volume.value = startingVolume;
    }

    public void UpdateMusicSlider(Slider volume)
    {
        _MasterMixer.GetFloat("Music", out startingVolume);
        _MasterMixer.SetFloat("Music", startingVolume);
        volume.value = startingVolume;
    }

    public void UpdatePlayerAudioSlider(Slider volume)
    {
        _MasterMixer.GetFloat("PlayerAudio", out startingVolume);
        _MasterMixer.SetFloat("PlayerAudio", startingVolume);
        volume.value = startingVolume;
    }

    public void UpdateEnemySlider(Slider volume)
    {
        _MasterMixer.GetFloat("Enemy", out startingVolume);
        _MasterMixer.SetFloat("Enemy", startingVolume);
        volume.value = startingVolume;
    }


    public void SetMasterVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Master", volume.value);
    }

    public void SetMusicVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Music", volume.value);
    }

    public void SetPlayerVolume(Slider volume)
    {
        _MasterMixer.SetFloat("PlayerAudio", volume.value);
    }
    
    public void SetEnemyVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Enemy", volume.value);
    }

}