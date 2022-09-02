using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdate : MonoBehaviour
{
    [SerializeField] public float scoreTotal = 0;
    [SerializeField] public float scoreBuffer = 0;
    [SerializeField] private float previousScoreGain = 0;
    [SerializeField] private TextMeshProUGUI scoreGainUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Animator anim;
    [SerializeField] public bool doublePoints;
    [SerializeField] public float pointsMultiplier;

    private void Start()
    {
        scoreText.text = ("" + scoreTotal);
    }

    public void UpdateScoreGain()
    {
        if(doublePoints == true)
        {
            scoreBuffer *= pointsMultiplier;
        }
        scoreTotal += scoreBuffer;
        scoreText.text = ("" + scoreTotal);
        scoreGainUI.text = ("+" + scoreBuffer);
        PlayAnimation();
    }

    public void UpdateScoreLose(int scoreLost)
    {
        scoreTotal -= scoreLost;
        scoreText.text = ("" + scoreTotal);
        scoreGainUI.text = ("-" + scoreLost);
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        int scoreGainAnim = Random.Range(0, 3);
        while (scoreGainAnim == previousScoreGain) { scoreGainAnim = Random.Range(0, 3); }
        anim.StopPlayback();
        if (scoreGainAnim == 0) { anim.Play("ScoreGain", 0); }
        if (scoreGainAnim == 1) { anim.Play("ScoreGain_2", 0); }
        if (scoreGainAnim == 2) { anim.Play("ScoreGain_3", 0); }
        previousScoreGain = scoreGainAnim;
        Invoke("ResetBuffer", .1f);
    }

    public void CalculateScore(float scoreWorth)
    {
        scoreBuffer += scoreWorth;
        if (IsInvoking("UpdateScoreGain"))
        {
            CancelInvoke();
        }
        Invoke("UpdateScoreGain", .1f);
    }

    private void ResetBuffer()
    {
        scoreBuffer = 0;
    }
}
