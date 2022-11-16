using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public GameObject Map;
    [HideInInspector ]public float speedMap;
    public float speedMapRestart;
    public Vector3 startMapPosition;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        speedMap = speedMapRestart;
    }

    void Update()
    {
        if (GameManager.instance !=null)
        {
            if (GameManager.instance.gameState == GameState.InGame)
            {
                Map.transform.Translate(Vector3.back * Time.deltaTime * speedMap);
            }
        }
        else
        {
            if (PatternManager.instance.createMap)
            {
                Map.transform.Translate(Vector3.back * Time.deltaTime * speedMap);
            }

        }

    }


    

}
