using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DifficultyLevel {  Easy, Normal, Expert, Legendaire, End=-1 }


[CreateAssetMenu(fileName = "Pattern")]
public class Pattern : ScriptableObject
{
    public float sizePattern;
    public float startMove;

    public DifficultyLevel difficulty;

    public List<GameObject> gameObjects = new List<GameObject>();
    public List<Vector3> positions = new List<Vector3>();
    public List<Quaternion> rotation = new List<Quaternion>();

    [HideInInspector] public Texture2D preview2D;

}
