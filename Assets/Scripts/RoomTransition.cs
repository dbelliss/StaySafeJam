using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameManager.PLAYER_TAG)
        {
            RoomManager.instance.NextRoom();
        }
    }
}
