using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
public class GroundEdit : MonoBehaviour
{
    // Reference to the ground editing toggle script so we can
    // check whether ground editing has been enabled.
    public GroundEditButtonScript groundEditButtonScript;
    public UserData userData;
    public GroundData groundData;
    public GameObject frog;
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
        string groundDataString = userData.data.ContainsKey("soilTiles")
            ? userData.data["soilTiles"] : "\"\"";
        groundData = JsonConvert.DeserializeObject<GroundData>("{\"soilTiles\":" + groundDataString + "}");
        if (groundData.soilTiles == null) { // No soil tiles
            groundData.soilTiles = new List<GroundCoords>();
        }
        else { // Otherwise, load the soil tiles visually
            foreach (GroundCoords soilCoord in groundData.soilTiles) {
                tileMap.SetTile(new Vector3Int(soilCoord.x, soilCoord.y,
                    soilCoord.z), soilTile);
            }
        }
    }


    void Update() {
        // As soon as left button is clicked, record whether a
        // grass or soil tile has been clicked to determine whether
        // we are in soil tile mode or grass tile mode.
        if (Input.GetKeyDown("q") && groundEditButtonScript.groundEditEnabled) {
            // Vector3 worldPoint = Camera.main.ScreenToWorldPoint(frog.transform.position);
            // var tpos = tileMap.WorldToCell(worldPoint);
            var tpos = tileMap.WorldToCell(frog.transform.position);
            var tile = tileMap.GetTile(tpos);
            soilMode = tile.name.Contains("Ground");
        }
        // While user holds and drags left click button, update all
        // tiles based on mode.
        if (Input.GetKey("q") && groundEditButtonScript.groundEditEnabled) {
            // Vector3 worldPoint = Camera.main.ScreenToWorldPoint(frog.transform.position);
            // var tpos = tileMap.WorldToCell(worldPoint);
            var tpos = tileMap.WorldToCell(frog.transform.position);

            // Only edit the ground within the given bounds.
            if (InBounds(tpos)) {
                if (soilMode) {
                    tileMap.SetTile(tpos, soilTile);
                    groundData.Add(tpos);
                    userData.SaveKeyAndValue("soilTiles",
                        JsonConvert.SerializeObject(groundData.soilTiles));
                }
                else if (!tileMap.GetTile(tpos).name.Contains("Ground")) {
                    // Replace the soil tile with the initial grass tile.
                    tileMap.SetTile(tpos, initTileArray[tpos[0] + WIDTH/2, tpos[1] + HEIGHT/2]);
                    groundData.Remove(tpos);
                    userData.SaveKeyAndValue("soilTiles",
                        JsonConvert.SerializeObject(groundData.soilTiles));
                }
            }
        }
    }

    private bool InBounds(Vector3Int tpos) {
        return tpos[0] >= TOP_LEFT.Item1 && tpos[1] <= TOP_LEFT.Item2
            && tpos[0] <= BOTTOM_RIGHT.Item1 && tpos[1] >= BOTTOM_RIGHT.Item2;
    }
}
