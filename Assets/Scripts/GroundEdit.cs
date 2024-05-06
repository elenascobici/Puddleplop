using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundEdit : MonoBehaviour
{
    // Reference to the ground editing toggle script so we can
    // check whether ground editing has been enabled.
    public GroundEditButtonScript groundEditButtonScript;
    public UserData userData;
    public GroundData groundData;
    const int WIDTH = 14;
    const int HEIGHT = 8;
    const int EDITABLE_LENGTH = 20;
    public RuleTile soilTile;
    public Tilemap tileMap;
    private bool soilMode = true;
    private TileBase[,] initTileArray = new TileBase[EDITABLE_LENGTH+1, HEIGHT];

    // Define the corners of the area in which the player can place
    // soil.
    private Tuple<int,int,int> TOP_LEFT = Tuple.Create(2,3,0);
    private Tuple<int,int,int> BOTTOM_RIGHT = Tuple.Create(13,-4,0);

    void Start() {
        // Record the initial grass tiles, so that we can replace
        // soil tiles with the grass tile that were there before.
        for (int i = 0; i <= EDITABLE_LENGTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                initTileArray[i, j] = tileMap.GetTile(new Vector3Int(i - WIDTH/2, j - HEIGHT/2, 0));
            }
        }
        string groundDataString = userData.data["soilTiles"];
        groundData = JsonUtility.FromJson<GroundData>("{\"groundData\":" + groundDataString + "}");
        Debug.Log(groundData.soilTiles);
    }


    void Update() {
        // As soon as left button is clicked, record whether a
        // grass or soil tile has been clicked to determine whether
        // we are in soil tile mode or grass tile mode.
        if (Input.GetMouseButtonDown(0) && groundEditButtonScript.groundEditEnabled) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tpos = tileMap.WorldToCell(worldPoint);
            var tile = tileMap.GetTile(tpos);
            soilMode = tile.name.Contains("Ground");
        }
        // While user holds and drags left click button, update all
        // tiles based on mode.
        if (Input.GetMouseButton(0) && groundEditButtonScript.groundEditEnabled) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tpos = tileMap.WorldToCell(worldPoint);

            // Only edit the ground within the given bounds.
            if (InBounds(tpos)) {
                if (soilMode) {
                    tileMap.SetTile(tpos, soilTile);
                }
                else if (!tileMap.GetTile(tpos).name.Contains("Ground")) {
                    // Replace the soil tile with the initial grass tile.
                    tileMap.SetTile(tpos, initTileArray[tpos[0] + WIDTH/2, tpos[1] + HEIGHT/2]);
                }
            }
        }
    }

    private bool InBounds(Vector3Int tpos) {
        return tpos[0] >= TOP_LEFT.Item1 && tpos[1] <= TOP_LEFT.Item2
            && tpos[0] <= BOTTOM_RIGHT.Item1 && tpos[1] >= BOTTOM_RIGHT.Item2;
    }
}
