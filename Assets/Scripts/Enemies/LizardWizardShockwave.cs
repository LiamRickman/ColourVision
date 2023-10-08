using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the individual shockwave projectiles fired by the lizard wizard

public class LizardWizardShockwave : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------

    //Movement
    public float moveSpeed;
    private Vector3 moveDirection;

    //Game Objects
    private Rigidbody2D rb;

    //---------- SET UP SHOCKWAVE PROJECTILE ----------
    private void Start()
    {
        //Find components
        rb = GetComponent<Rigidbody2D>();

        //Destroy projectile after 5f
        Destroy(gameObject, 5f);
    }

    //---------- FIXED UPDATE ----------
    private void FixedUpdate()
    {
        //Moves the projectile in the move direction set in the lizard wizard script
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y).normalized * moveSpeed; ;
    }

    //---------- COLLISION DETECTION ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the projectile collides with the player the projectile is destroyed
        if (collision.transform.CompareTag("Player Hitbox"))
        {
            Destroy(gameObject);
        }

        //If the projectile collides with the tilemap collisions it is destroyed
        if (collision.transform.CompareTag("Collisions"))
        {
            Destroy(gameObject);
        }
    }

    //---------- GET / SET FUNCTIONS ----------

    //Move Direction
    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

}
