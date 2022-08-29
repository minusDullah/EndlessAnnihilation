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

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        buffBoostSlider.maxValue = character.buffBoostCDTimer;
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
            grenadeSlider.value = 0;
        }
        
        if (character.buffBoostCD)
        {
            if (buffBoostSlider.value <= 0)
            {
                buffBoostSlider.value = character.buffBoostCDTimer;
            }
            buffBoostSlider.value -= 1 * Time.deltaTime;
        }
        else
        {
            buffBoostSlider.value = 0;
        }


    }
}
