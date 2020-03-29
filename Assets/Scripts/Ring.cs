using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    RingManager ringManager;
    FixedJoint2D joint;
    bool isGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        ringManager = RingManager.instance;
        ringManager.AddRing(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grab(PlayerController player)

    {
        if (isGrabbed)
        {
            return;
        }

        //joint.connectedBody = player.GetComponent<Rigidbody2D>();

    }
}
