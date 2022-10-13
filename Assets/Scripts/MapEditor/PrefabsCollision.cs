using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrefabsCollision : MonoBehaviour
{
    public bool isRightPlacement;
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
            Debug.Log(other.transform.name + "Enter");
            isRightPlacement = false;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (MapEditor.instance.obstaclesOnMap.Contains(other.gameObject))
        {
            isRightPlacement = true;
        }
    }

}
