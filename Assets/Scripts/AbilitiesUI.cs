using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfimaGames.LowPolyShooterPack;
using UnityEngine.InputSystem;
using FishNet.Object;

public class AbilitiesUI : NetworkBehaviour
{
    [SerializeField] private Slider grenadeSlider;
    [SerializeField] private Slider buffBoostSlider;
    [SerializeField] private Character character;
    [SerializeField] private WaveSpawner waveSpawner;

    void Start()
    {
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();
        grenadeSlider.maxValue = character.grenadeCDTimer;
    }

    void Update()
    {
        if (!IsOwner)
            return;

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
