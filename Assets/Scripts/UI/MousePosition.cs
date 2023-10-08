using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script calculates the mouses world position
//This script was made while testing a player ranged weapon however this was not implemented.
//A toned down version was used in the player controller to direct melee attack animations later.

public class MousePosition : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    public Camera mainCamera;
    private Vector3 mouseWorldPos;

    private void Update()
    {
        //Creates a vecttor from the mouses input
        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Sets the Z position
        mouseWorldPos.z = 0f;

        //Moves the object to the mouses position (Was planned to use as a crosshair for the ranged weapon)
        transform.position = mouseWorldPos;
    }
}
