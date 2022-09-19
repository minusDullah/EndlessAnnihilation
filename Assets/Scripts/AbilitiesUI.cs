using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfimaGames.LowPolyShooterPack;
using UnityEngine.InputSystem;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField] private Slider grenadeSlider;
    [SerializeField] private Slider buffBoostSlider;
    [SerializeField] private Character character;
    [SerializeField] private WaveSpawner waveSpawner;

    void Start()
    {
        grenadeSlider.maxValue = character.grenadeCDTimer;
    }

    void Update()
    {
        if (character.grenadeCD)
        {
            if (grenadeSlider.value <= 0)
            {
                grenadeSlider.value = character.grenadeCDTimer;
            }
            grenadeSlider.value -= 1 * Time.deltaTime;
        }
        else
        {
            grenadeSlider.value = buffBoostSlider.minValue;
        }

        if (character.buffBoostCD)
        {
            buffBoostSlider.value = buffBoostSlider.maxValue;
        }
        else
        {
            buffBoostSlider.value = waveSpawner.buffKillCounter;
        }
    }
}
