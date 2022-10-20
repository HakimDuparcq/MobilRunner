using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PrefabData : MonoBehaviour
{
    public int crossNumber; // to know if is not in something
    public Transform offset;
    public float size;
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
