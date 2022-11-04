using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE
{
    Left, Middle, Right, ExtraLeft, ExtraRight
}
public enum HitX
{
    None, Left, Middle, Right
}
public enum HitY
{
    None, Up, Middle, Down
}
public enum HitZ
{
    None, Forward, Middle, Backward
}
public enum AirState
{
    Run, RollGround, RollAir, Jump, Down, Downing, NONE
}

public class Player : MonoBehaviour
{
    public static Player instance;

    public Rigidbody rb;
    public CapsuleCollider myCollider;
    public Animator animator;
    public CurveMovement curveMovement;

    [HideInInspector] public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown, Tap, canTouch;

    public GameObject cube;

    public AirState airState;
    public SIDE side;
    public HitX hitX;
    public HitY hitY;
    public HitZ hitZ;

    public float NexPosX, NexPosY;

    public float playerHeight;
    public LayerMask Ground;

    public bool isGrounded;
    public bool isJumping = false;
    public bool isDowning = false;
    public bool isRolling = false;
    public bool isHitSide = false;

    private Vector3 fp;//First touch position Phone
    private Vector3 lp;//Last touch position Phone
    private float dragDistance = Screen.width * 7 / 100;//Minimum distance for a swipe Phone

    private Vector3 groundHitPosition;


    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        NexPosX = 0;
        NexPosY = gameObject.transform.position.y;
        canTouch = true;
    }

    void Update()
    {
        GetInput();
        MoveXY();
        SetAirState();

    }

    void FixedUpdate()
    {
        Moving();


    }

    public void SetAirState()
    {

        RaycastHit hit;
        Vector3 foot = transform.position - new Vector3(0, playerHeight * 0.5f - 0.1f, 0);
        isGrounded = Physics.Raycast(foot, Vector3.down, out hit, 0.2f, Ground);
        groundHitPosition = hit.point;
        Debug.DrawLine(foot, foot + Vector3.down * (0.2f), Color.black,Time.fixedDeltaTime);

        animator.SetBool("Grounded", isGrounded);

        airState = AirState.NONE;
        if (isGrounded && !isJumping && !isDowning && !isRolling)
        {
            airState = AirState.Run;
        }
        if (isJumping && !isDowning && !isRolling)
        {
            airState = AirState.Jump;
        }
        if (!isGrounded && !isJumping  && !isRolling)
        {
            airState = AirState.Down;
        }
        if (!isGrounded && !isJumping && !isRolling && isDowning)
        {
            airState = AirState.Downing;
        }
        if (isGrounded && !isJumping && !isDowning && isRolling)
        {
            airState = AirState.RollGround;
        }
        if (!isGrounded && !isJumping && !isDowning && isRolling)
        {
            airState = AirState.RollAir;

        }

    }

    private void GetInput()
    {
#if UNITY_EDITOR
        Tap = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
        SwipeLeft = Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow);
        SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        SwipeUp = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow);
        SwipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
#else

        SwipeLeft = SwipeDown = SwipeRight = SwipeUp = Tap  = false;

        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase==TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
                canTouch = true;
                Debug.Log("canTouchBegin");
            }
            else if (touch.phase==TouchPhase.Moved)
            {
                lp = touch.position;
            }
            else if (touch.phase==TouchPhase.Ended)
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Canceled)
            {
                lp = touch.position;
            }
            if (canTouch && Mathf.Abs(lp.x-fp.x)>dragDistance )
            {
                if (lp.x > fp.x)
                {
                    SwipeRight= true;
                    canTouch = false;
                }
                else
                {
                    SwipeLeft = true;
                    canTouch = false;
                }

            }
            if (canTouch && Mathf.Abs(lp.y - fp.y) > dragDistance)
            {
                if (lp.y > fp.y)
                {
                    SwipeUp = true;
                    canTouch = false;
                }
                else
                {
                    SwipeDown = true;
                    canTouch = false;
                }
            }

            if (canTouch && Mathf.Abs(lp.y - fp.y) < dragDistance && Mathf.Abs(lp.x - fp.x) < dragDistance)
            {
                Tap = true;
            }
        }
        
        
