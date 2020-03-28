using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadingText : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    Color color = Color.black;

    float endTime;

    [SerializeField]
    float timeToLive;
    [SerializeField]
    float risingSpeed = .1f;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        endTime = timeToLive + Time.time;
        StartCoroutine(FadeAndRise());
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
    }

    IEnumerator FadeAndRise()
    {
        Color startColor = color;
        float startTime = Time.time;
        while (Time.time < endTime)
        {
            float progress = (Time.time - startTime) / (endTime - startTime);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, 1 - progress);
            textMesh.transform.position += Vector3.up * risingSpeed;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
