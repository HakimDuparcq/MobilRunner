using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public enum State { None, Selection, Placement}


public class MapEditor : MonoBehaviour
{
    public static MapEditor instance;

    public Camera worldCamera;
    public GameObject actualObstacle;
    public Category[] catalog;

    public Button Selector;
    public Button Save;
    public Button Load;
    public Button ResetPattern;
    public Button DestroyObjectScene;

    public GameObject plane;
    public GameObject contener;
    public float sizeXmap;


    public List<GameObject> obstaclesOnMap = new List<GameObject>();

    public GameObject newObstacle;
    public GameObject followObstacle;
    public GameObject selectObstacle;

    public Material previouPlaceMaterial;
    public Material canPlaceMaterial;
    public Material canNotPlaceMaterial;

    public LayerMask layerGround;
    public LayerMask layerObstacle;
    public float hitDistance;


    public Pattern pattern;

    public State state;



    public int actualCategory;

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < catalog.Length; i++)
        {
            for (int ii = 0; ii < catalog[i].prefabsButton.Length; ii++)
            {
                int x = ii;
                catalog[i].prefabsButton[ii].onClick.AddListener(delegate { ClickButtonPrefabs(x); });
                catalog[i].prefabsButton[ii].GetComponent<RawImage>().texture = AssetPreview.GetAssetPreview(catalog[i].prefabsObject[ii]);
            }
            
        }
        Selector.onClick.AddListener(OnSelection);
        Save.onClick.AddListener(OnSave);
        Load.onClick.AddListener(OnLoad);
        ResetPattern.onClick.AddListener(OnResetPattern);
        DestroyObjectScene.onClick.AddListener(OnDestroyObjectScene);

        ShowCategory(-1);
        for (int i = 0; i < catalog.Length; i++)
        {

            int x = i;
            catalog[i].categoryButton.onClick.AddListener(delegate { ShowCategory(x); });

        }
        


    }

    void Update()
    {
        if (state==State.Placement)
        {
            ObjectFollowMouse();


        }
        else if (state==State.Selection)
        {
            SelectionMode();
        }

    }



    public void ClickButtonPrefabs(int i )
    {
        actualObstacle = catalog[actualCategory].prefabsObject[i];
        state = State.Placement;
    }

    public void ShowCategory(int x)
    {
        actualCategory = x;
        for (int i = 0; i < catalog.Length; i++)
        {
            catalog[i].scrollRect.gameObject.SetActive(false);
        }
        if (x!=-1)
        {
            catalog[x].scrollRect.gameObject.SetActive(true);
        }

    }

    public void OnSelection()
    {
        state = State.Selection;
    }

    public void SelectionMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, hitDistance, layerObstacle))
            {
                selectObstacle = hit.transform.gameObject;
            }
            else
            {
                selectObstacle = null;
            }
        }

        if (selectObstacle != null)
        {
            state = State.Placement;
            followObstacle = selectObstacle;
            selectObstacle = null;
            

        }
    }


    public void ObjectFollowMouse()  
    {
        RaycastHit hit;
        
        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, hitDistance,layerGround))
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
                    newObstacle.transform.parent = contener.transform;
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
            for (int ii = 0; ii < catalog.Length; ii++)
            {
                for (int iii = 0; iii < catalog[ii].prefabsObject.Length; iii++)
                {
                    if (catalog[ii].prefabsObject[iii].GetComponent<PrefabsCollision>().typeObstacle == obstaclesOnMap[i].GetComponent<PrefabsCollision>().typeObstacle)
                    {
                        pattern.gameObjects.Add(catalog[ii].prefabsObject[iii]);
                    }
                }
                
            }


            pattern.positions.Add(obstaclesOnMap[i].transform.position);

        }
    }

    public void OnLoad()
    {
        OnDestroyObjectScene();
        for (int i = 0; i < pattern.gameObjects.Count; i++)
        {
            GameObject obstacle = Instantiate(pattern.gameObjects[i], pattern.positions[i], Quaternion.identity);
            obstacle.transform.parent = contener.transform;
            obstaclesOnMap.Add(obstacle);
        }
    }

    public void OnResetPattern()
    {
        pattern.gameObjects = new List<GameObject>();
        pattern.positions = new List<Vector3>();
    }

    public void OnDestroyObjectScene()
    {
        foreach (GameObject child in obstaclesOnMap)
        {
            Destroy(child.gameObject);
        }
    }

}
