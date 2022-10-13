using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TypeObstacle{cube, barriere}

public class PrefabsCollision : MonoBehaviour
{
    public TypeObstacle typeObstacle;
    public int crossNumber; // to know if is not in something
    void Start()
    {

    }

    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (MapEditor.instance.obstaclesOnMap.Contains(other.gameObject))
        {
            crossNumber++;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (MapEditor.instance.obstaclesOnMap.Contains(other.gameObject))
        {
            crossNumber--;

        }
    }

}
