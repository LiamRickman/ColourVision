using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the ranged enemy varient that shoots at the player when they get in range.
//A YouTube tutorial was used to get the basics of player tracking and spawning projectiles.
//YouTube Link: https://youtu.be/kOzhE3_P2Mk

public class EnemyRanged : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------

    //Health
    public int health = 3;
    private bool dead;
    private bool hurt;

    //Colour
    public float colourValue = 0.05f;

    //Ranged Stats
    public float fireRate;
    private float nextFire;
    private bool canShoot;
    private bool isActive;
    public float attackRange = 1f;

    //Game Objects
    private PlayerController player;
    public Animator animator;
    private Collider2D hitbox;
    public GameObject projectile;

    //What is player
    public LayerMask whatIsPlayer;

    //Room/Level Control
    public int roomNumber;
    public int currentLevel;

    private void Start()
    {
        //Find animator and set animations to correct varient depending on what level it is.
        animator = GetComponent<Animator>();
        animator.SetInteger("Current Level", currentLevel);

        //Find Gameobjects
        player = GameObject.FindObjectOfType<PlayerController>();
        hitbox = transform.GetComponentInChildren<Collider2D>();

        //Set up nextFire
        nextFire = Time.time;
    }

    //---------- UPDATE ----------
    private void Update()
    {
        //Runs CheckHealth function
        CheckHealth();

        //While the player is alive the animations are constantly updated.
        if (dead == false)
        {
            UpdateAnimations();
        }

        //Creates a circle collider that looks for the player
        Collider2D inRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);

        //If the player is in the collider range the enemy can start shooting
        if (inRange)
        {
            canShoot = true;
        }
        //Otherwise they will be unable to shoot.
        else
        {
            canShoot = false;
        }
    }

    //---------- FIXED UPDATE ----------
    private void FixedUpdate()
    {
        //If the player is alive
        if (player.GetIsDead() == false) 
        {
            //If the players room number matches the enemys room number
            if (roomNumber == player.GetRoomNumber())
            {
                //Enemy is set to active
                isActive = true;
            }
            //While the enemy is alive
            if (dead == false)
            {
                //If enemy is active
                if (isActive)
                {
                    //Cant fire if the enemy has just been damaged
                    if (hurt == false)
                    {
                        //If the enemy can shoot
                        if (canShoot)
                        {
                            //if the fire delay is low enough
                            if (Time.time > nextFire)
                            {
                                //play attack sound effect
                                SFXManager.PlaySound("ThrowieAttack");
                                //Plays attack animation depending on what level it is.
                                //Pink (Level 1)
                                if (currentLevel == 1)
                                {
                                    animator.Play("Pink Attack");
                                }
                                //Cyan (Level 2)
                                else if (currentLevel == 2)
                                {
                                    animator.Play("Cyan Attack");
                                }
                                //Yellow (Level 3)
                                else if (currentLevel == 3)
                                {
                                    animator.Play("Yellow Attack");
                                }

                                //Spawns a projectile on top of the enemy
                                Instantiate(projectile, transform.position, Quaternion.identity);

                                //Reset the fire rate counter
                                nextFire = Time.time + fireRate;
                            }
                        }
                    }
                }
            }
        }
    }

    //---------- ENEMY TAKES DAMAGE ----------
    public void TakeDamage(int damage)
    {
        hurt = true;

        //If the enemys health is above 0 the following code will run
        if (health > 0)
        {
            //hurt SFX
            SFXManager.PlaySound("ThrowieHurt");
            //The enemy takes damage
            health -= damage; 
        }

        //If the enemy has not died the appropriate hurt animations will be played
        if (dead == false)
        {
            //Pink Hurt (Level 1)
            if (currentLevel == 1)
            {
                animator.Play("Pink Hurt");
            }
            //Cyan Hurt (Level 2)
            else if (currentLevel == 2)
            {
                animator.Play("Cyan Hurt");
            }
            //Yellow Hurt (Level 3)
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
            //Death SFX
            //SFXManager.PlaySound("ThrowieDeath");

            //Destroys the hitbox so they cannot hurt the player anymore
            Destroy(hitbox);

            //Play Death Animation relative to current level
            //Dead boolean is set to false to stop code looping
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

    //---------- DESTROY PIGMENT AND ADD COLOUR ----------
    //This function is called from the death animations to allow the animation to play before destroying the gameobject
    public void DestroyPigment()
    {
        print("DESTROY RANGED PIGMENT");
        
        //Add colour to the colour value
        player.AddColour(colourValue);

        //Destroy the ranged enemy
        Destroy(gameObject);
    }

    //Updates the animation float values in relation to the players position so the enemy faces towards the player.
    private void UpdateAnimations()
    {
        animator.SetFloat("Horizontal", player.transform.position.x - transform.position.x);
        animator.SetFloat("Vertical", player.transform.position.y - transform.position.y);
    }

    //Draws attack range on screen to see how big of an area they effect
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //---------- GET / SET FUNCTIONS ----------
    public void SetActive(bool active)
    {
        isActive = active;
    }

    //Sets hurt bool to false so enemy can fire again
    public void ToggleHurtOff()
    {
        hurt = false;
    }

    // Harry's Version
    /*
    //---------- DECLARE VARIABLES ----------
    
    public int health;
    public float colourValue;

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    private PlayerController player;

    public int roomNumber;

    [SerializeField]
    private float range;

    // Start is called before the first frame update
    void Start()
    {
        //Find Player
        var playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerController>();

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {

        if (roomNumber == player.GetRoomNumber())
        {
            
            // follow/stop/retreat movements for the ranged enemy
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
            

            //spwans the enemy projectil at enemy loaction if there are no current projetiles
            if (timeBtwShots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }


        }
    }
    
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        print("Ranged Enemy health: " + health);
    }
    */
}