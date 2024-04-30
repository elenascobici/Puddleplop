using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GroundEdit : MonoBehaviour
{
    public RuleTile soilTile;
    public Tile grassTile;
    public Tilemap tileMap;
    private bool soilMode = true;
    void Update() {
        // As soon as left button is clicked, record whether a
        // grass or soil tile has been clicked to determine whether
        // we are in soil tile mode or grass tile mode.
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tpos = tileMap.WorldToCell(worldPoint);
            var tile = tileMap.GetTile(tpos);
            soilMode = tile.name.Contains("Ground");
        }
        // While user holds and drags left click button, update all
        // tiles based on mode.
        if (Input.GetMouseButton(0)) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tpos = tileMap.WorldToCell(worldPoint);

            if (soilMode) {
                tileMap.SetTile(tpos, soilTile);
            }
            else {
                tileMap.SetTile(tpos, grassTile);
            }
        }
    }
}
