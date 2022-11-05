using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Linq;
using System;
using System.IO;

public enum State { None, Selection, Placement, Rotation}


public class MapEditor : MonoBehaviour
{
    public static MapEditor instance;

    public State state;

    public Camera worldCamera;
    public Category[] catalog;

    public GameObject Canvas;
    public Button Selector;
    public Button Save;
    public Button Load;
    public Button ResetPattern;
    public Button DestroyObjectScene;
    public Toggle GrilleMapZ;
    public Toggle GrilleMapX;
    public Toggle OverPose;
    public Button NextPattern;
    public Button PreviousPattern;
    public TextMeshProUGUI TextpatternNumber;
    public Button RotationModeButton;
    public Slider RotationX;
    public Slider RotationY;
    public Slider RotationZ;
    public Button StartPattern;
    public Button EndPattern;
    public Button StartMove;

    //public GameObject plane;
    public GameObject contener;
    public float sizeXmap;
    public float hitDistance;

    public List<GameObject> obstaclesOnMap = new List<GameObject>();

    public GameObject actualObstacle;
    public GameObject newObstacle;
    public GameObject followObstacle;

    public GameObject startPattern;
    public GameObject endPattern;
    public GameObject startMove;

    public GameObject startPatternOnMap;
    public GameObject endPatternOnMap;
    public GameObject startMoveOnMap;

    public Material previouPlaceMaterial;
    public Material canPlaceMaterial;
    public Material canNotPlaceMaterial;
    public Material greenMaterial;

    public LayerMask layerGround;
    public LayerMask layerObstacle;
    public LayerMask layerGroundObstacle;

    public Pattern[] patterns;
    private int patternNumber;
    private int actualCategory;

    private bool isGrillModeZ;
    private bool isGrillModeX;
    private bool isOverPose;

#if UNITY_EDITOR

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {

        ListenerUI();

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
        else if (state == State.Rotation)
        {
            RotationMode();
        }

        

    }

