using System.Collections;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class DoublePointsPowerUp : MonoBehaviour
{
    [Header("Perk Stats")]
    [SerializeField] private float PerkCooldown = 15f;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private BoxCollider boxCollider;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
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
        boxCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);
        scoreUI.doublePoints = false;
        Destroy(gameObject);
    }
}
