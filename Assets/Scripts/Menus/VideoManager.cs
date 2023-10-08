using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

//This script controls the cutscene video player. It waits until the video player has finished and then loads the next scene
//This can be used for multiple cutscenes as it uses a public string to select the new scene

public class VideoManager : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    //Video Player
    private VideoPlayer videoPlayer;
    
    //Frame counters
    private float currentFrame;
    private float frameCount;

    //Scene selection
    public string nextScene;

    private void Start()
    {
        //Find video player
        videoPlayer = GetComponent<VideoPlayer>();

        //Set the frame count of the video
        //Some videos tested got stuck on the last couple of frames so I lowered by 2 to fix this
        frameCount = videoPlayer.frameCount - 2;
    }

    private void Update()
    {
        //Update which frame is currently playing
        currentFrame = videoPlayer.frame;

        //Once the last frame is played the next scene is loaded.
        if (currentFrame == frameCount)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
