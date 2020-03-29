using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMainMenu()
    {
        GameManager.instance.ToMainMenu();
    }

    public void StartSinglePlayer()
    {
        GameManager.instance.SetSinglePlayer(true);
        GameManager.instance.ToLevel();
    }

    public void StartMultiPlayer()
    {
        GameManager.instance.SetSinglePlayer(false);
        GameManager.instance.ToLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel(int i)
    {
        GameManager.instance.LoadLevel(i);
    }
}
