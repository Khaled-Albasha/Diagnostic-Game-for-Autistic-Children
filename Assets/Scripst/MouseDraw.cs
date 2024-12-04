using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDraw : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject linePrefab; // Now uses a prefab
    
    public Color[] availableColors;
    public Transform lineParent;
    public int initialPoolSize = 10;

    private Color currentColor = Color.black;
    private List<Vector3> linePoints = new List<Vector3>();
    private LineRenderer currentLine;
    private Stack<GameObject> linePool = new Stack<GameObject>();
    private int sortingOrder = 0;


    void Start()
    {
        mainCamera = Camera.main;
        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is not assigned!");
            return; // Early exit if no prefab
        }

        if (lineParent == null)
        {
            lineParent = new GameObject("Lines").transform;
        }

        // Pre-instantiate lines for pooling
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject lineObject = InstantiateLine();
            lineObject.SetActive(false);
            linePool.Push(lineObject);
        }
    }

    GameObject InstantiateLine()
    {
        GameObject lineObject = Instantiate(linePrefab, lineParent);
        LineRenderer lr = lineObject.GetComponent<LineRenderer>();
        if (lr == null)
        {
            Debug.LogError("Line prefab does not have a LineRenderer component!");
        }
        return lineObject;
    }


    void StartNewLine()
    {
        if (linePool.Count > 0)
        {
            currentLine = linePool.Pop().GetComponent<LineRenderer>();
            currentLine.gameObject.SetActive(true);
        }
        else
        {
            currentLine = InstantiateLine().GetComponent<LineRenderer>();
        }

        currentLine.sortingOrder = sortingOrder++;
        currentLine.material.color = currentColor; // Use the material from the prefab
        currentLine.positionCount = 0;

        linePoints.Clear();
        AppendToLine();
    }


    void AppendToLine()
    {
        if (currentLine == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        if (linePoints.Count == 0 || Vector3.Distance(linePoints[linePoints.Count - 1], mousePos) > 0.01f)
        {
            linePoints.Add(mousePos);
            currentLine.positionCount = linePoints.Count;
            currentLine.SetPositions(linePoints.ToArray());
        }
    }



    public void SetLineColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < availableColors.Length)
        {
            currentColor = availableColors[colorIndex];
            if (currentLine != null)
            {
                currentLine.material.color = currentColor; // Set color on the current line
            }
        }
        else
        {
            Debug.LogWarning("Invalid color index: " + colorIndex);
        }
    }

    public void ClearAllLines()
    {
        sortingOrder = 0; // Reset sorting order
        foreach (Transform child in lineParent)
        {
            child.gameObject.SetActive(false);
            linePool.Push(child.gameObject);
        }
    }
    private void OnDisable()
    {
        ClearAllLines();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartNewLine();
        }
        else if (Input.GetMouseButton(0))
        {
            AppendToLine();
        }
    }
}
