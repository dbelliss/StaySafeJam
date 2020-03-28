using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.Audio;

public class GameLoader : MonoBehaviour
{
    public const string NOT_FIRST_GAME_LOAD_KEY = "NotFirstLoad";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForLogo());
    }

    IEnumerator WaitForLogo()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
