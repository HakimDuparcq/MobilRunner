using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Camera EditorCamera;
    [HideInInspector] public float x, y, z;
    public float speed;



    void Start()
    {
        
    }

    void Update()
    {
        GetInput();

        Vector3 movement = new Vector3(x, z, y);
        EditorCamera.transform.Translate(movement * Time.deltaTime * speed );

    }

    private void GetInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        y = Convert.ToInt32(Input.GetKey(KeyCode.LeftShift)) - Convert.ToInt32(Input.GetKey(KeyCode.Space));

    }




}
