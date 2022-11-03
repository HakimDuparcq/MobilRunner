using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public GameObject Map;
    public float speedMap;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //speedMap = 1;
    }

    void Update()
    {
        Map.transform.Translate(Vector3.back * Time.deltaTime * speedMap);
    }
}
