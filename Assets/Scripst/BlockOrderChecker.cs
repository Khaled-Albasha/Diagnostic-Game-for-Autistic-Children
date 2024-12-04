using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockOrderChecker : MonoBehaviour
{
    public List<DraggableBlock> blocksToCheck;
    private Dictionary<DraggableBlock, Vector2> originalPositions;

    public GameObject checkMark;

    
    
    public MainMenuManager menuManager;

    public float timer = 3f;
    public float delay = 3f;
    bool startTimer;

    public bool isCorrectOrder;
    public bool startT;

    private float currentTimer = 0f;

    GameObject manager;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager");
        originalPositions = new Dictionary<DraggableBlock, Vector2>();
        foreach (var block in blocksToCheck)
        {
            if (block != null)
            {
                originalPositions[block] = block.transform.position;
            }
        }
    }

    private void RepositionBlocks()
    {
        foreach (var block in blocksToCheck)
        {
            if (block != null && originalPositions.ContainsKey(block))
            {
                block.transform.position = originalPositions[block];
                block.transform.rotation = Quaternion.identity;
            }
        }
    }

    public void CheckBlockOrder()
    {
        bool correctOrder = true;

        List<DraggableBlock> sortedBlocks = blocksToCheck.OrderBy(block => block.transform.position.y).ToList();

        for (int i = 0; i < blocksToCheck.Count; i++)
        {

            if (blocksToCheck[i] != sortedBlocks[i])
            {
                correctOrder = false;
                break;
            }
        }

        isCorrectOrder = correctOrder;
        


    }


    private void Update()
    {
        if (startTimer)
        {
            currentTimer += Time.deltaTime;
        }
        if (timer > 0)
        {
            CheckBlockOrder();


            if (isCorrectOrder && startT)
            {
                checkMark.SetActive(true);

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (currentTimer <= 30)
                    {
                        manager.GetComponent<ScoreManager>().motorSkillsPoints += 5;
                        manager.GetComponent<ScoreManager>().flexibilityPoints += 10;
                    }
                    else if (currentTimer <= 40)
                    {
                        manager.GetComponent<ScoreManager>().motorSkillsPoints += 4;
                        manager.GetComponent<ScoreManager>().flexibilityPoints += 8;
                    }
                    else if (currentTimer <= 60)
                    {
                        manager.GetComponent<ScoreManager>().motorSkillsPoints += 3;
                        manager.GetComponent<ScoreManager>().flexibilityPoints += 5;
                    }
                    else if (currentTimer <= 80)
                    {
                        manager.GetComponent<ScoreManager>().motorSkillsPoints += 2;
                        manager.GetComponent<ScoreManager>().flexibilityPoints += 3;
                        
                    }
                    else
                    {
                        manager.GetComponent<ScoreManager>().motorSkillsPoints += 1;
                        manager.GetComponent<ScoreManager>().flexibilityPoints += 1;
                    }
                    menuManager.GoToNextLevel();

                }
            }
            else
            {
                checkMark.SetActive(false);
                timer = delay;
            }
        }
    }




    private void OnEnable()
    {
        startTimer = true;

    }

    private void OnDisable()
    {
        currentTimer = 0;
        startTimer = false;
        RepositionBlocks();
        timer = delay;
    }
}
