using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        switch (this.gameObject.name) {
            case "ShopExteriorDoor":
                SceneManager.LoadScene("ShopScene");
                break;
            case "MarketPathCollider":
                SceneManager.LoadScene("MarketScene");
                break;
            default:
                break;
        }

    }
}
