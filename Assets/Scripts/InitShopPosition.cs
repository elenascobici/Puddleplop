using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InitPosition : MonoBehaviour
{
    public Transform storeTransform;
    public Tilemap tileMap;

    void Start()
    {
        UpdateStorePosition();
    }

    void UpdateStorePosition()
    {
        Vector3 storePosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 2 * Screen.height / 3f, Camera.main.nearClipPlane));
        storePosition.x += tileMap.cellSize.x * 2;
        storeTransform.position = storePosition;
    }

    void Update()
    {
        UpdateStorePosition();
    }
}
