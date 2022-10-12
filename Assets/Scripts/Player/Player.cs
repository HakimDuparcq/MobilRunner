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
     public float jumpPower, downSpeed, rollPower;

    [HideInInspector] public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;

    public Rigidbody rb;

    private float NexPosX, NexPosY;
    private float x, y, z;

    public GameObject cube;


    public float groundDrag;
    public float playerHeight;
    public LayerMask Ground;
    bool isGrounded;
    public float jumpCouldown;
    private bool isJumping = false;

    void Start()
    {
        y = 2;
        transform.position = new Vector3(0, 2, 0);
    }

    void Update()
    {
        GetInput();
        MoveX();
        Debug.Log(isGrounded);
    }

    void FixedUpdate()
    {
        Moving();
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

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (SwipeLeft) // Left
        {
            if (side == SIDE.Left)
            {

            }
            else if (side == SIDE.Middle)
            {
                side = SIDE.Left;
                NexPosX = -sideDistance;
            }
            else if (side == SIDE.Right)
            {
                side = SIDE.Middle;
                NexPosX = 0;

            }
        }
        else if (SwipeRight) // Right
        {
            if (side == SIDE.Left)
            {
                side = SIDE.Middle;
                NexPosX = 0;
            }
            else if (side == SIDE.Middle)
            {
                side = SIDE.Right;
                NexPosX = sideDistance;
            }
            else if (side == SIDE.Right)
            {

            }
        }



        Jump();
        Roll();
    }

    private void Moving()
    {
        x = Mathf.Lerp(transform.position.x, NexPosX, Time.fixedDeltaTime * timeSwitchSide);

        y= Mathf.Lerp(transform.position.y, NexPosY, Time.fixedDeltaTime * downSpeed);
        Vector3 moveVector = new Vector3(x, y , 0);

        rb.MovePosition(moveVector);
    }

    private void Jump()
    {
        if (isGrounded && !isJumping)
        {
            NexPosY = transform.position.y;
            if (SwipeUp)
            {
                NexPosY = jumpPower;
                isJumping = true;
                StartCoroutine(JumpCouldown(jumpCouldown));
            }
        }
        else
        {
            NexPosY -= downSpeed * Time.deltaTime;
        }
    }




    private void Roll()
    {
        if (isGrounded)
        {

        }
        else
        {
            if (SwipeDown)
            {
                NexPosY = 0 + playerHeight/2;
            }
        }
    }


    public IEnumerator JumpCouldown(float time)
    {
        yield return new WaitForSeconds(time);
        isJumping = false;

    }


    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
    }


}
