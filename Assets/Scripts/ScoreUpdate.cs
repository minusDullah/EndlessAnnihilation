using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdate : MonoBehaviour
{
    [SerializeField] public int scoreTotal;
    private TextMeshProUGUI scoreText;
    private Animator anim;
    private TextMeshProUGUI scoreGainText;

    private void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        anim = GameObject.Find("ScoreGain").GetComponent<Animator>();
        scoreText.text = ("" + 0);
    }

    public void updateScore()
    {
        int scoreGainAnim = Random.Range(0, 3);
        if(scoreGainAnim == 0) { anim.Play("ScoreGain", 0); }
        if(scoreGainAnim == 1) { anim.Play("ScoreGain_2", 0); }
        if(scoreGainAnim == 2) { anim.Play("ScoreGain_3", 0); }
        scoreText.text = ("" + scoreTotal);
    }
}