#endif
    }

    private void MoveXY()
    {
        if (SwipeLeft) 
        {
            if (side == SIDE.Left && transform.position.x <-curveMovement.sideDistance +0.1f)
            {
                side = SIDE.ExtraLeft;
                StartCoroutine(HitSideCurve(1, -1, side));
                animator.SetTrigger("HitSideLeft");
                StartCoroutine(Skin.instance.PlayParticleHeadStars());

            }
            else if (side == SIDE.Middle)
            {
                side = SIDE.Left;
                StartCoroutine(SwitchSideCurve(curveMovement.sideDistance + transform.position.x ,-1 , side));
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("roll")) { animator.SetTrigger("Left"); }

            }
            else if (side == SIDE.Right)
            {
                side = SIDE.Middle;
                StartCoroutine(SwitchSideCurve(Mathf.Abs(transform.position.x), -1,side));
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("roll")) { animator.SetTrigger("Left"); }

            }
        }
        else if (SwipeRight) 
        {

            if (side == SIDE.Left)
            {
                side = SIDE.Middle;
                StartCoroutine(SwitchSideCurve(Mathf.Abs(transform.position.x), 1,side));
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("roll")) { animator.SetTrigger("Right"); }

            }
            else if (side == SIDE.Middle)
            {
                side = SIDE.Right;
                StartCoroutine(SwitchSideCurve(curveMovement.sideDistance - transform.position.x, 1, side));
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("roll")) { animator.SetTrigger("Right"); }

            }
            else if (side == SIDE.Right && transform.position.x  >  curveMovement.sideDistance - 0.1f)
            {
                side = SIDE.ExtraRight;
                StartCoroutine(HitSideCurve(1, 1, side));
                animator.SetTrigger("HitSideRight");
                StartCoroutine(Skin.instance.PlayParticleHeadStars());

            }
        }



        Jump();
        Roll();
    }

    private void Moving()
    {

        Vector3 moveVector = new Vector3(NexPosX, NexPosY, 0);

        rb.MovePosition(moveVector);
    }


    public IEnumerator SwitchSideCurve(float sideDistancePlayer, int direction, SIDE actualSide )
    {
        float startPos = gameObject.transform.position.x;
        float evaluation = 0;
        curveMovement.switchSideTimer = 0;

        float coeffTime = 0;
        if (sideDistancePlayer<= curveMovement.sideDistance)
        {
            coeffTime = curveMovement.sideDistance / sideDistancePlayer;
            coeffTime = Mathf.Clamp(coeffTime, 1, curveMovement.speedMax1Side);

        }
        else
        {
            coeffTime = sideDistancePlayer / curveMovement.sideDistance;
            coeffTime = Mathf.Clamp(coeffTime, 1, curveMovement.speedMax2Sides);

        }

        for (int i = 0; i < curveMovement.switchSideCurve.keys[curveMovement.switchSideCurve.keys.Length - 1].time / Time.fixedDeltaTime ; i++)
        {
            if (actualSide== side )//|| !isHitSide)
            {
                curveMovement.switchSideTimer += Time.fixedDeltaTime;
                evaluation = curveMovement.switchSideCurve.Evaluate(curveMovement.switchSideTimer * coeffTime);
                NexPosX = evaluation * sideDistancePlayer * direction + startPos;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            else
            {
                yield break;
            }
        }
    }

    public IEnumerator HitSideCurve(float sideDistancePlayer, int direction, SIDE actualSide)  //Extra Left Extra Right
    {
        float startPos = gameObject.transform.position.x;
        float evaluation = 0;
        curveMovement.sideHitTimer = 0;

        float coeffTime = 0;
        if (sideDistancePlayer <= curveMovement.sideDistance)
        {
            coeffTime = curveMovement.sideDistance / sideDistancePlayer;
            coeffTime = Mathf.Clamp(coeffTime, 1, curveMovement.speedMax1Side);

        }
        else
        {
            coeffTime = sideDistancePlayer / curveMovement.sideDistance;
            coeffTime = Mathf.Clamp(coeffTime, 1, curveMovement.speedMax2Sides);

        }

        for (int i = 0; i < curveMovement.sideHitCurve.keys[curveMovement.sideHitCurve.keys.Length - 1].time / Time.fixedDeltaTime; i++)
        {
            if (actualSide == side)
            {
                curveMovement.sideHitTimer += Time.fixedDeltaTime;
                evaluation = curveMovement.sideHitCurve.Evaluate(curveMovement.sideHitTimer);// * coeffTime);
                NexPosX = evaluation * sideDistancePlayer * direction + startPos;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            else
            {
                yield break;
            }
        }
        if (actualSide == SIDE.ExtraLeft)
        {
            side = SIDE.Left;
        }
        else if (actualSide == SIDE.ExtraRight)
        {
            side = SIDE.Right;
        }
    }

    private void Jump()
    {
        if (airState == AirState.Run)
        {
            NexPosY = groundHitPosition.y + playerHeight / 2f;
        }

        if (SwipeUp)
        {
            if (airState == AirState.Run || airState == AirState.RollGround)
            {
                StartCoroutine(JumpCurve());
            }
        }

        if (airState == AirState.Down)
        {
            isDowning = true;
            Debug.Log("error1");
            Debug.Log(airState);
            StartCoroutine(GoDown());
            
        }
    }


    private void Roll()
    {
        if (SwipeDown)//!animator.GetCurrentAnimatorStateInfo(0).IsName("roll"))
        {
            if (airState==AirState.Jump || airState==AirState.Down || airState == AirState.Downing)
            {
                StartCoroutine(ColliderSwipeDown());
                isRolling = true;
                isJumping = false;
                isDowning = false;
                StartCoroutine(RollCurve());
            }

            if (airState == AirState.Run)
            {
                animator.SetTrigger("Roll");
                StartCoroutine(ColliderSwipeDown());
                //isRolling = true;
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

    public IEnumerator RollCurve()
    {
        animator.SetTrigger("Roll");
        float startPos = gameObject.transform.position.y;
        float evaluation = 0;
        curveMovement.rollTimer = 0;
        for (int i = 0; i < curveMovement.rollCurve.keys[curveMovement.rollCurve.keys.Length - 1].time / Time.fixedDeltaTime; i++)
        {
            if (isRolling)
            {
                curveMovement.rollTimer += Time.fixedDeltaTime;
                evaluation = curveMovement.rollCurve.Evaluate(curveMovement.rollTimer);
                NexPosY = evaluation + startPos;
                yield return new WaitForSeconds(Time.fixedDeltaTime);


            }
            if (isGrounded)
            {
                isRolling = false;
                yield break;
            }
        }
        isRolling = false;
    }

    public IEnumerator GoDown()
    {
        isDowning = true;
        Debug.Log("error2");

        animator.SetTrigger("Falling");

        float startPos = gameObject.transform.position.y;
        float evaluation = 0;
        curveMovement.downTimer = 0;
        for (int i = 0; i < curveMovement.downCurve.keys[curveMovement.downCurve.keys.Length - 1].time / Time.fixedDeltaTime; i++)
        {


            if (!isGrounded && !isJumping)
            {
                curveMovement.downTimer += Time.fixedDeltaTime;
                evaluation = curveMovement.downCurve.Evaluate(curveMovement.downTimer);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                NexPosY = evaluation + startPos;

            }
            else if (isGrounded)
            {
                isDowning = false;
                yield break;
            }
        }
        isDowning = false;
    }

    public IEnumerator JumpCurve()
    {
        isJumping = true;
        animator.SetTrigger("Jump");

        float startPos = gameObject.transform.position.y;
        float evaluation = 0;
        curveMovement.jumpTimer = 0;
        for (int i = 0; i < curveMovement.jumpCurve.keys[curveMovement.jumpCurve.keys.Length - 1].time / Time.fixedDeltaTime; i++)
        {
            if (isJumping)
            {
                curveMovement.jumpTimer += Time.fixedDeltaTime;
                evaluation = curveMovement.jumpCurve.Evaluate(curveMovement.jumpTimer);
                NexPosY = evaluation + startPos;
                yield return new WaitForSeconds(Time.fixedDeltaTime);

            }
            if (isGrounded && i > curveMovement.jumpCurve.keys[curveMovement.jumpCurve.keys.Length - 1].time / Time.deltaTime * 0.07f)
            {
                isJumping = false;
                yield break;
            }
        }
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
        cube.transform.position = collision.contacts[0].point;
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 2);
            //Debug.Log(contact.normal);
        }

        if (collision.contacts[0].normal == Vector3.up) //collision.gameObject.name == "Plane")
        {
            hitX = HitX.None;
            hitY = HitY.None;
            hitZ = HitZ.None;
            return;
        }

        HitDetectionSide(collision);

        if (hitX == HitX.Left)
        {
            animator.SetTrigger("HitSideLeft");
            side = SIDE.Right;
            StartCoroutine(SwitchSideCurve(curveMovement.sideDistance - Mathf.Abs( transform.position.x), 1, side));
            StartCoroutine(Skin.instance.PlayParticleHeadStars());
            StartCoroutine(CameraMovement.instance.ShakeCamera(2,0.5f));
        }
        else if (hitX == HitX.Right)
        {
            animator.SetTrigger("HitSideRight");
            side = SIDE.Left;
            StartCoroutine(SwitchSideCurve(curveMovement.sideDistance - Mathf.Abs(transform.position.x), -1, side));
            StartCoroutine(CameraMovement.instance.ShakeCamera(2,0.5f));
            StartCoroutine(Skin.instance.PlayParticleHeadStars());

        }
        else if (hitZ == HitZ.Forward && collision.contacts[0].normal == new Vector3(0,0,-1))
        {
            animator.SetTrigger("FallBackward");
            MapController.instance.speedMap = 0;
        }
        else
        {
            
        }

    }

    private void HitDetectionSide(Collision collision)
    {
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
    }
}
