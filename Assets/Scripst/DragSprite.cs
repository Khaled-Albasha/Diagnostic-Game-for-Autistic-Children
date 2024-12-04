using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSprite : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 startPos;
    private bool flag;
    public bool isDraging;
    public GameObject helperPanel;

    public GameObject explosionPrefab;
    private Transform target;
    public float disableDistance = 2f;


    public List<GameObject> targetObjects = new List<GameObject>();
    private List<string> targetNames = new List<string>();
    GameObject switcher;
    MainMenuManager menuManager;

    private void Start()
    {
        mainCamera = Camera.main;
        startPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Target").GetComponent<Transform>();
        switcher = GameObject.FindGameObjectWithTag("Switcher");
        menuManager = FindObjectOfType<MainMenuManager>();

        ExtractNames();
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
        if (menuManager != null)
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


        if ((transform.position - target.position).sqrMagnitude < disableDistance * disableDistance && !isDraging && flag)
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.SetParent(transform.parent);
            explosion.transform.position = target.position;

            AudioManager.Instance.PlaySoundEffect("Yay");
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
            transform.position = startPos;
            flag = false;
        }
    }


    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (!helperPanel.activeInHierarchy)
            {


                switch (touch.phase)
                {


                    case TouchPhase.Began:


                        RaycastHit2D beginHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                        if (beginHit.collider != null && beginHit.collider.gameObject == this.gameObject)
                        {
                            isDraging = true;
                            flag = true;
                            offset = transform.position - mainCamera.ScreenToWorldPoint(touch.position);
                            offset.z = 0;
                            AudioManager.Instance.PlaySoundEffect("Drag");
                        }


                        break;


                    case TouchPhase.Moved:

                        if (isDraging == true)
                        {


                            Vector3 newPosition = mainCamera.ScreenToWorldPoint(touch.position) + offset;
                            newPosition.z = transform.position.z; // Maintain original z

                            Vector3 clampedPosition = new Vector3(
                               Mathf.Clamp(newPosition.x, -mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize * mainCamera.aspect),
                               Mathf.Clamp(newPosition.y, -mainCamera.orthographicSize, mainCamera.orthographicSize),
                               newPosition.z
                           );

                            transform.position = clampedPosition;
                        }


                        break;
                    case TouchPhase.Ended:
                        if (isDraging == true)
                        {
                            isDraging = false;

                            AudioManager.Instance.PlaySoundEffect("Drop");
                        }

                        break;


                }
            }


        }
    }




    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!helperPanel.activeInHierarchy)
            {
                isDraging = true;
                flag = true;
                offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
                offset.z = 0; // Ensure z-offset remains zero
                AudioManager.Instance.PlaySoundEffect("Drag");
            }

        }

        if (Input.GetMouseButton(0))
        {



            if (!helperPanel.activeInHierarchy)
            {


                Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
                newPosition.z = transform.position.z; // Maintain original z

                Vector3 clampedPosition = new Vector3(
                   Mathf.Clamp(newPosition.x, -mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize * mainCamera.aspect),
                   Mathf.Clamp(newPosition.y, -mainCamera.orthographicSize, mainCamera.orthographicSize),
                   newPosition.z
               );


                transform.position = clampedPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!helperPanel.activeInHierarchy)
            {
                isDraging = false;
                AudioManager.Instance.PlaySoundEffect("Drop");
            }
        }
    }


    private void OnDisable()
    {
        transform.position = startPos;
    }



}