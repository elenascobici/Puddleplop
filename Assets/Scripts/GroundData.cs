using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using Newtonsoft.Json;

[Serializable]
public class GroundData
{
    public List<GroundCoords> soilTiles;
    public void Add(Vector3Int coords) {
        foreach (GroundCoords tile in soilTiles) {
            if (tile.x == coords.x && tile.y == coords.y && tile.z == coords.z) {
                return;
            }
        }
        soilTiles.Add(new GroundCoords(coords));
    }

    public void Remove(Vector3Int coords) {
        foreach (GroundCoords tile in soilTiles) {
            if (tile.x == coords.x && tile.y == coords.y && tile.z == coords.z) {
                soilTiles.Remove(tile);
                return;
            }
        }
        
    }
}

[Serializable]
public class GroundCoords
{
    public int x;
    public int y;
    public int z;

    [JsonConstructor]
    public GroundCoords(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public GroundCoords(Vector3Int coords) {
        this.x = coords.x;
        this.y = coords.y;
        this.z = coords.z;
    }
}
