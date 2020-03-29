using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfLevelUnlocked : MonoBehaviour
{
    [SerializeField]
    int levelNum;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(GameManager.instance.IsLevelUnlocked(levelNum));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
