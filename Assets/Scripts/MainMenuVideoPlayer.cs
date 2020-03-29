using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuVideoPlayer : MonoBehaviour
{
    RawImage rawImage;
    VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(3f);
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
