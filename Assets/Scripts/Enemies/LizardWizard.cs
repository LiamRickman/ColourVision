using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//The lizard wizard script controls the boss character and its attack stages.
//A YouTube tutorial was used as a baseline for the ranged attack just as with the ranged enemy
//YouTube Link: https://youtu.be/kOzhE3_P2Mk

public class LizardWizard : MonoBehaviour
{
    //===== DECLARING VARIABLES =====
    //Health
    public float maxHealth;
    private float currentHealth;
    private bool destroyed;
    public float deathDelay;
    private bool hurt = false;

    //Positions
    //private Vector3 startingPosition;

    //Misc. Game Objects
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitbox;
    private Animator animator;

    //Shockwave Stage
    private bool shockwaveFire;
    private float shockwaveNextFire;
    public float shockwaveFireRate;
    public GameObject shockwaveProjectile;
    public float shockwaveRange = 4f;
    public Transform shockwavePos;
    private Vector3 shockwaveDirection;
    private Vector3[] shockwaveDirectionArray = new[] {new Vector3 (0, 1), //N
                                                       new Vector3 (0.4f, 1),
                                                       new Vector3 (1, 1), //NE
                                                       new Vector3 (1, 0.4f),
                                                       new Vector3 (1, 0), //E
                                                       new Vector3 (1, -0.4f),
                                                       new Vector3 (1, -1), //SE
                                                       new Vector3 (0.4f, -1),
                                                       new Vector3 (0, -1), //S
                                                       new Vector3 (-0.4f, -1f),
                                                       new Vector3 (-1, -1), //SW
                                                       new Vector3 (-1, -0.4f),
                                                       new Vector3 (-1, 0), //W
                                                       new Vector3 (-1, 0.4f),
                                                       new Vector3 (-1, 1),// NW
                                                       new Vector3 (-0.4f, 1)
    };

    //Pigment Stage
    [Range(1, 3)]
    public int pigmentType;
    private int lastPigment;
    public float spawnRate;
    private float spawnNext;
    private int pigmentAmount;
    public GameObject enemyMelee;
    public GameObject enemyRanged;
    public GameObject enemyFlying;
    public GameObject[] pigmentSpawns;
    private GameObject pigmentSpawn;
    private Vector3 randomPigmentPosition;
    private string listText;
    List<Vector3> list = new List<Vector3>();

    //Ranged Stage
    public GameObject rangedProjectile;
    public float rangedFireRate;
    private float nextFire;
    private bool rangedFire;

    //Stage Management
    private enum State
    {
        Inactive,
        Shockwave,
        Pigments,
        PigmentInactive,
        Ranged,
        PlayerDead,
    }
    private State state;
    public float maxShockwaveDuration;
    public float maxRangedDuration;
    private float currentStageDuration;
    private int currentStage = 2;
    private bool pigmentStageEnd;
    private bool isActive;

    //What is player
    public LayerMask whatIsPlayer;

    //Healthbar
    private Image bossHealthFill;
    private Image bossHealthBorder;
    private float healthFill;

    //---------- SET UP LIZARD WIZARD ----------
    private void Start()
    {
        //Find game objects
        animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        hitbox = transform.GetComponentInChildren<Collider2D>();
        bossHealthFill = GameObject.Find("Boss Health Fill").GetComponent<Image>();
        bossHealthBorder = GameObject.Find("Boss Health Border").GetComponent<Image>();

        //Set starting position
        //startingPosition = transform.position;

        //Set health variables
        currentHealth = maxHealth;
        healthFill = 1f;

        //Set firerates
        nextFire = Time.time;
        shockwaveNextFire = Time.time;

        //Set default state
        state = State.Inactive;

        //Create list of pigment spawns
        SetPigmentList();

        //Play spawn animation
        animator.Play("Spawn");
    }

    //---------- UPDATE ----------
    private void Update()
    {
        //Run necessary functions
        UpdateHealthBar();
        CheckDead();
        ChooseStage();
        ShowUI();
        DebugInputs();
        UpdateSprite();

        //Deactivate the boss if the player dies
        if (player.GetIsDead())
        {
            state = State.PlayerDead;
        }

        //Checks if the lizard has been destroyed
        if (destroyed)
        {
            state = State.Inactive;
            animator.Play("Death");
        }
    }

