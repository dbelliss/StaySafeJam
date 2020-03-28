using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    PlayerType playerType;
    public enum PlayerType {
        RED = 0,
        BLUE = 1,
        ON = 2,
        OFF = 3
    }

    GameManager gameManager;


    bool isJumping = false;
    bool isMovingRight = false;
    bool isMovingLeft = false;
    bool isGrabbing = false;
    bool isPulling = false;
    bool isLifting = false;
    bool isGrounded = false;
    Rigidbody2D rb;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    float jumpForce = 1;

    [SerializeField]
    float movementSpeed = 1;

    float swapCooldown = .2f;
    float lastSwap = 0;

    [SerializeField]
    float pullForce = 1;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        if (gameManager.isSinglePlayer())
        {
            if (playerType == PlayerType.RED)
            {
                playerType = PlayerType.ON;
            }
            else
            {
                playerType = PlayerType.OFF;
            }
        }
        rb = GetComponent<Rigidbody2D>();

        if (groundCheck == null)
        {
            Debug.LogError("No ground check set for " + name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        CheckGrounded();
    }

    void CheckGrounded()
    {
        if (groundCheck)
        {
            RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, .1f, Vector2.down, .1f);
            if (hit.transform)
            {
                if (hit.transform.gameObject.tag == GameManager.GROUND_TAG)
                {
                    isGrounded = true;
                    return;
                }
            }
            
        }
        isGrounded = false;
    }

    void GetInputs()
    {
        float swap = Input.GetAxisRaw("Swap");
        if (swap != 0 && (Time.time - lastSwap) > swapCooldown)
        {
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
        if (horizontal > 0)
        {
            isMovingRight = true;
            isMovingLeft = false;
        }
        else if(horizontal < 0)
        {
            isMovingLeft = true;
            isMovingRight = false;
        }
        else
        {
            isMovingLeft = false;
            isMovingRight = false;
        }

        float jump = Input.GetAxis("Jump" + playerInputIdentifier);
        if (jump > 0)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        float grab = Input.GetAxis("Grab" + playerInputIdentifier);
        if (grab > 0)
            {
            isGrabbing = true;
        }
        else
        {
            isGrabbing = false;
        }

        float pull = Input.GetAxis("Pull" + playerInputIdentifier);
        if (pull > 0) {
            isPulling = true;
        }
        else
        {
            isPulling = false;
        }

        float lift = Input.GetAxis("Lift" + playerInputIdentifier);
        if (lift > 0) {
            isLifting = true;
        }
        else
        {
            isLifting = false;
        }
    }

    private void FixedUpdate()
    {
        if (isJumping && isGrounded)
        {
            Debug.Log("Jumping");
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (isMovingLeft)
        {
            rb.velocity = new Vector2(-1 * movementSpeed, rb.velocity.y);
        }
        else if (isMovingRight)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (isPulling)
        {
            Debug.Log("Pull");
        }

        if (isGrabbing)
        {
            Debug.Log("Grab");
        }

        if (isLifting)
        {
            Debug.Log("Lift");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameManager.PLAYER_TAG)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameManager.SPIKE_TAG)
        {
            Die();   
        }
    }

    public void Die()
    {
        Debug.Log("Dying");
    }
}
