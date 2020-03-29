using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RoomCamera : MonoBehaviour
{
    public static RoomCamera instance;
    
    float transitionTime = 1f;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        PixelPerfectCamera ppc = GetComponent<PixelPerfectCamera>();
        ppc.cropFrameX = true;
        ppc.cropFrameY = true;
    }


    public void StartTransition(Vector3 deltaPosition)
    {
        StartCoroutine(Transition(deltaPosition + transform.position));
    }

    IEnumerator Transition(Vector3 endPosition)
    {
        PlayerController.blockingInput = true;
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
        PlayerController.blockingInput = false;
    }
}
