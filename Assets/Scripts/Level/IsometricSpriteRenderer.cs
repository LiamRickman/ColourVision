using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This script organises the sprite sorting order so that the player can move in front and behind objects in the isometric viewpoint.
//A YouTube tutorial helped in the creation of this script
//YouTube Link: https://youtu.be/t1UwAGFLmrk

public class IsometricSpriteRenderer : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    public float value;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //Find sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Set the sprite sorting order relative to the players current position in the world.
        //Places them in front or behind of objects as they move around.
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * value);
    }
}
