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

    [SerializeField]
    float offset = 5;
    float cloudWidth;

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

    public void Update()
    {

        if (clouds.Length == 0)
        {
            return;
        }
        // Move clouds
        cloudWidth = clouds[0].GetComponent<SpriteRenderer>().sprite.rect.width / 16f + offset;
        for (int i = 0; i < clouds.Length; i++)
        {
            GameObject cloud = clouds[i];
            if (cloud.transform.localPosition.x < -cloudWidth)
            {
                cloud.transform.localPosition += Vector3.right * cloudWidth * 2;
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
