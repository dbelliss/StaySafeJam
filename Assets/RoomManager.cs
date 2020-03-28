using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    GameObject cameraPositionsGroup;
    [SerializeField]
    GameObject checkpointsGroup;
    [SerializeField]
    GameObject roomTransitionsGroup;

    List<GameObject> cameraPositions = new List<GameObject>();
    List<GameObject> checkpoints = new List<GameObject>();
    List<GameObject> roomTransitions = new List<GameObject>();

    public static RoomManager instance;
    int curRoomNum = 0;

    private void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!cameraPositionsGroup || !checkpointsGroup || !roomTransitionsGroup)
        {
            Debug.LogError("Mising group");
            return;
        }

        foreach (Transform child in cameraPositionsGroup.transform)
        {
            cameraPositions.Add(child.gameObject);
        }

        foreach (Transform child in checkpointsGroup.transform)
        {
            checkpoints.Add(child.gameObject);
        }

        foreach (Transform child in roomTransitionsGroup.transform)
        {
            roomTransitions.Add(child.gameObject);
        }

        if (!(cameraPositions.Count == checkpoints.Count && cameraPositions.Count == roomTransitions.Count))
        {
            Debug.LogError("Unequal number of camera positions, checkpoints, and room transitions");
        }

        Camera.main.transform.position = cameraPositions[curRoomNum].transform.position;

        for (int i = 1; i < checkpoints.Count; i++)
        {
            checkpoints[i].SetActive(false);
        }
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

    public GameObject GetCheckpoint()
    {
        return checkpoints[curRoomNum];
    }

}
