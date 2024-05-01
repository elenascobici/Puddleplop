using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundEdit : MonoBehaviour
{
    const int WIDTH = 14;
    const int HEIGHT = 8;
    public RuleTile soilTile;
    public Tilemap tileMap;
    private bool soilMode = true;
    private TileBase[,] initTileArray = new TileBase[WIDTH, HEIGHT];

    void Start() {
        // Record the initial grass tiles, so that we can replace
        // soil tiles with the grass tile that were there before.
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                initTileArray[i, j] = tileMap.GetTile(new Vector3Int(i - WIDTH/2, j - HEIGHT/2 - 1, 0));
            }
        }
    }


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
            else if (!tileMap.GetTile(tpos).name.Contains("Ground")) {
                // Replace the soil tile with the initial grass tile.
                tileMap.SetTile(tpos, initTileArray[tpos[0] + WIDTH/2, tpos[1] + HEIGHT/2 + 1]);
            }
        }
    }
}
