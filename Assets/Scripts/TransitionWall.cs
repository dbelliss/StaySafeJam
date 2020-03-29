using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionWall : MonoBehaviour
{
    bool triggered = false;

    bool isPlayer1Inside;
    bool isPlayer2Inside;

    float horizontalMoveFactor = 19f;
    float verticalMoveFactor = 13f;

    [SerializeField]
    WallDirection wallDir;

    public enum WallDirection
    {
        LEFT,
        UP,
        RIGHT,
        DOWN,
    }

    Vector3 roomDir;
    
    // Start is called before the first frame update
    void Start()
    {
        switch(wallDir)
        {
            case WallDirection.LEFT:
                roomDir = Vector3.left * 19;
                break;
            case WallDirection.DOWN:
                roomDir = Vector3.down * verticalMoveFactor;
                break;
            case WallDirection.UP:
                roomDir = Vector3.up * verticalMoveFactor;
                break;
            case WallDirection.RIGHT:
                roomDir = Vector3.right * 19;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isPlayer2Inside && isPlayer1Inside && !triggered)
        {
            triggered = true;
            RoomCamera.instance.StartTransition(roomDir);
            StartCoroutine(DisableTemporarily());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.player1.gameObject)
        {
            Debug.Log("Player1 is going " + wallDir.ToString());
            isPlayer1Inside = true;
        }
        else if (collision.gameObject == PlayerController.player2.gameObject)
        {
            isPlayer2Inside = true;
        }
    }

    IEnumerator DisableTemporarily()
    {
        triggered = true;
        yield return new WaitForSeconds(.3f);
        triggered = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.player1.gameObject)
        {
            Debug.Log("Player1 is not going " + wallDir.ToString());
            isPlayer1Inside = false;
        }
        else if (collision.gameObject == PlayerController.player2.gameObject)
        {
            Debug.Log("Player2 is not going " + wallDir.ToString());
            isPlayer2Inside = false;
        }
    }
}
