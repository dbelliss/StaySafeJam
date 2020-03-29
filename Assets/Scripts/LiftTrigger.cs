using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    [SerializeField]
    PlayerController toToss;

    Collider2D trigger;

    [SerializeField]
    GameObject following;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (!toToss)
        {
            Debug.LogError("No object to toss");
        }
        trigger = GetComponent<Collider2D>();
        trigger.enabled = false;
        offset = transform.position - following.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = offset + following.transform.position;
    }

    public void SetTrigger(bool isOn)
    {
        trigger.enabled = isOn;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == toToss.gameObject)
        {
            toToss.GetOtherPlayer().Toss();
            toToss.Tossed();
        }
    }
}
