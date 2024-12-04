using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticlesOnDisable : MonoBehaviour
{

    void OnDisable()
    {
        Destroy(gameObject);
    }
}
