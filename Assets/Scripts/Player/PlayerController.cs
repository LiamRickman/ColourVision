using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This is the primary player controller for the game. Many game mechanics are run through the player as they are present in the majority of scenes.

//Various YouTube tutorials and other resources were used while creating this script. These are linked in comments at the appropriate sections. A full list is below:
//Dash Movement: https://youtu.be/Bf_5qIt9Gr8


public class PlayerController : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------

    //Standard Movement.
    public float moveSpeed = 3f;
    private Vector3 moveDirection;

    //Dash Movement.
    public float dashSpeed = 10f;
    private float currentDashSpeed;
    public int maxDashes = 3;
    private int currentDashes;
    public float dashDelay = 2;
    private float currentDashDelay;
    private Vector3 dashDirection;
    private float rollSpeedDrop = 2.5f;
    private float rollSpeedMinimum = 7.5f;

    //Movement States.
    private enum State
    {
        Normal,
        Dashing
    }

    private State state;

    //Health.
    private int hearts;
    public int maxHearts = 5;
    public HeartSystem heartSystem;

    //Colourbar UI.
    private float currentColour;
    public Animator colourBorderAnimator;
    public Animator colourFillAnimator;
    private Image colourFill;
    private Image colourBorder;

    //Take Damage.
    public float damageDelay;
    private float currentDamageDelay;
    public BoxCollider2D damageHitbox;
    private bool canDamage;
    private bool isDamaged;
    private float colourDelay;

    //Dead conditions
    private float deathDelay = -1f;
    private bool dead;

    //Attacking.
    private bool isAttacking;
    private Vector3 mouseWorldPos;
    private Camera mainCamera;

    //Room Control.
    private Vector3 startPos;
    public int currentRoomNumber = 1;

    //Level Control.
    public int currentLevel;
    private bool colourFull;

    //Rigidbody.
    private Rigidbody2D rb;

    //Player Animator.
    public Animator playerAnimator;

    //---------- SET UP PLAYER ----------
    private void Start()
    {
        //Get Components.
        rb = GetComponent<Rigidbody2D>();
        damageHitbox = GameObject.Find("Damage Hitbox").GetComponent<BoxCollider2D>();
        colourFill = GameObject.Find("Colour Fill").GetComponent<Image>();
        colourBorder = GameObject.Find("Colour Border").GetComponent<Image>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //Set up health.
        hearts = maxHearts;
        heartSystem.DrawHearts(hearts, maxHearts);

        //Set start position of player for checkpoint.
        startPos = new Vector3(transform.position.x, transform.position.y);

        //Set default state.
        state = State.Normal;

        //Limit FPS for testing.
        //Application.targetFrameRate = 60;
    }

    //---------- STANDARD UPDATES ----------
    private void Update()
    {
        //Run the necessary functions
        if (dead == false)
        {
            ProcessInputs();
            UpdateAnimations();
            UpdateColourBar();
        }
        else
        {
            //Sets move speed to 0 when the player dies so they stop moving.
            moveSpeed = 0;
        }

        //Runs the check if player has died function
        CheckDead();

        //Calculates the mouse world position for the player attack animations
        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

    }

    //---------- PHYSICS UPDATES ----------
    private void FixedUpdate()
    {
        //Runs move player function
        MovePlayer();
                
        //If the players dash amount goes below the max they start adding more up to max.
        if (currentDashes < maxDashes)
        {
            //If the dash delay reaches 0 the dash amount increases by one and the dash delay is reset.
            if (currentDashDelay <= 0)
            {
                currentDashes += 1;
                currentDashDelay = dashDelay;
            }

            //Counts down the dash delay timer
            currentDashDelay -= Time.deltaTime;
        }

        //Colour changing delay for when the player takes damage
        if (colourDelay <= 0)
        {
            isDamaged = false;
        }
        else
        {
            colourDelay -= Time.deltaTime;
        }

        //Counts down the death delay
        deathDelay -= Time.deltaTime;

        //Counts down the damage delay
        currentDamageDelay -= Time.deltaTime;  
    }

        //---------- PROCESS INPUTS ----------

        //All player inputs run through this function.
        private void ProcessInputs()
        {
            //Switch states depending on whether player is moving normally or dashing.
            switch (state)
            {
                //Standard movement state.
                case State.Normal:

                    //Get move directions from input.
                    float moveX = Input.GetAxisRaw("Horizontal");
                    float moveY = Input.GetAxisRaw("Vertical");

                    //Edits move Y speed to fit isometric format.
                    moveY = moveY / 2;

                    //Set move direction
                    moveDirection = new Vector3(moveX, moveY);

                    //Normalize move direction so speed is consistant across all directions.
                    moveDirection = moveDirection.normalized;

                    //Check if the player is trying to dash.
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        //If the player has at least one dash this will run.
                        if (currentDashes > 0)
                        {
                            //Players dash count is lowered and the dash direction is set as the players current move direction.
                            //Sets the state to the dashing state.
                            currentDashes -= 1;
                            dashDirection = moveDirection;
                            state = State.Dashing;
                        }
                    }
                    break;

                //Dashing movement state.
                //Dash movement used a YouTube tutorial to help with the programming: https://youtu.be/Bf_5qIt9Gr8
                case State.Dashing:
                    //Slows the current dash speed over time.
                    currentDashSpeed -= dashSpeed * rollSpeedDrop * Time.deltaTime;

                    //Once the dash speed reaches the minimum speed the state is changed back to normal and dash speed is reset.
                    if (currentDashSpeed < rollSpeedMinimum)
                    {
                        state = State.Normal;
                        currentDashSpeed = dashSpeed;
                    }
                    break;
            }

        //These inputs were used for debugging and testing the UI scripts to ensure they updated accordingly.
        
        //Manually Take Damage
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hearts < maxHearts)
            {
                hearts += 1;
                heartSystem.DrawHearts(hearts, maxHearts);
            }   
        }

        //Manually add colour
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            AddColour(0.05f);
        }
    }

    //---------- MOVE PLAYER ----------

    //Moves the player correctly depending on what state they are currently in
    private void MovePlayer()
    {
        //Check which movement state the player is currently in.
        switch (state)
        {
            //If in standard movement
            case State.Normal:
                //Sets the rigidbody velocity to the move direction and movespeed.
                rb.velocity = moveDirection * moveSpeed;
                //Allows the player to be damaged while moving normally.
                canDamage = true;

                break;

            //If in dashing movement
            case State.Dashing:
                //Sets the rigidbody velocity to the dash direction and speed.
                rb.velocity = dashDirection * dashSpeed;
                //Stops the player from taking damage while dashing.
                canDamage = false;

                break;
        }
    }

    //---------- UPDATE PLAYER ANIMATIONS ----------

    //Updates the player animations so the correct animation shows at the appropriate time.
    private void UpdateAnimations()
    {
        //Update movement variables needed for various animations
        playerAnimator.SetFloat("Horizontal", moveDirection.x);
        playerAnimator.SetFloat("Vertical", moveDirection.y);
        playerAnimator.SetFloat("Magnitude", moveDirection.magnitude);

        //Update attacking variable for the attack animation.
        playerAnimator.SetBool("Attacking", isAttacking);

        //Sets last direction variables for animations to allow them to play the correct one next (Idle, attacking)
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            playerAnimator.SetFloat("lastHorizontal", Input.GetAxisRaw("Horizontal"));
            playerAnimator.SetFloat("lastVertical", Input.GetAxisRaw("Vertical"));
        }

        //Updates colour animation variables to ensure the correct colour bar varient is shown on each level.
        colourBorderAnimator.SetInteger("Level", currentLevel);
        colourFillAnimator.SetInteger("Level", currentLevel);

        //Update attack direction
        playerAnimator.SetFloat("MouseHorizontal", mouseWorldPos.x - transform.position.x);
        playerAnimator.SetFloat("MouseVertical", mouseWorldPos.y - transform.position.y);
    }

    //---------- HEALTH SYSTEM ----------

    //Add health to the player if they have lost health (Can be called from other scripts such as healthpack script)
    public void AddHealth(int amount)
    {
        //Checks if the player is under max health and if so will add the appropriate amount of hearts.
        if (hearts < maxHearts)
        {
            hearts += amount;
            //Debug.Log("Current Health: " + hearts);

            //Updates UI with the new values
            heartSystem.DrawHearts(hearts, maxHearts);
        }
    }

    //---------- TAKE DAMAGE ----------
    //Makes the player lose health, can be called from other scripts.
    public void TakeDamage(int damage)
    {
        //Checks if the player can take damage
        if (currentDamageDelay <= 0)
        {
            //Resets the damage delay
            currentDamageDelay = damageDelay;

            //Lower player health
            hearts -= damage;

            //Update hearts UI to show new values
            heartSystem.DrawHearts(hearts, maxHearts);

            //Triggers the colour change to show they have been damaged
            isDamaged = true;
            colourDelay = 0.25f;
        }
        else
        {
            Debug.Log("Player Invincible");
        }

        //Checks if the player has died after taking damage
        if (hearts <= 0)
        {
            //Plays the appropriate death animation
            playerAnimator.Play("Death");

            //Sets the delay to allow the animation to play
            deathDelay = 2f;

            //Sets the bool to stop enemies from activating.
            dead = true;
        }
    }

    //---------- COLOUR BAR ----------

    //Adds to the colour amount (can  be called from other scripts)
    public void AddColour(float colour)
    {
        //As the colour bar has an angled design, making each increment the same amount made the first and last increments very difficult to see.
        //This if statement corrects this by making the first and last increments slightly larger.
        if (currentColour < 0.1f)
        {
            currentColour = 0.1f;
        }
        else if (currentColour > 0.9f)
        {
            currentColour = 1f;
        }
        else if (currentColour >= 0.1f && currentColour <= 0.9f)
        {
            currentColour += colour;
        }

        //This checks if the colour has been filled and allows the player to progress to the next level.
        if (currentColour == 1)
        {
            colourFull = true;
        }
    }

    //Updates the colour bar UI
    private void UpdateColourBar()
    {
        //Updates the colourbar with the current colour amount
        colourFill.fillAmount = currentColour;


        //If the current level is the boss level, the colour bar is hidden as it is no longer needed in the boss fight
        if (currentLevel == 4)
        {
            colourBorder.enabled = false;
            colourFill.enabled = false;
        }
        //Any other levels the colour bar is visible.
        else
        {
            colourBorder.enabled = true;
            colourFill.enabled = true;
        }
    }

    //---------- CHECK IF PLAYER HAS DIED ----------
    private void CheckDead()
    {
        //If the death delay goes between these parameters the game over screen is loaded
        if (deathDelay < 0 && deathDelay > -1f)
        {
            if (currentLevel == 1)
            {
                SceneManager.LoadScene("Level1_GameOver");
            }
            else if (currentLevel == 3)
            {
                SceneManager.LoadScene("Level3_GameOver");
            }
            else if (currentLevel == 4)
            {
                SceneManager.LoadScene("Level4_GameOver");
            }
        }
    }

    //---------- GET / SET FUNCTIONS ----------

    //Room Number
    public int GetRoomNumber()
    {
        return currentRoomNumber;
    }

    public void SetRoomNumber(int newRoomNumber)
    {
        currentRoomNumber = newRoomNumber;
    }

    //Current Level
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    //Start Position
    public Vector3 GetStartPos()
    {
        return startPos;
    }

    public void SetStartPos(Vector3 newStartPos)
    {
        startPos = newStartPos;
    }

    //Is Attacking?
    public bool GetAttacking()
    {
        return isAttacking;
    }

    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    //Can Damage?
    public bool GetCanDamage()
    {
        return canDamage;
    }

    public void SetCanDamage(bool damage)
    {
        canDamage = damage;
    }

    //Health
    public int GetHealth()
    {
        return hearts;
    }

    //Max Health
    public int GetMaxHealth()
    {
        return maxHearts;
    }

    //Colour Full?
    public bool GetColourFull()
    {
        return colourFull;
    }

    //Is Damaged?
    public bool GetIsDamaged()
    {
        return isDamaged;
    }

    //Is player dead?
    public bool GetIsDead()
{
        return dead;
    }
}
