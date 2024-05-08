using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSatchelScript : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;
    public Button toggleSatchelOpenButton;
    public bool satchelOpen;
    // Start is called before the first frame update
    void Start()
    {
        satchelOpen = false;
        toggleSatchelOpenButton.onClick.AddListener(ToggleButtonClicked);
    }

    void ToggleButtonClicked() {
        satchelOpen = !satchelOpen;
        toggleSatchelOpenButton.image.sprite = 
            toggleSatchelOpenButton.image.sprite == disabledSprite 
                ? enabledSprite : disabledSprite;
    }
}
