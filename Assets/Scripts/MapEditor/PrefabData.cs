using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ObstacleType {Collision, NoCollision, Move, Start, End, StartMove, GoodCoin, BadCoin}
public enum Montable { No, Yes, Ground}

public class PrefabData : MonoBehaviour
{
    public ObstacleType obstacleType;
    public Montable montable;
    public int crossNumber; // to know if is not in something
    public Transform offset;

    [Tooltip("Size for mode grille")]
    public float size;


    [Header("Speed if Move")]
    public float speed;

    void Start()
    {

    }

    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (MapEditor.instance!= null && MapEditor.instance.obstaclesOnMap.Contains(other.gameObject))
        {
            crossNumber++;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (MapEditor.instance!=null  && MapEditor.instance.obstaclesOnMap.Contains(other.gameObject))
        {
            crossNumber--;

        }
    }

}
