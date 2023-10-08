using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script moves the player between rooms in the level

public class Teleport : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    private PlayerController player;
    public int newRoomNumber;
    public Vector2 teleportDestination;

    private void Start()
    {
        //Find player game object
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //---------- COLLISION DETECTION ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player collides with the object the players position is set to the teleport destination
        //The room number and player start position (for checkpoints) is also updated.
        if (collision.transform.CompareTag("Player"))
        {
            //Update player position
            player.transform.position = (teleportDestination);

            //Update room number
            player.SetRoomNumber(newRoomNumber);

            //Set player checkpoint
            player.SetStartPos(teleportDestination);
        }
    }
}
