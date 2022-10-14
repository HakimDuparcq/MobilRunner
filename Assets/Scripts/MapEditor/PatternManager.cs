using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public static PatternManager instance;
    public Pattern[] patterns;
    public GameObject contener;
    public bool createMap;



    public float offset;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (createMap)
        {
            CreateMap();
        }
    }

    void Update()
    {
        
    }





    public void CreateMap()
    {
        
        for (int i = 0; i < patterns.Length; i++)
        {
            OnLoadPattern(i);
        }

    }



    public void OnLoadPattern(int number)
    {
        offset += patterns[number].sizePattern;
        for (int ii = 0; ii < patterns[number].gameObjects.Count; ii++)
        {
            GameObject obstacle = Instantiate(patterns[number].gameObjects[ii], patterns[number].positions[ii]+  Vector3.forward * offset   , Quaternion.identity);
            obstacle.transform.parent = contener.transform;
        }
    }



}
