using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelData : MonoBehaviour
{
    public string LevelName;
    public string LevelTitle;
    [TextArea]
    public string LevelDescription;
}
