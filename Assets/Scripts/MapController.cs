using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject Map;
    public Vector3 deplacement;
    void Start()
    {
        
    }

    void Update()
    {
        Map.transform.position += deplacement;
    }
}
