using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE
{
    Left, Middle,Right
}
public enum HitX
{
    None, Left,Middle,Right
}
public enum HitY
{
    None, Up, Middle, Down
}
public enum HitZ
{
    None, Forward, Middle, Backward
}
public class Player : MonoBehaviour
{
    public SIDE side;
    public HitX hitX;
    public HitY hitY;
    public HitZ hitZ;

    [Range(0.1f, 5)] public float sideDistance;
    [Range(0.1f, 15)] public float timeSwitchSide;
     public float jumpPower, downSpeed, rollPower, ySpeed;

    [HideInInspector] public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;

    public Rigidbody rb;
    public CapsuleCollider myCollider;

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
        //Debug.Log(isGrounded);
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

        y= Mathf.Lerp(transform.position.y, NexPosY, Time.fixedDeltaTime * ySpeed);
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
                NexPosY = jumpPower + gameObject.transform.position.y;
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
        if (SwipeDown)
        {
            StartCoroutine(ColliderSwipeDown());
        }

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

    private IEnumerator ColliderSwipeDown()
    {
        myCollider.center = new Vector3(0, -0.5f, 0);
        myCollider.height = 1;
        yield return new WaitForSeconds(2);
        myCollider.center = new Vector3(0, 0, 0);
        myCollider.height = 2;
    }

    public IEnumerator JumpCouldown(float time)
    {
        yield return new WaitForSeconds(time);
        isJumping = false;

    }


    public void OnCollisionEnter(Collision collision)
    {
        GetHit(collision);

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name != "Plane")
        {
            hitX = HitX.None;
            hitY = HitY.None;
            hitZ = HitZ.None;
        }
    }

    private void GetHit(Collision collision)
    {
        Debug.Log(collision.transform.name);


        cube.transform.position = collision.contacts[0].point;
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 2);
        }

        if (collision.gameObject.name == "Plane")
        {
            hitX = HitX.None;
            hitY = HitY.None;
            hitZ = HitZ.None;
            return;
        }

        //Debug.Log((collision.contacts[0].point.x - gameObject.transform.position.x));
        if (collision.contacts[0].point.x - gameObject.transform.position.x < -0.17f)  // - 0.17f
        {
            hitX = HitX.Left;
        }
        else if (collision.contacts[0].point.x - gameObject.transform.position.x < 0.17f)  //  0.17f
        {
            hitX = HitX.Middle;
        }
        else
        {
            hitX = HitX.Right;
        }

        // Debug.Log((collision.contacts[0].point.y - gameObject.transform.position.y));
        if (collision.contacts[0].point.y - gameObject.transform.position.y < -myCollider.height / 2 * 0.33f)  //-0.33f
        {
            hitY = HitY.Down;
        }
        else if (collision.contacts[0].point.y - gameObject.transform.position.y < myCollider.height / 2 * 0.33f)     //0.33f
        {
            hitY = HitY.Middle;
        }
        else
        {
            hitY = HitY.Up;
        }

        //Debug.Log((collision.contacts[0].point.x - gameObject.transform.position.x));
        if (collision.contacts[0].point.z - gameObject.transform.position.z < -0.17f)  //-0.17f
        {
            hitZ = HitZ.Backward;
        }
        else if (collision.contacts[0].point.z - gameObject.transform.position.z < 0.17f)
        {
            hitZ = HitZ.Middle;
        }
        else
        {
            hitZ = HitZ.Forward;
        }

        Debug.Log("X " + (collision.contacts[0].point.x - gameObject.transform.position.x)
            + "  Y " + (collision.contacts[0].point.y - gameObject.transform.position.y)
            + "  Z " + (collision.contacts[0].point.z - gameObject.transform.position.z));
    }
}
