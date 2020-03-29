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

    }

    public void Play()
    {
        GameManager.instance.ToLevel();
    }

    public void ToggleSinglePlayer(bool val)
    {
        GameManager.instance.SetSinglePlayer(!val);
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