    //---------- FIXED UPDATE ----------
    private void FixedUpdate()
    {
        //Only runs if the lizard is activated
        if (isActive)
        {
            //Stops the lizard attacking if they have just taken damage
            if (hurt == false)
            {
                //Start switch statement to control how the lizard wizard functions
                switch (state)
                {
                    //First state the boss uses where the spawn animation plays
                    case State.Inactive:

                        break;

                    //This state sets the boss to inactive if the player dies.
                    case State.PlayerDead:

                        isActive = false;
                        break;

                    //This state controls the shockwave attack
                    case State.Shockwave:

                        //A collider is made to check if the player enters the range that the lizard can fire the shockwave from.
                        Collider2D inRange = Physics2D.OverlapCircle(shockwavePos.position, shockwaveRange, whatIsPlayer);

                        //If the player is in range the lizard will start firing shockwaves at the appropriate firerate
                        if (inRange)
                        {
                            //Play animation
                            animator.Play("Slam");

                            //Waits for toggle from animation
                            if (shockwaveFire)
                            {
                                //Loops through the shockwave direction array and spawns a projectile that fires once in each of the directions in the array
                                for (int i = 0; i < shockwaveDirectionArray.Length; i++)
                                {
                                    //Sets the shockwave direction from the array
                                    shockwaveDirection = shockwaveDirectionArray[i];
                                    //Spawns a shockwave projectile on the lizard wizard
                                    GameObject shockwave = Instantiate(shockwaveProjectile, transform.position, Quaternion.identity);
                                    //Gets the shockwave projectile component so its direction can be updated
                                    LizardWizardShockwave theShockwave = shockwave.GetComponent<LizardWizardShockwave>();
                                    //Updates the shockwave projectile direction so it fires in a unique direction.
                                    theShockwave.SetMoveDirection(shockwaveDirection);

                                }
                                //Plays the shockwave attack sound
                                SFXManager.PlaySound("BossProjectile3");

                                //Stops shockwaves firing
                                shockwaveFire = false;

                                //OLD NON ANIMATION CONTROLLED SHOCKWAVES
                                //if (Time.time > shockwaveNextFire)
                                //{
                                //    //Loops through the shockwave direction array and spawns a projectile that fires once in each of the directions in the array
                                //    for (int i = 0; i < shockwaveDirectionArray.Length; i++)
                                //    {
                                //        //Sets the shockwave direction from the array
                                //        shockwaveDirection = shockwaveDirectionArray[i];
                                //        //Spawns a shockwave projectile on the lizard wizard
                                //        GameObject shockwave = Instantiate(shockwaveProjectile, transform.position, Quaternion.identity);
                                //        //Gets the shockwave projectile component so its direction can be updated
                                //        LizardWizardShockwave theShockwave = shockwave.GetComponent<LizardWizardShockwave>();
                                //        //Updates the shockwave projectile direction so it fires in a unique direction.
                                //        theShockwave.SetMoveDirection(shockwaveDirection);

                                //    }
                                //    //Plays the shockwave attack sound
                                //    SFXManager.PlaySound("BossProjectile3");
                                //    //Counts down the shockwave fire rate
                                //    shockwaveNextFire = Time.time + shockwaveFireRate;
                                //}
                            }
                        }
                        break;

                    //This state controls the pigment spawning stage
                    case State.Pigments:

                        //Only five pigments will be spawned per stage
                        if (pigmentAmount < 5)
                        {
                            //Controls the spawnrate of the pigments so they do not appear all at once
                            if (Time.time > spawnNext)
                            {
                                //Stores the last pigment type
                                lastPigment = pigmentType;
                                //Constantly loops to ensure the same pigment does not spawn straight after itself
                                while (pigmentType == lastPigment)
                                {
                                    //Random range for the pigment types
                                    pigmentType = Random.Range(1, 4);
                                }

                                //Picks a random number between 0 and the amount of values in the list of positions
                                int index = Random.Range(0, list.Count);
                                //Stores the vector position of the list value.
                                Vector3 randomPigmentPosition = list[index];
                                //Removes the value from the list to ensure no duplicates
                                list.RemoveAt(index);

                                //Controls how the pigment spawns
                                if (pigmentType == 1)
                                {
                                    //Plays a pigment spawn sound effect
                                    //SFXManager.PlaySound("BossProjectile2");

                                    //Spawns a melee enemy at the random position calculated earlier
                                    GameObject pigment = Instantiate(enemyMelee, randomPigmentPosition, Quaternion.identity);

                                    //Gets the melee pigment so it can be activated.
                                    EnemyMelee thePigment = pigment.GetComponent<EnemyMelee>();

                                    //Activates the melee pigment
                                    thePigment.SetActive(true);

                                }
                                //Ranged Pigment
                                else if (pigmentType == 2)
                                {
                                    //Plays a pigment spawn sound effect
                                    //SFXManager.PlaySound("BossProjectile2");

                                    //Spawns a ranged enemy at the random position calculated earlier
                                    GameObject pigment = Instantiate(enemyRanged, randomPigmentPosition, Quaternion.identity);

                                    //Gets the ranged pigment so it can be activated.
                                    EnemyRanged thePigment = pigment.GetComponent<EnemyRanged>();

                                    //Activates the ranged pigment
                                    thePigment.SetActive(true);
                                }
                                //Flying Pigment
                                else if (pigmentType == 3)
                                {
                                    //Plays a pigment spawn sound effect
                                    //SFXManager.PlaySound("BossProjectile2");

                                    //Spawns a flying enemy at the random position calculated earlier
                                    GameObject pigment = Instantiate(enemyFlying, randomPigmentPosition, Quaternion.identity);

                                    //Gets the flying pigment so it can be activated.
                                    EnemyFlying thePigment = pigment.GetComponent<EnemyFlying>();

                                    //Activates the flying pigment
                                    thePigment.SetActive(true);
                                }

                                //Resets the spawn rate
                                spawnNext = Time.time + spawnRate;

                                //Adds to the pigment amount so no more than 5 can spawn
                                pigmentAmount += 1;
                            }
                        }
                        break;

                    //The inactive state is used to pause the boss while the player kills the pigments that were spawned.
                    case State.PigmentInactive:

                        //Checks if there are any pigments in the scene
                        if (GameObject.FindGameObjectWithTag("Damage") != null)
                        {
                            //If there are pigments present the stage will not end
                            pigmentStageEnd = false;
                        }
                        //If there are no pigments left the stage will end and move on.
                        else
                        {
                            pigmentStageEnd = true;
                        }
                        break;

                    //This state controls the ranged attacks that fire at the player. Similar to the standard ranged attacks this used a YouTube tutorial to get the basics of spawning and firing projectiles at an object
                    //YouTube Link: https://youtu.be/kOzhE3_P2Mk
                    case State.Ranged:

                        //Play animation
                        //The attack animation controls the firerate of the projectiles by toggling rangedFire with an animation event.
                        animator.Play("Attack");

                        //Checks if the lizard can fire. (Can't fire if they are taking damage)
                        if (rangedFire && hurt == false)
                        {
                            //Play sound effect
                            SFXManager.PlaySound("BossProjectile");

                            //Spawn projectile on the lizard wizard
                            Instantiate(rangedProjectile, transform.position, Quaternion.identity);

                            //Toggle the fire boolean off.
                            rangedFire = false;
                        }

                        //OLD ATTACK METHOD
                        //Checks if the lizard wizard can fire the projectile
                        //if (Time.time > nextFire)
                        //{
                        //    //Plays a sound effect
                        //    SFXManager.PlaySound("BossProjectile");

                        //    animator.Play("Attack");

                        //    //Spawns a ranged projectile on the lizard wizard
                        //    Instantiate(rangedProjectile, transform.position, Quaternion.identity);

                        //    //Resets the fire rate counter
                        //    nextFire = Time.time + rangedFireRate;
                        //}
                        break;
                }
            }
        }
    }

