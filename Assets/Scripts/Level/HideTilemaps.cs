using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This is a small script that allows whole tilemaps to be hidden from view.
//This is primarily used for the collision tilemap.

public class HideTilemaps : MonoBehaviour
{
    //---------- DECLARE VARIABLES ----------
    private TilemapRenderer tilemapRenderer;
    private void Start()
    {
        //Find tilemap renderer component
        tilemapRenderer = GetComponent<TilemapRenderer>();

        //Disabled the tilemap renderer
        tilemapRenderer.enabled = false;
    }
}
