using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This healthpack script allows the player to add to their health by collecting the healthpack.

public class Healthpack : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    [Range(1, 4)]
    public int currentLevel = 1;

    //Game Objects
    private PlayerController player;
    public Animator healthpackAnimator;

    private void Start()
    {
        //Find player game object
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        //Set healthpack colour to the current level varient
        healthpackAnimator.SetInteger("Level", currentLevel);
    }

    //---------- COLLISION DETECTION ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player has lost health they can collide with the object and add health to themself whilst destroying the healthpack.
        if (player.GetHealth() < player.GetMaxHealth())
        {
            if (collision.transform.CompareTag("Player"))
            {
                Destroy(gameObject);
                player.AddHealth(1);
            }
        }
    }
}

