using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool blockingInput = false;

    bool isPlayerHoldingRope = false;

    [SerializeField]
    PlayerType playerType;
    public enum PlayerType
    {
        RED = 0,
        BLUE = 1,
        ON = 2,
        OFF = 3
    }

    public static PlayerController player1;
    public static PlayerController player2;

    GameManager gameManager;
    WorldCanvas worldCanvas;

    bool startJumping = false;
    bool isJumping = false;
    bool isMovingRight = false;
    bool isMovingLeft = false;
    bool isGrabbing = false;
    bool startGrabbing = false;
    bool stopGrabbing = false;
    bool isPulling = false;
    bool wasPulling = false;
    bool startPulling = false;
    bool stopPulling = false;
    bool isLifting = false;
    bool startLifting = false;
    bool stopLifting = false;
    bool isGrounded = false;
    bool startHolding = false;
    bool stopHolding = false;
    bool isHolding = false;
    bool isHoldingSwap = false;
    Rigidbody2D rb;

    [SerializeField]
    float maxJumpBoosts = 3f;
    [SerializeField]
    float curJumpBoosts = 0;

    Ring curRing;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    float jumpForce = 1f;

    [SerializeField]
    float liftForce = 20f;

    [SerializeField]
    float movementForce = 1;

    float swapCooldown = .2f;
    float lastSwap = 0;

    [SerializeField]
    float friction = .4f;

    DistanceJoint2D distanceJoint;

    [SerializeField]
    float maxDistanceJointDistance = 3f;

    public float curDistanceJointDistance = 3f;

    [SerializeField]
    float maxPullPower = 50f;

    [SerializeField]
    LiftTrigger liftTrigger;

    // Animations
    SpriteRenderer sr;
    Animator animator;
    const string animIsWalkingID = "isWalking";
    const string animVerticalSpeedID = "verticalSpeed";
    const string animIsGroundedID = "isGrounded";
    const string animIsGrabbing = "isGrabbing";
    const string animRespawnID = "respawn";
    const string animDieID = "dead";
    const string animTossReadyID = "isTossReady";
    const string animTossID = "toss";

    bool isBeingPulled;
    float timeWasPulled = 0;

    // Music
    AudioSource jumpSource;

    [SerializeField]
    AudioClip grabSound;
    [SerializeField]
    AudioClip damagedSound;
    [SerializeField]
    AudioClip grabReleaseSound;
    [SerializeField]
    AudioClip pullSound;

    [SerializeField]
    Transform grabPivot;
    [SerializeField]
    float grabDistance = 1.5f;

    [SerializeField]
    float maxSpeed = 5;

    float pullCooldown = .5f;
    float lastPullTime = 0;

    [SerializeField]
    GameObject playerIcon;

    private void Awake()
    {
        gameManager = GameManager.instance;
        if (gameManager.isSinglePlayer())
        {
            if (playerType == PlayerType.RED)
            {
                playerType = PlayerType.ON;
                player1 = this;
            }
            else
            {
                playerType = PlayerType.OFF;
                player2 = this;
            }
        }
        else
        {
            if (playerType == PlayerType.RED)
            {
                player1 = this;
            }
            else
            {
                player2 = this;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        jumpSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        if (groundCheck == null)
        {
            Debug.LogError("No ground check set for " + name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerType == PlayerType.ON)
        {
            playerIcon.SetActive(true);
            playerIcon.transform.position = transform.position + Vector3.up * 1.25f;
        }
        else
        {
            playerIcon.SetActive(false);
        }

        UpdateAnimations();
        Debug.DrawLine(transform.position, transform.position + (Vector3)(rb.velocity.normalized * 2), Color.green, .5f);
    }

    void UpdateAnimations()
    {
        if (rb.velocity.x > 0.1f)
        {
            sr.flipX = false;
        }
        if (rb.velocity.x < -.1f)
        {
            sr.flipX = true;
        }

        if (Mathf.Abs(rb.velocity.x) > .1f)
        {
            animator.SetBool(animIsWalkingID, true);
        }
        else
        {
            animator.SetBool(animIsWalkingID, false);
        }

        if (isLifting)
        {
            animator.SetBool(animTossReadyID, true);
        }
        else
        {
            animator.SetBool(animTossReadyID, false);
        }

        animator.SetFloat(animVerticalSpeedID, rb.velocity.y);
        animator.SetBool(animIsGroundedID, isGrounded);
        animator.SetBool(animIsGrabbing, isGrabbing);
    }

    void CheckGrounded()
    {
        if (groundCheck)
        {
            Debug.DrawLine(groundCheck.transform.position, groundCheck.transform.position + Vector3.down * .1f, Color.red, .5f);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(groundCheck.position, .15f, Vector2.down, .05f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.tag == GameManager.GROUND_TAG)
                {
                    isGrounded = true;

                    if (Time.time - timeWasPulled > .5f)
                    {
                        isBeingPulled = false;
                    }
                    return;
                }
            }

        }
        isGrounded = false;
    }

    Vector3 GetTextSpawnPosition()
    {
        return transform.position + Vector3.one * .05f;
    }

    void GetInputs()
    {
        float swap = Input.GetAxisRaw("Swap");

        if (swap != 0)
        {
            if ((Time.time - lastSwap) > swapCooldown && !isHoldingSwap && gameManager.isSinglePlayer())
            {
                isHoldingSwap = true;
                //worldCanvas.SpawnText(GetTextSpawnPosition(), "Swap!", Color.black);
                lastSwap = Time.time;
                if (playerType == PlayerType.ON)
                {
                    playerType = PlayerType.OFF;
                }
                else
                {
                    playerType = PlayerType.ON;
                }
            }
        }
        else
        {
            isHoldingSwap = false;
        }

        if (playerType == PlayerType.OFF)
        {
            return;
        }

        string playerInputIdentifier = "";
        if (playerType == PlayerType.ON || playerType == PlayerType.RED)
        {
            playerInputIdentifier = "0";
        }
        else
        {
            playerInputIdentifier = "1";
        }

        float horizontal = Input.GetAxisRaw("Horizontal" + playerInputIdentifier);
        if (horizontal > .7f)
        {
            isMovingRight = true;
            isMovingLeft = false;
        }
        else if (horizontal < -.7f)
        {
            isMovingLeft = true;
            isMovingRight = false;
        }
        else
        {
            isMovingLeft = false;
            isMovingRight = false;
        }

        float jump = Input.GetAxisRaw("Jump" + playerInputIdentifier);
        if (jump > 0)
        {
            startJumping = true;
        }
        else
        {
            startJumping = false;
        }

        float grab = Input.GetAxis("Grab" + playerInputIdentifier);
        if (grab > 0)
        {
            startGrabbing = true;
        }
        else
        {
            stopGrabbing = true;
        }

        float pull = Input.GetAxis("Pull" + playerInputIdentifier);
        if (pull > 0 && !wasPulling)
        {
            isPulling = true;
            wasPulling = true;
        }
        else
        {
            wasPulling = false;
        }

        float lift = Input.GetAxis("Lift" + playerInputIdentifier);
        if (lift > 0)
        {
            startLifting = true;
        }
        else
        {
            stopLifting = true;
        }

        float hold = Input.GetAxis("Hold" + playerInputIdentifier);
        if (hold > 0)
        {
            startHolding = true;
        }
        else
        {
            stopHolding = true;
        }


        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            RoomManager.instance.Die();
        }
    }

    private void FixedUpdate()
    {
        if (!blockingInput)
        {
            GetInputs();
        }
        else
        {
            isMovingLeft = false;
            isMovingRight = false;
            ClearInputs();
        }
        CheckGrounded();

        if (startJumping && isGrounded && !isJumping && curJumpBoosts == 0)
        {
            isJumping = true;
            jumpSource.Play();
        }
        else if (isJumping && startJumping && curJumpBoosts < maxJumpBoosts)
        {
            // Keep boosting while player holds button
            curJumpBoosts++;
            rb.AddForce(Vector2.up * jumpForce);
        }
        else if ((isJumping && !startJumping) || (isJumping && curJumpBoosts >= maxJumpBoosts))
        {
            isJumping = false;
        }
        else if (!startJumping && isGrounded)
        {
            curJumpBoosts = 0;
        }

        if (isMovingLeft && Mathf.Abs(rb.velocity.x) < maxSpeed && !isLifting)
        {
            rb.AddForce(Vector2.left * movementForce);
        }
        else if (isMovingRight && Mathf.Abs(rb.velocity.x) < maxSpeed && !isLifting)
        {
            rb.AddForce(Vector2.right * movementForce);
        }
        else if (!isBeingPulled)
        {
            rb.velocity = new Vector2(rb.velocity.x * friction, rb.velocity.y);
        }

        if (startPulling && !isPulling && !curRing)
        {
            isPulling = true;
            //worldCanvas.SpawnText(GetTextSpawnPosition(), "Get over here!", Color.black);
        }
        else if (isPulling && Time.time - lastPullTime > pullCooldown && !curRing)
        {
            lastPullTime = Time.time;
            if (player1 == this)
            {
                player2.Pulled();
            }
            else
            {
                player1.Pulled();
            }
        }

        if (startGrabbing && !isGrabbing)
        {
            isGrabbing = true;
        }
        else if (stopGrabbing && isGrabbing)
        {
            ReleaseRing();
        }
        if (isGrabbing)
        {
            Ring ring = RingManager.instance.GetClosestRing(grabPivot.position, grabDistance);
            if (ring)
            {
                GrabRing(ring);
            }
            else
            {
                ReleaseRing();
            }
        }

        if (startHolding && !isHolding)
        {
            //worldCanvas.SpawnText(GetTextSpawnPosition(), "Holding tight!", Color.black);
            isHolding = true;
            curDistanceJointDistance = Vector2.Distance(player1.transform.position, player2.transform.position);
            UpdateDistanceJointDistance();
        }
        if (stopHolding && isHolding)
        {
            //worldCanvas.SpawnText(GetTextSpawnPosition(), "Letting go!", Color.black);
            isHolding = false;
            curDistanceJointDistance = maxDistanceJointDistance;
            UpdateDistanceJointDistance();
        }

        if (startLifting && !isLifting)
        {
            StartLifting();
        }

        if (stopLifting && isLifting || !isGrounded)
        {
            StopLifting();
        }

        ClearInputs();
    }

    private void ClearInputs()
    {
        stopHolding = false;
        startHolding = false;
        startLifting = false;
        stopLifting = false;
        startJumping = false;
        startPulling = false;
        isPulling = false;
        stopPulling = false;
        startGrabbing = false;
        stopGrabbing = false;
    }

    public void StartLifting()
    {
        //worldCanvas.SpawnText(GetTextSpawnPosition(), "Ready!", Color.black);
        isLifting = true;
        liftTrigger.SetTrigger(true);
    }

    public void StopLifting()
    {
        //worldCanvas.SpawnText(GetTextSpawnPosition(), "Done!", Color.black);
        liftTrigger.SetTrigger(false);
        isLifting = false;
    }

    public void GrabRing(Ring ring)
    {
        if (!curRing)
        {
            jumpSource.PlayOneShot(grabSound);
        }

        curRing = ring;
        Vector3 pivotOffset = grabPivot.localPosition;
        transform.position = ring.transform.position - pivotOffset - new Vector3(0, .5f);
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    public void ReleaseRing()
    {
        if (curRing)
        {
            jumpSource.PlayOneShot(grabReleaseSound);
            rb.AddForce(jumpForce * Vector2.up);
        }
        isGrabbing = false;
        curRing = null;
    }

    public void Tossed()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(liftForce * Vector2.up);
    }

    public void Toss()
    {
        animator.SetTrigger(animTossID);
    }

    public void Die()
    {
        ClearInputs();
        StopLifting();
        isLifting = false;
        rb.velocity = Vector3.zero;
        jumpSource.PlayOneShot(damagedSound);
        UpdateAnimations();
        ClearInputs();
        animator.SetTrigger(animDieID);
    }

    public void Pulled()
    {
        StopLifting();
        jumpSource.PlayOneShot(pullSound);
        float distance = Vector2.Distance(player1.transform.position, player2.transform.position);
        float pullpower = maxPullPower * (distance / maxDistanceJointDistance);

        Vector2 direction = (GetOtherPlayer().transform.position - transform.position).normalized;

        if (isGrounded && GetOtherPlayer().isGrounded)
        {
            direction += Vector2.up * .5f;
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 5, Color.red, 1f);

        rb.AddForce(direction * pullpower);
        isBeingPulled = true;
        timeWasPulled = Time.time;
    }

    public PlayerController GetOtherPlayer()
    {
        if (this == player1)
        {
            return player2;
        }
        return player1;
    }

    public PlayerType GetPlayerType()
    {
        return playerType;
    }

    public void UpdateDistanceJointDistance()
    {
        player1.distanceJoint.distance = Mathf.Min(curDistanceJointDistance, GetOtherPlayer().curDistanceJointDistance);
    }

    public void Respawn()
    {
        animator.SetTrigger(animRespawnID);
        transform.position = RoomManager.instance.GetCheckpoint().transform.position + new Vector3(Random.Range(-.5f, .5f), 0, 0);
    }

    public static bool IsRopeHeld()
    {
        return player1.isHolding || player2.isHolding;
    }
}
