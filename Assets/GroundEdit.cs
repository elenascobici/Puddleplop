using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GroundEdit : MonoBehaviour
{
    public RuleTile soilTile;
    public Tile grassTile;
    public Tilemap tileMap;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tpos = tileMap.WorldToCell(worldPoint);
            var tile = tileMap.GetTile(tpos);

            if (tile.name.Contains("Ground")) {
                tileMap.SetTile(tpos, soilTile);
            }
            else {
                tileMap.SetTile(tpos, grassTile);
            }
        }
    }
}
