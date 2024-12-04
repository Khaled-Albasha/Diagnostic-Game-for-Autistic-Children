using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public BubbleSpawner Spawner { get; set; }
    public float Speed { get; set; }
    public GameObject explosionPrefab;
    private MainMenuManager menuManager;

    private void Start()
    {
        menuManager = FindObjectOfType<MainMenuManager>();
    }


    private void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * Speed);

        if (transform.position.y < Spawner.destroyY)
        {
            Spawner.ReturnBubbleToPool(gameObject);
        }


        if (menuManager == null)
        {
            Debug.LogError("MenuManager not found in the scene!");

        }
        else if (menuManager.mobileTouch)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }

    }



    private void HandleTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++) // Check all touches
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(touch.position));

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {

                    PopBubble();

                }
            }
        }
    }



    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));



            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {

                PopBubble();

            }

        }
    }


    private void PopBubble()
    {
        Spawner.score++;
        AudioManager.Instance.PlaySoundEffect("Pop");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Spawner.ReturnBubbleToPool(gameObject);
    }


}