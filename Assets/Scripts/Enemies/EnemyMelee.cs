using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the melee pigment that patrols between two points attacking the player if they get close.

public class EnemyMelee : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    //Health
    public int health;
    private bool dead;

    //Colour Value
    public float colourValue;

    //Speed
    private float speed;
    public float moveSpeed;
    private float step;

    //Stun Variables
    private float currentStunDelay;
    public float stunDelay;

    //Movement Variables
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 currentPos;

    private bool moveToEnd;
    private bool move = true;

    //Gameobjects
    private PlayerController player;
    public Animator animator;
    private Collider2D hitbox;
    public GameObject end;

    //Animation Variables
    private float startX;
    private float startY;
    private float endX;
    private float endY;
    private bool updateAnim = true;

    //Is Enemy Active
    public int roomNumber;
    public int currentLevel;
    private bool isActive;

    // Start is called before the first frame update
    private void Start()
    {
        //Find Gameobjects
        player = GameObject.FindObjectOfType<PlayerController>();
        hitbox = transform.GetComponentInChildren<Collider2D>();

        //Update Anim
        animator = GetComponent<Animator>();
        animator.SetInteger("Current Level", currentLevel);

        //Set speed values
        speed = moveSpeed;
        step = speed * Time.deltaTime;

        //Set positions
        startPos = transform.position;
        endPos = end.transform.position;
    }

    //---------- UPDATE ----------
    private void Update()
    {
        //Update animations to fit current level theme
        animator.SetInteger("Current Level", currentLevel);

        //Runs check health function
        CheckHealth();
    }

    //---------- FIXED UPDATE ----------
    private void FixedUpdate()
    {
        //If the player is alive the enemy will move otherwise it will stay still
        if (player.GetIsDead() == false && roomNumber == player.GetRoomNumber())
        {
            isActive = true;
        }
        //Checks if the enemy is not dead and is active
        if (dead == false && isActive)
        {
            //If the stun delay is less than or equal to 0 the speed is reset.
            if (currentStunDelay <= 0)
            {
                speed = moveSpeed;
            }
            //Otherwise the enemy speed is set to 0 and the stun delay will count down.
            else
            {
                speed = 0;
                currentStunDelay -= Time.deltaTime;
            }
            //Current position is updated to the enemys position
            currentPos = transform.position;

            //If the enemy can move they will begin the move script
            if (move)
            {
                //Checks if the enemy is set to move to the end.
                if (moveToEnd == true)
                {
                    //Animations are updated to face the end position
                    if (updateAnim)
                    {
                        //Float updates
                        endX = endPos.x - transform.position.x;
                        endY = endPos.y - transform.position.y;

                        //Magnitude animation value updated
                        animator.SetFloat("Magnitude", (endPos - transform.position).magnitude);

                        //Calculates which direction the animation should face depending on the endX and endY floats.
                        //End X calculation
                        if (endX < 0)
                        {
                            animator.SetFloat("Horizontal", -1f);
                        }
                        else if (endX > 0)
                        {
                            animator.SetFloat("Horizontal", 1f);
                        }
                        //End Y calculation
                        if (endY < 0)
                        {
                            animator.SetFloat("Vertical", -1f);
                        }
                        else if (endY > 0)
                        {
                            animator.SetFloat("Vertical", 1f);
                        }

                        //Sets the last horizontal and vertical directions for the death and attack animations.
                        if (endPos.x - transform.position.x == 1 || endPos.x - transform.position.x == -1 || endPos.y - transform.position.y == -1 || endPos.y - transform.position.y == -1)
                        {
                            animator.SetFloat("Last Horizontal", (endPos.x - transform.position.x));
                            animator.SetFloat("Last Vertical", (endPos.y - transform.position.y));
                        }

                        //Flips the update animation boolean so it can update to face the other direction.
                        updateAnim = false;
                    }

                    //Moves the enemy towards the end position at "step" speed
                    transform.position = Vector3.MoveTowards(transform.position, endPos, step);
                }
                //If the player is not moving towards the end it will move back to its start position.
                else
                {
                    //Enemy animations are updated to face start position
                    if (updateAnim == false)
                    {
                        //Magnitude animation value updated
                        animator.SetFloat("Magnitude", (startPos - transform.position).magnitude);

                        //Float updates
                        startX = startPos.x - transform.position.x;
                        startY = startPos.y - transform.position.y;

                        //Calculates which direction the animation should face depending on the direction of startX and startY.
                        //Start X Calculations
                        if (startX < 0)
                        {
                            animator.SetFloat("Horizontal", -1f);
                        }
                        else if (startX > 0)
                        {
                            animator.SetFloat("Horizontal", 1f);
                        }
                        //Start Y calculations
                        if (startY < 0)
                        {
                            animator.SetFloat("Vertical", -1f);
                        }
                        else if (startY > 0)
                        {
                            animator.SetFloat("Vertical", 1f);
                        }

                        //Sets the last horizontal and vertical directions for the death and attack animations.
                        if (startPos.x - transform.position.x == 1 || startPos.x - transform.position.x == -1 || startPos.y - transform.position.y == -1 || startPos.y - transform.position.y == -1)
                        {
                            animator.SetFloat("Last Horizontal", (startPos.x - transform.position.x));
                            animator.SetFloat("Last Vertical", (startPos.y - transform.position.y));
                        }
                        //Flips update animation boolean so it can update animations facing the other direction
                        updateAnim = true;
                    }

                    //Moves the enemy towards the start position at "step" speed.
                    transform.position = Vector3.MoveTowards(transform.position, startPos, step);
                }

                //Checks if the enemy has reached the end position and if so will set it to move back to the start position
                if (currentPos == endPos)
                {
                    moveToEnd = false;
                }
                //If the enemy has reached the start position it will move towards the end positon
                else if (currentPos == startPos)
                {
                    moveToEnd = true;
                }
            }
        }
    }

    //---------- CHECK IF ENEMY HAS DIED ----------
    private void CheckHealth()
    {
        //If the enemy reaches 0 health some booleans are updated
        if (health <= 0)
        {
            dead = true;
            move = false;
        }

        //Once the enemy dies this will run
        if (dead)
        {
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

    //---------- ENEMY TAKES DAMAGE ----------
    public void TakeDamage(int damage)
    {
        //If the enemys health is above 0 the following code will run
        if (health > 0)
        {
            //Hurt sound effect
            SFXManager.PlaySound("RollieHurt");
            //The stun delay is reset
            currentStunDelay = stunDelay;

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
            else if (currentLevel == 3)
            {
                animator.Play("Yellow Hurt");
            }
        }
    }

    //---------- CHECK IF ENEMY HAS COLLIDED WITH PLAYER ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //As long as the enemy is not dead this will run
        if (dead == false)
        {
            //If the enemy collides with the player hitbox the attack animations will play
            if (collision.transform.CompareTag("Player Hitbox"))
            {
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
    }

    //---------- DESTROY PIGMENT AND ADD COLOUR ----------
    //This function is called from the death animations to allow the animation to play before destroying the gameobject
    public void DestroyPigment()
    {
        print("DESTROY MELEE PIGMENT");
        //Add to the colour value
        player.AddColour(colourValue);

        //Destroy melee enemy
        Destroy(gameObject);
    }

    //---------- GET / SET FUNCTIONS ----------
    public void SetActive(bool active)
    {
        isActive = active;
    }
}
