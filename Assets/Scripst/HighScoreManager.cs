using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] private int maxScoresToSave = 7;

    public int scoreTest;
  
    [SerializeField] private Transform scoreListContent;
    [SerializeField] private GameObject scoreEntryPrefab;

    private List<int> highScores = new List<int>();

    private void Start()
    {
        LoadHighScores();
        DisplayHighScores();

        LoadHighScores();
        DisplayHighScores();

        if (highScores.Count == 0)
        {
            AddHighScore(0);
            AddHighScore(0);
            AddHighScore(0);
            AddHighScore(0);
            AddHighScore(0);
        }
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddHighScore(scoreTest);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ClearHighScores();
        }
    }
    public void AddHighScore(int newScore)
    {
        if (highScores.Count >= maxScoresToSave)
        {
            // Remove the oldest score (last element)
            highScores.RemoveAt(highScores.Count - 1);
        }

        // Insert the new score at the beginning (newest first)
        highScores.Insert(0, newScore); // Newest score at the top

        SaveHighScores();
        DisplayHighScores();
    }

    private void SaveHighScores()
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore_" + i, highScores[i]);
        }
        PlayerPrefs.Save();
    }

    private void LoadHighScores()
    {
        highScores.Clear(); // Clear the list before loading

        for (int i = 0; i < maxScoresToSave; i++)
        {
            if (PlayerPrefs.HasKey("HighScore_" + i))
            {
                highScores.Add(PlayerPrefs.GetInt("HighScore_" + i));
            }
        }
    }

    private void DisplayHighScores()
    {
        // Clear existing score entries more efficiently
        for (int i = scoreListContent.childCount - 1; i >= 0; i--)
        {
            Destroy(scoreListContent.GetChild(i).gameObject);
        }


        for (int i = 0; i < highScores.Count; i++)
        {
            GameObject entry = Instantiate(scoreEntryPrefab, scoreListContent);
            TMP_Text scoreText = entry.GetComponentInChildren<TMP_Text>();

            if (scoreText != null)
            {
                scoreText.text = $"{i + 1}. {highScores[i]}"; // String interpolation for cleaner formatting
            }
            else
            {
                Debug.LogError("TMP_Text component not found on score entry prefab!");
            }
        }
    }
    public void ClearHighScores()
    {
        highScores.Clear(); 
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save(); 
        DisplayHighScores(); 
    }

}