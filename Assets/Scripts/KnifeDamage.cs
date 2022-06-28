using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    [SerializeField] private float knifeDamage;


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Hitbox" || other.gameObject.tag == "HitboxHead")
		{
			//Get damageable component on object
			IDamageable damageable = other.transform.GetComponentInParent<IDamageable>();
			//Damage object
			damageable?.TakeDamage(knifeDamage);
		}
	}
}
