using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VidController : MonoBehaviour
{
    [SerializeField] private string videoFileName;
    private VideoPlayer videoPlayer;
    [HideInInspector]public bool started = false;
    public UnityEvent VideoDone;
    public bool Queued;
    public bool VerticalMaxed = true;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Set Path
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.Log(videoPath);
        videoPlayer.url = videoPath;

        videoPlayer.Prepare();
    }
    private void Update()
    {
        videoPlayer.Prepare();
        if (started && !videoPlayer.isPlaying)
        {
            started = false;
            VideoDone.Invoke();
            Debug.Log("Video Done");
        }
        if(Queued && videoPlayer.isPrepared)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    { 
        if (videoPlayer != null && videoPlayer.isPrepared) 
        {
            // Set up display object
            videoPlayer.targetMaterialRenderer.enabled = true;
            Transform displayTransform = videoPlayer.targetMaterialRenderer.gameObject.transform;
            Vector3 newScale = displayTransform.localScale;
            if (VerticalMaxed)
            {
                newScale.x = videoPlayer.width * newScale.y / videoPlayer.height;
            }
            else
            {
                newScale.y = videoPlayer.height * newScale.x / videoPlayer.width;
            }
            displayTransform.localScale = newScale;

            // Start video
            videoPlayer.Play();
            started = true;
        }
        else
        {
            Debug.Log("No ready videoplayer, enqueuing");
            Queued = true;
        }
    }
}
