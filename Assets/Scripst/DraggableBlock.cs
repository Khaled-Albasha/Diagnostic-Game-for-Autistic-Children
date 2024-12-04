using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    public Vector2 clamp;

    private Rigidbody2D rb;
    private BoxCollider2D blockCollider;
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    public BlockOrderChecker checker;
    public GameObject helperPanel;
    private MainMenuManager menuManager;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        blockCollider = GetComponent<BoxCollider2D>();
        mainCamera = Camera.main;
        menuManager = FindObjectOfType<MainMenuManager>();
    }

    void Update()
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



        if (isDragging)
        {
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);  // Mouse position used here for both
            targetPosition.z = 0f;
            rb.position = Vector3.Lerp(rb.position, targetPosition + offset, 10f * Time.deltaTime);

            Vector3 clampedPosition = new Vector3(
               Mathf.Clamp(targetPosition.x, -clamp.x, clamp.x),
               Mathf.Clamp(targetPosition.y, -clamp.y, clamp.y),
               targetPosition.z
           );

            transform.position = clampedPosition;
        }
    }


    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isDragging && !helperPanel.activeInHierarchy)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                        if (hit.collider != null && hit.collider.gameObject == gameObject)
                        {


                            StartDragging(touch.position);
                        }


                    }
                    break;


                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        StopDragging();
                    }
                    break;

            }
        }

    }
    private void StartDragging(Vector3 clickPosition)
    {
        isDragging = true;
        checker.startT = false;
        checker.isCorrectOrder = false;
        checker.timer = checker.delay;
        AudioManager.Instance.PlaySoundEffect("Drag");

        offset = transform.position - mainCamera.ScreenToWorldPoint(clickPosition); // Use the clickPosition provided
        offset.z = 0f;


        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        blockCollider.enabled = false;
        transform.rotation = Quaternion.identity;

        rb.isKinematic = true;
    }




    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isDragging && !helperPanel.activeInHierarchy)
            {

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    StartDragging(Input.mousePosition);
                }
            }



        }


        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                StopDragging();
            }
        }
    }

    private void StopDragging()
    {

        isDragging = false;
        checker.startT = true;
        AudioManager.Instance.PlaySoundEffect("Drop");

        blockCollider.enabled = true;
        rb.isKinematic = false;

    }
}