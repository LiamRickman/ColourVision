using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used alongside the ranged enemy to shoot a projectile at the player.
//As with the ranged enemy a YouTube tutorial was used to get the basics of player tracking and spawning objects
//YouTube Link: https://youtu.be/kOzhE3_P2Mk

public class EnemyRangedProjectile : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------

    //Movement
    public float moveSpeed = 2f;
    private Vector2 moveDirection;

    //Game Objects
    private Rigidbody2D rb;
    private PlayerController player;
    private Animator animator;

    //Level
    private int currentLevel;

    //---------- SET UP PROJECTILE AND FIRE AT PLAYER ----------
    private void Start()
    {
        //Projectile SFX
        SFXManager.PlaySound("ThrowieAttack");
        //Find Game objects
        player = GameObject.FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //set current level to the same as the players
        currentLevel = player.GetCurrentLevel();

        //Set animations relative to the current level
        animator.SetInteger("Current Level", currentLevel);

        //Set move direction towards the player
        moveDirection = (player.transform.position - transform.position).normalized * moveSpeed;

        //Move the projectile towards the player
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);

        //Destroy the projectile after 3f
        Destroy(gameObject, 3f);
    }

    //--------- CHECK FOR COLLISIONS -----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the projectile has collided with the player hitbox and destroys the object if it has
        if (collision.transform.CompareTag("Player Hitbox"))
        {
            Destroy(gameObject);
        }

        //Check if the projectile has collided with any collisions in the scene and destroy the object if it has.
        if (collision.transform.CompareTag("Collisions"))
        {
            Destroy(gameObject);
        }
    }
}
