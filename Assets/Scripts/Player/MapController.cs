using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject Map;
    public float speedMap;
    void Start()
    {
        
    }

    void Update()
    {
        Map.transform.Translate(Vector3.back * Time.deltaTime * speedMap);
    }
}
