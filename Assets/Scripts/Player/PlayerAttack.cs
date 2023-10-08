using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The player attack script is used to check whether the player is trying to attack and if any enemies are in range of the player.
//A YouTube tutorial was used as a blueprint for the script and modified to better suit the project.
//YouTube Link: https://youtu.be/1QfxdUpVh5I


public class PlayerAttack : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    //Attack Delays
    private float currentAttackDelay;
    public float attackDelay;

    //Attack Position and Range
    public Transform attackPos;
    public float attackRange;

    //Attack Damage
    public int damage;

    //What enemies to target
    public LayerMask whatIsMeleeEnemies;
    public LayerMask whatIsRangedEnemies;
    public LayerMask whatIsFlyingEnemies;
    
    //Player controller
    private PlayerController player;

    private void Start()
    {
        //Find player controller
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        //Counts down the attack delay constantly to see if the player can attack
        currentAttackDelay -= Time.deltaTime;

        //Checks if the player is alive
        if(player.GetIsDead() == false)
        {
            //If the attack delay reaches 0 and the player has pressed mouse 1 the player will attack.
            //This will last as long as the player holds down mouse 1
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAttackDelay <= 0)
                {
                    //Updates player variable so animations can play appropriately
                    player.SetAttacking(true);

                    //Plays attacking sound effect
                    SFXManager.PlaySound("PlayerAttack2");

                    //Resets the attack delay
                    currentAttackDelay = attackDelay;

                    //MELEE ENEMIES
                    //Creates a circle collider from the attack position and range and creates an array of any melee enemies inside the collider using a Layer Mask.
                    Collider2D[] meleeEnemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsMeleeEnemies);
                    //Loops through the array above and makes any melee enemies take damage.
                    for (int i = 0; i < meleeEnemiesToDamage.Length; i++)
                    {
                        meleeEnemiesToDamage[i].GetComponent<EnemyMelee>().TakeDamage(damage);
                    }

                    //RANGED ENEMIES
                    //Creates a circle collider from the attack position and range and creates an array of any ranged enemies inside the collider using a Layer Mask.
                    Collider2D[] rangedEnemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsRangedEnemies);
                    //Loops through the array above and makes any ranged enemies take damage.
                    for (int i = 0; i < rangedEnemiesToDamage.Length; i++)
                    {
                        rangedEnemiesToDamage[i].GetComponent<EnemyRanged>().TakeDamage(damage);
                    }

                    //FLYING ENEMIES
                    //Creates a circle collider from the attack position and range and creates an array of any flying enemies inside the collider using a Layer Mask.
                    Collider2D[] flyingEnemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsFlyingEnemies);
                    //Loops through the array above and makes any flying enemies take damage.
                    for (int i = 0; i < flyingEnemiesToDamage.Length; i++)
                    {
                        flyingEnemiesToDamage[i].GetComponent<EnemyFlying>().TakeDamage(damage);
                    }
                }
            }
            else
            {
                //Stops the player animation from showing the attacking varient
                player.SetAttacking(false);
            }
        }  
    }

    //Draws an outline of the player attack range for use in engine.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
