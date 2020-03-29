using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Pause") > 0)
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        GameManager.instance.Restart();
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.ToMainMenu();
    }
}
