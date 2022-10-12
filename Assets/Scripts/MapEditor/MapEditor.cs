using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapEditor : MonoBehaviour
{
    public Camera worldCamera;
    public GameObject actualObstacle;
    public GameObject[] obstacles;
    public Button[] SelectObstacles;

    public GameObject plane;

    public float sizeXmap;
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
                    Debug.Log(hit.transform.position);
                    Vector3 positionObject;
                    if (hit.point.x<sizeXmap*0.33f && hit.point.x<0)
                    {
                        positionObject = new Vector3(-2, hit.point.y, hit.point.z);
                    }
                    else if (hit.point.x> sizeXmap * 0.33f  && hit.point.x < sizeXmap * 0.66f)
                    {
                        positionObject = new Vector3(0, hit.point.y, hit.point.z);

                    }
                    else
                    {
                        positionObject = new Vector3(2, hit.point.y, hit.point.z);

                    }
                    Instantiate(actualObstacle, positionObject, Quaternion.identity);

                }

                //Debug.Log(hit.transform.name);

            }
        }
        
    }
}
