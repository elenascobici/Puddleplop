using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundEditButtonScript : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;
    public Button toggleGroundEditButton;
    public bool groundEditEnabled;
    // Start is called before the first frame update
    void Start()
    {
        groundEditEnabled = false;
        toggleGroundEditButton.onClick.AddListener(ToggleButtonClicked);
    }

    void ToggleButtonClicked() {
        groundEditEnabled = !groundEditEnabled;
        toggleGroundEditButton.image.sprite = 
            toggleGroundEditButton.image.sprite == disabledSprite 
                ? enabledSprite : disabledSprite;
    }

    void Update() {
        if (Input.GetKeyDown("q") && !groundEditEnabled) {
            ToggleButtonClicked();
        }
    }
}
