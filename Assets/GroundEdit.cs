using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GroundEdit : MonoBehaviour
{
    public RuleTile soilTile;
    public Tilemap tileMap;
    private Vector3 worldPoint;
    void Update() {
        if (Input.GetMouseButtonDown(0))
            {
                worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
                var tpos = tileMap.WorldToCell(worldPoint);

                // Try to get a tile from cell position
                var tile = tileMap.GetTile(tpos);

                if (tile)
                {
                    tileMap.SetTile(tpos, soilTile);
                }
            }
    }
}
