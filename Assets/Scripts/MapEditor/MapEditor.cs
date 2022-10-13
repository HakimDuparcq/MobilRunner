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

    public GameObject plane;

    public float sizeXmap;


    public List<GameObject> obstaclesOnMap = new List<GameObject>();

    public GameObject newObstacle;
    public GameObject followObstacle;

    public Material cantPlaceMaterial;

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //sizeXmap = plane.transform.localScale.z;
        //Debug.Log(sizeXmap + "size");

        //Choose the obstacle wwith UI
        for (int i = 0; i < SelectObstacles.Length; i++)
        {
            int x = i;
            SelectObstacles[i].onClick.AddListener(delegate { ClickButtonUI(x); });
        }

    }

    void Update()
    {
        RayCasting();
    }

    private void FixedUpdate()
    {
        ObjectFollowMouse();
    }

    public void ClickButtonUI(int i )
    {
        actualObstacle = obstacles[i];
    }

    public void RayCasting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == plane)
                {
                    Vector3 positionObject=new  Vector3(0,0,0);
                    if (hit.point.x < -sizeXmap * 0.33f)
                    {
                        positionObject = new Vector3(-sizeXmap / 2, hit.point.y, hit.point.z);
                    }
                    else if (hit.point.x > -sizeXmap * 0.33f && hit.point.x < sizeXmap * 0.33f)
                    {
                        positionObject = new Vector3(0, hit.point.y, hit.point.z);
                    }
                    else if (hit.point.x > sizeXmap * 0.33f)
                    {
                        positionObject = new Vector3(sizeXmap / 2, hit.point.y, hit.point.z);
                    }
                    newObstacle =  Instantiate(actualObstacle, positionObject, Quaternion.identity);
                    newObstacle.AddComponent<Rigidbody>();
                    newObstacle.GetComponent<Rigidbody>().isKinematic=true;
                    newObstacle.GetComponent<Rigidbody>().useGravity=false;
                    newObstacle.GetComponent<Collider>().isTrigger = true;

                    StartCoroutine(TestPlacement());
                }

            }
        }

        
    }

    public void ObjectFollowMouse()  // Mettre un layer mask pour que cube suive mieux souris
    {
        RaycastHit hit;
        
        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
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

                }
                //move it
                followObstacle.transform.position = positionObject;
            }
        }
        else
        {
            if (followObstacle!=null)
            {
                Debug.Log("Destroy");
                Destroy(followObstacle);
            }
        }
    }

    public IEnumerator TestPlacement()
    {
        yield return new WaitForSeconds(0.2f);
        if (newObstacle.GetComponent<PrefabsCollision>().isRightPlacement)
        {
            obstaclesOnMap.Add(newObstacle);
        }
        else
        {
            Destroy(newObstacle);
        }
    }
}
