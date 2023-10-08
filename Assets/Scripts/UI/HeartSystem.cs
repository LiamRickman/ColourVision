using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the hearts UI system.
//I used a YouTube tutorial as a baseline
//YouTube tutorial: https://youtu.be/D6SeFFKZOFc
public class HeartSystem : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    public GameObject heart;
    public GameObject emptyHeart;

    //This can be called from other scripts such as the Player Controller
    public void DrawHearts(int hearts, int maxHearts)
    {
        //Loops through each object in the current UI and destroys it so it is empty to add new prefabs
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //Loops for max health and creates different hearts depending on how much health the player has compared to max health.
        for (int i = 0; i < maxHearts; i++)
        {
            //Creates as many full hearts as the player has health
            if (i + 1 <= hearts)
            {
                GameObject newHeart = Instantiate(heart, transform.position, Quaternion.identity);
                newHeart.transform.SetParent(transform); ;
                newHeart.transform.localScale = new Vector3(1, 1, 1);
            }
            //Any extra spaces left are filled with empty hearts to show the players max health.
            else
            {
                GameObject newHeart = Instantiate(emptyHeart, transform.position, Quaternion.identity);
                newHeart.transform.SetParent(transform);
                newHeart.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
