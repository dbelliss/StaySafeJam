using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().isOn = !GameManager.instance.isSinglePlayer();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
