using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string GROUND_TAG = "Ground";
    public static string PLAYER_TAG = "Player";
    public static string SPIKE_TAG = "Spike";
    public static string RING_TAG = "Ring";

    public static GameManager instance;

    [SerializeField]
    bool singlePlayer = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isSinglePlayer()
    {
        return singlePlayer;
    }
}
