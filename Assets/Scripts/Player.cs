using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE
{
    Left,
    Middle,
    Right
}


public class Player : MonoBehaviour
{
    public SIDE side;

    [Range(0.1f, 5)] public float sideDistance;
    [Range(0.1f, 15)] public float timeSwitchSide;
    [Range(0.1f, 15)] public float jumpPower, rollPower;

    [HideInInspector] public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;

    public CharacterController charac;

    private float NexPos;
    private float x, y, z;


    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        MoveX();


    }
    private void GetInput()
    {
        SwipeLeft = Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow);
        SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        SwipeUp = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow);
        SwipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
    }

    private void MoveX()
    {
        

        if (SwipeLeft) // Left
        {
            if (side == SIDE.Left)
            {

            }
            else if (side == SIDE.Middle)
            {
                side = SIDE.Left;
                NexPos = -sideDistance;
            }
            else if (side == SIDE.Right)
            {
                side = SIDE.Middle;
                NexPos = 0;

            }
        }
        else if (SwipeRight) // Right
        {
            if (side == SIDE.Left)
            {
                side = SIDE.Middle;
                NexPos = 0;
            }
            else if (side == SIDE.Middle)
            {
                side = SIDE.Right;
                NexPos = sideDistance;
            }
            else if (side == SIDE.Right)
            {

            }
        }


        Vector3 moveVector = new Vector3(x- transform.position.x, y*Time.deltaTime, 0);

        x = Mathf.Lerp(x, NexPos, Time.deltaTime * timeSwitchSide);
        charac.Move(moveVector);
        Jump();

    }



    private void Jump()
    {
        if (charac.isGrounded)
        {
            if (SwipeUp)
            {
                y = jumpPower;
            }
        }
        else
        {
            y -= jumpPower * Time.deltaTime;
        }
    }


    private void Roll()
    {
        if (charac.isGrounded)
        {
            if (SwipeDown)
            {
                y -= rollPower * Time.deltaTime;
            }
        }
        else
        {
            
        }
    }
}
