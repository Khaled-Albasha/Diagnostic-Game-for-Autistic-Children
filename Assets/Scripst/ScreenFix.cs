using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFix : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f;
    private Camera mainCamera;

    void Awake() // Use Awake to ensure this runs before Start
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("AspectRatioScaler needs to be attached to a Camera!");
            enabled = false; // Disable the script if no camera is found
            return;
        }
        ScaleCamera();
    }

    public void ScaleCamera()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleRatio = windowAspect / targetAspect;

        if (scaleRatio < 1.0f)
        {
            // Letterboxing
            mainCamera.rect = new Rect(0, (1f - scaleRatio) / 2f, 1f, scaleRatio);
        }
        else
        {
            // Pillarboxing
            float inverseScaleRatio = 1f / scaleRatio;
            mainCamera.rect = new Rect((1f - inverseScaleRatio) / 2f, 0, inverseScaleRatio, 1f);
        }
    }

    // Optionally update the camera if the screen size changes during gameplay
    void Update()
    {
        if (Screen.width != prevScreenWidth || Screen.height != prevScreenHeight)
        {
            ScaleCamera();
            prevScreenWidth = Screen.width;
            prevScreenHeight = Screen.height;
        }
    }
    private int prevScreenWidth;
    private int prevScreenHeight;

}
