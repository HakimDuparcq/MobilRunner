using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pattern")]
public class Pattern : ScriptableObject
{
    public float sizePattern;

    public List<GameObject> gameObjects = new List<GameObject>();
    public List<Vector3> positions = new List<Vector3>();



}
