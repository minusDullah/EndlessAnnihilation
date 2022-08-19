using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfimaGames.LowPolyShooterPack;
using UnityEngine.InputSystem;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField] private Slider grenadeSlider;
    [SerializeField] private Slider damageBoostSlider;
    [SerializeField] private Character character;

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        damageBoostSlider.maxValue = character.damageBoostCDTimer;
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
        
        if (character.damageBoostCD)
        {
            if (damageBoostSlider.value <= 0)
            {
                damageBoostSlider.value = character.damageBoostCDTimer;
            }
            damageBoostSlider.value -= 1 * Time.deltaTime;
        }
        else
        {
            damageBoostSlider.value = 0;
        }


    }
}
