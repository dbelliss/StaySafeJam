using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RoomCamera : MonoBehaviour
{
    public static RoomCamera instance;
    
    [SerializeField]
    float transitionTime = 2f;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //// set the desired aspect ratio (the values in this example are
        //// hard-coded for 16:9, but you could make them into public
        //// variables instead so you can set them at design time)
        //float targetaspect = 4f / 3f;

        //// determine the game window's current aspect ratio
        //float windowaspect = (float)Screen.width / (float)Screen.height;

        //// current viewport height should be scaled by this amount
        //float scaleheight = windowaspect / targetaspect;

        //// obtain camera component so we can modify its viewport
        //Camera camera = GetComponent<Camera>();

        //// if scaled height is less than current height, add letterbox
        //if (scaleheight < 1.0f)
        //{
        //    Rect rect = camera.rect;

        //    rect.width = 1.0f;
        //    rect.height = scaleheight;
        //    rect.x = 0;
        //    rect.y = (1.0f - scaleheight) / 2.0f;

        //    camera.rect = rect;
        //}
        //else // add pillarbox
        //{
        //    float scalewidth = 1.0f / scaleheight;

        //    Rect rect = camera.rect;

        //    rect.width = scalewidth;
        //    rect.height = 1.0f;
        //    rect.x = (1.0f - scalewidth) / 2.0f;
        //    rect.y = 0;

        //    camera.rect = rect;
        //}
        PixelPerfectCamera ppc = GetComponent<PixelPerfectCamera>();
        ppc.cropFrameX = true;
        ppc.cropFrameY = true;
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
