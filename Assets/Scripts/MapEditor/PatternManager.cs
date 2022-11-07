using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public static PatternManager instance;
    public Pattern[] patterns;
    public GameObject contener;
    public bool createMap;

    public GameObject startMoveObject;


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
            GameObject patterni = new GameObject();
            patterni.name = patterns[i].name;
            patterni.transform.parent = contener.transform;

            OnLoadPattern(patterni, i);

            MapController.instance.patternsSpeed.Add(0.0f);
        }

        

    }



    public void OnLoadPattern(GameObject patterni,int number)
    {
        offset += patterns[number].sizePattern;

        //Start Move
        GameObject startMove = Instantiate(startMoveObject,  Vector3.forward * (offset + patterns[number].startMove), Quaternion.identity);  
        startMove.transform.parent = patterni.transform;
        startMove.GetComponent<BoxCollider>().enabled = true;
        startMove.GetComponent<BoxCollider>().isTrigger = true;
        //startMove.GetComponent<MeshRenderer>().enabled = false;

        for (int ii = 0; ii < patterns[number].gameObjects.Count; ii++)
        {
            GameObject obstacle = Instantiate(patterns[number].gameObjects[ii], patterns[number].positions[ii]+  Vector3.forward * offset   , Quaternion.identity);
            obstacle.transform.parent = patterni.transform;
        }

        


    }

   

}
