using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitPlayerStatsScript : MonoBehaviour
{
    public UserData userData;
    public TextMeshProUGUI coinCount;
    public TextMeshProUGUI xpLevel;
    public Animator xpBarAnimator;
    
    void Start()
    {
        xpBarAnimator.Play("Idle");
        coinCount.text = userData.data["coins"];
        xpLevel.text = userData.data["xpLevel"];
    }
}
