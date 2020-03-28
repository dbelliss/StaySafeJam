using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    [SerializeField]
    PlayerController toToss;

    // Start is called before the first frame update
    void Start()
    {
        if (!toToss)
        {
            Debug.LogError("No object to toss");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject == toToss.gameObject)
        {
            toToss.Toss();
        }
    }
}
