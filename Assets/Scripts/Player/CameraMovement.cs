using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float yCamFollow;
    public float speedFollow;

    public LayerMask Ground;

    public Transform followCam;


    void Start()
    {
    }

    void Update()
    {
        
    }

    public void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(Player.instance.gameObject.transform.position, Vector3.down, out hit, Mathf.Infinity, Ground))
        {
            yCamFollow = Mathf.Lerp(yCamFollow, hit.point.y, Time.deltaTime * speedFollow);
        }
        else
        {
            yCamFollow = Mathf.Lerp(yCamFollow, Player.instance.gameObject.transform.position.y, Time.deltaTime * speedFollow);
        }


        followCam.position = new Vector3(Player.instance.gameObject.transform.position.x / 2f, yCamFollow, Player.instance.gameObject.transform.position.z);
    }
}
