using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isSinglePlayer()
    {
        return singlePlayer;
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
        fadeCanvas.interactable = true;
        fadeCanvas.blocksRaycasts = true;
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
}
