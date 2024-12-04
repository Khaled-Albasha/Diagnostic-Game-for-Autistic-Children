using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAndClick : MonoBehaviour
{
    public GameObject explosionPrefab;
    GameObject switcher;
    MainMenuManager menuManager;

    private void Start()
    {
        switcher = GameObject.FindGameObjectWithTag("Switcher");
        menuManager = FindObjectOfType<MainMenuManager>();
    }

    void Update()
    {
        if (menuManager != null)
        {
            if (menuManager.mobileTouch)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    HandleInput(Input.GetTouch(0).position);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    HandleInput(Input.mousePosition);
                }
            }
        }
        else
        {
            Debug.LogError("MenuManager not found in the scene!");
        }
    }

    void HandleInput(Vector3 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject.CompareTag("clickable"))
            {
                string clickedObjectName = clickedObject.name;

                if (clickedObjectName.EndsWith(" (1)"))
                {
                    string baseName = clickedObjectName.Substring(0, clickedObjectName.Length - 4);
                    GameObject targetObject = GameObject.Find(baseName); // Find only once

                    if (targetObject != null && targetObject.activeInHierarchy)
                    {
                        GameObject explosion = Instantiate(explosionPrefab);
                        explosion.transform.SetParent(transform);
                        explosion.transform.position = clickedObject.transform.position;

                        doAction();

                    }
                }
            }
        }
    }

    private void doAction()
    {
        switcher.GetComponent<RandomObjectSwitcher>().ShowNextObject();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySoundEffect("Yay2");
        }
    }
}