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
    public float distanceFirstPattern;

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
        offset = distanceFirstPattern;
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
        

        //Start Move
        GameObject startMove = Instantiate(startMoveObject,  Vector3.forward * (offset + patterns[number].startMove), Quaternion.identity);  
        startMove.transform.parent = patterni.transform;
        startMove.GetComponent<Collider>().enabled = true;
        startMove.GetComponent<Collider>().isTrigger = true;
        startMove.GetComponent<MeshRenderer>().enabled = false;

        if (patterns[number].difficulty==DifficultyLevel.End) {startMove.tag = "Finish";}

        for (int ii = 0; ii < patterns[number].gameObjects.Count; ii++)
        {
            GameObject obstacle = Instantiate(patterns[number].gameObjects[ii], patterns[number].positions[ii]+  Vector3.forward * offset   , patterns[number].rotation[ii]  );
            obstacle.transform.parent = patterni.transform;
            if (obstacle.GetComponent<PrefabData>().obstacleType==ObstacleType.NoCollision)
            {
                foreach (var collider in obstacle.GetComponents<Collider>()) { collider.enabled = false; }
            }
        }
        offset += patterns[number].sizePattern;

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
                movePatterns[i].listGameObject[ii].transform.Translate(Vector3.forward * Time.deltaTime * speedMove);
            }
        }

    }


    public void ResetMap()
    {
        for (int i = 0; i < contener.transform.childCount; i++)
        {
            Destroy(contener.transform.GetChild(i).gameObject);
        }
        movePatterns = new List<ListGameObject>();
        MapController.instance.Map.transform.position = Vector3.zero;
        CreateMap();
    }

}
