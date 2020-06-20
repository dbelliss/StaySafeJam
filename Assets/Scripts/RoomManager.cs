using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    List<Grave> checkpoints = new List<Grave>();

    Grave curCheckpoint;
    Vector3 checkpointCameraPosition;

    public static RoomManager instance;
    int curRoomNum = 0;

    bool isRestarting = false;
    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        checkpointCameraPosition = Camera.main.transform.position;
    }

    public void Die()
    {
        if (isRestarting)
        {
            return;
        }
        isRestarting = true;
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
        checkpointCameraPosition = Camera.main.transform.position;
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
        PlayerController.player1.blockingInput = false;
        PlayerController.player2.blockingInput = false;
        yield return new WaitForSeconds(.3f);
        yield return GameManager.instance.FadeOut(.8f);
        PlayerController.player1.Respawn();
        PlayerController.player2.Respawn();
        Camera.main.transform.position = checkpointCameraPosition;
        yield return GameManager.instance.FadeIn(1f);
        PlayerController.player1.blockingInput = false;
        PlayerController.player2.blockingInput = false;
        isRestarting = false;
    }

}
