using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    Animator animator;
    const string animActivateID = "activate";
    const string animDeactivateID = "deactivate";

    [SerializeField]
    bool isFirst = false;

    AudioSource audioSource;

    private void Awake()
    {
        RoomManager.instance.AddCheckpoint(this);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isFirst)
        {
            Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameManager.PLAYER_TAG && RoomManager.instance.GetCheckpoint() != this)
        {
            Activate();
        }
    }

    public void Activate()
    {
        Grave lastCheckpoint = RoomManager.instance.GetCheckpoint();
        if (lastCheckpoint)
        {
            lastCheckpoint.Deactivate();
            audioSource.PlayOneShot(audioSource.clip);
        }
        
        RoomManager.instance.GraveSetCheckpoint(this);
        animator.SetTrigger(animActivateID);
    }

    public void Deactivate()
    {
        animator.SetTrigger(animDeactivateID);
    }
}
