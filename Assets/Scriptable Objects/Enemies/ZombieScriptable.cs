using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zombie Scriptable Object", menuName = ("ScriptableObjects/Zombie"))]
public class ZombieScriptable : ScriptableObject
{
    [Header("Defensive Stats")]
    public float health;
    public float minHealth;
    public float maxHealth;
    public float scoreWorth;
    public int destroyTimer;
    public int chanceOfPowerUp;
    public GameObject bloodFX;
    public GameObject minimapIcon;

    [Header("Attack Stats")]
    public float damage;
    public float minSpeed;
    public float maxSpeed;
    public float attackCooldown;
    public float animationBuffer;

    [Header("Audio")]
    public AudioClip[] enemyDie;

    [Header("Drops")]
    public GameObject[] powerUps;
}
