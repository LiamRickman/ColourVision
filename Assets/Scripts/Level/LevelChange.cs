using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script moves the player between levels once they reach the end
public class LevelChange : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    private PlayerController player;
    public string newLevel;

    public void Start()
    {
        //Find player game object
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //---------- COLLISION DETECTION ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player collides with the object AND the colour bar is full the next scene can be loaded.
        if (collision.transform.CompareTag("Player"))
        {
            if (player.GetColourFull())
            {
                SceneManager.LoadSceneAsync(newLevel);
            }
            //Otherwise a debug log will be printed
            else
            {
                Debug.Log("Fill the colour bar first!");
            }
        }
    }
}
