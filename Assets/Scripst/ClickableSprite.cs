using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSprite : MonoBehaviour
{
    public GameObject helperPanel;
    public List<GameObject> targetObjects = new List<GameObject>();
    private List<string> targetNames = new List<string>();
    GameObject switcher;
    MainMenuManager menuManager; // Reference to your MenuManager

    private void Start()
    {
        switcher = GameObject.FindGameObjectWithTag("Switcher");
        ExtractNames();
        menuManager = FindObjectOfType<MainMenuManager>(); // Find the MenuManager in the scene
    }

    private void ExtractNames()
    {
        targetNames.Clear();
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
            {
                targetNames.Add(obj.name);
            }
            else
            {
                targetNames.Add(string.Empty);
                Debug.LogWarning("A GameObject in targetObjects is null on " + gameObject.name);
            }
        }
    }

    private void Update()
    {
        if (menuManager != null) // Check if MenuManager was found
        {
            if (menuManager.mobileTouch)
            {
                HandleTouchInput();
            }
            else
            {
                HandleMouseInput();
            }
        }
        else
        {
            Debug.LogError("MenuManager not found in the scene!");
        }
    }


    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ProcessClick();
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ProcessClick();

            }
        }
    }

    private void ProcessClick()
    {
        if (switcher != null && !helperPanel.activeInHierarchy)
        {
            string shownObjectName = switcher.GetComponent<RandomObjectSwitcher>().shownObjectName;
            RandomObjectSwitcher rSwitcher = switcher.GetComponent<RandomObjectSwitcher>();
            if (targetNames[0] == shownObjectName)
            {
                rSwitcher.ePoints += 2;
            }
            else if (targetNames[1] == shownObjectName)
            {
                rSwitcher.ePoints += 1;
            }

            rSwitcher.ShowNextObject();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySoundEffect("Click");
            }
        }
    }


}