    public void ListenerUI()
    {
        for (int i = 0; i < catalog.Length; i++)
        {
            for (int ii = 0; ii < catalog[i].prefabsButton.Length && ii < catalog[i].prefabsObject.Length; ii++)
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
        GrilleMapZ.onValueChanged.AddListener(delegate { GrilleModeZActivation(); });
        GrilleModeZActivation();
        GrilleMapX.onValueChanged.AddListener(delegate { GrilleModeXActivation(); });
        GrilleModeXActivation();
        OverPose.onValueChanged.AddListener(delegate { isOverPose = OverPose.isOn; });

        ShowCategory(-1);
        for (int i = 0; i < catalog.Length; i++)
        {

            int x = i;
            catalog[i].categoryButton.onClick.AddListener(delegate { ShowCategory(x); });

        }
        OnClickNExtPattern(0);
        NextPattern.onClick.AddListener(delegate { OnClickNExtPattern(1); });
        PreviousPattern.onClick.AddListener(delegate { OnClickNExtPattern(-1); });

        RotationModeButton.onClick.AddListener(() => state = State.Rotation);
        RotationX.onValueChanged.AddListener(delegate { OnRotation(); });
        RotationY.onValueChanged.AddListener(delegate { OnRotation(); });
        RotationZ.onValueChanged.AddListener(delegate { OnRotation(); });

        StartPattern.onClick.AddListener(delegate { OnPoseStartPattern(); });
        EndPattern.onClick.AddListener(delegate { OnPoseEndPattern(); });
        StartMove.onClick.AddListener(delegate { OnPoseStartMovePattern(); });

    }

    public IEnumerator TakeScreenShot()
    {
        Canvas.SetActive(false);
        Texture2D screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        //Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100);
        AssetDatabase.CreateAsset(screenCapture, "Assets/Scripts/MapEditor/Data/preview/" + patterns[patternNumber].name +"preview.asset");
        Canvas.SetActive(true);
    }

    public void RotationMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, hitDistance, layerObstacle))
            {
                followObstacle = null;
                actualObstacle = hit.transform.gameObject;

            }

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
                //GameObject selectObstacle = hit.transform.gameObject;
                state = State.Placement;
                followObstacle = null;


                for (int ii = 0; ii < catalog.Length; ii++)
                {
                    for (int iii = 0; iii < catalog[ii].prefabsObject.Length; iii++)
                    {
                        if (catalog[ii].prefabsObject[iii].name + "(Clone)" == hit.transform.gameObject.name)
                        {
                            actualObstacle =catalog[ii].prefabsObject[iii];
                        }
                    }
                }
                obstaclesOnMap.Remove(hit.transform.gameObject);
                Destroy(hit.transform.gameObject);
                
            }
            
        }
    }


    public void ObjectFollowMouse()  
    {
        RaycastHit hit;
        
        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, hitDistance,layerGroundObstacle))
        {
            Vector3 positionObject = new Vector3(0, 0, 0);
            //createobject
            if (followObstacle == null)
            {
                followObstacle = Instantiate(actualObstacle, positionObject, Quaternion.identity);
                followObstacle.AddComponent<Rigidbody>();
                followObstacle.GetComponent<Rigidbody>().isKinematic = true;
                followObstacle.GetComponent<Rigidbody>().useGravity = false;
                followObstacle.GetComponent<Collider>().isTrigger = true;
                followObstacle.layer = 0; //Default
            }

            

            float zPos = 0;
            float xPos = 0;
            if (isGrillModeZ) //GRILLE
            {
                if (followObstacle.GetComponent<PrefabData>().size == 0 )
                {
                    zPos = hit.point.z - hit.point.z % 1;
                }
                else
                {
                    zPos = hit.point.z - hit.point.z % followObstacle.GetComponent<PrefabData>().size;
                }
            }
            else
            {
                zPos = hit.point.z;
            }

            if (isGrillModeX)
            {
                if (hit.point.x < -sizeXmap * 0.33f)
                {
                    xPos = -sizeXmap / 2;
                }
                else if (hit.point.x > -sizeXmap * 0.33f && hit.point.x < sizeXmap * 0.33f)
                {
                    xPos = 0;
                }
                else if (hit.point.x > sizeXmap * 0.33f)
                {
                    xPos = sizeXmap / 2;
                }
            }
            else
            {
                xPos = hit.point.x;
            }

            positionObject = new Vector3(xPos, hit.point.y, zPos);


            if (followObstacle!= null && followObstacle.GetComponent<PrefabData>().offset != null)
            {
                positionObject -= followObstacle.GetComponent<PrefabData>().offset.localPosition;
            }
            

            
            //move it
            followObstacle.transform.position = positionObject;

            //materials
            Material[] matArray = { followObstacle.GetComponent<MeshRenderer>().material, null };

            if (followObstacle.GetComponent<PrefabData>().crossNumber == 0  || isOverPose)
            {
                matArray[1] = previouPlaceMaterial;
            }
            else
            {
                matArray[1] = canNotPlaceMaterial;
            }

            if (Input.GetMouseButtonDown(0) )
            {
                if (followObstacle.GetComponent<PrefabData>().crossNumber == 0  || isOverPose)
                {
                    newObstacle = Instantiate(actualObstacle, positionObject, Quaternion.identity);
                    newObstacle.transform.parent = contener.transform;
                    matArray[1] = canPlaceMaterial;

                    if (followObstacle.GetComponent<PrefabData>().obstacleType == ObstacleType.Start)
                    {
                        startPatternOnMap = newObstacle;
                    }
                    else if (followObstacle.GetComponent<PrefabData>().obstacleType == ObstacleType.End)
                    {
                        endPatternOnMap = newObstacle;
                    }
                    else if (followObstacle.GetComponent<PrefabData>().obstacleType == ObstacleType.StartMove)
                    {
                        startMoveOnMap = newObstacle;
                    }
                    else
                    {
                        obstaclesOnMap.Add(newObstacle);
                    }
                }
                

            }
            followObstacle.GetComponent<MeshRenderer>().materials = matArray;

            
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
        OnResetPattern();
        patterns[patternNumber].sizePattern = endPatternOnMap.transform.position.z-startPatternOnMap.transform.position.z ;

        for (int i = 0; i < obstaclesOnMap.Count; i++)
        {
            for (int ii = 0; ii < catalog.Length; ii++)
            {
                for (int iii = 0; iii < catalog[ii].prefabsObject.Length; iii++)
                {
                    if (catalog[ii].prefabsObject[iii].name+"(Clone)" == obstaclesOnMap[i].name)
                    {
                        patterns[patternNumber].gameObjects.Add(catalog[ii].prefabsObject[iii]);
                        patterns[patternNumber].positions.Add(obstaclesOnMap[i].transform.position - Vector3.forward * startPatternOnMap.transform.position.z);
                        patterns[patternNumber].rotation.Add(obstaclesOnMap[i].transform.rotation);
                        EditorUtility.SetDirty(patterns[patternNumber]);
                    }
                }
            }
        }

        StartCoroutine(TakeScreenShot());
        AssetDatabase.SaveAssets();

    }

    public void OnLoad()
    {
        OnDestroyObjectScene();
        for (int i = 0; i < patterns[patternNumber].gameObjects.Count; i++)
        {
            GameObject obstacle = Instantiate(patterns[patternNumber].gameObjects[i], patterns[patternNumber].positions[i], patterns[patternNumber].rotation[i]);

            obstacle.transform.parent = contener.transform;
            obstaclesOnMap.Add(obstacle);
        }
    }

    public void OnResetPattern()
    {
        patterns[patternNumber].gameObjects = new List<GameObject>();
        patterns[patternNumber].positions = new List<Vector3>();
        patterns[patternNumber].rotation = new List<Quaternion>();
    }

    public void OnDestroyObjectScene()
    {
        for (int i = 0; i < obstaclesOnMap.Count; i++)
        {
            Destroy(obstaclesOnMap[i]);
        }
        obstaclesOnMap.RemoveRange(0, obstaclesOnMap.Count);
    }

    public void GrilleModeZActivation()
    {
        isGrillModeZ = GrilleMapZ.isOn;
    }

    public void GrilleModeXActivation()
    {
        isGrillModeX = GrilleMapX.isOn;
    }

    public void OnClickNExtPattern(int numberAdd)
    {
        if (patternNumber + numberAdd>=0)
        {
            patternNumber = (patternNumber + numberAdd) % (patterns.Length);
        }
        else
        {
            patternNumber = patterns.Length - 1;
        }
        
        TextpatternNumber.text = "Number " + patternNumber.ToString() +", " + patterns[patternNumber].name; 
    }

    public void OnRotation()
    {
        actualObstacle.transform.rotation = Quaternion.Euler(RotationX.value, RotationY.value, RotationZ.value);
    }
#endif

    public void OnPoseStartPattern()
    {
        actualObstacle = startPattern;
        state = State.Placement;
    }


    public void OnPoseEndPattern()
    {
        actualObstacle = endPattern;
        state = State.Placement;
    }

    public void OnPoseStartMovePattern()
    {
        actualObstacle = startMove;
        state = State.Placement;
    }

}
