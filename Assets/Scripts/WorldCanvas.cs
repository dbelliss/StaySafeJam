using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    public static WorldCanvas instance;

    [SerializeField]
    public FadingText ftPrefab;
    
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if (!ftPrefab)
        {
            Debug.LogError("No FadingTextPrefab set");
        }
    }
    public void SpawnText(Vector3 position, string text, Color color)
    {
        FadingText ft = Instantiate(ftPrefab);
        ft.SetText(text);
        ft.SetColor(color);
        ft.transform.position = position;
        ft.transform.SetParent(transform);
    }
}
