using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    // Array to hold the potential starting position objects
    public Transform[] startPositions;
    public Transform targetPosition;

    public GameObject introPanel;
    public bool flag;

    public float moveSpeed = 5f; // Adjust movement speed as needed

    private bool isMoving = false;


    void Start()
    {
       

    }
    private void OnEnable()
    {
        StopCoroutine(DelayedStart());

        // Ensure startPositions and targetPosition are assigned in the Inspector.
        if (startPositions.Length == 0 || targetPosition == null)
        {
            Debug.LogError("Start positions or target position not assigned!");
            enabled = false; // Disable the script to prevent errors
            return;
        }
        flag = true;
    }

    IEnumerator DelayedStart()
    {

        yield return null; // Wait for one frame

        // Randomly select a start position from the array
        int randomIndex = Random.Range(0, startPositions.Length);
        transform.position = startPositions[randomIndex].position;

        isMoving = true;

    }





    void Update()
    {
        if (isMoving)
        {
            // Smoothly move towards the target using Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);


            // Check if the object has reached the target (using a small threshold for accuracy). Using sqrMagnitude is more efficient
            if (Vector3.SqrMagnitude(transform.position - targetPosition.position) < 0.01f)
            {
                isMoving = false; // Stop further movement
                StartCoroutine(DisappearAfterDelay());
            }

        }
        if (!introPanel.activeInHierarchy && flag)
        {
            StartCoroutine(DelayedStart());
            flag = false;
        }
       
    }


    IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false); 

      
    }
}
