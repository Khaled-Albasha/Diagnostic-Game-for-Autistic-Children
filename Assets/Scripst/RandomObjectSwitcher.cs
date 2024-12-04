using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RandomObjectSwitcher : MonoBehaviour
{

    public GameObject[] objects;

    public MainMenuManager menuManager;
    private List<int> availableIndices;
    private int currentIndex = -1;
    public List<DragSprite> dragSprites;
    private Dictionary<DragSprite, Vector2> originalPositions;

    public string shownObjectName;

    GameObject manager;

    public int ePoints;
    public int mPoints;
    public int aPoints;
    public int sPoints;
    bool startTimer;
    public bool timerMode;
    public bool timerMode2;
    private float currentTimer = 0f;
    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager");
        originalPositions = new Dictionary<DragSprite, Vector2>();
        foreach (var dragSprite in dragSprites)
        {
            if (dragSprite != null)
            {
                originalPositions[dragSprite] = dragSprite.transform.position;
            }
        }
    }
    private void RepositionBlocks()
    {
        foreach (var dragSprite in dragSprites)
        {
            if (dragSprite != null && originalPositions.ContainsKey(dragSprite))
            {
                dragSprite.transform.position = originalPositions[dragSprite];
            }
        }
    }

    private void Update()
    {
        if (startTimer)
        {
            currentTimer += Time.deltaTime;
        }
    }

    public void ShowNextObject()
    {
        if (availableIndices.Count == 0)
        {
            if (timerMode)
            {
                if (currentTimer <= 50)
                {
                    sPoints += 10;
                    aPoints += 5;
                }
                else if (currentTimer <= 70)
                {
                    sPoints += 8;
                    aPoints += 4;
                }
                else if (currentTimer <= 90)
                {
                    sPoints += 5;
                    aPoints += 3;
                }
                else if (currentTimer <= 120)
                {
                    sPoints += 3;
                    aPoints += 2;
                }
                else
                {
                    sPoints += 1;
                    aPoints += 1;
                }
                manager.GetComponent<ScoreManager>().attentionAndFocusPoints += aPoints;
                manager.GetComponent<ScoreManager>().sensoryProcessingPoints += sPoints;
            }
            else
            {
                manager.GetComponent<ScoreManager>().emotionalPoints += (ePoints/2);
                if (timerMode2)
                {
                    if (currentTimer <= 40)
                    {
                        mPoints += 5;
                       
                    }
                    else if (currentTimer <= 50)
                    {
                        mPoints += 4;
                    }
                    else if (currentTimer <= 60)
                    {
                        mPoints += 3;
                    }
                    else if (currentTimer <= 80)
                    {
                        mPoints += 2;
                    }
                    else
                    {
                        mPoints += 1;
                    }
                }
                manager.GetComponent<ScoreManager>().motorSkillsPoints += mPoints;
            }


            menuManager.GoToNextLevel();

            return;
        }


        int randomIndex = Random.Range(0, availableIndices.Count);
        int nextIndex = availableIndices[randomIndex];


        if (currentIndex != -1)
        {
            objects[currentIndex].SetActive(false);
        }
        objects[nextIndex].SetActive(true);


        currentIndex = nextIndex;
        availableIndices.RemoveAt(randomIndex);
        shownObjectName = objects[nextIndex].name;

        RepositionBlocks();
    }


    private void ResetAvailableObjects()
    {
        availableIndices.Clear();
        for (int i = 0; i < objects.Length; i++)
        {
            availableIndices.Add(i);
        }
    }
    private void OnDisable()
    {
        currentTimer = 0;
        startTimer = false;
        ResetAvailableObjects();
        ResetAllPoints();
    }
    private void OnEnable()
    {
        startTimer = true;
        availableIndices = new List<int>();
        for (int i = 0; i < objects.Length; i++)
        {
            availableIndices.Add(i);
        }


        ShowNextObject();
    }
    public void ResetAllPoints()
    {
        ePoints = 0;
        mPoints = 0;
        aPoints = 0;
        sPoints = 0;
    }
    
}

