using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    [SerializeField]
    PlayerController toToss;

    Collider2D trigger;

    // Start is called before the first frame update
    void Start()
    {
        if (!toToss)
        {
            Debug.LogError("No object to toss");
        }
        trigger = GetComponent<Collider2D>();
        trigger.enabled = false;
    }

    public void SetTrigger(bool isOn)
    {
        trigger.enabled = isOn;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject == toToss.gameObject)
        {
            if (toToss == PlayerController.player1.gameObject)
            {
                PlayerController.player2.Toss();
            }
            else
            {
                PlayerController.player1.Toss();
            }
            toToss.Tossed();
        }
    }
}
