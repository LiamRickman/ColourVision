using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows the camera to follow the players or another object in the scene. 
//It also has a toggle to allow it to be used as a standard camera in other scenes such as menus.

public class CameraFollow : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothAmount;
    public bool follow;

    private void FixedUpdate()
    {
        //If the follow boolean has been ticked in engine it will run the follow function
        if (follow)
        {
            Follow();
        }
    }

    private void Follow()
    {
        //Sets the target position with an offset if added in the scene
        Vector3 targetPosition = target.position + offset;

        //Smooths the position so its not as jarring for the player
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothAmount * Time.fixedDeltaTime);

        //Sets the camera position to the new smooth position
        transform.position = smoothPosition;
    }
}