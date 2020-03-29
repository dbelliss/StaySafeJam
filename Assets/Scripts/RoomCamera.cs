using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public static RoomCamera instance;
    
    [SerializeField]
    float transitionTime = 2f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTransition(Vector3 endPosition)
    {
        StartCoroutine(Transition(endPosition));
    }

    IEnumerator Transition(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        float endTime = Time.time + transitionTime;
        while (Time.time < endTime)
        {
            float progress = (Time.time - startTime) / (endTime - startTime);
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPosition;
    }
}
