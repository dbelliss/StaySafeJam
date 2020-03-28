using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    bool isSingleplayer;
    PlayerController player1;
    PlayerController player2;
    Vector3 offset;
    float leftBound = -1;
    float rightBound = 1;
    float topBound = -1;
    float bottomBound = 1;

    float stepSize = .01f;
    
    // Start is called before the first frame update
    void Start()
    {
        
        isSingleplayer = GameManager.instance.isSinglePlayer();
        player1 = PlayerController.player1;
        player2 = PlayerController.player2;
        offset = transform.position - GetFollowPosition();
    }

    private Vector3 GetFollowPosition()
    {
        if (isSingleplayer)
        {
            if (player1.GetPlayerType() == PlayerController.PlayerType.ON)
            {
                return player1.transform.position;
            }
            return player2.transform.position;
        }
        else
        {
            return (player1.transform.position + player2.transform.position)/2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 followPosition = GetFollowPosition();

        if ((transform.position - followPosition).x < leftBound)
        {
            transform.position += Vector3.right * stepSize;
        }
        if ((transform.position - followPosition).x > rightBound)
        {
            transform.position += Vector3.left * stepSize;
        }


        if ((transform.position - followPosition).y < bottomBound)
        {
            transform.position += Vector3.up * stepSize;
        }
        if ((transform.position - followPosition).y > topBound)
        {
            transform.position += Vector3.down * stepSize;
        }
    }
}
