using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdate : MonoBehaviour
{
    [SerializeField] public int scoreTotal;
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        scoreText.text = ("" + 0);
    }

    public void updateScore()
    {
        scoreText.text = ("" + scoreTotal);
    }
}
