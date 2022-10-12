using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charac : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 2f;
    public float rotateSpeed = 50f;
    public GameObject cube;

    void Start()
    {
        
    }

    void Update()
    {
        ProcessMovement();
    }


    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Character Controller Enter");
    }

    private void ProcessMovement()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            controller.Move(transform.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            controller.Move(-transform.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            controller.Move(-transform.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            controller.Move(transform.right * Time.deltaTime * moveSpeed);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.transform.name);
        Instantiate(cube, hit.point, Quaternion.identity);
    }
}
