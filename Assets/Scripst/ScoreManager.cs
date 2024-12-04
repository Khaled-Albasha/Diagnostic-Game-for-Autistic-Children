using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public int emotionalPoints;
    public int attentionAndFocusPoints;
    public int motorSkillsPoints;
    public int flexibilityPoints;
    public int sensoryProcessingPoints;

    public int finalScore;

    public TMP_Text scoreText;

    public SpriteRenderer emotionalBarSprite;  // Directly reference SpriteRenderer
    public SpriteRenderer attentionBarSprite;
    public SpriteRenderer motorSkillsBarSprite;
    public SpriteRenderer flexibilityBarSprite;
    public SpriteRenderer sensoryProcessingBarSprite;

    public float startPos;
    public float maxBarWidth = 10f;

    public HighScoreManager hm;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CalculateAndShowScore();
        }
    }
    private void Start()
    {
        ResetScores();
        UpdateGraph();
    }

    public void ResetScores()
    {
        emotionalPoints = 0;
        attentionAndFocusPoints = 0;
        motorSkillsPoints = 0;
        flexibilityPoints = 0;
        sensoryProcessingPoints = 0;
    }

    public void CalculateAndShowScore()
    {
        finalScore = (emotionalPoints + attentionAndFocusPoints + motorSkillsPoints + flexibilityPoints + sensoryProcessingPoints);

        scoreText.text = "" + finalScore + "/50";
        UpdateGraph();
        hm.AddHighScore(finalScore);

    }
    public void UpdateGraph()
    {
        SetBarWidth(emotionalBarSprite, emotionalPoints);
        SetBarWidth(attentionBarSprite, attentionAndFocusPoints);
        SetBarWidth(motorSkillsBarSprite, motorSkillsPoints);
        SetBarWidth(flexibilityBarSprite, flexibilityPoints);
        SetBarWidth(sensoryProcessingBarSprite, sensoryProcessingPoints);
    }


    void SetBarWidth(SpriteRenderer barSprite, int score)
    {
        
        barSprite.size = new Vector2(score/2.5f, barSprite.size.y);

        // Correctly position the bar (assuming it's anchored to the left)
        barSprite.transform.localPosition = new Vector3(startPos + barSprite.size.x, barSprite.transform.localPosition.y, barSprite.transform.localPosition.z);
    }
}
