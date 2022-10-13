using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapEditor : MonoBehaviour
{
    public static MapEditor instance;

    public Camera worldCamera;
    public GameObject actualObstacle;
    public GameObject[] obstacles;

    public Button[] SelectObstacles;
    public Button Save;
    public Button Load;

    public GameObject plane;

    public float sizeXmap;


    public List<GameObject> obstaclesOnMap = new List<GameObject>();

    public GameObject newObstacle;
    public GameObject followObstacle;

    public Material previouPlaceMaterial;
    public Material canPlaceMaterial;
    public Material canNotPlaceMaterial;

    public LayerMask LayerMask;
    public float hitDistance;

    public Pattern pattern;



    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < SelectObstacles.Length; i++)
        {
            int x = i;
            SelectObstacles[i].onClick.AddListener(delegate { ClickButtonUI(x); });
        }
        Save.onClick.AddListener(OnSave);
        Load.onClick.AddListener(OnLoad);
        
    }

    void Update()
    {
        ObjectFollowMouse();

    }



    public void ClickButtonUI(int i )
    {
        actualObstacle = obstacles[i];
    }

    

    public void ObjectFollowMouse()  // Mettre un layer mask pour que cube suive mieux souris
    {
        RaycastHit hit;
        
        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, hitDistance,LayerMask))
        {
            //if touch plane
            Vector3 positionObject = new Vector3(0, 0, 0);
            if (hit.transform.gameObject == plane )
            {
                if (hit.point.x < - sizeXmap * 0.33f )
                {
                    positionObject = new Vector3(-sizeXmap/2, hit.point.y, hit.point.z);
                }
                else if (hit.point.x > - sizeXmap * 0.33f && hit.point.x < sizeXmap * 0.33f)
                {
                    positionObject = new Vector3(0, hit.point.y, hit.point.z);
                }
                else if(hit.point.x > sizeXmap*0.33f)
                {
                    positionObject = new Vector3(sizeXmap/2, hit.point.y, hit.point.z);
                }

                //createobject
                if (followObstacle == null)
                {
                    followObstacle = Instantiate(actualObstacle, positionObject, Quaternion.identity);
                    followObstacle.AddComponent<Rigidbody>();
                    followObstacle.GetComponent<Rigidbody>().isKinematic = true;
                    followObstacle.GetComponent<Rigidbody>().useGravity = false;
                    followObstacle.GetComponent<Collider>().isTrigger = true;
                }
                //move it
                followObstacle.transform.position = positionObject;

                if (followObstacle.GetComponent<PrefabsCollision>().crossNumber == 0)
                {
                    followObstacle.GetComponent<MeshRenderer>().material = previouPlaceMaterial;
                }
                else
                {
                    followObstacle.GetComponent<MeshRenderer>().material = canNotPlaceMaterial;
                }



                if (Input.GetMouseButtonDown(0) && followObstacle.GetComponent<PrefabsCollision>().crossNumber==0)
                {
                    newObstacle = Instantiate(actualObstacle, positionObject, Quaternion.identity);
                    newObstacle.GetComponent<MeshRenderer>().material = canPlaceMaterial;
                    obstaclesOnMap.Add(newObstacle);
                }

            }
        }
        else
        {
            if (followObstacle!=null)
            {
                Destroy(followObstacle);
            }
        }

        

    }

    public void OnSave()
    {
        pattern.sizePattern = 100;

        for (int i = 0; i < obstaclesOnMap.Count; i++)
        {
            for (int ii = 0; ii < obstacles.Length; ii++)
            {
                if (obstacles[ii].GetComponent<PrefabsCollision>().typeObstacle == obstaclesOnMap[i].GetComponent<PrefabsCollision>().typeObstacle)
                {
                    pattern.gameObjects.Add(obstacles[ii]);
                    Debug.Log("noice");
                }
            }


            pattern.positions.Add(obstaclesOnMap[i].transform.position);

        }
    }

    public void OnLoad()
    {

    }

}
