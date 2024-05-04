using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundEditButtonScript : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;
    public Button toggleGroundEditButton;
    // Start is called before the first frame update
    void Start()
    {
        toggleGroundEditButton.onClick.AddListener(ToggleButtonClicked);
    }

    void ToggleButtonClicked() {
        toggleGroundEditButton.image.sprite = 
            toggleGroundEditButton.image.sprite == disabledSprite 
                ? enabledSprite : disabledSprite;
    }
}
