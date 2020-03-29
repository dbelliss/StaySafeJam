using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RoomCamera : MonoBehaviour
{
    public static RoomCamera instance;
    
    float transitionTime = 1f;

    [SerializeField]
    float scrollSpeed = .2f;

    [SerializeField]
    GameObject[] clouds;

    Vector3 lastCloudStartPos;

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
        lastCloudStartPos = clouds[clouds.Length - 1].transform.localPosition;
    }

    public void Update()
    {
        // Move clouds
        for (int i = 0; i < clouds.Length; i++)
        {
            GameObject cloud = clouds[i];
            if (cloud.transform.localPosition.x < -cloud.transform.localScale.x)
            {
                cloud.transform.localPosition += Vector3.right * cloud.transform.localScale.x * 2;
            }
            cloud.transform.localPosition += Vector3.left * Time.deltaTime * scrollSpeed;

        }
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
