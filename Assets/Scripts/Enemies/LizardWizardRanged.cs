using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the ranged projectile that the lizard wizard shoots
//A YouTube tutorial was used as a baseline for the ranged attack just as with the ranged enemy
//YouTube Link: https://youtu.be/kOzhE3_P2Mk

//Unlike the standard ranged enemy this projectile can be hit back at the lizard wizard when the player attacks

public class LizardWizardRanged : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------

    //Move Speed
    public float moveSpeed = 3f;

    //Damage
    public int damage = 1;
    private bool canDamageLizard;

    //Return
    public float returnRange = 1f;
    public Transform returnPos;

    //Directions
    private Vector3 moveDirection;

    //Game Objects
    private Rigidbody2D rb;
    private PlayerController player;
    private LizardWizard lizardWizard;

    //State Machine
    private enum State
    {
        ToPlayer,
        ToLizardWizard,
    }
    private State state;

    //What is player
    public LayerMask whatIsPlayer;

    //---------- SET UP PROJECTILE ----------
    private void Start()
    {
        //Find game objects
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerController>();
        lizardWizard = GameObject.FindObjectOfType<LizardWizard>();

        //Set move direction (defaults to player position)
        moveDirection = (player.transform.position - transform.position).normalized * moveSpeed;

        //Destroy gameobject after 5f
        Destroy(gameObject, 5f);

        //Set default state as to player
        state = State.ToPlayer;
    }

    //---------- UPDATE ----------
    private void Update()
    {
        //Create a collider and check if the player is inside the collider
        Collider2D inRange = Physics2D.OverlapCircle(returnPos.position, returnRange, whatIsPlayer);

        //Checks if the player is in range
        if (inRange)
        {
            //Checks if the player is attacking and starts returning the projectile to the lizard.
            if (player.GetAttacking())
            {
                state = State.ToLizardWizard;
            }
        }

        //If the lizard is killed the projectile is destroyed too
        if (lizardWizard.GetDestroyed())
        {
            Destroy(gameObject);
        }
    }

    //---------- FIXED UPDATE ----------
    private void FixedUpdate()
    {
        //State machine
        switch (state)
        {
            //Moves the projectile to the player
            case State.ToPlayer:
                rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
                //Stops the projectile damaging the lizard wizard
                canDamageLizard = false;

                break;

            //Moves the projectile to the Lizard Wizard
            case State.ToLizardWizard:
                //Lets the projectile damage the lizard wizard
                canDamageLizard = true;

                //Calculates a new move direction and moves it towards the lizard wizard
                moveDirection = (lizardWizard.transform.position - transform.position).normalized * moveSpeed;
                rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
                break;
        }
    }

    //---------- COLLISION DETECTION ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the projectile has collided with the player and destroys the object
        if (collision.transform.CompareTag("Player Hitbox"))
        {
            Destroy(gameObject);
        }

        //If the projectile can damage the lizard and collides with the lizard hitbox the boss will take damage
        if (canDamageLizard)
        {
            if (collision.transform.CompareTag("Lizard Hitbox"))
            {
                //Sound effect plays
                SFXManager.PlaySound("Boss");
                lizardWizard.TakeDamage(damage);

                //Projectile is destroyed
                Destroy(gameObject);
            }
        }

        //If the projectile collides with any tilemap collisions it is destroyed
        if (collision.transform.CompareTag("Collisions"))
        {
            Destroy(gameObject);
        }
    }

    //---------- DRAW RANGE ----------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(returnPos.position, returnRange);
    }

}
