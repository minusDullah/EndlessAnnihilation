using System.Collections;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class DoublePointsPowerUp : MonoBehaviour
{
    [Header("Perk Stats")]
    [SerializeField] private float PerkCooldown = 15f;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private SphereCollider sphereCollider;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
        scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scoreUI.doublePoints = true;
            StartCoroutine(PerkLength());
        }
    }

    IEnumerator PerkLength()
    {
        sphereCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);
        scoreUI.doublePoints = false;
        Destroy(gameObject);
    }
}
