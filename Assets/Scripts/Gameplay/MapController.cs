using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public GameObject Map;
    public float speedMap;

    //public List<GameObject> patterns = new List<GameObject>();
    public List<float> patternsSpeed = new List<float>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
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

        SetSpeedPatterns(PatternManager.instance.contener);
    }


    public void SetSpeedPatterns(GameObject contener)
    {
        for (int i = 0; i < contener.transform.childCount; i++) //parcour pattern
        {
            for (int ii = 0; ii < contener.transform.GetChild(i).transform.childCount; ii++)  //parcour obstacle
            {
                if (contener.transform.GetChild(i).transform.GetChild(ii).GetComponent<PrefabData>().obstacleType == ObstacleType.Move 
                    && patternsSpeed[i]!=0)
                {
                    contener.transform.GetChild(i).transform.GetChild(ii).Translate(Vector3.back * Time.deltaTime * speedMap);
                }
                 
            }
        }


    }

}
