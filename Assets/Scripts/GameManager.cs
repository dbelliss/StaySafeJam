using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string GROUND_TAG = "Ground";
    public static string PLAYER_TAG = "Player";
    public static string SPIKE_TAG = "Spike";
    public static string ENEMY_TAG = "Enemy";
    public static string RING_TAG = "Ring";

    public static GameManager instance;

    [SerializeField]
    bool singlePlayer = true;

    [SerializeField]
    CanvasGroup fadeCanvas;

    const string levelPrefix = "Level";

    const string UNLOCKED = "UNLOCKED";

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }

        if (!fadeCanvas)
        {
            Debug.LogError("No fade canvas set");
        }
        else
        {
            fadeCanvas.alpha = 1;
            StartCoroutine(FadeIn(1f));
        }
    }

    public bool isSinglePlayer()
    {
        return singlePlayer;
    }

    public void SetSinglePlayer(bool val)
    {
        singlePlayer = val;
    }

    public IEnumerator FadeOut(float fadeTime)
    {
        float startTime = Time.time;
        float endTime = startTime + fadeTime;

        while (Time.time < endTime)
        {
            float progress = (Time.time - startTime) / (endTime - startTime);
            fadeCanvas.alpha = Mathf.Lerp(0, 1, progress);
            yield return new WaitForEndOfFrame();
        }

        fadeCanvas.alpha = 1;
    }

    public IEnumerator FadeIn(float fadeTime)
    {
        float startTime = Time.time;
        float endTime = startTime + fadeTime;

        while (Time.time < endTime)
        {
            float progress = (Time.time - startTime) / (endTime - startTime);
            fadeCanvas.alpha = Mathf.Lerp(1, 0, progress);
            yield return new WaitForEndOfFrame();
        }

        fadeCanvas.alpha = 0;
        fadeCanvas.interactable = false;
        fadeCanvas.blocksRaycasts = false;
    }

    public void ToMainMenu()
    {
        StartCoroutine(LoadLevel("MainMenu"));
    }

    public void ToLevel()
    {
        StartCoroutine(LoadLevel(levelPrefix + "1"));
    }

    public void NextLevel()
    {
        int nextLevelIndex = SceneManager.GetSceneByName(levelPrefix + "1").buildIndex + 1;
        PlayerPrefs.SetString(levelPrefix + nextLevelIndex.ToString(), UNLOCKED);
        StartCoroutine(levelPrefix + nextLevelIndex);
    }

    public bool IsLevelUnlocked(int levelNum)
    {
        return PlayerPrefs.GetString(levelPrefix + levelNum.ToString(), "") == UNLOCKED;
    }

    public void Restart()
    {

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadLevel(string levelName)
    {
        yield return FadeOut(1f);
        SceneManager.LoadScene(levelName);
        yield return FadeIn(1f);
    }
}
