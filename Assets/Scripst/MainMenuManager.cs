using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
public class MainMenuManager : MonoBehaviour
{
    public List<GameObject> gameObjectsToManage = new List<GameObject>();
    public GameObject scoreScreen;


    public GameObject mainMenu;
    public GameObject helperPanel;


    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelTitleText;
    public TextMeshProUGUI levelDescriptionText;

    public GameObject helper;

    public bool mobileTouch;

   
    void Awake()
    {


    }

    
    void Start()
    {
        UpdateLevelInfo();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }

    void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void EnablePanel(GameObject objectToEnable)
    {
        
        objectToEnable.SetActive(true);
        UpdateLevelInfo();

    }
    public void DisablePanel(GameObject objectToDisable)
    {
       
        objectToDisable.SetActive(false);
        UpdateLevelInfo();

    }

    public void RestartLevel()
    {
        UpdateLevelInfo();
        GameObject activeLevel = gameObjectsToManage.FirstOrDefault(level => level.activeInHierarchy);
        
        if (activeLevel != null)
        {
            activeLevel.SetActive(false);
            activeLevel.SetActive(true);
            UpdateLevelInfo();
        }

    }
    private void UpdateLevelInfo()
    {
        GameObject activeLevel = gameObjectsToManage.FirstOrDefault(level => level.activeInHierarchy);


        if (activeLevel != null)
        {
            mainMenu.SetActive(false);
           


            LevelData levelData = activeLevel.GetComponent<LevelData>();

            if (levelData != null)
            {
                levelNameText.text = levelData.LevelName;
                levelTitleText.text = levelData.LevelTitle;
                levelDescriptionText.text = levelData.LevelDescription;
            }
        }
        else
        {
            helper.SetActive(false);
            levelNameText.text = "";
            levelTitleText.text = "";
            levelDescriptionText.text = "";
        }
    }
    public void DisableAllLevels()
    {

        gameObjectsToManage.ForEach(level => level.SetActive(false));
        UpdateLevelInfo();
    }
    public void GoToNextLevel()
    {
        
        
        int currentIndex = gameObjectsToManage.FindIndex(level => level.activeInHierarchy);

       
        if (currentIndex >= 0 && currentIndex < gameObjectsToManage.Count - 1)
        {
            AudioManager.Instance.PlaySoundEffect("Win");
            helperPanel.SetActive(true);
            if (helperPanel.activeInHierarchy)
            {
                helper.SetActive(false);
            }
            gameObjectsToManage[currentIndex].SetActive(false);

           
            gameObjectsToManage[currentIndex + 1].SetActive(true);
           
           
            UpdateLevelInfo();
        }
        else if (currentIndex == gameObjectsToManage.Count - 1)
        {

            helper.SetActive(false);
            gameObjectsToManage[currentIndex].SetActive(false);
            scoreScreen.SetActive(true);
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.PlaySoundEffect("Win2");
            GetComponent<ScoreManager>().CalculateAndShowScore();

        }
        else
        {
            Debug.LogError("No active level found.");
        }
    }
}
