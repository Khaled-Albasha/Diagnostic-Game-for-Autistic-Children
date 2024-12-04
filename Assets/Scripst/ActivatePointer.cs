using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePointer : MonoBehaviour
{
    public GameObject pointer;
    // Start is called before the first frame update
    private void OnEnable()
    {
        pointer.SetActive(true);
    }
}
