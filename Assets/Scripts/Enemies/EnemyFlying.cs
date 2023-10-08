using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the flying enemy.
//This enemy flys towards the player when in range and backs away if the player gets out of this range.

public class EnemyFlying : MonoBehaviour

{
    //---------- DECLARE VARIABLES ----------

    //Health
    public int health;
    private bool dead;

    //Colour Value
    public float colourValue;

    //Speed
    private float speed;
    public float maxSpeed;

    //Game objects
    private PlayerController player;
    public Animator animator;
    private Collider2D hitbox;

    //Positions
    public float stoppingDistance;
    public float retreatDistance;
    private Vector3 startPos;

    //Room/Level Control
    public int roomNumber;
    public int currentLevel;

    //Is enemy active
    private bool isActive;

    //Range
    public float range;


    //---------- SET UP ENEMY ---------- 
    private void Start()
    {
        //Find Gameobjects
        animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();
        hitbox = transform.GetComponentInChildren<Collider2D>();

        //Set animations to current level varients
        animator.SetInteger("Current Level", currentLevel);

        //Set starting position so the enemy has somewhere to return to
        startPos = transform.position;

        //Set max speed
        speed = maxSpeed;
    }

    //---------- UPDATE ----------
    private void Update()
    {
        //Run appropriate functions
        CheckHealth();
    }

    //---------- FIXED UPDATE ----------
    private void FixedUpdate()
    { 
        //If the player is alive and in the same room as the enemy, the enemy will be set to active
        if (player.GetIsDead() == false && roomNumber == player.GetRoomNumber())
        {
            isActive = true;
        }

        //If the enemy is still alive and active the enemy will try to follow the player
        if (dead == false)
        {
            if (isActive)
            {

                // follow/stop/retreat movements for the flying enemy
                if (Vector2.Distance(transform.position, player.transform.position) <= range)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
                else if (Vector2.Distance(transform.position, player.transform.position) < stoppingDistance && Vector2.Distance(transform.position, player.transform.position) > retreatDistance)
                {
                    transform.position = this.transform.position;
                }
                else if (Vector2.Distance(transform.position, player.transform.position) < retreatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.deltaTime);
                }
                else
                {
                    GoHome();
                }

                //As long as the enemy is alive animations will update relative to the players current position.
                if (dead == false)
                {
                    //Animations
                    animator.SetFloat("Horizontal", (player.transform.position.x - transform.position.x));
                    animator.SetFloat("Vertical", (player.transform.position.y - transform.position.y));
                }
            }
        }
    }

    //Sends the enemy back to the start position when out of range of the player
    public void GoHome()
    {
        if (dead == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

            //Updates animations relative to the start position
            animator.SetFloat("Horizontal", (startPos.x - transform.position.x));
            animator.SetFloat("Vertical", (startPos.y - transform.position.y));
        }      
    }

    //---------- ENEMY TAKES DAMAGE ----------
    public void TakeDamage(int damage)
    {
        //If the enemys health is above 0 the following code will run
        if (health > 0)
        {
            //hurt sound effect
            SFXManager.PlaySound("FlyingHurt");
            //The enemy takes damage
            health -= damage;
        }

        //If the enemy has not died the appropriate hurt animations will be played
        if (dead == false)
        {
            //Pink Varient (Level 1)
            if (currentLevel == 1)
            {
                animator.Play("Pink Hurt");
            }
            //Cyan Varient (Level 2)
            else if (currentLevel == 2)
            {
                animator.Play("Cyan Hurt");
            }
            //Yellow Varient (Level 3)
            else
            {
                animator.Play("Yellow Hurt");
            }
        }
    }

    //---------- CHECK IF ENEMY HAS DIED ----------
    private void CheckHealth()
    {
        //If the enemy reaches 0 health they are set to dead
        if (health <= 0)
        {
            dead = true;
        }

        //Once the enemy dies this will run
        if (dead)
        {
            //Destroys the hitbox so they cannot hurt the player anymore
            Destroy(hitbox);

            //Play Death Animation relative to current level
            //Pink Varient (Level 1)
            if (currentLevel == 1)
            {
                animator.Play("Pink Death");
                dead = false;
            }
            //Cyan Varient (Level 2)
            else if (currentLevel == 2)
            {
                animator.Play("Cyan Death");
                dead = false;
            }
            //Yellow Varient (Level 3)
            else
            {
                animator.Play("Yellow Death");
                dead = false;
            }
        }
    }

    //---------- CHECK IF ENEMY HAS COLLIDED WITH PLAYER ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the enemy collides with the player hitbox the attack animations will play
        if (collision.transform.CompareTag("Player Hitbox"))
        {
            //attacking sound effect
            SFXManager.PlaySound("FlyingAttack");
            //Pink Attack (Level 1)
            if (currentLevel == 1)
            {
                animator.Play("Pink Attack");
            }
            //Cyan Attack (Level 2)
            else if (currentLevel == 2)
            {
                animator.Play("Cyan Attack");
            }
            //Yellow Attack (Level 3)
            else 
            {
                animator.Play("Yellow Attack");
            }
        }
    }

    //---------- DESTROY PIGMENT AND ADD COLOUR ----------
    //This function is called from the death animations to allow the animation to play before destroying the gameobject
    public void DestroyPigment()
    {
        print("DESTROY FLYING PIGMENT");
        //Update colour value
        player.AddColour(colourValue);

        //Destroy flying enemy
        Destroy(gameObject);
    }

    //Draws an outline of the range the flying enemy can see the player from
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    //---------- SET / GET FUNCTIONS ----------
    public void SetActive(bool active)
    {
        isActive = active;
    }
}

