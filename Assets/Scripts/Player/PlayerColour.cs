using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script makes the player sprite flash greyscale when they take damage
//I primarily used a YouTube tutorial for this seen here: https://youtu.be/ktybkQZp2A4
//The tutorial featued a downloadable shader that controls the majority of the greyscale effect.

public class PlayerColour : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    private PlayerController player;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        //Find necessary components
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //If the player has been damaged the player sprite is set to greyscale.
        if (player.GetIsDamaged())
        {
            spriteRenderer.material.SetFloat("_GrayscaleAmount", 1);
        }
        //Otherwise there colour will be set to standard.
        else
        {
            spriteRenderer.material.SetFloat("_GrayscaleAmount", 0);
        }
    }
}
