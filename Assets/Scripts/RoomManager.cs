using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    List<Grave> checkpoints = new List<Grave>();

    Grave curCheckpoint;

    public static RoomManager instance;
    int curRoomNum = 0;

    private void Awake()
    {
        instance = this;    
    }

    public void Die()
    {
        PlayerController.player1.Die();
        PlayerController.player2.Die();
        StartRespawn();
    }

    public Grave GetCheckpoint()
    {
        return curCheckpoint;
    }

    public void GraveSetCheckpoint(Grave g)
    {
        curCheckpoint = g;
    }

    public void AddCheckpoint(Grave g)
    {
        checkpoints.Add(g);
    }

    public void StartRespawn()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        PlayerController.blockingInput = true;
        yield return new WaitForSeconds(.3f);
        yield return GameManager.instance.FadeOut(.8f);
        PlayerController.player1.Respawn();
        PlayerController.player2.Respawn();
        yield return GameManager.instance.FadeIn(1f);
        PlayerController.blockingInput = false;

    }

}
