using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    GameObject cameraPositionsGroup;
    [SerializeField]
    GameObject roomTransitionsGroup;

    List<GameObject> cameraPositions = new List<GameObject>();
    List<Grave> checkpoints = new List<Grave>();
    List<GameObject> roomTransitions = new List<GameObject>();

    Grave curCheckpoint;

    public static RoomManager instance;
    int curRoomNum = 0;

    private void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!cameraPositionsGroup || !roomTransitionsGroup)
        {
            Debug.LogError("Mising group");
            return;
        }

        foreach (Transform child in cameraPositionsGroup.transform)
        {
            cameraPositions.Add(child.gameObject);
        }

        foreach (Transform child in roomTransitionsGroup.transform)
        {
            roomTransitions.Add(child.gameObject);
        }

        if (!(cameraPositions.Count == roomTransitions.Count))
        {
            Debug.LogError("Unequal number of camera positions, checkpoints, and room transitions");
        }

        Camera.main.transform.position = cameraPositions[curRoomNum].transform.position;
    }

    public void NextRoom()
    {
        if (curRoomNum == cameraPositions.Count - 1)
        {
            Debug.Log("No more rooms");
            return;
        }

        roomTransitions[curRoomNum].SetActive(false);
        curRoomNum += 1;
        roomTransitions[curRoomNum].SetActive(true);

        RoomCamera.instance.StartTransition(cameraPositions[curRoomNum].transform.position);
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
        yield return new WaitForSeconds(.5f);
        yield return GameManager.instance.FadeOut(1f);
        PlayerController.player1.Respawn();
        PlayerController.player2.Respawn();
        yield return GameManager.instance.FadeIn(1f);
        PlayerController.blockingInput = false;

    }

}
