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


    public void SetMasterVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Master", volume.value);
    }

    public void SetMusicVolume(Slider volume)
    {
        _MasterMixer.SetFloat("Music", volume.value);
    }

}