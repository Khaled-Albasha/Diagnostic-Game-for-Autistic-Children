using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int poolSize = 20;
    public float spawnXMin = -5f;
    public float spawnXMax = 5f;
    public float spawnY = 5f;
    public float destroyY = -5f;

    // New variables for random speed and size ranges
    public float minBubbleSpeed = 2f;
    public float maxBubbleSpeed = 5f;
    public float minBubbleSize = 0.5f;
    public float maxBubbleSize = 1.5f;

    public float spawnRate = 1;
    bool isSpawning = true;

    public float timerDuration = 10f;
    private float currentTimer;
    private bool isTimerRunning = false;

    
    public MainMenuManager menuManager;

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            UpdateScoreText();
        }
    }

    private int _score = 0;
    public TMP_Text scoreText;
    public TMP_Text timerText;

    public GameObject helperPanel;
    private List<GameObject> bubblePool;
    GameObject manager;

    private void Awake()
    {

        manager = GameObject.FindGameObjectWithTag("Game Manager");
        bubblePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bubble = Instantiate(bubblePrefab);
            bubble.SetActive(false);
            bubblePool.Add(bubble);
        }
        UpdateTimerText();
    }

    private IEnumerator SpawnBubbles()
    {
        while (isSpawning)
        {
            SpawnBubble();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnBubble()
    {
        if (!helperPanel.activeInHierarchy)
        {
            GameObject bubble = GetPooledBubble();

            if (bubble != null)
            {
                float randomX = Random.Range(spawnXMin, spawnXMax);
                bubble.transform.position = new Vector2(randomX, spawnY);

                // Set random speed and size
                float randomSpeed = Random.Range(minBubbleSpeed, maxBubbleSpeed);
                float randomSize = Random.Range(minBubbleSize, maxBubbleSize);
                bubble.transform.localScale = Vector3.one * randomSize; // Assuming uniform scaling

                bubble.SetActive(true);

                Bubble bubbleComponent = bubble.GetComponent<Bubble>();
                if (bubbleComponent == null)
                {
                    bubbleComponent = bubble.AddComponent<Bubble>();
                }
                bubbleComponent.Spawner = this;
                bubbleComponent.Speed = randomSpeed; // Pass the random speed to the bubble
            }
        }
    }

    private GameObject GetPooledBubble()
    {
        for (int i = 0; i < bubblePool.Count; i++)
        {
            if (!bubblePool[i].activeInHierarchy)
            {
                return bubblePool[i];
            }
        }

        if (bubblePool.Count < 100)
        {
            GameObject bubble = Instantiate(bubblePrefab);
            bubble.SetActive(false);
            bubblePool.Add(bubble);
            return bubble;
        }

        return null;
    }

    public void ReturnBubbleToPool(GameObject bubble)
    {
        bubble.SetActive(false);
    }

    private void OnDisable()
    {
        isSpawning = false;
        ReturnAllBubblesToPool();
        StopTimer();
        ResetScoreAndTimer();
    }

    private void OnEnable()
    {
        isSpawning = true;
        
        
            StartCoroutine(SpawnBubbles());
            StartTimer();
        
        
    }

    private void ReturnAllBubblesToPool()
    {
        foreach (GameObject bubble in bubblePool)
        {
            if (bubble != null)
            {
                bubble.SetActive(false);
            }
        }
    }


    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }

    private IEnumerator TimerCoroutine()
    {
        isTimerRunning = true;
        currentTimer = timerDuration;

        while (currentTimer > 0)
        {
            UpdateTimerText();
            if (!helperPanel.activeInHierarchy)
            {
                currentTimer -= Time.deltaTime;
            }

            yield return null;
        }
        isTimerRunning = false;
        TimerFinishedAction();
      
    }

    private void TimerFinishedAction()
    {
        if(score >= 40)
        {
            manager.GetComponent<ScoreManager>().attentionAndFocusPoints += 5;
        }
        else if (score >= 30)
        {
            manager.GetComponent<ScoreManager>().attentionAndFocusPoints += 4;
        }
        else if (score >= 20)
        {
            manager.GetComponent<ScoreManager>().attentionAndFocusPoints += 3;
        }
        else if (score >= 10)
        {
            manager.GetComponent<ScoreManager>().attentionAndFocusPoints += 2;
        }
        else if (score >= 5)
        {
            manager.GetComponent<ScoreManager>().attentionAndFocusPoints += 1;
        }
        menuManager.GoToNextLevel();
    }

    private void StartTimer()
    {
        if (!isTimerRunning)
        {
            StartCoroutine(TimerCoroutine());
        }
    }

    private void StopTimer()
    {
        isTimerRunning = false;
        StopCoroutine(TimerCoroutine());
    }

    private void ResetScoreAndTimer()
    {
        score = 0;
        currentTimer = timerDuration;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "" + Mathf.Ceil(currentTimer).ToString();
        }
    }
}
