using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += LoadGameScene; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGameScene(UnityEngine.Video.VideoPlayer vp) 
    {
        SceneManager.LoadScene("SampleScene");
    }
}