    //---------- LIZARD WIZARD TAKES DAMAGE ----------
    public void TakeDamage(int damage)
    {
        //Plays a damage effect
        SFXManager.PlaySound("BossHurt");

        //Play hurt animation and toggle boolean so lizard cant attack
        animator.Play("Hurt");
        hurt = true;

        //Lowers the lizard wizards health
        currentHealth -= damage;

        //Updates the healthbar
        UpdateHealth(currentHealth / maxHealth);
    }

    public void ToggleHurtOff()
    {
        hurt = false;
    }

    //---------- CHECK IF LIZARD HAS DIED ----------
    private void CheckDead()
    {
        if (isActive)
        {
            //If the lizard has reached 0 health the death condition will run
            if (currentHealth <= 0)
            {
                //Death sound effect plays
                SFXManager.PlaySound("BossDeath1");

                //Boolean toggle
                destroyed = true;

                //Deactivate the boss
                isActive = false;
            }
        }
    }

    //---------- UPDATE SPRITE ----------
    private void UpdateSprite()
    {
        //Depending on what state is active the lizard wizard sprite will be visible or invisible.
        //The hitbox will also be toggled depending on whether they are visible or not.
        if (state == State.Pigments || state == State.PigmentInactive)
        {
            spriteRenderer.enabled = false;
            hitbox.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            hitbox.enabled = true;
        }

        //Animation Updates
        animator.SetFloat("Horizontal", player.transform.position.x - transform.position.x);
        animator.SetFloat("Vertical", player.transform.position.y - transform.position.y);
    }

    //---------- UPDATE UI ----------
    private void ShowUI()
    {
        //If the lizard wizard is destroyed the health bar will disappear
        if (destroyed)
        {
            bossHealthBorder.enabled = false;
            bossHealthFill.enabled = false; ;
        }
        else
        {
            //If the boss level is active, the boss is activated and the healthbar appears
            if (player.GetCurrentLevel() == 4)
            {
                bossHealthBorder.enabled = true;
                bossHealthFill.enabled = true;
            }
            //Otherwise the boss and healthbar are deactivated.
            else
            {
                isActive = false;
                bossHealthBorder.enabled = false;
                bossHealthFill.enabled = false; ;
            }
        }
    }

