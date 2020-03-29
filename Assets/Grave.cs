using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    Animator animator;
    const string animActivateID = "activate";
    const string animDeactivateID = "deactivate";

    private void Awake()
    {
        RoomManager.instance.AddCheckpoint(this);
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameManager.PLAYER_TAG)
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
        }
        
        RoomManager.instance.GraveSetCheckpoint(this);
        animator.SetTrigger(animActivateID);
    }

    public void Deactivate()
    {
        animator.SetTrigger(animDeactivateID);
    }
}
