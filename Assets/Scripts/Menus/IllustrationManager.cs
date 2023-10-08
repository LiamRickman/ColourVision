using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IllustrationManager : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    public string newLevel;
    public float delay;
    public bool isDelay = true;
    private float currentDelay;

    private void Start()
    {
        //Sets delay
        currentDelay = delay;
    }

    private void Update()
    {
        //Counts down the delay
        currentDelay -= Time.deltaTime;

        //Toggle so if the menu is set to use the delay it will wait until the delay is finished and then load the next scene.
        if (isDelay)
        {
            if (currentDelay <= 0)
            {
                SceneManager.LoadSceneAsync(newLevel);
            }
        }
        //Otherwise it will wait for the user to press Enter to continue to the next level
        else if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadSceneAsync(newLevel);
        }
    }
}
