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

    private float offset;

    public List<ListGameObject> movePatterns = new List<ListGameObject>();

    public float speedMove;

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
        SetSpeedPatterns();

    }





    public void CreateMap()
    {
        
        for (int i = 0; i < patterns.Length; i++)
        {
            GameObject patterni = new GameObject();
            patterni.name = patterns[i].name;
            patterni.transform.parent = contener.transform;

            OnLoadPattern(patterni, i);

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

        if (patterns[number].difficulty==DifficultyLevel.End) {startMove.tag = "Finish";}

        for (int ii = 0; ii < patterns[number].gameObjects.Count; ii++)
        {
            GameObject obstacle = Instantiate(patterns[number].gameObjects[ii], patterns[number].positions[ii]+  Vector3.forward * offset   , Quaternion.identity);
            obstacle.transform.parent = patterni.transform;
        }

        


    }




    public void SetMovePatterns(GameObject pattern)
    {
        movePatterns.Add(new ListGameObject());
        movePatterns[movePatterns.Count - 1].pattern = pattern;
        movePatterns[movePatterns.Count - 1].listGameObject = new List<GameObject>();

        for (int i = 0; i < pattern.transform.childCount; i++)
        {
            if (pattern.transform.GetChild(i).GetComponent<PrefabData>().obstacleType == ObstacleType.Move)
            {
                movePatterns[movePatterns.Count - 1].listGameObject.Add(pattern.transform.GetChild(i).gameObject);
            }
        }


    }


    public void SetSpeedPatterns()
    {
        for (int i = 0; i < movePatterns.Count; i++)
        {
            for (int ii = 0; ii < movePatterns[i].listGameObject.Count; ii++)
            {
                movePatterns[i].listGameObject[ii].transform.Translate(Vector3.back * Time.deltaTime * speedMove);
            }
        }

    }


}
