using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls when the player takes damage.
//It is seperate from the player controller to allow it to have a different hitbox from the feet hitbox to help with the isometric viewpoint

public class PlayerTakeDamage : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    private PlayerController player;

    private void Start()
    {
        //Find the player controller
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //Check if the player has collided with any trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the player can currently take damage
        if (player.GetCanDamage())
        {
            //If the player collides with anything with the damage tag the player will take damage by calling the take damage function in the Player Controller.
            if (collision.transform.CompareTag("Damage"))
            {
                player.TakeDamage(1);
            }
        } 
    }
}
