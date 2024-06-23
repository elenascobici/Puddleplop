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
    public XpBarScript xpBar;
    
    void Start()
    {
        xpBarAnimator.Play("Idle");
        coinCount.text = userData.data["coins"];
        xpLevel.text = userData.data["xpLevel"];
        xpBar.Init(1);
    }
}