    public void UpdateHealth(float health)
    {
        //When updating the healthbar the first and last increments needed to be larger to be more obvious on the UI.
        //This if statement calculates when it is the first or last increment and makes it a larger value.
        //Otherwise it will add up as normal.
        if (healthFill < 0.06f)
        {
            healthFill = 0.06f;
        }
        else if (healthFill > 0.94f)
        {
            healthFill = 0.94f;
        }
        else if (healthFill >= 0.06f && healthFill <= 0.94f)
        {
            healthFill = health;
        }
    }

    //Updates the healthbar with the appropriate amount
    private void UpdateHealthBar()
    {
        bossHealthFill.fillAmount = healthFill;
    }

    //---------- CHOOSE ATTACK STAGE ----------
    private void ChooseStage()
    {
        //Counts down the stage duration as long as it is above 0
        if (currentStageDuration > 0)
        {
            currentStageDuration -= Time.deltaTime;
        }

        //If the boss is active it will move between stages
        if (isActive)
        {
            //Move from shockwave to pigments
            if (currentStage == 2 && currentStageDuration <= 0)
            {
                //Reset pigment amount
                pigmentAmount = 0;

                //Reset pigment list to get fresh positions
                SetPigmentList();

                //Set new stage
                currentStage = 3;
                state = State.Pigments;
            }

            //move from pigments to ranged
            if (currentStage == 3 && pigmentStageEnd == true)
            {
                //Reset boolean
                pigmentStageEnd = false;

                //Boss takes damage as all pigments have died
                TakeDamage(3);

                //Sets stage duration to ranged duration
                currentStageDuration = maxRangedDuration;

                //Sets new stage
                currentStage = 4;
                state = State.Ranged;
            }

            //Pause pigment stage when 5 pigments are on screen
            else if (currentStage == 3 && pigmentAmount >= 5)
            {
                //Change state
                state = State.PigmentInactive;

                //Reset pigment amount
                pigmentAmount = 0;
            }

            //move from ranged to shockwave
            if (currentStage == 4 && currentStageDuration <= 0)
            {
                //Sets stage duration to shockwave duration
                currentStageDuration = maxShockwaveDuration;

                //Sets new stage
                currentStage = 2;
                state = State.Shockwave;
            }
        }
    }

    //---------- CREATE PIGMENT LIST ----------
    private void SetPigmentList()
    {
        //Clears the list so it can be recreated with new values
        list.Clear();

        //Loops through the pigment spawns adding their positions to the list
        for (int i = 0; i < pigmentSpawns.Length; i++)
        {
            pigmentSpawn = pigmentSpawns[i];

            //Inserts the new pigment spawn position into the list
            list.Insert(i, (new Vector3(pigmentSpawn.transform.position.x, pigmentSpawn.transform.position.y)));

            //converts the list index to string value for debugging
            listText = list[i].ToString();
        }
    }

    //---------- LIZARD WIZARD DIES ----------
    public void DestroyLizardWizard()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    //---------- SHOW SHOCKWAVE RANGE ----------
    //Draws shockwave attack range on screen
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shockwavePos.position, shockwaveRange);
    }

    //---------- DEBUG INPUTS ----------
    //These allowed me to test each stage seperately before making the stage management and test any other functions as needed. Toggled off for the final build.
    private void DebugInputs()
    {
        //Swap to Inactive stage
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            state = State.PigmentInactive;
            Debug.Log("Pigment Inactive Stage");
        }

        //Swap to shockwave stage
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            state = State.Shockwave;
            Debug.Log("Shockwave Stage");
        }

        //Swap to pigment stage
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            pigmentAmount = 0;
            state = State.Pigments;
            SetPigmentList();
            Debug.Log("Pigment Stage");
        }

        //Swap to ranged stage
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            state = State.Ranged;
            Debug.Log("Ranged Stage");
        }

        //Set the boss to active
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            isActive = true;
        }

        //Lower boss health
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            TakeDamage(1);
        }
    }

    //---------- GET / SET FUNCTIONS ----------
    //Is lizard destroyed
    public bool GetDestroyed()
    {
        return destroyed;
    }

    public void ToggleRangedFire()
    {
        rangedFire = true;
    }
    public void ToggleShockwaveFire()
    {
        shockwaveFire = true;
    }

    public void SetStartState()
    {
        isActive = true;
        currentStage = 4;
        currentStageDuration = maxRangedDuration;
        state = State.Ranged;
    }
}

